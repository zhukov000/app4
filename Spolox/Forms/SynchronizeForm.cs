using App3.Class.Static;
using App3.Class.Synchronization;
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

        public void WriteSyncStatus(List<SyncResult> list)
        {
            listBox1.Items.Clear();
            foreach (SyncResult current in list)
            {
                listBox1.Items.Add(string.Format("{6} IP-адрес: {0} Район: {5} Статус: {1} Получено байт: {2} Изменено: {3} Удалено: {4}", new object[]
                {
                    current.IpAddress,
                    current.Status,
                    current.Bytes,
                    current.Reserved,
                    current.Deleted,
                    current.Description,
                    current.Timestamp
                }));
            }
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
            if (Synchronizer.Status == Synchronizer.Status_Sync.SUSPEND)
            {
                if (MessageBox.Show("Выполнить синхронизацию с выбранными узлами?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.listBox1.Items.Clear();
                    this.listBox1.Items.Add("Синхронизация начата");
                    List<SyncResult> list = null;
                    List<string> arg_50_0 = Synchronizer.Run(ref list);
                    this.listBox1.BeginUpdate();
                    foreach (string current in arg_50_0)
                    {
                        this.listBox1.Items.Add(current);
                    }
                    this.listBox1.EndUpdate();
                }
            }
            else
            {
                MessageBox.Show("Синхронизация уже запущена (см. внизу главной формы). Подождите пока синхронизация завершится и повторите снова");
            }
        }

        private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            string s = listBox1.Items[e.Index].ToString();
            if (s.IndexOf("EMPTY") > 0)
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), listBox1.Font, Brushes.Red, e.Bounds);
            else
                e.Graphics.DrawString(listBox1.Items[e.Index].ToString(), listBox1.Font, Brushes.Black, e.Bounds);
        }
    }
}
