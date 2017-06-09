using System;
using System.Windows.Forms;
using App3.Class.Static;

namespace App3.Forms
{
    public partial class StartupForm : Form
    {
        private string startupType;
        public string TStartup
        {
            get { return startupType; }
        }
        public StartupForm()
        {
            InitializeComponent();
            Text += Class.Config.APPVERSION;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            startupType = StartupType.Run;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            startupType = StartupType.Monitor;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            startupType = StartupType.Log;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            startupType = StartupType.Socket;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            startupType = StartupType.Server;
        }
    }
}
