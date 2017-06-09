using App3.Controls;
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
    public partial class Journal : Form
    {
        public Journal(Form pParent)
        {
            this.InitializeComponent();
            base.MdiParent = pParent;
            DateTime now = DateTime.Now;
            this.dateTimePicker1.Value = now.AddDays(-7.0);
            this.dateTimePicker2.Value = now;
        }

        public void ShowData()
        {
            object obj = DataBase.First("select min(start) mins from journal", "mins");
            if (obj == null)
            {
                obj = DateTime.MinValue;
            }
            this.ShowData((DateTime)obj, DateTime.Now);
        }

        public void ShowData(DateTime dt01, DateTime dt02)
        {
            this.panel3.Controls.Clear();
            this.ganttChart1 = new GanttChart();
            this.ganttChart1.BarHeight = 20;
            this.ganttChart1.AllowChange = false;
            this.ganttChart1.Dock = DockStyle.Fill;
            this.panel3.Controls.Add(this.ganttChart1);
            this.ganttChart1.FromDate = dt01;
            this.ganttChart1.ToDate = dt02;
            List<object[]> list = DataBase.RowSelect(string.Format("select start, COALESCE(finish, start) finish from journal where start < '{0}' and finish > '{1}' order by start", dt02, dt01));
            this.dataGridView1.Rows.Clear();
            if (list.Count > 0)
            {
                using (List<object[]>.Enumerator enumerator = list.GetEnumerator())
                {
                    while (enumerator.MoveNext())
                    {
                        object[] expr_B9 = enumerator.Current;
                        DateTime dateTime = Convert.ToDateTime(expr_B9[0]);
                        DateTime dateTime2 = Convert.ToDateTime(expr_B9[1]);
                        if (dateTime < dt01)
                        {
                            dateTime = dt01;
                        }
                        if (dateTime2 > dt02)
                        {
                            dateTime2 = dt02;
                        }
                        BarInformation barInformation = new BarInformation("Работа", dateTime, dateTime2, Color.LawnGreen, Color.Khaki, 0);
                        this.ganttChart1.AddChartBar(barInformation.RowText, barInformation, barInformation.FromTime, barInformation.ToTime, barInformation.Color, barInformation.HoverColor, barInformation.Index);
                        this.dataGridView1.Rows.Add(new object[]
                        {
                            dateTime,
                            dateTime2
                        });
                    }
                    return;
                }
            }
            MessageBox.Show("Данные о запусках не были полученны");
        }

        private void Journal_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                base.Hide();
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.ShowData(this.dateTimePicker1.Value, this.dateTimePicker2.Value);
        }

    }
}
