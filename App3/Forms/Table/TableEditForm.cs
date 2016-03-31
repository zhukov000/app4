using App3.Class;
using App3.Dialogs;
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

namespace App3.Forms
{
    public partial class TableEditForm : Form
    {
        public bool CanAdd
        {
            get { return dbTable1.CanAdd; }
            set { dbTable1.CanAdd = value; }
        }
        public bool CanEdit
        {
            get { return dbTable1.CanEdit; }
            set { dbTable1.CanEdit = value; }
        }
        public bool CanDel
        {
            get { return dbTable1.CanDel; }
            set { dbTable1.CanDel = value; }
        }

        public TableEditForm(Form pParent, string pTableName, string pHeader )
        {
            InitializeComponent();
            MdiParent = pParent;
            dbTable1.TableName = pTableName;
            Text = pHeader;
        }

        public TableEditForm(string pTableName, string pHeader)
        {
            InitializeComponent();
            dbTable1.TableName = pTableName;
            Text = pHeader;
        }

        public void AddForeign(string LocalField, string LinkTable, string LinkField, string ShowField)
        {
            dbTable1.AddForeign(LocalField, LinkTable, LinkField, ShowField);
        }

        private void TableEditForm_Load(object sender, EventArgs e)
        {

        }

        private void TableEditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbTable1.SaveChange();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            dbTable1.CancelChange();
            if (MessageBox.Show("Закончить редактоирование (несохраненные данные будут потеряны)?", "Вопрос", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                Close();
            }
        }
    }
}
