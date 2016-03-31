using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class ViewLog : Form
    {
        public ViewLog()
        {
            InitializeComponent();
        }

        private void ViewLog_Load(object sender, EventArgs e)
        {
            Logger.Instance.FlushLog();
            string s = Logger.Instance.LogFileName();
            if (File.Exists(s))
            {
                listBox1.Items.Clear();
                using (FileStream fs = File.Open(s, FileMode.Append, FileAccess.Read))
                {
                    using (StreamReader log = new StreamReader(fs))
                    {
                        while (log.Peek() >= 0)
                        {
                            listBox1.Items.Add(log.ReadLine());
                        } 
                    }
                }
            }

        }
    }
}
