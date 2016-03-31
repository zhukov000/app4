using App3;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace fias2pgsql
{
    public partial class Form1 : Form
    {
        private bool isProcessRunning = false;

        private Dictionary<string, string[]> PK;

        private Encoding fromEncodind = Encoding.UTF8; // из какой кодировки
        private Encoding toEncoding = Encoding.UTF8; // в какую кодировку

        public Form1()
        {
            InitializeComponent();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // грузим кладр
            string Message = "см.рис.1";

            do
            {
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                // 1. Установка соединения с БД

                OleDbConnection conn = new OleDbConnection(String.Format(@"Provider=VFPOLEDB.1;Data Source={0}", textBox8.Text));
                try
                {
                    conn.Open();
                }
                catch (Exception ex)
                {
                    Message = ex.Message + "\nВозможно необходимо установить провайдер \"Microsoft OLE DB Driver for FoxPro\" для работы с DBF";
                    break;
                }

                try
                {
                    DataBase.OpenConnection(
                            string.Format(
                                "Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                textBox1.Text,
                                textBox4.Text,
                                textBox2.Text,
                                textBox3.Text,
                                textBox5.Text
                            )
                        );
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    break;
                }

                // 2. Получаем список файлов из директории
                if (!Directory.Exists(textBox8.Text))
                {
                    Message = "Директория с файлами не найдена";
                    break;
                }
                if (conn.State != ConnectionState.Open)
                {
                    Message = "Соединение не может быть установлено";
                    break;
                }
                string[] kladr_files = Directory.GetFiles(textBox8.Text, "*.dbf");

                int n = kladr_files.Count();
                if ((n == 0) || (MessageBox.Show(string.Format("В указанной директории найдено {0} файлов. Загружаем данные?", n), "Загружаем?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
                {
                    Message = "Загрузка отменена";
                    break;
                }
                progressBar1.Minimum = 0;
                progressBar1.Maximum = n;
                progressBar1.Value = 0;

                if (MessageBox.Show("Требуется перекодировка данных?", "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    fromEncodind = Encoding.GetEncoding(1251);
                    toEncoding = Encoding.GetEncoding(866);
                }
                bool b = false;

                // 3. цикл по файлам, грузим каждый
                foreach (string kladr_file in kladr_files)
                {
                    progressBar1.Value++;
                    if (!LoadDBFFile2DB(kladr_file, conn)) b = true;
                }
                if (b)
                {
                    Message = "Все файлы загружены успешно";
                }
                else
                {
                    Message = "В ходе загрузки возникли ошибки. См. сообщения в логе";
                }
                DataBase.CloseConnection();
                conn.Close();

                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Log(String.Format("Время работы программы {0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10));
            } while (false);
            MessageBox.Show(Message, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
            fromEncodind = Encoding.UTF8;
            toEncoding = Encoding.UTF8;
        }


        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // загрузка файлов
            string Message = "см.рис.1";
            
            do
            {
                if ( MessageBox.Show("Данная функция в тестовом режиме. Если хотите загрузить данные об адресных объектах лучше используйте загрузку Кладр. Хотите продолжить?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes )
                {
                    return;
                }
                Stopwatch stopWatch = new Stopwatch();
                stopWatch.Start();
                // 1. Установка соединения с БД
                try
                {
                    DataBase.OpenConnection(
                            string.Format(
                                "Server={0};Port={1};User Id={2};Password={3};Database={4};",
                                textBox1.Text,
                                textBox4.Text,
                                textBox2.Text,
                                textBox3.Text,
                                textBox5.Text
                            )
                        );
                }
                catch (Exception ex)
                {
                    Message = ex.Message;
                    break;
                }
                // 2. Получаем список файлов из директории
                if (!Directory.Exists(textBox7.Text))
                {
                    Message = "Что-то не найду я директории с файлами. Вот ищу-ищу, а найти не могу :-(";
                    break;
                }
                string [] fias_files = Directory.GetFiles(textBox7.Text, "*.xml");
                int n = fias_files.Count();
                if ((n == 0) || (MessageBox.Show(string.Format("В указанной директории найдено {0} файлов. Загружаем данные?", n), "Загружаем?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes))
                {
                    Message = "Какая-то проблема с файлами? Посмотри здесь: http://fias.nalog.ru/Public/DownloadPage.aspx";
                    break;
                }
                progressBar1.Minimum = 0;
                progressBar1.Maximum = n;
                progressBar1.Value = 0;
                bool b = false;
                
                if (!checkBox1.Checked)
                { // очистка данных перед загрузкой
                    ClearScheme(fias_files);
                }
                else
                { // получить список первичных ключей для обновления данных в таблицах
                    PK = PrimaryKeys();
                }
                // 3. цикл по файлам, грузим каждый
                foreach(string fias_file in fias_files)
                {
                    progressBar1.Value++;
                    if (!LoadXMLFile2DB(fias_file)) b = true;
                }
                if (b)
                {
                    Message = "Не все файлы одинаково пролезли";
                }
                else
                {
                    Message = "Хвала небесам! Все зашло!";
                }
                DataBase.CloseConnection();
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                Log(String.Format("Время работы программы {0:00}:{1:00}:{2:00}.{3:00}",
                    ts.Hours, ts.Minutes, ts.Seconds,
                    ts.Milliseconds / 10));
            } while (false);
            MessageBox.Show(Message, "Результат", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            CancelButton = button2;
            textBox1.Text = ConfigurationManager.AppSettings["DBServerHost"];
            textBox4.Text = ConfigurationManager.AppSettings["DBServerPort"];
            textBox2.Text = ConfigurationManager.AppSettings["DBUser"];
            textBox3.Text = ConfigurationManager.AppSettings["DBPassword"];
            textBox5.Text = ConfigurationManager.AppSettings["DBName"];
            textBox6.Text = ConfigurationManager.AppSettings["DBSchema"];
        }

        /// <summary>
        /// Вывести сообщение
        /// </summary>
        /// <param name="mess"></param>
        private void Log(string mess)
        {
            if (listBox1.InvokeRequired)
                listBox1.BeginInvoke(new Action(() => listBox1.Items.Insert(0,mess)));
            else
                listBox1.Items.Insert(0,mess);
        }

        private delegate bool DBChange(XElement el);

        private void ClearScheme(string [] files)
        {
            foreach(string file in files)
            {
                IEnumerable<XElement> bardQuotes =
                from el in SimpleStreamAxis(file)
                select el;

                foreach (XElement e in bardQuotes)
                {
                    DeleteTable(e);
                    break;
                }
            }
        }

        private bool LoadDBFFile2DB(string DBFFile, OleDbConnection Conn)
        {
            bool r = true;
            int maxRows = 1000000; // ограничение по количеству записей, выбираемых из таблицы
            int offset = 0;
            string TableName = Path.GetFileNameWithoutExtension(DBFFile);

            if (isProcessRunning)
            {
                MessageBox.Show("Уже вчитываю");
                return false;
            }

            Log(string.Format("Читаю файл {0}", DBFFile));

            DataTable dt = Conn.GetOleDbSchemaTable(System.Data.OleDb.OleDbSchemaGuid.Columns, new object[] { null, null, TableName });
            string [] columns = dt.Rows.Cast<DataRow>()
                                 .Select(x => x[3].ToString().ToLower())
                                 .ToArray();
            string column = string.Join(",", columns);

            string regionFilter = "";
            if(columns.Contains("code"))
            {
                regionFilter = "where code like '61%'";
            }

            DataTable ResultSet = new DataTable();
            /*string mySQL = string.Format(
                "select top {1} * from {0} {3} order by {2}", 
                TableName, maxRows, column, regionFilter
            );*/

            string mySQL = string.Format(
                "select * from {0} {3}",
                TableName, maxRows, column, regionFilter
            );

            OleDbCommand MyQuery = new OleDbCommand(mySQL, Conn);
            OleDbDataAdapter DA = new OleDbDataAdapter(MyQuery);

            DA.Fill(ResultSet);

            DeleteTable(TableName);

            ProgressDialog progressDialog = new ProgressDialog(0, ResultSet.Rows.Count);

            isProcessRunning = true;
            Thread backgroundThread = new Thread(
                new ThreadStart(() =>
                {
                    foreach (DataRow row in ResultSet.Rows)
                    {
                        if (!Add2DB(TableName, row, ResultSet.Columns))
                        {
                            r = false;
                            continue;
                        }
                        progressDialog.Inc();
                    }

                    Log("Файл загружен");

                    if (progressDialog.InvokeRequired)
                        progressDialog.BeginInvoke(new Action(() => progressDialog.Close()));

                    isProcessRunning = false;
                }
            ));
            backgroundThread.Start();
            progressDialog.ShowDialog();
            

            if (!r) Log("В процессе загрузки произошли ошибки");
            return r;
        }

        /// <summary>
        /// Загрузка XML в БД
        /// </summary>
        /// <param name="XMLFile"></param>
        /// <returns></returns>
        private bool LoadXMLFile2DB(string XMLFile)
        {
            bool r = true;

            if (isProcessRunning)
            {
                MessageBox.Show("Уже вчитываю");
                return false;
            }

            ProgressDialog progressDialog = new ProgressDialog(0, 10000);

            Log(string.Format("Читаю файл {0}", XMLFile));

            IEnumerable<XElement> bardQuotes =
                from el in SimpleStreamAxis(XMLFile)
                select el;

            DBChange d = Add2DB;
            if (checkBox1.Checked)
            {
                d = UpdateInDB;
            }
            else
            {
                r = CreateTable(bardQuotes.First());
            }
            if (r)
            {
                isProcessRunning = true;
                Thread backgroundThread = new Thread(
                    new ThreadStart(() =>
                    {

                        foreach (XElement e in bardQuotes)
                        {
                            if (!d(e))
                            {
                                r = false;
                                continue;
                            }
                            progressDialog.Inc();
                        }

                        Log("Файл загружен");

                        if (progressDialog.InvokeRequired)
                            progressDialog.BeginInvoke(new Action(() => progressDialog.Close()));

                        isProcessRunning = false;
                    }
                ));
                backgroundThread.Start();
                progressDialog.ShowDialog();
            }

            if (!r) Log("В процессе загрузки произошли ошибки");
            return r;
        }

        /// <summary>
        /// Получение типа для данных в БД по значению
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string GetType4DB(string val)
        {
            string t = "text";
            DateTime temp1;
            
            if (DateTime.TryParse(val, out temp1))
            {
                t = "timestamp";
            }

            Guid temp2;
            if (Guid.TryParse(val, out temp2) )
            {
                t = "UUID";
            }
            return t;
        }

        /// <summary>
        /// Удалить таблицу
        /// </summary>
        /// <param name="el"></param>
        private void DeleteTable(XElement el)
        {
            DeleteTable(el.Name.ToString());
        }

        /// <summary>
        /// Очистка таблицы
        /// </summary>
        /// <param name="t"></param>
        private void DeleteTable(string t)
        {
            try
            {
                DataBase.RunCommand(string.Format("DELETE FROM {0}.{1};", textBox6.Text, t));
                // DataBase.RunCommand(string.Format("DROP TABLE IF EXISTS {0}.{1};", textBox6.Text, t));
                Log(String.Format("Таблица {0}.{1} почищена", textBox6.Text, t));
            }
            catch (Exception ex)
            {
                Log(ex.Message);
            }
        }

        /// <summary>
        /// Проверка таблицы на существование
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        private bool ExistsTable(string pName)
        {
            DataSet ds = new DataSet();
            try
            {
                DataBase.RowSelect(string.Format("SELECT * FROM {0}.{1} LIMIT 1", textBox6.Text, pName), ds);
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Получение справочника первичных ключей
        /// </summary>
        /// <returns></returns>
        private Dictionary<string, string[]> PrimaryKeys()
        {
            Dictionary<string, string[]> result = new Dictionary<string, string[]>();
            DataSet ds = new DataSet();
            try
            {
                DataBase.RowSelect(string.Format("SELECT * FROM {0}._primarykeys_", textBox6.Text), ds);
                foreach(DataRow row in ds.Tables[0].Rows)
                {
                    string [] CurVal;
                    if ( result.TryGetValue(row["tablename"].ToString(), out CurVal) )
                    { // уже есть
                        string[] NewVal = new string[CurVal.Count()+1];
                        CurVal.CopyTo(NewVal,0);
                        NewVal[CurVal.Count()] = row["columnname"].ToString();
                        result[row["tablename"].ToString()] =  NewVal;
                    }
                    else 
                    { // новое
                        result[row["tablename"].ToString()] = new string[] { row["columnname"].ToString() };
                    }
                }
            }
            catch
            {
                DataBase.RunCommand(String.Format("CREATE TABLE {0}._primarykeys_(tablename character varying(30), columnname character varying(30), idrow serial NOT NULL, CONSTRAINT _primarykeys__pkey PRIMARY KEY (idrow))", textBox6.Text));
                Log("Таблица для хранения первичных ключей {0}._primarykeys создана");
            }
            return result;
        }

        /// <summary>
        /// Создать таблицу для хранения элемента XML
        /// </summary>
        /// <param name="el"></param>
        private bool CreateTable(XElement el)
        {
            bool r = true;

            if (el.Attributes().Count() == 0)
            {
                r = false;
            }
            else
            {
                r = CreateTable(
                    el.Name.ToString(), 
                    string.Join(",", el.Attributes().Select(o => string.Format("{0} {1}", o.Name, GetType4DB(o.Value))).ToArray())
                );
            }
            return r;
        }

        /// <summary>
        /// Создать таблицу
        /// </summary>
        /// <param name="t">Имя таблицы</param>
        /// <param name="c">Содержимое: поле тип, ...</param>
        /// <returns></returns>
        private bool CreateTable(string t, string c)
        {
            bool r = true;

            try
            {
                DataBase.RunCommand(
                    string.Format("CREATE TABLE {0}.{1}({2});",
                        textBox6.Text, t, c
                    )
                );
                Log(string.Format("Таблица {0} создана", t));
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                r = ExistsTable(t);
            }

            return r;
        }

        private bool Add2DB(string t, DataRow row, DataColumnCollection cols)
        {
            bool r = true;
            string f = string.Join(",", cols.Cast<DataColumn>()
                                 .Select(x => x.ColumnName)
                                 .ToArray() 
            );
            string v = "";

            if (!fromEncodind.Equals(toEncoding))
            {
                string del = "";
                foreach(object o in row.ItemArray)
                {
                    string s = o.ToString();
                    var bytes = fromEncodind.GetBytes(s);
                    s = toEncoding.GetString(bytes);
                    v = v + del + string.Format("'{0}'", s.Trim());
                    del = ",";
                }
                
            }
            else
            {
                v = string.Join(",", row.ItemArray.Select(
                        o => string.Format("'{0}'", o.ToString().Trim())
                    ).ToArray()
                );
            }

            if (!ExistsTable(t))
            {
                CreateTable(
                    t,
                    string.Join(
                        ",", 
                        cols.Cast<DataColumn>()
                                 .Select(o => string.Format("{0} {1}", o.ToString(), GetType4DB(row[o.ToString()].ToString())))
                                 .ToArray()
                    )
                    
                );
            }

            // заменяем пустые строки на null
            v = v.Replace("''", "null");

            try
            {
                DataBase.RunCommand(
                    string.Format("INSERT INTO {0}.{1}({2}) VALUES({3})",
                        textBox6.Text, t, f, v
                    )
                );
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                r = false;
            }
            return r;
        }

        /// <summary>
        /// Добавить в БД данные из элемента XML
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private bool Add2DB(XElement el)
        {
            bool r = true;
            string t = el.Name.ToString();
            string f = string.Join(",", el.Attributes().Select(o => o.Name.ToString()).ToArray() );
            string v = string.Join(",", el.Attributes().Select(o => string.Format("'{0}'", o.Value)).ToArray());
            bool b = false;

            do
            {
                b = false;
                string nf = "";

                try
                {
                    DataBase.RunCommand(
                        string.Format("INSERT INTO {0}.{1}({2}) VALUES({3})",
                            textBox6.Text, t, f, v
                        )
                    );
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    r = false;
                    if (DataBase.Like(ex.Message, "%колонка%в таблице%не существует%"))
                    {
                        b = true;
                        int i = ex.Message.IndexOf("\"");

                        if (i >= 0)
                        {
                            int j = ex.Message.IndexOf("\"", i + 1);
                            if (j > 0)
                            {
                                nf = ex.Message.Substring(i + 1, j - i - 1);
                            }
                        }
                        if (nf == "")
                        {
                            break;
                        }
                    }
                }

                if (b)
                { // добавление колонки
                    try
                    {
                        DataBase.RunCommand(
                            string.Format("ALTER TABLE {0}.{1} ADD COLUMN {2} {3}",
                                textBox6.Text, t, nf, 
                                GetType4DB(
                                   el.Attributes()
                                    .Select(o => o)
                                    .Where(o => (o.Name.ToString().ToUpper() == nf.ToUpper()))
                                    .First()
                                    .Value
                                )
                            )
                        );
                        r = true;
                        Log(string.Format("Колонка {0} в таблице {1} создана!", nf, t));
                    }
                    catch (Exception ex)
                    {
                        Log(ex.Message);
                        r = false;
                        break;
                    }
                }
            } while (b);

            return r;
        }

        /// <summary>
        /// Обновить в БД данные по элементу XML
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private bool UpdateInDB(XElement el)
        {
            bool r = true;
            string t = el.Name.ToString();
            string w = "";

            string[] keys;

            if ( !PK.TryGetValue(t.ToLower(), out keys) )
            {
                // если нет информации о ключах
                foreach(XAttribute a in el.Attributes())
                {
                    if (GetType4DB(a.Value) == "UUID")
                    {
                        w = string.Format("{0} = '{1}'", a.Name.ToString(), a.Value.ToString());
                        break;
                    }
                }
            }
            else
            {
                string sep = "";
                foreach (XAttribute a in el.Attributes())
                {
                    if (keys.Contains(a.Name.ToString().ToLower()))
                    {
                        w = sep + w + string.Format("({0} = '{1}')", a.Name.ToString(), a.Value.ToString());
                        sep = "AND";
                    }
                }
            }

            if (w == "")
            {
                Log(String.Format("Обновление данных в таблице {0} невозможно, т.к. нет поля PK", t));
                r = Add2DB(el);
            }
            else
            {
                string s = string.Join(",", el.Attributes().Select(o => string.Format("{1} = '{0}'", o.Value, o.Name)).ToArray());
                int i = 0;
                try
                {
                    i = DataBase.RunCommand(
                        string.Format("UPDATE {0}.{1} SET {2} WHERE {3}",
                            textBox6.Text, t, s, w
                        )
                    );
                }
                catch (Exception ex)
                {
                    Log(ex.Message);
                    r = false;
                }
                if (!r || i == 0) // не получилось обновить
                {
                    Add2DB(el);
                }
            }
            return r;
        }

        /// <summary>
        /// Потоковая обработка файла xml
        /// </summary>
        /// <param name="inputUrl"></param>
        /// <returns></returns>
        static IEnumerable<XElement> SimpleStreamAxis(string inputUrl)
        {
            using (XmlReader reader = XmlReader.Create(inputUrl))
            {
                reader.MoveToContent();
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            XElement el = XElement.ReadFrom(reader)
                                                    as XElement;
                            if (el != null)
                                yield return el;
                            break;
                    }
                }
                reader.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                textBox7.Text = fd.SelectedPath;
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkBox1.Checked)
            {
                if (MessageBox.Show("Снятие этого флага приведет к полной перезаписи данных. Продолжить?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    checkBox1.Checked = true;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // обзор папок для кладр
            FolderBrowserDialog fd = new FolderBrowserDialog();
            if (fd.ShowDialog() == DialogResult.OK)
            {
                textBox8.Text = fd.SelectedPath;
            }
        }
    }
}
