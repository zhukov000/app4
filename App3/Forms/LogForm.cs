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
using System.Windows.Forms;

namespace App3.Forms
{
    public partial class LogForm : Form
    {
        public LogForm()
        {
            this.InitializeComponent();
            base.MdiParent = null;
        }

        public LogForm(Form pParent)
        {
            this.InitializeComponent();
            base.MdiParent = pParent;
        }

        public void UpdateView()
        {
            Logger.Instance.FlushLog();
            string text = Logger.LogDirectory();
            this.treeView1.BeginUpdate();
            this.treeView1.Nodes.Clear();
            TreeNode treeNode = this.treeView1.Nodes.Add("node", "Сполох");
            TreeNode treeNode2 = this.treeView1.Nodes.Add("node", "ОКО");
            TreeNode treeNode3 = this.treeView1.Nodes.Add("node", "Запуск");
            foreach (string current in Directory.GetFiles(text, "*spolox.log").OrderByDescending(p => p))
            {
                treeNode.Nodes.Add(current, this.getData(current));
            }
            treeNode2.Nodes.Add(text + "OKOGate.log", "OKOGate");
            foreach (string current2 in Directory.GetFiles(text, "*launcher.log").OrderByDescending(p => p))
            {
                treeNode3.Nodes.Add(current2, this.getData(current2));
            }
            this.treeView1.EndUpdate();
        }

        private string getData(string file)
        {
            return string.Format("{0}.{1}.{2}", Path.GetFileName(file).Substring(6, 2), Path.GetFileName(file).Substring(4, 2), Path.GetFileName(file).Substring(0, 4));
        }

        private void LogForm_Load(object sender, EventArgs e)
        {
        }

        private void LogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.MdiFormClosing && base.MdiParent != null)
            {
                base.Hide();
                e.Cancel = true;
            }
        }

        private void ShowFile()
        {
            if (this.treeView1.SelectedNode.Name != "node")
            {
                this.listBox1.Items.Clear();
                this.label1.Text = "";
                if (File.Exists(this.treeView1.SelectedNode.Name))
                {
                    using (StreamReader streamReader = new StreamReader(this.treeView1.SelectedNode.Name))
                    {
                        while (streamReader.Peek() >= 0)
                        {
                            string text = streamReader.ReadLine();
                            string item = "";
                            try
                            {
                                item = text.Derypt();
                            }
                            catch (Utils.DecryptException)
                            {
                                item = text;
                            }
                            catch (Exception arg_8C_0)
                            {
                                item = arg_8C_0.Message;
                            }
                            this.listBox1.Items.Add(item);
                        }
                    }
                    if (this.listBox1.Items.Count > 0)
                    {
                        this.listBox1.SelectedItem = this.listBox1.Items[this.listBox1.Items.Count - 1];
                        return;
                    }
                }
                else
                {
                    this.listBox1.Items.Add("-= Файл не удалось открыть =-");
                }
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.button1.Visible = false;
            this.ShowFile();
            this.button1.Visible = true;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.label1.Text = this.listBox1.Items[this.listBox1.SelectedIndex].ToString();
        }

        private void копироватьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(this.listBox1.Items[this.listBox1.SelectedIndex].ToString().Substring(14));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.treeView1.SelectedNode.Name != "node")
            {
                if (File.Exists(this.treeView1.SelectedNode.Name) && MessageBox.Show("Вы действительно хотите удалить файлы журнала от " + this.treeView1.SelectedNode.Text, "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    File.Delete(this.treeView1.SelectedNode.Name);
                    this.listBox1.Items.Clear();
                    this.label1.Text = "";
                    this.treeView1.Nodes.Remove(this.treeView1.SelectedNode);
                    return;
                }
            }
            else if (MessageBox.Show("Вы действительно хотите удалить файлы журнала " + this.treeView1.SelectedNode.Text, "Вопрос", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                foreach (TreeNode treeNode in this.treeView1.SelectedNode.Nodes)
                {
                    if (File.Exists(treeNode.Name))
                    {
                        File.Delete(treeNode.Name);
                    }
                }
                this.listBox1.Items.Clear();
                this.label1.Text = "";
                this.UpdateView();
            }
        }
    }
}
