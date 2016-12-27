using App3.Class;
using App3.Class.Static;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace App3.Forms
{
    public partial class GlobalStat : Form
    {
        public GlobalStat()
        {
            InitializeComponent();
        }

        void UpdData()
        {
            
        }

        private void GlobalStat_Load(object sender, EventArgs e)
        {
            try
            {
                (new gisDataReportsTableAdapters.report_all_statusTableAdapter()).Fill(this.gisDataReports.report_all_status);
                LoadStat();
            }
            catch(Exception ex)
            {
                Class.Singleton.Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
            }
        }

        private void BuildDataGridThreadSafe(List<object[]> data)
        {
            dataGridView1.Rows.Clear();
            int sum = 0;
            foreach (object[] row in data)
            {
                int idx = dataGridView1.Rows.Add(row[0], row[2]);
                sum += row[2].ToInt();
                dataGridView1.Rows[idx].DefaultCellStyle.BackColor = ColorTranslator.FromHtml(row[1].ToString());
                dataGridView1.Rows[idx].DefaultCellStyle.ForeColor = Utils.FontColor(ColorTranslator.FromHtml(row[1].ToString()));
            }
            dataGridView1.Rows.Add("Всего:", sum);
        }

        private void BuildChartThreadSafe(List<object[]> data)
        {
            chart1.Series.Clear();
            var seriesColumns = new Series("RandomColumns");

            seriesColumns.ChartType = SeriesChartType.StackedColumn;
            chart1.Series.Add(seriesColumns);

            foreach (object[] row in data)
            {
                seriesColumns.Points.Add(row[2].ToDouble()).Color = ColorTranslator.FromHtml(row[1].ToString());
            }
        }

        public void LoadStat()
        {
            List<object[]>[] stats = Utils.CommonStatistic();
            List<object[]> s = stats[0].Union(stats[1]).ToList();
            if (stats != null)
            {
                if (dataGridView1.InvokeRequired)
                    dataGridView1.BeginInvoke(new Action(() => { BuildDataGridThreadSafe(s); }));
                else
                    BuildDataGridThreadSafe(s);

                if (chart1.InvokeRequired)
                    chart1.BeginInvoke(new Action(() => { BuildChartThreadSafe(s); }));
                else
                    BuildChartThreadSafe(s);
            }
        }

        private void GlobalStat_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            System.Drawing.Printing.PageSettings pg = new System.Drawing.Printing.PageSettings();
            pg.Margins.Top = 0;
            pg.Margins.Bottom = 0;
            pg.Margins.Left = 0;
            pg.Margins.Right = 0;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
            (new ReportForm()).Show();
        }
    }
}
