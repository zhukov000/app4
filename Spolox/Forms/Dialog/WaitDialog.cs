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
    public partial class WaitDialog : Form
    {
        public WaitDialog(Form parent)
        {
            InitializeComponent();
            Height = parent.Height+20;
            Width = parent.Width;
            WindowState = FormWindowState.Maximized;
        }
    }
}
