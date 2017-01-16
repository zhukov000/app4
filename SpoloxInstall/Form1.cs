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
                    Logger.Instance.WriteToLog("Установка postgisPath выполнена ");
                    log.Items.Add("2. Установка PostGIS выполнена");
                    button6.Enabled = false;
                }
                else
                {
                    Logger.Instance.WriteToLog("Установка PostGIS не выполнена");
                    log.Items.Add("- Установка PostGIS не выполнена");
                }
            }
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
            else
            {
                string curDirectory = Utils.AppDirectory();
                string fileName = Path.GetFileNameWithoutExtension(backupPath.Text);
                string filePath = Path.GetDirectoryName(backupPath.Text);
                string sqlFile = filePath + "\\" + fileName + ".sql";

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

                    // bool f = Utils.RunApplication(postgisPath.Text);
                
                    if (MessageBox.Show("Установка завершена успешно?", "Вопрос", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
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
            fileDialog.Filter = "Утилита восстановления (pg_restore.exe)|pg_restore.exe";
            if (fileDialog.ShowDialog() == DialogResult.OK)
            {
                pgbinPath.Text = fileDialog.FileName;
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
