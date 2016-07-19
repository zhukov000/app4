using App3;
using App3.Class;
using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Monitor
{
    public partial class Form1 : Form
    {
        Thread backgroundThread = null;
        List<SpoloxNode> nodes = new List<SpoloxNode>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == this.WindowState)
            {
                mynotifyicon.Visible = true;
                mynotifyicon.ShowBalloonTip(500);
                this.Hide();
            }
            else if (FormWindowState.Normal == this.WindowState)
            {
                mynotifyicon.Visible = false;
            }
        }

        private void mynotifyicon_DoubleClick(object sender, EventArgs e)
        {
            this.Show();
            Application.OpenForms[Name].Focus();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            bool f = false;
            try
            {
                DataBase.OpenConnection(
                    string.Format(
                        "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;",
                        Config.Get("DBServerHost"),
                        Config.Get("DBServerPort"),
                        Config.Get("DBUser"),
                        Config.Get("DBPassword"),
                        Config.Get("DBName")
                    ));
                f = true;
            }
            catch (DataBaseConnectionErrorExcepion ex)
            {
                MessageBox.Show("DataBasse: " + ex.Message);
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: Соединение с БД не было установлено. Причина: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: При открытии соединения с базой произошла ошибка: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            if (!f)
            {
                Close();
            }
            {
                UpdateMonitor();
                if (MessageBox.Show("Проверка соединения", "Проверить соединение с найденными узлами?", MessageBoxButtons.YesNo) == DialogResult.Yes) GetConnection();
            }
        }

        private void UpdateMonitor()
        {
            int i = 0, j = 0;
            nodes.Clear();
            List<object[]> rows = DataBase.RowSelect(string.Format("select id_region, ipaddress, coalesce(port,{0}), description from syn_nodes {1} join oko.ipaddresses on ipv4 = ipaddress order by description", Config.Get("WebPort"), Config.Get("OptionJoin")));
            int ll = rows.Count / 6;
            foreach (object[] row in rows)
            {
                SpoloxNode node1 = new SpoloxNode(row[0].ToInt(), row[1].ToString(), row[2].ToInt(), row[3].ToString());
                node1.Location = new System.Drawing.Point(i * 81, 96 * j);
                node1.Size = new System.Drawing.Size(80, 95);
                panel1.Controls.Add(node1);
                nodes.Add(node1);
                j++;
                if (j > 5)
                {
                    j = 0;
                    i++;
                }
            }
        }

        private void GetConnection()
        {
            foreach (SpoloxNode node in nodes)
            {
                if (Utils.IsLocalIpAddress(node.Ip)) continue;
                string url = String.Format("http://{0}:{1}/test", node.Ip, node.Port);
                string content = Utils.getHtmlContent(url);
                bool f = false;
                if (content.Length > 0)
                    f = true;
                if (node.InvokeRequired)
                    node.Invoke(new Action(() => { node.IsOn = f; }));
                else
                    node.IsOn = f;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DataBase.CloseConnection();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Visible = false;
            backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    GetConnection();
                    if (button1.InvokeRequired)
                    {
                        button1.Invoke(new Action(() => { button1.Visible = true; }));
                    }
                    else
                    {
                        button1.Visible = true;
                    }
                    Thread.Sleep(Config.Get("SleepPause").ToInt());
                }
            ));
            backgroundThread.IsBackground = true;
            backgroundThread.Start();
        }
    }
}
