using App3;
using App3.Class;
using App3.Class.Singleton;
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

namespace SpoloxInstall
{
    public partial class Settings : Form
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            municName.Items.Clear();
            
            foreach(var row in DictRegions.Dict)
            {
                municName.Items.Add(new ComboboxItem(row.Value, row.Key));
            }
            postgresqlPath.Text = Utils.AppDirectory() + @"\sources\postgresql-9.6.1-1-windows.exe";
            postgisPath.Text = Utils.AppDirectory() + @"\sources\postgis-bundle-pg96x32-setup-2.3.0-1.exe";
            backupPath.Text = Utils.AppDirectory() + @"\sources\spolox_data.zip";
            arhivePath.Text = Utils.AppDirectory() + @"\sources\spolox_exe.zip";
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip3_Opening(object sender, CancelEventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(!File.Exists(postgresqlPath.Text))
            {
                MessageBox.Show("Путь к установщику PostgreSQL не найден");
            }
            else
            {
                Logger.Instance.WriteToLog("Запуск файла " + postgresqlPath.Text);

                bool f = Utils.RunApplication(postgresqlPath.Text);

                if (f && MessageBox.Show("Установка завершена успешно?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Logger.Instance.WriteToLog("Установка PostgreSQL выполнена ");
                    log.Items.Add("1. Установка PostgreSQL выполнена");
                    button5.Enabled = false;
                }
                else
                {
                    Logger.Instance.WriteToLog("Установка PostgreSQL не выполнена");
                    log.Items.Add("- Установка PostgreSQL не выполнена");
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (!File.Exists(postgisPath.Text))
            {
                MessageBox.Show("Путь к установщику PostGIS не найден");
            }
            else
            {
                Logger.Instance.WriteToLog("Запуск файла " + postgisPath.Text);

                bool f = Utils.RunApplication(postgisPath.Text);

                if (f && MessageBox.Show("Установка завершена успешно?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        DataBase.OpenConnection(
                            string.Format(
                                "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;",
                                "localhost",
                                "5432",
                                dbUser.Text,
                                dbPass.Text,
                                dbName.Text
                            ));
                        
                        DataBase.RunCommand("CREATE EXTENSION postgis");
                        DataBase.RunCommand("CREATE EXTENSION hstore");
                    }
                    catch (Exception ex)
                    {
                        Logger.Instance.WriteToLog(string.Format("{0}.{1}: {2}", System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message));
                        f = false;
                    }
                    if (f)
                    {
                        Logger.Instance.WriteToLog("Установка PostGIS выполнена ");
                        log.Items.Add("2. Установка PostGIS выполнена");
                        button6.Enabled = false;
                    }
                    else
                    {
                        Logger.Instance.WriteToLog("Расширение не создано");
                        log.Items.Add("- Расширение не создано");
                    }
                }
                else
                {
                    Logger.Instance.WriteToLog("Установка PostGIS не выполнена");
                    log.Items.Add("- Установка PostGIS не выполнена");
                }
            }
        }

        private bool MunicSelect()
        {
            bool f = true;
            if (municName.SelectedIndex < 0)
                f = false;
            return f;

        }

        private int GetMunicId()
        {
            ComboboxItem item = (ComboboxItem) municName.SelectedItem;
            return item.Value.ToInt();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!File.Exists(backupPath.Text))
            {
                MessageBox.Show("Путь к БД не найден");
            }
            else if (!File.Exists(pgbinPath.Text))
            {
                MessageBox.Show("Путь к pg_restore не найден");
            }
            else if(!MunicSelect())
            {
                MessageBox.Show("Не выбран муниципалитет");
            }
            else
            {
                string curDirectory = Utils.AppDirectory();
                string fileName = Path.GetFileNameWithoutExtension(backupPath.Text);
                string filePath = Path.GetDirectoryName(backupPath.Text);
                string sqlFile = filePath + "\\spolox.sql";

                if (!File.Exists(sqlFile) || MessageBox.Show("Найден файл " + sqlFile + " использовать его (Yes) или выполнить разархивирование (No)?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                { // разархивирование
                    string cmd = string.Format("e \"{0}\\{1}.zip\" -o\"{0}\" -y", filePath, fileName);
                    string z7 = string.Format("\"{0}\\bin\\7zip\\7z\"", curDirectory);
                    Utils.RunCmd(z7, cmd);
                    Logger.Instance.WriteToLog("Разархивирование: " + z7 + cmd);
                }

                if (File.Exists(sqlFile))
                {
                    Logger.Instance.WriteToLog("Запуск файла " + fileName);

                    string cmd = string.Format(" {0} < \"{1}\" {2}", dbName.Text, sqlFile, dbUser.Text);
                    string psql = "\"" + pgbinPath.Text + "\"";

                    string tmpBat = curDirectory + "\\sources\\restore.bat";
                    Utils.WriteToFile(tmpBat, psql + cmd);

                    Utils.RunCmd(tmpBat, "");

                    Logger.Instance.WriteToLog("Восстановление бэкапа: " + psql + cmd);

                    if (MessageBox.Show("Установка завершена успешно?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {

                        if (File.Exists(curDirectory + "\\sources\\update.sql"))
                        {
                            try
                            {
                                DataBase.OpenConnection(
                                    string.Format(
                                        "Server={0};Port={1};User Id={2};Password={3};Database={4};MaxPoolSize=40;",
                                        "localhost",
                                        "5432",
                                        dbUser.Text,
                                        dbPass.Text,
                                        dbName.Text
                                    ));

                                StreamReader file = new StreamReader(curDirectory + "\\sources\\update.sql");
                                string line = "";
                                while ((line = file.ReadLine()) != null)
                                {
                                    DataBase.RunCommand(line);
                                }
                                // включить район
                                DataBase.RunCommand("UPDATE oko.ipaddresses SET listen = true, send = true WHERE id_region = " + GetMunicId());
                                file.Close();
                            }
                            catch (Exception ex)
                            {
                                Logger.Instance.WriteToLog(string.Format("При открытии соединения с базой произошла ошибка: {0}", ex.Message));
                            }
                        }

                        Logger.Instance.WriteToLog("Восстановление БД выполнено");
                        log.Items.Add("3. Восстановление БД выполнено");
                        button7.Enabled = false;
                    }
                    else
                    {
                        Logger.Instance.WriteToLog("Восстановление БД не выполнено");
                        log.Items.Add("- Восстановление БД не выполнено");
                    }
                }
                else
                {
                    Logger.Instance.WriteToLog("Восстановление БД не выполнено");
                    log.Items.Add("- Восстановление БД не выполнено");
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Utils.AppDirectory() + @"\sources";
            fileDialog.Filter = "Установщик PostgreSQL (.exe)|*.exe";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                postgresqlPath.Text = fileDialog.FileName;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Utils.AppDirectory() + @"\sources";
            fileDialog.Filter = "Установщик PostGIS (.exe)|*.exe";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                postgisPath.Text = fileDialog.FileName;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Utils.AppDirectory() + @"\sources";
            fileDialog.Filter = "Архив бэкапа (.zip)|*.zip";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                backupPath.Text = fileDialog.FileName;
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = @"C:\Program Files\PostgreSQL\9.6\bin\";
            fileDialog.Filter = "Утилита восстановления (psql.exe)|psql.exe";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                pgbinPath.Text = fileDialog.FileName;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (!File.Exists(arhivePath.Text))
            {
                MessageBox.Show("Путь к архиву программы не найден");
            }
            else
            {
                string curDirectory = Utils.AppDirectory();
                // создание директории
                Directory.CreateDirectory(dirPath.Text);
                // разархивирование
                string z7 = string.Format("\"{0}\\bin\\7zip\\7z\"", curDirectory);
                string cmd = string.Format("e \"{0}\" -o\"{1}\" -y", arhivePath.Text, dirPath.Text);
                Utils.RunCmd(z7, cmd);
                Logger.Instance.WriteToLog("Разархивирование и копирование файлов выполнено: " + z7 + cmd);
                log.Items.Add("4. Разархивирование и копирование файлов выполнено:");
                button7.Enabled = false;
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if(!Directory.Exists(dirPath.Text))
            {
                MessageBox.Show("Папка с программой не найдена: " + dirPath.Text);
            }
            else if (!MunicSelect())
            {
                MessageBox.Show("Не выбран муниципалитет");
            }
            else
            {
                string configFile = dirPath.Text + "\\App3.exe.config";
                if (File.Exists(configFile))
                {
                    log.Items.Add(configFile);
                    Config.Set("DBUser", dbUser.Text, configFile);
                    Config.Set("DBPassword", dbPass.Text, configFile);
                    Config.Set("DBName", dbName.Text, configFile);
                    Config.Set("StartWeb", "0", configFile);
                    Config.Set("XMLGuard", "0", configFile);
                    Config.Set("COMConn", "1", configFile);
                    Config.Set("CurrenRegion", GetMunicId().ToString(), configFile);
                    Config.Set("COMPortName", portName.Text, configFile);
                    Config.Set("RedirectAllIncommingServer", "", configFile);
                    Config.Set("RedirectAllIncommingPort", "", configFile);
                }
                else
                {
                    MessageBox.Show("Файл конфигурации не найден: " + configFile);
                }
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            string fileExe = dirPath + "\\App3.exe";
            if (File.Exists(fileExe))
            {
                Utils.RunApplication(fileExe);
                Close();
            }
            else
            {
                Logger.Instance.WriteToLog("Исполняемый файл не найден: " + fileExe);
                log.Items.Add("Исполняемый файл не найден: " + fileExe);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.InitialDirectory = Utils.AppDirectory() + @"\sources";
            fileDialog.Filter = "Архив с программой (.zip)|spolox_exe.zip";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                postgisPath.Text = fileDialog.FileName;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                fbd.RootFolder = Environment.SpecialFolder.MyComputer;
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    dirPath.Text = fbd.SelectedPath;
                }
            }
        }
    }

    [Serializable]
    public partial class ComboboxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public ComboboxItem()
        {
            Text = "";
            Value = null;
        }

        public ComboboxItem(string pText, object pValue)
        {
            Text = pText;
            Value = pValue;
        }

        public ComboboxItem(KeyValuePair<int, string> pair)
        {
            Text = pair.Value;
            Value = pair.Key;
        }
    }
}
