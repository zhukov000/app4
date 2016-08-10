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
using System.Threading;
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class LogForm : Form
    {
        public LogForm(Form pParent)
        {
            InitializeComponent();
            this.MdiParent = pParent;
        }

        /// <summary>
        /// Обновить информацию о файлах
        /// </summary>
        public void UpdateView()
        {
            Logger.Instance.FlushLog();
            string logDir = Logger.LogDirectory();

            treeView1.BeginUpdate();

            treeView1.Nodes.Clear();
            TreeNode spolox = treeView1.Nodes.Add("node", "Сполох");
            TreeNode oko = treeView1.Nodes.Add("node", "ОКО");
            TreeNode launcher = treeView1.Nodes.Add("node", "Запуск");
            // лог сполоха
            foreach(string file in Directory.GetFiles(logDir, "*spolox.log").OrderByDescending(f => f))
            {
                spolox.Nodes.Add(file, getData(file));
                // MessageBox.Show(getData(file) + ";" + file);
            }
            // лог око
            oko.Nodes.Add(logDir + "OKOGate.log", "OKOGate");
            // лог лаунчера
            foreach (string file in Directory.GetFiles(logDir, "*launcher.log"))
            {
                launcher.Nodes.Add(file, getData(file));
            }

            treeView1.EndUpdate();
        }

        private string getData(string file)
        {
            return string.Format("{0}.{1}.{2}",
                    Path.GetFileName(file).Substring(6, 2),
                    Path.GetFileName(file).Substring(4, 2),
                    Path.GetFileName(file).Substring(0, 4));
        }

        private void LogForm_Load(object sender, EventArgs e)
        {

        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void ShowFile()
        {
            if (treeView1.SelectedNode.Name != "node")
            {
                listBox1.Items.Clear();
                label1.Text = "";
                if (File.Exists(treeView1.SelectedNode.Name))
                {
                    using (StreamReader sr = new StreamReader(treeView1.SelectedNode.Name))
                    {
                        while (sr.Peek() >= 0)
                        {
                            listBox1.Items.Add(Utils.Derypt(sr.ReadLine()));
                        }
                    }
                    if (listBox1.Items.Count > 0)
                    {
                        listBox1.SelectedItem = listBox1.Items[listBox1.Items.Count - 1];
                    }
                }
                else
                {
                    listBox1.Items.Add("-= Файл не удалось открыть =-");
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            button1.Visible = false;
            ShowFile();
            button1.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            label1.Text = listBox1.Items[listBox1.SelectedIndex].ToString();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(listBox1.Items[listBox1.SelectedIndex].ToString().Substring(14));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (treeView1.SelectedNode.Name != "node")
            {
                if (File.Exists(treeView1.SelectedNode.Name) && MessageBox.Show("Вы действительно хотите удалить файлы журнала от " + treeView1.SelectedNode.Text, "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(treeView1.SelectedNode.Name);
                    listBox1.Items.Clear();
                    label1.Text = "";
                    treeView1.Nodes.Remove(treeView1.SelectedNode);
                }
            }
            else
            {
                if (MessageBox.Show("Вы действительно хотите удалить файлы журнала " + treeView1.SelectedNode.Text, "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    foreach(TreeNode trNode in treeView1.SelectedNode.Nodes)
                    {
                        if (File.Exists(trNode.Name))
                            File.Delete(trNode.Name);
                    }
                    listBox1.Items.Clear();
                    label1.Text = "";
                    UpdateView();
                }
            }
        }
    }
}
