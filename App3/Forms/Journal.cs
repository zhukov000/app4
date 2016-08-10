using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using App3.Class;
using App3.Controls;

namespace App3.Forms
{
    public partial class Journal : Form
    {
        private GanttChart ganttChart1;
        

        public Journal(Form pParent)
        {
            InitializeComponent();
            this.MdiParent = pParent;

            DateTime dt = DateTime.Now;
            dateTimePicker1.Value = dt.AddDays(-7);
            dateTimePicker2.Value = dt;
        }

        private void Journal_Load(object sender, EventArgs e)
        {
            // ShowData();
        }

        public void ShowData()
        {
            object mind = DataBase.First("select min(start) mins from journal", "mins");
            if (mind == null)
                mind = DateTime.MinValue;

            ShowData((DateTime)mind, DateTime.Now);
        }

        public void ShowData(DateTime dt01, DateTime dt02)
        {
            panel3.Controls.Clear();

            ganttChart1 = new GanttChart();
            ganttChart1.BarHeight = 20;
            ganttChart1.AllowChange = false;
            ganttChart1.Dock = DockStyle.Fill;

            panel3.Controls.Add(ganttChart1);

            ganttChart1.FromDate = dt01;
            ganttChart1.ToDate = dt02;

            List<object []> rows = DataBase.RowSelect(
                string.Format(
                    "select start, COALESCE(finish, start) finish from journal where start < '{0}' and finish > '{1}' order by start",
                    dt02,
                    dt01
                )
            );

            dataGridView1.Rows.Clear();

            if ( rows.Count > 0 )
            {
                foreach (object[] row in rows)
                {
                    DateTime dt3 = Convert.ToDateTime(row[0]);
                    DateTime dt4 = Convert.ToDateTime(row[1]);

                    if (dt3 < dt01) dt3 = dt01;
                    if (dt4 > dt02) dt4 = dt02;

                    BarInformation bar = new BarInformation("Работа", dt3, dt4, Color.LawnGreen, Color.Khaki, 0);
                    ganttChart1.AddChartBar(bar.RowText, bar, bar.FromTime, bar.ToTime, bar.Color, bar.HoverColor, bar.Index);
                    dataGridView1.Rows.Add(dt3, dt4);
                }
            }
            else
            {
                MessageBox.Show("Данные о запусках не были полученны");
            }
        }

        private void Journal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowData(dateTimePicker1.Value, dateTimePicker2.Value);
        }

    }
}
