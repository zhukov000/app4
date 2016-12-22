using App3.Class.Socket;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
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
            SendObject data = new SendObject(textBox3.Text);
            string s = SocketClient.SendObjectFromSocket(data, textBox1.Text, Convert.ToInt32(textBox2.Text) );
            listBox1.Items.Add(s);
        }
    }
}
