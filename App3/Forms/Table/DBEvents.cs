using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App3.Class;
using App3.Class.Singleton;

namespace App3.Forms
{
    public partial class DBEvents : Form
    {
        public DBEvents(Form pParentForm)
        {
            InitializeComponent();
            this.MdiParent = pParentForm;
            // таблица
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns.Add("objectnumber", "Объект");
            dataGridView1.Columns.Add("datetime", "Дата/Время");
            dataGridView1.Columns.Add("message", "Сообщение");
            dataGridView1.Columns.Add("address", "Адрес");
            dataGridView1.Columns.Add("district", "Район");
            dataGridView1.Columns[0].Width = 50;
            dataGridView1.Columns[1].Width = 120;
            dataGridView1.Columns[2].Width = 250;
            dataGridView1.Columns[3].Width = 100;
            dataGridView1.Columns[4].Width = 150;
        }

        private void UpdateColors()
        {
            try
            {

                for (int i = 0; i < dataGridView1.Rows.Count ; i++)
                {
                    if (!Utils.IsOdd(i))
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.LightBlue;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                MessageBox.Show(ex.Message);
            }
        }

        private void DBEvents_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 3;
            // 
            comboBox2.Items.Clear();
            foreach(object[] row in DataBase.RowSelect("select num, name from regions2map order by num"))
            {
                ComboboxItem item = new ComboboxItem();
                item.Value = row[0];
                item.Text = row[1].ToString();
                comboBox2.Items.Add(item);
            }
        }

        private void DBEvents_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        public void AddRow(string ObjectNumber, string DateTime, string Message, string Address)
        {
            Color c = Color.LightBlue;
            if (dataGridView1.Rows.Count > 0)
            {
                if (dataGridView1.Rows[0].DefaultCellStyle.BackColor == c)
                {
                    c = Color.White;
                }
            }
            dataGridView1.Rows.Insert(0, ObjectNumber, DateTime, Message, Address);
            dataGridView1.Rows[0].DefaultCellStyle.BackColor = c;
        }

        private void UpdateData(DateTime beginDate, DateTime endDate, int last = -1, int district_id = -1)
        {
            DataSet ds = new DataSet();
            string sql = String.Format("SELECT m.objectnumber, m.datetime, m.code, m.typenumber, m.class, ms.message, ip.ipaddress as address, rm.name as district " +
                                    "FROM oko.event m JOIN oko.message_text ms ON m.class = ms.class AND m.code = ms.code " +
                                    "LEFT JOIN regions2map rm on rm.num = m.region_id " +
                                    "LEFT JOIN oko.ipaddresses ip on ip.id_region = m.region_id " +
                                    "WHERE datetime BETWEEN '{0}' AND '{1}'", beginDate, endDate);
            if (district_id != -1)
            {
                sql += " AND ip.id_region = " + district_id.ToString();
            }
            sql += " ORDER BY datetime desc ";
            if (last != -1)
            {
                sql += "LIMIT " + last.ToString();
            }
            try
            {
                DataBase.RowSelect(sql, ds);
            } catch (Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                MessageBox.Show(ex.Message);
                return;
            }
            dataGridView1.Rows.Clear();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                dataGridView1.Rows.Add(
                        row["objectnumber"],
                        row["datetime"],
                        row["message"],
                        row["address"],
                        row["district"]
                    );
            }
            UpdateColors();

            string district = "";
            if (district_id >= 0)
            {
                district = Address.RegionById(district_id);
            }
            /*foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                DateTime s = DateTime.Parse(row.Cells["datetime"].Value.ToString());
                if (s > endDate || s < beginDate)
                {
                    row.Visible = false;
                    continue;
                }
                if (district_id >= 0 && district != row.Cells["district"].Value.ToString())
                {
                    row.Visible = false;
                    continue;
                }
                if (last == 0)
                {
                    row.Visible = false;
                    continue;
                }
                if (last > 0) last--;
                row.Visible = true;
            }*/
        }

        private void DBEvents_Enter(object sender, EventArgs e)
        {
            UpdateData(DateTime.Now.AddMonths(-1), DateTime.Now, comboBox1.Text.ToInt());
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int ObjCode = dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToInt();
            (new ObjectForm(this.MdiParent, ObjCode)).Show();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4.Checked)
            {
                comboBox2.Enabled = true;
            }
            else
            {
                comboBox2.Enabled = false;
            }
        }

        private void dataFilter(DateTime beginDate, DateTime endDate, int last = -1, int district_id = -1)
        {
            UpdateData(beginDate, endDate, last, district_id);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int last = -1;
            int district_id = -1;
            DateTime bd = DateTime.MinValue;
            DateTime ed = DateTime.MaxValue;
            if (checkBox1.Checked)
            {
                bd = begTimePicker.Value;
            }
            if (checkBox2.Checked)
            {
                ed = endTimePicker.Value;
            }
            if (radioButton2.Checked)
            {
                int.TryParse(comboBox1.Text, out last);
            }
            if (radioButton4.Checked)
            {
                int i = comboBox2.SelectedIndex;
                ComboboxItem item = (ComboboxItem)comboBox2.Items[i];
                district_id = item.Value.ToInt();
            }
            dataFilter(bd, ed, last, district_id);
        }

        private void radioButton2_CheckedChanged_1(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }
    }
}
