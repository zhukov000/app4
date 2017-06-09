using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Collections;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using App3.Forms;
using App3.Class;
using App3.Class.Singleton;
using App3.Class.Static;
using System.Reflection;

namespace App3
{
    public partial class MessForm : Form
    {
        private const int MAX_EVENT_NUMBER = 1000;

        ArrayList CacheMessageTypes = new ArrayList();
        ArrayList CheckedTypes = new ArrayList();

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="pParentForm"></param>
        public MessForm(Form pParentForm)
        {
            InitializeComponent();
            this.MdiParent = pParentForm;
            UpdateMessageTypes();
            UpdateCheckedTypes();
        }

        public delegate int GRIDLOGDelegate(string time, string type, string message, Int64 id);
        /// <summary>
        /// Потокозащищенный метод добавления записи в лог
        /// </summary>
        /// <param name="message"></param>
        /// <param name="time"></param>
        private int GRIDLOG(string time, string type, string message, Int64 id)
        {
            if (this.InvokeRequired)
            {
                return (int)this.Invoke(new GRIDLOGDelegate(GRIDLOG), time, type, message, id);
            }

            dataGridView1.Rows.Insert(0, time, type, message, id);


            if (dataGridView1.Rows.Count > MAX_EVENT_NUMBER)
            {
                for (int i = MAX_EVENT_NUMBER; i > MAX_EVENT_NUMBER / 2; i--)
                {
                    dataGridView1.Rows.RemoveAt(i);
                }
            }

            return 1;
        }

        /// <summary>
        /// Установить сообщение на родительской форме в панели статуса
        /// </summary>
        /// <param name="pMessage"></param>
        private void SetStatusText(string pMessage)
        {
            ((MainForm)this.MdiParent).SetStatusText(pMessage);
        }

        /// <summary>
        /// Добавить тип сообщений
        /// </summary>
        /// <param name="pConstantName"></param>
        public void AddMessageType(string pConstantName)
        {
            if (CacheMessageTypes.IndexOf(pConstantName) < 0)
            {
                bool isAdded = false;
                try
                {
                    int i = DataBase.RunCommand(
                        new StringBuilder().AppendFormat(
                                "insert into oko.tmessages(constant_name) values('{0}')", pConstantName
                            ).ToString()
                        );
                    if (i > 0) isAdded = true;
                }
                catch(Exception ex)
                {
                    // SetStatusText(ex.Message);
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
                }
                if (isAdded)
                {
                    CacheMessageTypes.Add(pConstantName);
                    CheckedTypes.Add(pConstantName);
                    checkedListBox1.Items.Add(pConstantName, true);
                }
            }
        }

        /// <summary>
        /// Обновить типы сообщений из БД
        /// </summary>
        private void UpdateMessageTypes()
        {
            DataSet ds = new DataSet();
            bool isOpen = false;
            try
            {
                DataBase.RowSelect("select * from oko.tmessages order by constant_name;", ds);
                isOpen = true;
            }
            catch(Exception ex)
            {
                // SetStatusText(ex.Message);
                Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message), Logger.LogLevel.ERROR);
            }
            if (isOpen)
            {
                checkedListBox1.Items.Clear();
                CacheMessageTypes = new ArrayList();
                if (ds.Tables.Count > 0)
                {
                    try
                    {
                        foreach (DataRow Row in ds.Tables[0].Rows)
                        {
                            CacheMessageTypes.Add(Row["constant_name"].ToString());
                            checkedListBox1.Items.Add(Row["constant_name"].ToString(), Convert.ToBoolean(Row["show"]));
                        }
                    }
                    catch (Exception ex2)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex2.Message), Logger.LogLevel.ERROR);
                        return;
                    }
                }
                Logger.Instance.WriteToLog("Инициализация из справочника", Logger.LogLevel.DEBUG);
                try
                {
                    foreach (KeyValuePair<int, Tuple<string, bool>> current in DBDict.TMessages)
                    {
                        this.CacheMessageTypes.Add(current.Value.Item1);
                        this.checkedListBox1.Items.Add(current.Value.Item1, current.Value.Item2);
                    }
                }
                catch (Exception ex3)
                {
                    Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", base.GetType().Name, MethodBase.GetCurrentMethod().Name, ex3.Message), Logger.LogLevel.ERROR);
                }
            }
        }

        /// <summary>
        /// Добавить событие на форму
        /// </summary>
        /// <param name="pText"></param>
        public void AddEvent(string pType, string pText, Int64 pObjectId)
        {
            dataGridView1.BeginInvoke(
                new GRIDLOGDelegate(GRIDLOG),           // делегат для обработки 
                string                     // параметры метода
                .Format(
                    "{0} {1}",
                    DateTime.Now.ToShortDateString(),
                    DateTime.Now.ToShortTimeString()
                ),
                pType,
                pText,
                pObjectId
            );
        }

        /// <summary>
        /// Установить "правильный" размер таблицы
        /// </summary>
        private void SetTableSize()
        {
            dataGridView1.Columns["RowText"].Width = this.Width - 250;
        }

        private void EventsForm_Load(object sender, EventArgs e)
        {
            SetTableSize();
            foreach (DataGridViewColumn c in dataGridView1.Columns)
            {
                c.DefaultCellStyle.Font = new Font("Arial", 8.5F, GraphicsUnit.Pixel);
            }
            tabControl1.TabPages.RemoveAt(1);
        }

        /// <summary>
        /// Изменение размеров формы
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventsForm_Resize(object sender, EventArgs e)
        {
            SetTableSize();
        }

        /// <summary>
        /// Обновить данные в таблице
        /// </summary>
        private void UpdateDataGridView()
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                if (CheckedTypes.IndexOf(dataGridView1.Rows[i].Cells[1].Value.ToString()) < 0)
                {
                    dataGridView1.Rows.RemoveAt(i--);
                }
            }
        }

        /// <summary>
        /// Обновить типы
        /// </summary>
        private void UpdateCheckedTypes()
        {
            CheckedTypes = new ArrayList();
            foreach (object Check in checkedListBox1.CheckedItems)
            {
                CheckedTypes.Add(Check.ToString());
            }
        }

        /// <summary>
        /// Кнопка "Применить фильтр"
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            UpdateCheckedTypes();
            UpdateDataGridView();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            string sConstantName = checkedListBox1.Items[e.Index].ToString();
            if (e.NewValue == CheckState.Checked)
            { // 2 checked
                DataBase.RunCommand(
                    String.Format(
                        "update oko.tmessages set show = true where constant_name = '{0}'", sConstantName
                    )
                );
                CheckedTypes.Add(sConstantName);
            }
            else
            { // 2 unchecked
                DataBase.RunCommand(
                    String.Format(
                        "update oko.tmessages set show = false where constant_name = '{0}'", sConstantName
                    )
                );
                CheckedTypes.Remove(sConstantName);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            long id = dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToInt64();
            Handling.onObjectCardOpen(id);
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedIndex == 1)
            {
                if (base.WindowState == FormWindowState.Maximized)
                {
                    base.WindowState = FormWindowState.Normal;
                    this.tabControl1.TabPages[1].Text = "Развернуть";
                }
                else
                {
                    base.WindowState = FormWindowState.Maximized;
                    this.tabControl1.TabPages[1].Text = "Свернуть";
                }
                this.tabControl1.SelectTab(0);
            }
        }
    }
}
