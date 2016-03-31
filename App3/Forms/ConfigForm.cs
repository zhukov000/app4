using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Specialized;
//using System.Collections;
using App3.Class;
using App3.Class.Static;

namespace App3.Forms
{
    public partial class ConfigForm : Form
    {
        public ConfigForm(Form pParentForm)
        {
            InitializeComponent();
            MdiParent = pParentForm;
            CancelButton = button2;
        }

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfigForm_Enter(object sender, EventArgs e)
        {
            comboBox1.Items.Clear();
            foreach (string key in Config.Aliases())
            {
                comboBox1.Items.Add(key);
            }
            comboBox1.SelectedIndex = 0;

            dbTable1.HideColumns(new string[] { "id", "name", "color", "instat" });

            stateTable.HideColumns(new string[] { "id", "name", "color", "inprocess", "warn", "music" });
            statusTable.HideColumns(new string[] { "id"});

            stateTable.CanEdit = true;
            statusTable.CanEdit = true;

            dbTable2.SetColumnSize(new int[] { 50, 230 });
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBox1.Text = Config.GetByAlias(comboBox1.SelectedItem.ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Config.SetByAlias(comboBox1.SelectedItem.ToString(), textBox1.Text);
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            ShowStates();
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, Color.White);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.FromArgb(0, Color.Black);
            dataGridView1.ClearSelection(); 
            //
            ShowRegStat();

            checkBox1.Checked = DBDict.Settings["START_XML_GUARD"].ToBool();
        }

        private void ShowRegStat()
        {
            Queue<object[]> list = new Queue<object[]>( Utils.RegionStatus() );
            object[] obj = list.Dequeue();
            panel3.BackColor = ColorTranslator.FromHtml(obj[0].ToString());
            numericUpDown1.Value = obj[1].ToInt();
            obj = list.Dequeue();
            panel4.BackColor = ColorTranslator.FromHtml(obj[0].ToString());
            numericUpDown2.Value = obj[1].ToInt();
            obj = list.Dequeue();
            panel5.BackColor = ColorTranslator.FromHtml(obj[0].ToString());
            obj = list.Dequeue();
            panel8.BackColor = ColorTranslator.FromHtml(obj[0].ToString());
        }

        private void ShowStates()
        {
            List<ObjectState> list = DBDict.TState.Select(x => x.Value).ToList(); 
            dataGridView1.Rows.Clear();
            foreach(var obj in list)
            {
                int idx = dataGridView1.Rows.Add();
                DataGridViewRow row = dataGridView1.Rows[idx];
                // DataGridViewRow row = dataGridView1.Rows[dataGridView1.Rows.GetLastRow(DataGridViewElementStates.None)];
                row.ReadOnly = true;
                row.Cells[0].Value = obj.Status;
                row.Cells[1].Value = obj.Name;
                row.DefaultCellStyle.BackColor = ColorTranslator.FromHtml(obj.Color);
                row.DefaultCellStyle.ForeColor = Utils.FontColor(row.DefaultCellStyle.BackColor);
            }
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int i = e.RowIndex;
            ColorDialog cDilog = new ColorDialog();
            cDilog.Color = dataGridView1.Rows[i].DefaultCellStyle.BackColor;

            if (cDilog.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows[i].DefaultCellStyle.BackColor = cDilog.Color;
                dataGridView1.ClearSelection();
                int id = dataGridView1.Rows[i].Cells[1].Value.ToInt();
                string s = ColorTranslator.ToHtml(cDilog.Color); //.Substring(1);
                DataBase.RunCommand(
                    string.Format("update oko.tstate set color = '{0}' where id = {1}", s, id)
                );
                DBDict.Update();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= numericUpDown2.Value )
            {
                numericUpDown2.Value = numericUpDown1.Value - 1;
            }
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (numericUpDown1.Value <= numericUpDown2.Value)
            {
                numericUpDown1.Value = numericUpDown2.Value + 1;
            }
        }

        private void colorPanel_DoubleClick(object sender, EventArgs e)
        {
            ColorDialog cDilog = new ColorDialog();
            cDilog.Color = ((Panel)sender).BackColor;
            if (cDilog.ShowDialog() == DialogResult.OK)
            {
                ((Panel)sender).BackColor = cDilog.Color;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var wd = Utils.CreateWaitThread(this);
            DataBase.RunCommand(
                    string.Format("UPDATE oko.region_status SET color = '{0}', min_norma = {1}, max_norma = {2} WHERE id = 1",
                        ColorTranslator.ToHtml(panel3.BackColor), numericUpDown1.Value, 100
                    )
                );
            DataBase.RunCommand(
                    string.Format("UPDATE oko.region_status SET color = '{0}', min_norma = {1}, max_norma = {2} WHERE id = 2",
                        ColorTranslator.ToHtml(panel4.BackColor), numericUpDown2.Value, numericUpDown1.Value-1
                    )
                );
            DataBase.RunCommand(
                    string.Format("UPDATE oko.region_status SET color = '{0}', min_norma = {1}, max_norma = {2} WHERE id = 3",
                        ColorTranslator.ToHtml(panel5.BackColor), 1, numericUpDown2.Value - 1
                    )
                );
            DataBase.RunCommand(
                    string.Format("UPDATE oko.region_status SET color = '{0}' WHERE id = 4",
                        ColorTranslator.ToHtml(panel8.BackColor)
                    )
                );
            Utils.UpdateRegionStatus();
            ((MainForm)MdiParent).DistrictMapRefresh();
            Utils.DestroyWaitThread(wd);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dbTable1.SaveChange();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dbTable1.CancelChange();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            stateTable.SaveChange();
            statusTable.SaveChange();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            stateTable.CancelChange();
            statusTable.CancelChange();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            dbTable2.CancelChange();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            var wd = Utils.CreateWaitThread(this);
            dbTable2.SaveChange();
            Utils.UpdateRegionStatus();
            ((MainForm)MdiParent).DistrictMapRefresh();
            Utils.DestroyWaitThread(wd);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            DataBase.RunCommand(
                    string.Format("UPDATE settings SET value = '{0}' WHERE name = 'START_XML_GUARD'",
                        checkBox1.Checked.ToString()
                    )
                );
            DBDict.Settings["START_XML_GUARD"] = checkBox1.Checked.ToString();
        }


    }
}
