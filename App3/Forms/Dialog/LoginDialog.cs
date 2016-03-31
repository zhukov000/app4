using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms.Dialog
{
    public partial class LoginDialog : Form
    {
        public string Result
        {
            get 
            {
                return textBox1.Text;
            }
        }

        public LoginDialog()
        {
            InitializeComponent();
            AcceptButton = button1;
        }

        private void LoginDialog_Shown(object sender, EventArgs e)
        {
            textBox1.Text = "";
        }
    }
}
