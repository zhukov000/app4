using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace loadobjects
{
    public partial class Form1 : Form
    {

        private int RowNumber = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog1 = new OpenFileDialog();
            dialog1.Filter = "Документ Excel|*.xlsx";
            if (dialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dialog1.FileName;
                FileStream stream = File.Open(textBox1.Text, FileMode.Open, FileAccess.Read);
                // IExcelDataReader excelReader = ExcelReaderFactory.CreateBinaryReader(stream); для 2003
                IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                DataSet result = excelReader.AsDataSet();
                dataGridView1.DataSource = result;
                dataGridView1.DataMember = result.Tables[0].TableName.ToString();
                excelReader.Close();
                remove_empty_rows();
            }
        }

        private void remove_empty_rows()
        {
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                int number = 0; 
                if ((row.Cells[RowNumber].Value == null) || (!int.TryParse(row.Cells[RowNumber].Value.ToString(), out number)) || (number == 0))
                {
                    row.Selected = true;
                }
            }
            foreach (DataGridViewRow item in this.dataGridView1.SelectedRows)
            {
                dataGridView1.Rows.RemoveAt(item.Index);
            }
        }

    }
}
