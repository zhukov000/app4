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
            (new gisDataReportsTableAdapters.report_all_statusTableAdapter()).Fill(this.gisDataReports.report_all_status);
            LoadStat();
            // this.reportViewer1.RefreshReport();
            // LoadData();
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

        

        /*
        private void LoadData()
        {
            foreach(ComboboxItem obj in DBDict.TMinistry.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList())
            {
                listBox1.Items.Add(obj);
            }

            DBDict.Load2Combobox(ref comboBox1, DBDict.TRegion.Select(x => new ComboboxItem(x.Value.Item1, x.Key)).ToList(), null);

            DateTime dt = DateTime.Today;
            dtStart.Value =  dt.AddMonths(-1);
            dtFinish.Value = DateTime.Now;

            autoCompleteTextbox1.AutoCompleteMode = AutoCompleteMode.Suggest;
            autoCompleteTextbox1.AutoCompleteSource = AutoCompleteSource.CustomSource;
            autoCompleteTextbox1.AutoCompleteCustomSource = DataObjects();
            if (autoCompleteTextbox1.AutoCompleteCustomSource.Count == 1)
            {
                autoCompleteTextbox1.Text = autoCompleteTextbox1.AutoCompleteCustomSource[0];
            }
        }
         * */

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
            
            // reportViewer1.SetPageSettings(pg);
            
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // string RepPath = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            //reportViewer1.LocalReport.ReportPath = RepPath;
            //reportViewer1.SetDisplayMode(DisplayMode.PrintLayout);
            // this.report_all_statusBindingSource;
            //gisDataReportsTableAdapters.report_all_statusTableAdapter o = new gisDataReportsTableAdapters.report_all_statusTableAdapter();
            //o.Fill(this.gisDataReports.);
/*            reportViewer1.LocalReport.ReportEmbeddedResource = RepPath;
            if (RepPath == "App3.Reports.Report2.rdlc")
            {
                (new gisDataReportsTableAdapters.report_all_statusTableAdapter()).Fill(this.gisDataReports.report_all_status);
            }
            else
            {
                (new gisDataReportsTableAdapters.report_all_regionsTableAdapter()).Fill(this.gisDataReports.report_all_regions);
            }
            reportViewer1.RefreshReport();
  */           
            // MessageBox.Show(RepPath);
            // reportViewer1
        }

        /*
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                listBox1.Enabled = true;
            }
            else
            {
                listBox1.Enabled = false;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                autoCompleteTextbox1.Enabled = true;
            }
            else
            {
                autoCompleteTextbox1.Enabled = false;
            }
        }

        private void regionCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (regionCheck.Checked)
            {
                comboBox1.Enabled = true;
            }
            else
            {
                comboBox1.Enabled = false;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            ComboboxItem i = (ComboboxItem) comboBox1.SelectedItem;
            autoCompleteTextbox1.AutoCompleteCustomSource = DataObjects(i.Value.ToInt());
        }
        */

        private void button1_Click(object sender, EventArgs e)
        {
            ShowReport();
        }

        private void ShowReport()
        {
            /*string sql = "SELECT * FROM "

            if (region)
            {

            }
            ComboboxItem i = (ComboboxItem)comboBox1.SelectedItem;
            i.Value
            string s = autoCompleteTextbox1.Text;
            var l = s.Split((new string[] {"ID="}), 1, StringSplitOptions.None);
            l[1].ToInt()
            (new ReportForm(dtStart.Value, dtFinish.Value, object_id, region_id)).Show();
             * */
            (new ReportForm()).Show();
        }
    }
}
