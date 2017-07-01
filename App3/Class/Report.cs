using App3.Class.Singleton;
using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace App3.Class
{
    class Report
    {
        private string reportName;
        private string sourceSql;
        public BindingSource source;

        public Report(string reportName, string sqlStr)
        {
            this.reportName = reportName;
            this.sourceSql = sqlStr;
        }

        public void showReport(ReportViewer rep)
        {
            rep.LocalReport.DataSources.Clear();
            rep.LocalReport.ReportEmbeddedResource = reportName;
            // rep.LocalReport.ReportPath = reportName;

            DataSet ds = new DataSet();
            DataBase.RowSelect(sourceSql, ds);

            if (ds.Tables.Count > 0)
            {
                BindingSource bds = new BindingSource();
                bds.DataSource = ds.Tables[0];
                rep.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", bds));

                List<ReportParameterInfo> listParams = rep.LocalReport.GetParameters().ToList();

                if (listParams != null && listParams.Count > 0)
                {
                    foreach (ReportParameterInfo p in listParams)
                    {
                        rep.LocalReport.SetParameters(new ReportParameter(p.Name, "0"));
                    }
                }
                this.source = bds;
                rep.RefreshReport();
            }
            else
            {
                Logger.Log(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, "Результаты запроса не позволяют построить отчет, так как данные не выбраны"), Logger.LogLevel.ERROR);
                Logger.Log(sourceSql, Logger.LogLevel.DEBUG);
            }
        }

        public DataTable getDetailReport(LocalReport rep)
        {
            DataTable dataTable;
            DataSet ds = new DataSet();
            DataBase.RowSelect(sourceSql, ds);

            if (ds.Tables.Count > 0)
            {
                dataTable = ds.Tables[0];
                BindingSource bds = new BindingSource();
                bds.DataSource = dataTable;
                this.source = bds;
                return dataTable;
            }
            else
            {
                Logger.Log(sourceSql, Logger.LogLevel.DEBUG);
                throw new Exception("Детализированный отчет не был построен: " + sourceSql);
            }
        }
    }
}
