using App3.Class;
using App3.Class.Singleton;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class WarningForm : Form
    {
        private SoundPlayer alarmSound = new SoundPlayer(@"emergency004.wav");
        private int ColumnIdObject = 1;

        public WarningForm(Form Parent)
        {
            InitializeComponent();
            MdiParent = Parent;
            this.dataGridView1.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void WarningForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            alarmSound.Stop();
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            alarmSound.Stop();
        }

        private void AddRowThreadSafe(int pEventId, Int64 pObjId, int pNumber, string pDistrict, string pName, string pMessage, string pType, string pPhone, string pColor, string pTime)
        {
            dataGridView1.Rows.Insert(0, pEventId, pObjId, pNumber, pDistrict, pName, pMessage, pType, pPhone, pTime);
            try
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Rows[0].Height = 80;
                    dataGridView1.Rows[0].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(pColor);
                    dataGridView1.Rows[0].DefaultCellStyle.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(pColor));
                }
            }
            catch(Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        public void AddRow(int pEventId, Int64 pObjId, int pNumber, string pDistrict, string pName, string pMessage, string pType, string pPhone, string pColor, string pTime)
        {
            if (dataGridView1.InvokeRequired)
                dataGridView1.Invoke(new Action(() => { AddRowThreadSafe(pEventId, pObjId, pNumber, pDistrict, pName, pMessage, pType, pPhone, pColor, pTime); }));
            else
                AddRowThreadSafe(pEventId, pObjId, pNumber, pDistrict, pName, pMessage, pType, pPhone, pColor, pTime);
        }

        public void PlaySound()
        {
            alarmSound.PlayLooping();
        }

        public void StopSound()
        {
            alarmSound.Stop();
        }

        private void WarningForm_Load(object sender, EventArgs e)
        {
            dataGridView1.RowTemplate.Height = 80;
            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Arial", 12F, FontStyle.Bold, GraphicsUnit.Pixel);
            }
        }

        private void WarningForm_Shown(object sender, EventArgs e)
        {
            // timer1.Start();
        }

        private void обработаноToolStripMenuItem_Click(object sender, EventArgs e)
        {
            App3.Forms.Dialog.ComboDialog dlg = new Dialog.ComboDialog(
                "Выбор времени",
                "Выберите на какой период времени не реагировать на однотипные события от этого объекта",
                new ComboboxItem[] 
                {
                    new ComboboxItem("5 минут", 5 * 1000 * 60),
                    new ComboboxItem("10 минут", 10 * 1000 * 60),
                    new ComboboxItem("15 минут", 15 * 1000 * 60),
                    new ComboboxItem("30 минут", 30 * 1000 * 60),
                    new ComboboxItem("1 час", 60 * 1000 * 60),
                    new ComboboxItem("3 часа", 180 * 1000 * 60),
                    new ComboboxItem("6 часов", 360 * 1000 * 60),
                    new ComboboxItem("12 часов", 720 * 1000 * 60),
                    new ComboboxItem("1 день", 24 * 60 * 1000 * 60)
                }.ToList<ComboboxItem>(),
                2
            );
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                // MessageBox.Show(dlg.SelectedItem.Value.ToString());
                // Utils.FreezeObject()
            }
        }

        void OpenObjectForm(Int64 idObj)
        {
            // ObjectForm objFrm = new ObjectForm(this.MdiParent, idObj);
            // objFrm.Show();
            Handling.onObjectCardOpen(idObj);
        }

        private void кОбъектуToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 SelectedRowIdx = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            OpenObjectForm(dataGridView1.Rows[SelectedRowIdx].Cells[ColumnIdObject].Value.ToInt());
            dataGridView1.ClearSelection();
        }

        private void dataGridView1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dataGridView1.HitTest(e.X, e.Y);
                dataGridView1.ClearSelection();
                dataGridView1.Rows[hti.RowIndex].Selected = true;
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Int32 SelectedRowIdx = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            OpenObjectForm(dataGridView1.Rows[e.RowIndex].Cells[ColumnIdObject].Value.ToInt());
            dataGridView1.ClearSelection();
        }

        private int GetEventId()
        {
            return 0;
        }

        private void FixInTable(int i = 0)
        {
            if (dataGridView1.Rows.Count > i)
            {
                dataGridView1.Rows.RemoveAt(i);
            }
        }

        private int GetDataGridIndex()
        {
            Int32 SelectedRowIdx = dataGridView1.Rows.GetFirstRow(DataGridViewElementStates.Selected);
            if (SelectedRowIdx < 0) SelectedRowIdx = dataGridView1.Rows.Count - 1;
            return SelectedRowIdx;
        }

        /// <summary>
        /// Применить
        /// </summary>
        /// <param name="eventId"></param>
        private void AcceptAlert()
        {
            Int32 SelectedRowIdx = GetDataGridIndex();
            if (SelectedRowIdx >= 0)
            {
                Handling.AcceptAlarm(dataGridView1.Rows[SelectedRowIdx].Cells[ColumnIdObject].Value.ToInt());
                dataGridView1.ClearSelection();
                FixInTable(SelectedRowIdx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        private void CancelAlert()
        {
            Int32 SelectedRowIdx = GetDataGridIndex();
            if (SelectedRowIdx >= 0)
            {
                Handling.CancelAlarm(dataGridView1.Rows[SelectedRowIdx].Cells[ColumnIdObject].Value.ToInt());
                dataGridView1.ClearSelection();
                FixInTable(SelectedRowIdx);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eventId"></param>
        private void TestAlert()
        {
            Int32 SelectedRowIdx = GetDataGridIndex();
            if (SelectedRowIdx >= 0)
            {
                Handling.TestAlarm(dataGridView1.Rows[SelectedRowIdx].Cells[ColumnIdObject].Value.ToInt());
                dataGridView1.ClearSelection();
                FixInTable(SelectedRowIdx);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AcceptAlert();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            CancelAlert();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            TestAlert();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
        }

        
    }
}
