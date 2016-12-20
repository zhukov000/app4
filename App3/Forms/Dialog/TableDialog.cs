using App3.Class.Singleton;
using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace App3.Dialog
{
    public partial class TableDialog : Form
    {
        private String TableName = "";
        private String KeyField = "";
        private NpgsqlConnection conn = null;

        public String Value 
        {
            get 
            {
                string res = "";
                foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                {
                    res = row.Cells[KeyField].Value.ToString();
                    break;
                }
                return res;
            }
        }

        public TableDialog(string pTableName, string pHeader, string pKeyField)
        {
            InitializeComponent();
            TableName = pTableName;
            Text = "Выберите один из вариантов";
            label1.Text = pHeader;
            KeyField = pKeyField;
        }

        private void TableDialog_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(DataBase.ConnectionString); // DataBase.GetConnection();
            try
            {
                NpgsqlCommand command = new NpgsqlCommand("select * from " + TableName, conn);
                NpgsqlDataAdapter daResult = new NpgsqlDataAdapter(command);
                NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(daResult);
                DataTable dt = new DataTable();
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;
                daResult.Fill(dt);
                dataGridView1.DataSource = dt;
                dataGridView1.Rows[0].Selected = true;
            }
            catch(Exception ex)
            {
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                MessageBox.Show(ex.Message);
            }
        }

        private void TableDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataBase.FreeConn(conn);
        }

    }
}
