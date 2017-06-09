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

namespace App3.Forms.Dialog
{
    public partial class ReportDialog : Form
    {
        public ReportDialog()
        {
            InitializeComponent();
        }

        private void ReportDialog_Load(object sender, EventArgs e)
        {
        }

        public void ShowReport(string sql, string report)
        {
            DataSet myDS = new DataSet();
            DataBase.RowSelect(sql, myDS);
            DataTable dt = myDS.Tables[0];
            ReportDataSource DSReport = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(DSReport);
            reportViewer1.LocalReport.ReportEmbeddedResource = report;
            reportViewer1.RefreshReport();
        }

        public void ShowReport(DataTable dt, string report)
        {
            ReportDataSource DSReport = new ReportDataSource("DataSet1", dt);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(DSReport);
            reportViewer1.LocalReport.ReportEmbeddedResource = report;
            reportViewer1.RefreshReport();
        }
    }
}
