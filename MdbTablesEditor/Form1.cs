using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MdbTablesEditor
{
    public partial class Form1 : Form
    {
        private string ConnectionsStrMask = @"Dbq={0};Uid=Admin;Pwd=;";
        private BindingSource bindingSource1 = new BindingSource();
        private OdbcDataAdapter daResult = new OdbcDataAdapter();
        private OdbcConnection myConnection;

        public Form1()
        {
            InitializeComponent();
        }

        private void GetData(string selectCommand)
        {
            try
            {
                OdbcCommand command = new OdbcCommand(selectCommand, myConnection);
                daResult = new OdbcDataAdapter(command);
                OdbcCommandBuilder cb = new OdbcCommandBuilder(daResult);

                DataTable dt = new DataTable();
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;

                daResult.Fill(dt);
                bindingSource1.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            try { myConnection.Close(); }
            catch { }
            OpenFileDialog dial1 = new OpenFileDialog();
            dial1.Filter = "Access 2003|*.mdb|Access 2007|*.accdb;";
            dial1.InitialDirectory = @"C:\Users\user\Documents\Visual Studio 2013\Projects\OKO\App3\bin\Debug";
            if (dial1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dial1.FileName;
                myConnection = new OdbcConnection("Driver={Microsoft Access Driver (*.mdb)};" + string.Format(ConnectionsStrMask, dial1.FileName));
                // получить список таблиц и загрузить их в комбо
                List<string> list = GetTables(dial1.FileName);
                foreach (string s in list)
                {
                    comboBox1.Items.Add(s);
                }
                myConnection.Open();
            }
        }

        private List<string> GetTables(string FileName)
        {
            // Microsoft Access provider factory
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.OleDb");

            DataTable userTables = null;
            using (DbConnection connection = factory.CreateConnection())
            {
                // c:\test\test.mdb
                connection.ConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + FileName;
                // We only want user tables, not system tables
                string[] restrictions = new string[4];
                restrictions[3] = "Table";

                connection.Open();

                // Get list of user tables
                userTables = connection.GetSchema("Tables", restrictions);
            }

            List<string> tableNames = new List<string>();
            for (int i = 0; i < userTables.Rows.Count; i++)
                tableNames.Add(userTables.Rows[i][2].ToString());
            return tableNames;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                dataGridView1.DataSource = bindingSource1;
                GetData("select * from " + comboBox1.SelectedItem.ToString());
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            daResult.Update((DataTable)bindingSource1.DataSource);
        }
    }
}
