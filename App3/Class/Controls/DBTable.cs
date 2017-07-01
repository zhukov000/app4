using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using App3.Class;
using Npgsql;
using App3.Class.Singleton;
using App3.Dialog;

namespace App3.Controls
{
    public partial class DBTable : UserControl
    {
        const string suffix = "VAL";

        private NpgsqlConnection conn = null;
        private List<ForeignKey> foreigns = new List<ForeignKey>();
        private List<KeyValuePair<string, string>> hiddens = new List<KeyValuePair<string, string>>();
        private List<string> onlyHiddens = new List<string>();
        private BindingSource bindingSource1 = new BindingSource();
        private NpgsqlDataAdapter daResult = new NpgsqlDataAdapter();
        private string filter = "";
        private int[] sizes;

        public delegate void TableDoubleClickDelegate(object sender, DataGridViewCellEventArgs e);
        public event TableDoubleClickDelegate TableDoubleClick = null;

        public string ColumnName(int Index)
        {
            if (Index <= dataGridView1.Columns.Count)
                return dataGridView1.Columns[Index].Name;
            else
                return "";
        }

        public DataGridViewRowCollection Rows
        {
            get { return dataGridView1.Rows; }
        }

        public int RowCount
        {
            get
            {
                return dataGridView1.Rows.Count;
            }
        }
        public string Filter
        {
            get { return filter; }
            set 
            {
                filter = value;
                UpdateData();
            }
        }
        public string TableName
        {
            get;
            set;
        }
        public bool CanAdd
        {
            get { return dataGridView1.AllowUserToAddRows; }
            set
            {
                dataGridView1.AllowUserToAddRows = value;
            }
        }
        public bool CanEdit
        {
            get { return !dataGridView1.ReadOnly; }
            set { dataGridView1.ReadOnly = !value; }
        }
        public bool CanDel
        {
            get { return dataGridView1.AllowUserToDeleteRows; }
            set 
            { 
                dataGridView1.AllowUserToDeleteRows = value; 
                if (value)
                {
                    dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
                }
            }
        }


        public object Value(int RowIndex, int ColIndex)
        {
            object res = null;
            try
            {
                res = dataGridView1.Rows[RowIndex].Cells[ColIndex].Value;
            }
            catch(Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            return res;
        }

        public object Value(int RowIndex, string ColIndex)
        {
            object res = null;
            try
            {
                res = dataGridView1.Rows[RowIndex].Cells[ColIndex].Value;
            }
            catch (Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            return res;
        }

        protected override void OnControlAdded(ControlEventArgs e)
        {
            e.Control.DoubleClick += Control_DoubleClick;
            base.OnControlAdded(e);
        }

        void Control_DoubleClick(object sender, EventArgs e)
        {
            /*
            var hti = dataGridView1.HitTest(((MouseEventArgs)e).X, ((MouseEventArgs)e).Y);
            dataGridView1.ClearSelection();
            dataGridView1_CellDoubleClick(sender, new DataGridViewCellEventArgs(hti.ColumnIndex, hti.RowIndex) ); */
            OnDoubleClick(e);
        }


        public void Sorting(string name)
        {
            dataGridView1.Sort(dataGridView1.Columns[name], ListSortDirection.Ascending);
        }

        public void AddHidden(string HiddenField, string HiddenValue = null)
        {
            bool f = true;
            foreach (KeyValuePair<string, string> pair in hiddens)
            {
                if (HiddenField == pair.Key)
                {
                    f = false;
                    break;
                }
            }
            if (f) hiddens.Add(new KeyValuePair<string, string>(HiddenField, HiddenValue));
        }

        public void AddHidden2(string [] HiddenFields)
        {
            foreach(string s in HiddenFields)
            {
                AddHidden2(s);
            }
        }

        public void AddHidden2(string HiddenField)
        {
            bool f = true;
            foreach (string s in onlyHiddens)
            {
                if (HiddenField == s)
                {
                    f = false;
                    break;
                }
            }
            if (f) onlyHiddens.Add(HiddenField);
        }

        public void AddForeign(string LocalField, string LinkTable, string LinkField, string ShowField)
        {
            bool f = true;
            foreach (ForeignKey fk in foreigns)
            {
                if (LocalField == fk.FieldName)
                {
                    f = false;
                    break;
                }
            }
            if (f) foreigns.Add(new ForeignKey(LocalField, LinkTable, LinkField, ShowField));
        }

        private void ConnectionOpen()
        {
            if (conn == null)
            {
                try
                {
                    // для каждого контрола свое соединение
                    // conn = new NpgsqlConnection(DataBase.ConnectionString); // DataBase.GetConnection();
                    conn = DataBase.GetConnection();
                    // conn.Open();

                }
                catch (Exception ex)
                {
                    Logger.Log(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void DBTable_Load(object sender, EventArgs e)
        {
            CloseConnection();
            ConnectionOpen();
            UpdateData();
            
        }

        public void SetColumnSize(int [] psizes)
        {
            sizes = new int[psizes.Length];
            System.Array.Copy(psizes, sizes, psizes.Length);
        }

        private void GetData(string selectCommand)
        {
            try
            {
                NpgsqlCommand command = new NpgsqlCommand(selectCommand, conn);
                ConnectionOpen();
                daResult = new NpgsqlDataAdapter(command);
                NpgsqlCommandBuilder cb = new NpgsqlCommandBuilder(daResult);

                DataTable dt = new DataTable();
                dt.Locale = System.Globalization.CultureInfo.InvariantCulture;

                daResult.Fill(dt);
                bindingSource1.DataSource = dt;

            }
            catch(Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                MessageBox.Show(ex.Message);
            }
        }

        public void SaveChange()
        {
            if (bindingSource1.DataSource != null)
            {
                NpgsqlCommandBuilder npgsqlCommandBuilder = new NpgsqlCommandBuilder(this.daResult);
                npgsqlCommandBuilder.SetAllValues = false;
                this.daResult.InsertCommand = npgsqlCommandBuilder.GetInsertCommand();
                this.daResult.UpdateCommand = npgsqlCommandBuilder.GetUpdateCommand();
                this.daResult.DeleteCommand = npgsqlCommandBuilder.GetDeleteCommand();
                daResult.Update((DataTable)bindingSource1.DataSource);
            }
        }

        public void CancelChange()
        {
            GetData(daResult.SelectCommand.CommandText);
        }

        public void UpdateData()
        {
            dataGridView1.DataSource = bindingSource1;
            if (TableName == null)
                return;
            string req = "select * from " + TableName;
            if (filter.Trim().Length > 0)
            {
                req += " where " + filter;
            }
            GetData(req);
            //
            if (foreigns.Count() > 0)
            {
                // внешние ключи
                foreach (ForeignKey k in foreigns)
                {
                    string LocalField = k.FieldName;
                    dataGridView1.Columns[LocalField].Visible = false;
                    try 
                    {
                        if (dataGridView1.Columns.IndexOf(dataGridView1.Columns[LocalField + suffix]) < 0)
                        {
                            dataGridView1.Columns.Add(LocalField + suffix, LocalField + " значение");
                        }
                    }
                    catch(Exception ex)
                    {
                        Logger.Log(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                    }
                    
                }
            }
            if (dataGridView1.Columns.Count > 0)
            {
                // скрытые поля со значениями
                foreach (KeyValuePair<string, string> pair in hiddens)
                {
                    dataGridView1.Columns[pair.Key].Visible = false;
                }
                // скрытые поля без значений
                foreach (string s in onlyHiddens)
                {
                    dataGridView1.Columns[s].Visible = false;
                }
            }
        }

        public DBTable()
        {
            InitializeComponent();
        }

        public void HideColumn(string Name)
        {
            hiddens.Add(new KeyValuePair<string, string>(Name, null));
        }

        private void DBTable_Dispose(object sender, System.EventArgs e)
        {
            // DataBase.FreeConn(conn);
            CloseConnection();
        }

        private void dataGridView1_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            int rowidx = e.RowIndex;
            if (CanAdd)
            {
                rowidx--;
            }
            if (foreigns.Count() > 0 && rowidx >= 0 && rowidx < dataGridView1.Rows.Count)
            {
                foreach (ForeignKey k in foreigns)
                {
                    string LocalField = k.FieldName;
                    string s = dataGridView1.Rows[rowidx].Cells[LocalField].Value.ToString();
                    if (s != "")
                    {
                        dataGridView1.Rows[rowidx].Cells[LocalField + suffix].Value =
                            k.Val(s);
                    }
                }
                foreach (KeyValuePair<string, string> pair in hiddens)
                {
                    if (pair.Value != null)
                        dataGridView1.Rows[rowidx].Cells[pair.Key].Value = pair.Value;
                }
            }
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            string columnName = dataGridView1.Columns[e.ColumnIndex].Name;
            bool b = false;
            ForeignKey key = null;
            foreach (ForeignKey k in foreigns)
            {
                if (k.FieldName + suffix == columnName)
                {
                    b = true;
                    key = k;
                    break;
                }
            }
            //
            if (b)
            {
                TableDialog dialIpRegions = new TableDialog(
                    string.Format("({0}) src", key.SourceRequest),
                    "Выберите из справочника",
                    key.RefFieldName
                );
                if (dialIpRegions.ShowDialog() == DialogResult.OK)
                {
                    dataGridView1.Rows[e.RowIndex]
                        .Cells[columnName.Substring(0, columnName.Length - suffix.Length)]
                        .Value = dialIpRegions.Value;
                    string s = dialIpRegions.Value;
                    if (s != "")
                    {
                        dataGridView1.Rows[e.RowIndex].Cells[columnName].Value =
                            key.Val(s);
                    }
                }
                e.Cancel = true;
            }
        }


        internal void HideColumns(string[] p)
        {
            foreach(string s in p)
            {
                HideColumn(s);
            }
        }

        private void DBTable_Paint(object sender, PaintEventArgs e)
        {
            foreach (KeyValuePair<string, string> pair in hiddens)
            {
                dataGridView1.Columns[pair.Key].Visible = false;
            }
            foreach (string s in onlyHiddens)
            {
                dataGridView1.Columns[s].Visible = false;
            }
            if (sizes != null && dataGridView1.Columns != null)
            {
                for (int i = 0; i < sizes.Count() && i < dataGridView1.Columns.Count; i++ )
                {
                    dataGridView1.Columns[i].Width = sizes[i];
                }
            }
        }

        private void удалитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Int32 selectedCellCount = dataGridView1.GetCellCount(DataGridViewElementStates.Selected);
            if (selectedCellCount > 0)
            {
                HashSet<int> rows = new HashSet<int>();
                string s = "";
                for (int i = 0; i < selectedCellCount; i++)
                {
                    rows.Add(dataGridView1.SelectedCells[i].RowIndex);
                }
                s = String.Join(", ", rows.ToArray<int>());
                if (rows.Count == 1)
                {
                    s = "Вы желаете удалить строку " + s;
                }
                else
                {
                    s = "Вы желаете удалить строки " + s;
                }
                if(MessageBox.Show(s,"Вопрос",MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    int[] a = rows.ToArray<int>();
                    System.Array.Sort(a, (x, y) => -x.CompareTo(y));

                    foreach(int i in a)
                    {
                        dataGridView1.Rows.RemoveAt(i);
                    }
                }
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (TableDoubleClick != null)
            {
                TableDoubleClick(sender, e);
            }
        }

        private void CloseConnection()
        {
            try
            {
                DataBase.FreeConn(conn);
                // this.conn.Close();
            }
            catch(Exception ex)
            {
                Logger.Log(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
        }

        private void DBTable_ControlRemoved(object sender, ControlEventArgs e)
        {
            CloseConnection();
        }

    }
}
