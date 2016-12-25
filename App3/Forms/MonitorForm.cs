using App3.Class;
using App3.Class.Socket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class MonitorForm : Form
    {
        public MonitorForm()
        {
            InitializeComponent();
        }

        delegate void ShowProgressDelegate(string txt, DateTime time, int totalDigits, int digitsSoFar);
        delegate void SearchDelegate(int i);

        /// <summary>
        /// Потоконезависимое обновление прогресса
        /// </summary>
        /// <param name="pi"></param>
        /// <param name="time"></param>
        /// <param name="totalDigits"></param>
        /// <param name="digitsSoFar"></param>
        void ShowProgress(string pi, DateTime time, int totalDigits, int digitsSoFar)
        {
            if (!listBox1.InvokeRequired)
            {
                progressBar1.Maximum = totalDigits;
                progressBar1.Value = digitsSoFar;
                label2.Text = time.ToString("dd.MM.yy HH:mm");
                listBox1.Items.Add(pi);
            }
            else
            {
                ShowProgressDelegate showProgress = new ShowProgressDelegate(ShowProgress);
                BeginInvoke(showProgress, new object[] { pi, time, totalDigits, digitsSoFar });
            }
        }

        /// <summary>
        /// Пуск проверки связи
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            SearchDelegate s = new SearchDelegate(search);
            button1.Enabled = false;
            s.BeginInvoke((int)numericUpDown1.Value, null, null);
        }

        /// <summary>
        /// Включить элементы интерфейса
        /// </summary>
        /// <param name="i"></param>
        private void enable(int i)
        {
            if (!button1.InvokeRequired)
            {
                button1.Enabled = true;
                progressBar1.Value = i;
            }
            else
            {
                SearchDelegate en = new SearchDelegate(enable);
                BeginInvoke(en, new object[] { i });
            }

        }

        /// <summary>
        /// Выполнение поиска по узлам из БД
        /// </summary>
        /// <param name="timeout"></param>
        private void search(int timeout)
        {
            List<object[]> rows = DataBase.RowSelect("select sn.id, sn.ipv4, sn.port, ip.description from syn_nodes sn left join oko.ipaddresses ip on ipv4 = ip.ipaddress");
            int cnt = rows.Count();
            ShowProgress("На связи: ", DateTime.Now, cnt, 0);

            TcpClient client = null;
            IAsyncResult result = null;
            int i = 0;
            foreach (object[] comp in rows)
            {
                try
                {
                    string server = comp[1].ToString();
                    int port = comp[2].ToInt();
                    string desc = comp[3].ToString();
                    client = new TcpClient();
                    result = client.BeginConnect(server, port, null, null);
                    var success = result.AsyncWaitHandle.WaitOne(TimeSpan.FromSeconds(timeout));
                    if (!success)
                    {
                        ShowProgress( string.Format("FAILED {2} ({0}:{1})", server, port, desc), DateTime.Now, cnt, ++i);
                        continue;
                    }
                    NetworkStream stream = client.GetStream();
                    SendObject data = new SendObject("TEST");
                    // преобразуем сообщение в массив байтов
                    byte[] data_arr = SocketUtils.ObjectToByteArray(data);
                    // отправка сообщения
                    stream.Write(data_arr, 0, data_arr.Length);

                    // получаем ответ в виде слова
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data_arr, 0, data_arr.Length);
                        builder.Append(Encoding.Unicode.GetString(data_arr, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    if (builder.ToString() != "ACCEPTED")
                        Class.Singleton.Logger.Instance.WriteToLog(string.Format("Testing {0}.{1}: {2}"));
                    ShowProgress(string.Format("{0} ({1}:{2}) получен ответ {3}", desc, server, port, builder.ToString()), DateTime.Now, cnt, ++i);
                }
                catch (Exception ex)
                {
                    Class.Singleton.Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                }
            }
            enable(0);
        }
    }
}
