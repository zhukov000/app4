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
            listBox1.Items.Add(s);*/
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
            }
        }
    }
}
