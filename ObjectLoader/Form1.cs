using App3;
using App3.Class;
using Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows.Forms;

namespace ObjectLoader
{
    public partial class Form1 : Form
    {

        public delegate object HandleonlineData(string Data);


        Dictionary<string, KeyValuePair<string, HandleonlineData> > OnlineGeo = new Dictionary<string, KeyValuePair<string, HandleonlineData> >()
        {
            { "Open Street Map",
                new KeyValuePair<string, HandleonlineData> (
                    "http://nominatim.openstreetmap.org/search.php?q={0}&format=json&addressdetails=1",
                    OSMHandle
                )
            },
            { "Yandex",
                new KeyValuePair<string, HandleonlineData> (
                    "https://geocode-maps.yandex.ru/1.x/?format=json&results=1&geocode={0}" ,
                    YandexHandle
                )
            }
        };

        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_DragEnter(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files != null && files.Length != 0)
            {
                textBox1.Text = files[0];
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dial = new OpenFileDialog();
            dial.Filter = "Файл импорта|*.xls;*.xlsx";
            if(dial.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = dial.FileName;
            }
        }

        private string Decoding(string answer)
        {
            Encoding utf8 = Encoding.GetEncoding("utf-8");
            Encoding win1251 = Encoding.GetEncoding("windows-1251");

            byte[] utf8Bytes = win1251.GetBytes(answer);
            byte[] win1251Bytes = Encoding.Convert(utf8, win1251, utf8Bytes);

            return win1251.GetString(win1251Bytes).ToString();
        }

        delegate void ShowMessageDelegate(string message);
        delegate void LoadDataDelegate(int regionId, string geoName, string regionName);

        /// <summary>
        /// Загрузка данных в БД
        /// </summary>
        /// <param name="regionId"></param>
        /// <param name="geoName"></param>
        /// <param name="regionName"></param>
        private void LoadData(int regionId, string geoName, string regionName)
        {
            string file = textBox1.Text;

            FileStream stream = File.Open(file, FileMode.Open, FileAccess.Read);
            IExcelDataReader excelReader;
            try
            {
                excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                //if (!excelReader.IsClosed)
                //    excelReader.Close();
                if (!excelReader.IsValid)
                {
                    stream = File.Open(file, FileMode.Open, FileAccess.Read);
                    excelReader = ExcelReaderFactory.CreateBinaryReader(stream); // для 2003
                }
            }
            catch(Exception ex)
            {
                MenuChange(ex.Message);
                MenuChange("true");
                return;
            }
            DataSet result = excelReader.AsDataSet();
            bool skipFirst = checkBox2.Checked;
            bool enableGeo = checkBox1.Checked;
            bool updateAdr = checkBox3.Checked;

            Dictionary<string, int> TStatus = new Dictionary<string, int>()
            {
                { "Работа", 1 },
                { "Отключен", 0 },
                { "Контроль", 2 }
            };

            ShowMessage(string.Format("В файле {0} строк.", result.Tables[0].Rows.Count));
            int c_upd = 0;
            int c_ins = 0;
            int c_err = 0;
            foreach (DataRow row in result.Tables[0].Rows)
            {
                if (skipFirst)
                {
                    skipFirst = false;
                    ShowMessage("Заголовок пропущен");
                    continue;
                }
                // данные строки
                string objNumber = (row[0].ToString());
                string objName = (row[1].ToString());
                string objAddress = row[2].ToString();
                string objStatus = row[3].ToString();
                string objLastMess = row[4].ToString();
                string objLastMessTime = row[5].ToString();
                string objDesc = row[6].ToString();

                if (objNumber == "" || objNumber.ToInt() == 0)
                {
                    ShowMessage("Пропуск строки");
                    continue;
                }

                try
                {
                    // 1. Если включен геокодинг - получить информацию об адресе объекта
                    string server = OnlineGeo[geoName].Key;
                    IGeoObject geoObj = null;
                    if (enableGeo)
                    {
                        using (WebClient wc = new WebClient())
                        {
                            var json = wc.DownloadString(string.Format(server, string.Format("Ростовская область, {0}, {1}", regionName, objAddress)));
                            string answer = Decoding(json);
                            geoObj = (IGeoObject)OnlineGeo[geoName].Value(answer);
                        }
                    }
                    // 2. Проверить существует ли объект
                    
                    object osm_id = DataBase.First(string.Format("select osm_id from oko.object where number = {0} and region_id = {1}", objNumber, regionId), "osm_id");
                    if (osm_id == null)
                    {
                        // 2.1 Если объект не существует, то создать объект и адрес
                        AKObject Obj = new AKObject();
                        Obj.number = objNumber.ToInt();
                        Obj.name = objName ;
                        Obj.RegionId = regionId;
                        Obj.makedatetime = DateTime.Now.ToString();
                        try
                        {
                            Obj.TStatus = TStatus[objStatus];
                        }
                        catch
                        {
                            Obj.TStatus = 1;
                        }
                        Obj.Description = objDesc;

                        if (geoObj != null)
                        {
                            Address objAddr = new Address();
                            // изменить точку
                            // double[] coor = Geocoder.UTM2LL(new double[] { geoObj.latitude().ToDouble(), geoObj.longitude().ToDouble() });
                            Obj.SetPoint(geoObj.longitude().ToDouble(), geoObj.latitude().ToDouble(),  true);
                            // создание адреса

                            objAddr.Region = geoObj.region();
                            objAddr.Locality = geoObj.locality();
                            objAddr.Street = geoObj.street();
                            objAddr.House = geoObj.house();
                            objAddr.FullAddress = geoObj.addressString();
                            objAddr.latitude = geoObj.latitude();
                            objAddr.longitude = geoObj.longitude();

                            objAddr.Save2DB();
                            // Obj.SetAddress(objAddr);
                        }
                        // сохранение в БД
                        Obj.Save2DB();
                        ShowMessage(string.Format("Объект {0} создан", objNumber));
                        c_ins++;
                    }
                    else
                    {
                        // 2.2 Если существует, то 
                        // 2.2.1 обновить имя, статус, описание
                        AKObject Obj = new AKObject(objNumber.ToInt(), regionId);
                        Obj.name = objName;
                        Obj.TStatus = TStatus[objStatus];
                        Obj.Description = objDesc;
                        
                        if (updateAdr && geoObj != null)
                        {
                            // 2.2.2 установлена галка обновлять адрес - обновить адрес
                            Address objAddr = Obj.GetAddress();

                            // изменить точку
                            // double[] coor = Geocoder.UTM2LL(new double[] { geoObj.latitude().ToDouble(), geoObj.longitude().ToDouble() });
                            Obj.SetPoint(geoObj.longitude().ToDouble(), geoObj.latitude().ToDouble(),  true);

                            objAddr.Region = geoObj.region();
                            objAddr.Locality = geoObj.locality();
                            objAddr.Street = geoObj.street();
                            objAddr.House = geoObj.house();
                            objAddr.FullAddress = geoObj.addressString();
                            objAddr.latitude = geoObj.latitude();
                            objAddr.longitude = geoObj.longitude();

                            objAddr.Save2DB();
                            // Obj.SetAddress(objAddr);
                        }
                        Obj.Save2DB();
                        ShowMessage(string.Format("Объект {0} обновлен", objNumber));
                        c_upd++;
                    }
                    /*
                    // 3. Добавить сообщение
                    // 3.1 Получить код сообщения
                    object[] tm_row = DataBase.FirstRow(
                        string.Format("select \"OKO\", class, code from oko.message_text where message like '{0}'", objLastMess), 
                        -1);
                    // 3.2
                    if (tm_row.Count() == 0)
                    { // значение по умолчанию
                        tm_row = new object[3] { 2, 128, 0 };
                    }
                    DataBase.RunCommandInsert
                    (
                        "oko.event",
                        new Dictionary<string, object>()
                        {
                            {"code", tm_row[2].Q()},
                            {"class", tm_row[1].Q()},
                            {"oko_version", tm_row[0].Q()},
                            {"datetime", objLastMessTime.Q()},
                            {"objectnumber", objNumber.Q()},
                            {"region_id", regionId}
                        }
                    );
                    */
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message);
                    ShowMessage(ex.StackTrace);
                    c_err++;
                    if (checkBox4.Checked)
                    {
                        continue;
                    }
                    else
                    {
                        ShowMessage("Операция загрузки остановлена из-за возникшей ошибки");
                        break;
                    }
                }
            }
            excelReader.Close();
            ShowMessage(string.Format("Всего объектов: добавлено - {0}, обновлено - {1}, ошибок - {2}", c_ins, c_upd, c_err));
            // включить интерфейс программы
            MenuChange("true");
        }

        /// <summary>
        /// Включение выключение интерфейса программы
        /// </summary>
        /// <param name="b"></param>
        private void MenuChange(string b)
        {
            if (!button2.InvokeRequired && !groupBox1.InvokeRequired && !groupBox2.InvokeRequired && !groupBox3.InvokeRequired)
            {
                bool bb = false;
                if (b == "true") bb = true;

                groupBox1.Enabled = bb;
                groupBox2.Enabled = bb;
                groupBox3.Enabled = bb;
                button2.Enabled = bb;
            }
            else
            {
                ShowMessageDelegate menuChange = new ShowMessageDelegate(MenuChange);
                BeginInvoke(menuChange, new object[] { b });
            }
        }

        private void ShowMessage(string message)
        {
            if (!listBox1.InvokeRequired)
            {
                listBox1.Items.Add(message);
                listBox1.SelectedIndex = listBox1.Items.Count - 1; // autoscroll
            }
            else
            {
                ShowMessageDelegate showMessage = new ShowMessageDelegate(ShowMessage);
                BeginInvoke(showMessage, new object[] { message });
            }
        }

        private bool Verify()
        {
            bool res = false;
            string mess = "";
            do
            {

                if (!File.Exists(textBox1.Text))
                {
                    mess = "Не выбран источник записей либо указанный файл недоступен";
                    break;
                }
                if (GetMunicId() == -1)
                {
                    mess = "Не выбран район, для которого происходит загрузка данных";
                    break;
                }
                if (!checkBox1.Checked && MessageBox.Show("Вы уверены, что не хотите использовать ниодин из сервисов геокодирования (в этом случае адреса не будут внесены)","Вопрос", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    mess = "Отменено пользователем";
                    break;
                }
                if (checkBox3.Checked && !checkBox1.Checked)
                {
                    mess = "Невозможно обновлять адреса, если выключен сервер геокодинга";
                    break;
                }
                res = true;
            } while (false);
            if (!res)
            {
                MessageBox.Show(mess, "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return res;
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (Verify())
            {
                listBox1.Items.Clear();
                LoadDataDelegate s = new LoadDataDelegate(LoadData);
                MenuChange("false");
                s.BeginInvoke(GetMunicId(), comboBox1.Items[comboBox1.SelectedIndex].ToString(), GetMunicName(), null, null);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // заполняем комбики
            // 1. Сервера геокодинга
            comboBox1.Items.Clear();
            foreach(var pair in OnlineGeo)
            {
                comboBox1.Items.Add(pair.Key);
            }
            comboBox1.SelectedIndex = 1;
            // 2. Доступные регионы
            comboBox2.Items.Clear();
            bool flag = true;
            try
            {
                DataBase.OpenConnection(
                    string.Format(
                        "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;",
                        Config.Get("DBServerHost"),
                        Config.Get("DBServerPort"),
                        Config.Get("DBUser"),
                        Config.Get("DBPassword"),
                        Config.Get("DBName")
                    ));

                foreach(object[] row in DataBase.RowSelect("select fullname, num  from regions2map order by name") )
                {
                    comboBox2.Items.Add(new ComboboxItem(row[0].ToString(), row[1]));
                }

                flag = false;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (flag)
            {   // загружаем из справочника
                foreach (var row in SpoloxInstall.DictRegions.Dict)
                {
                    comboBox2.Items.Add(new ComboboxItem(row.Value, row.Key));
                }
            }
        }

        private int GetMunicId()
        {
            try
            {
                ComboboxItem item = (ComboboxItem)comboBox2.SelectedItem;
                return Convert.ToInt32(item.Value);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return -1;
            }
        }

        private string GetMunicName()
        {
            try
            {
                ComboboxItem item = (ComboboxItem)comboBox2.SelectedItem;
                return item.Text;
            }
            catch
            {
                return "";
            }
        }

        #region Обработка результатов от Геокодеров
        static private object OSMHandle(string Data)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            OSMObject[] Objects = json_serializer.Deserialize<OSMObject[]>(Data);

            if (Objects.Count() > 0)
            {
                return Objects[0];
            }
            else
            {
                return null;
            }
        }

        static private object YandexHandle(string Data)
        {
            JavaScriptSerializer json_serializer = new JavaScriptSerializer();
            YandexObject Object = json_serializer.Deserialize<YandexObject>(Data);
            return Object;
        }
        #endregion
    }
}
