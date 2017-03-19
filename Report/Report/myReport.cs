using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
using System.Data.Common;
using Microsoft.Reporting.WinForms;

namespace Report
{
    class myReport
    {
        public string reportName;
        public BindingSource source;
        public myReport(string reportName)
        {
            this.reportName = reportName;
        }

        public void showReport(string conn, string sqlStr, ReportViewer rep)
        {
            rep.LocalReport.DataSources.Clear();
            rep.LocalReport.ReportEmbeddedResource = reportName;

            string path = "D:/Visual Studio 2013/Projects/Report/Report/Reports/" + reportName;
            rep.LocalReport.ReportPath = path;

            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();

            NpgsqlCommand cmd = new NpgsqlCommand(sqlStr, connection);

            adapter.SelectCommand = cmd;

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            connection.Close();

            BindingSource bds = new BindingSource();
            bds.DataSource = dataTable;
            rep.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", bds));

            if (rep.LocalReport.GetParameters().Count != 0)
            {
                List<ReportParameterInfo> list = rep.LocalReport.GetParameters().ToList();
                foreach (ReportParameterInfo p in list)
                {
                    rep.LocalReport.SetParameters(new ReportParameter(p.Name, "111"));
                }
            }
            this.source = bds;
            rep.RefreshReport();
        }

        public DataTable getDetailReport(string conn, string sqlStr, LocalReport rep)
        {
            NpgsqlConnection connection = new NpgsqlConnection(conn);
            connection.Open();

            NpgsqlDataAdapter adapter = new NpgsqlDataAdapter();

            NpgsqlCommand cmd = new NpgsqlCommand(sqlStr, connection);

            adapter.SelectCommand = cmd;

            DataTable dataTable = new DataTable();
            adapter.Fill(dataTable);
            connection.Close();

            BindingSource bds = new BindingSource();
            bds.DataSource = dataTable;
            this.source = bds;

            return dataTable;
        }
    }
}
