using Microsoft.Reporting.WinForms;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Report
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string conStr = "Server=127.0.0.1;Port=5432;User Id=postgres;Password=postgres;Database=test;";

        private void Form1_Load(object sender, EventArgs e)
        {
            getViewList(conStr);

            this.reportViewer1.RefreshReport();
        }

        // Save To PDF
        private void button1_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/1/Desktop/EXPORT/";
            int num = new DirectoryInfo(path).GetFiles().Length;
            if (num == 0)
            {
                reportViewer1.ExportDialog(reportViewer1.LocalReport.ListRenderingExtensions()[3], null, path + "rep.pdf");
            }
            else
            {
                reportViewer1.ExportDialog(reportViewer1.LocalReport.ListRenderingExtensions()[3], null, path + "rep" + num + ".pdf");
            }
        }

        // Save to JPG
        private void button2_Click(object sender, EventArgs e)
        {
            string path = "C:/Users/1/Desktop/EXPORT/IMG/";
            int num = new DirectoryInfo(path).GetFiles().Length;
            if (num == 0)
            {
                reportViewer1.ExportDialog(reportViewer1.LocalReport.ListRenderingExtensions()[2], null, path + "repIMG.jpg");
            }
            else
            {
                reportViewer1.ExportDialog(reportViewer1.LocalReport.ListRenderingExtensions()[2], null, path + "repIMG" + num + ".jpg");
            }
        }

        // Show data in dataGridView and reportViewer
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string sqlStr = "SELECT * FROM " + comboBox1.SelectedItem.ToString();
            string reportName = comboBox1.SelectedItem.ToString() + ".rdlc";
            
            myReport myRep = new myReport(reportName);
            myRep.showReport(conStr, sqlStr, reportViewer1);

            /*dataGridView1.DataSource = myRep.source;
            dataGridView1.Update();*/
        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {
            configureExportbtn();
        }

        private void reportViewer1_Drillthrough(object sender, Microsoft.Reporting.WinForms.DrillthroughEventArgs e)
        {
            List<ReportParameterInfo> param = e.Report.GetParameters().ToList();

            string reportName = e.ReportPath + ".rdlc";
            string sqlStr = "SELECT * FROM task_and_answers " +
                            "WHERE task = '" + param[0].Values[0] + "'";
            myReport myRep = new myReport(reportName);

            LocalReport localRep = (LocalReport)e.Report;
            localRep.DataSources.Add(new ReportDataSource("DataSet1",
                myRep.getDetailReport(conStr, sqlStr, localRep)));

            /*dataGridView1.DataSource = myRep.source;
            dataGridView1.Update();   */         
        }

        // Show export to pdf and jpg
        private void configureExportbtn()
        {
            string[] exportOptions = { "EXCELOPENXML", "WORDOPENXML" };
            string exportOption = "IMAGE";
            Microsoft.Reporting.WinForms.RenderingExtension extension;
            foreach (string s in exportOptions)
            {
                extension =
                    reportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(s, StringComparison.CurrentCultureIgnoreCase));
                if (extension != null)
                {
                    System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField(
                        "m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    fieldInfo.SetValue(extension, false);
                }
            }
            extension =
                    reportViewer1.LocalReport.ListRenderingExtensions().ToList().Find(x => x.Name.Equals(exportOption, StringComparison.CurrentCultureIgnoreCase));
            if (extension != null)
            {
                System.Reflection.FieldInfo fieldInfo = extension.GetType().GetField(
                    "m_isVisible", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                fieldInfo.SetValue(extension, true);
            }
        }

        // Get a list of views
        private void getViewList(string conn)
        {
            comboBox1.Items.Clear();

            NpgsqlConnection con = new NpgsqlConnection(conn);
            con.Open();

            //список всех view
            string cmd = "SELECT * FROM pg_views " +
                         "WHERE viewowner = 'post'";
            NpgsqlCommand comm = new NpgsqlCommand(cmd, con);
            NpgsqlDataReader dReader = comm.ExecuteReader();
            if (dReader.HasRows)
            {
                foreach (DbDataRecord rec in dReader)
                {
                    comboBox1.Items.Add(rec["viewname"].ToString());
                }
            }
            dReader.Close();
            con.Close();
        }
    }
}
