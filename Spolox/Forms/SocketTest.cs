using App3.Class;
using App3.Class.Socket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class SocketTest : Form
    {
        public SocketTest()
        {
            InitializeComponent();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*SendObject data = new SendObject(textBox3.Text);
            string s = SocketClient.SendObjectFromSocket(data, textBox1.Text, Convert.ToInt32(textBox2.Text) );
            listBox1.Items.Add(s);
            TcpClient client = null;
            try
            {
                client = new TcpClient(textBox1.Text, Convert.ToInt32(textBox2.Text));
                NetworkStream stream = client.GetStream();

                SendObject obj = new SendObject(textBox3.Text);                    
                // преобразуем сообщение в массив байтов
                byte[] data = SocketUtils.ObjectToByteArray(obj);
                // отправка сообщения
                stream.Write(data, 0, data.Length);

                // получаем ответ в виде слова
                StringBuilder builder = new StringBuilder();
                int bytes = 0;
                do
                {
                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.Unicode.GetString(data, 0, bytes));
                }
                while (stream.DataAvailable);
                    
                listBox1.Items.Add(((System.Net.IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()+ ":" + builder.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                client?.Close();
            } */
            object[] array = new object[0];
            IDictionary<string, object> data = new Dictionary<string, object>
            {
                { "objectnumber",  10001 },
                { "alarmgroupid", 1 },
                { "datetime", DateTime.Now.Q() },
                { "code", 1 },
                { "typenumber", 1 },
                { "partnumber", 1 },
                { "zoneusernumber", 1 },
                { "class", 1 },
                { "address", "".Q() },
                { "region_id", 99 },
                { "channelnumber", 1 },
                { "oko_version", 2 },
                { "retrnumber", 1 },
                { "isrepeat", false },
                { "siglevel", 1 }
            };
            // Handling.SendDataBySocket(data);
            SendObject obj = new SendObject(data);
            obj.Message = textBox3.Text;
            SocketClient.SendObjectFromSocket2(obj, textBox1.Text, Convert.ToInt32(textBox2.Text));
        }
    }
}
