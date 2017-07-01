using App3.Class;
using App3.Class.Static;
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
    public partial class SyncObjects : Form
    {
        public SyncObjects(Form parent)
        {
            InitializeComponent();
            this.MdiParent = parent;
        }

        private void InitCombo()
        {
            int regid = Config.Get("CurrenRegion", "-1").ToInt();
            comboBox1.Items.Clear();
            foreach (var pair in DBDict.TRegion)
            {
                ComboboxItem item = new ComboboxItem(pair.Value.Item1, pair.Key);
                comboBox1.Items.Add(item);
                if (regid == pair.Key)
                {
                    comboBox1.SelectedItem = item;
                }
            }
        }

        private void SyncObjects_Load(object sender, EventArgs e)
        {
            InitCombo();
            object ipadr = DataBase.First("SELECT ipv4 FROM syn_nodes WHERE synout", "ipv4");
            if (ipadr != null) textBox1.Text = ipadr.ToString();
        }

        private void SyncObjects_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing && base.MdiParent != null)
            {
                base.Hide();
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выполнить синхронизацию", "Внесенные в БД изменения нельзя отменить, все равно продолжить?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                int regid = Config.Get("CurrenRegion", "-1").ToInt();
                try
                {
                    if (comboBox1.SelectedItem != null)
                    {
                        regid = ((ComboboxItem)comboBox1.SelectedItem).Value.ToInt();
                    }
                }
                catch { }
                Class.Synchronization.SyncResult res = Synchronizer.SyncObjectWithIP(textBox1.Text, textBox2.Text, regid);
                MessageBox.Show(String.Format("Синхронизация с узлом {1} выполнена, обновлено: {0} объектов", res.Reserved, res.IpAddress), "Выполнено");
            }
        }
    }
}
