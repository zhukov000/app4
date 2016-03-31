using App3.Class.Static;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class SynchronizeForm : Form
    {
        private Controls.DBTable dbTable1;

        public SynchronizeForm(Form pParent)
        {
            InitializeComponent();
            this.MdiParent = pParent;
            #region DBTable
            this.dbTable1 = new App3.Controls.DBTable();
            this.groupBox1.Controls.Add(this.dbTable1);
            // 
            // dbTable1
            // 
            this.dbTable1.CanAdd = true;
            this.dbTable1.CanDel = true;
            this.dbTable1.CanEdit = true;
            this.dbTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTable1.Filter = "";
            this.dbTable1.Location = new System.Drawing.Point(4, 19);
            this.dbTable1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dbTable1.Name = "dbTable1";
            this.dbTable1.Size = new System.Drawing.Size(418, 117);
            this.dbTable1.TabIndex = 0;
            this.dbTable1.TableName = "syn_nodes";
            #endregion
        }

        private void SynchronizeForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void SynchronizeForm_Load(object sender, EventArgs e)
        {

        }


        private void button1_Click(object sender, EventArgs e)
        {
            // syn_nodesTableAdapter.Update(gisDataSet.syn_nodes);
            dbTable1.SaveChange();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // syn_nodesTableAdapter.Fill(gisDataSet.syn_nodes);
            dbTable1.CancelChange();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Выполнить синхронизацию с выбранными узлами?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("Синхронизация начата");
                List<string> log = Synchronizer.Run();
                listBox1.BeginUpdate();
                foreach (string s in log)
                {
                    listBox1.Items.Add(s);
                }
                listBox1.EndUpdate();
            }
        }
    }
}
