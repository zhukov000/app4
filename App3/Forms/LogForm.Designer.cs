using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App3.Forms
{
    partial class LogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private SplitContainer splitContainer1;

        private TreeView treeView1;

        private Panel panel2;

        private ListBox listBox1;

        private Panel panel1;

        private Label label1;

        private ContextMenuStrip contextMenuStrip1;

        private ToolStripMenuItem копироватьToolStripMenuItem;

        private Button button1;


        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new Container();
            this.splitContainer1 = new SplitContainer();
            this.treeView1 = new TreeView();
            this.panel2 = new Panel();
            this.listBox1 = new ListBox();
            this.contextMenuStrip1 = new ContextMenuStrip(this.components);
            this.копироватьToolStripMenuItem = new ToolStripMenuItem();
            this.panel1 = new Panel();
            this.label1 = new Label();
            this.button1 = new Button();
            ((ISupportInitialize)this.splitContainer1).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.splitContainer1.Dock = DockStyle.Fill;
            this.splitContainer1.Location = new Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel2.Controls.Add(this.panel2);
            this.splitContainer1.Panel2.Controls.Add(this.panel1);
            this.splitContainer1.Size = new Size(661, 355);
            this.splitContainer1.SplitterDistance = 220;
            this.splitContainer1.SplitterWidth = 10;
            this.splitContainer1.TabIndex = 0;
            this.treeView1.Dock = DockStyle.Fill;
            this.treeView1.Location = new Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new Size(220, 355);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new TreeViewEventHandler(this.treeView1_AfterSelect);
            this.panel2.Controls.Add(this.listBox1);
            this.panel2.Dock = DockStyle.Fill;
            this.panel2.Location = new Point(0, 81);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(431, 274);
            this.panel2.TabIndex = 1;
            this.listBox1.ContextMenuStrip = this.contextMenuStrip1;
            this.listBox1.Dock = DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new Point(0, 0);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(431, 274);
            this.listBox1.TabIndex = 0;
            this.listBox1.SelectedIndexChanged += new EventHandler(this.listBox1_SelectedIndexChanged);
            this.contextMenuStrip1.Items.AddRange(new ToolStripItem[]
            {
                this.копироватьToolStripMenuItem
            });
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new Size(140, 26);
            this.копироватьToolStripMenuItem.Name = "копироватьToolStripMenuItem";
            this.копироватьToolStripMenuItem.Size = new Size(139, 22);
            this.копироватьToolStripMenuItem.Text = "Копировать";
            this.копироватьToolStripMenuItem.Click += new EventHandler(this.копироватьToolStripMenuItem_Click);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(431, 81);
            this.panel1.TabIndex = 0;
            this.label1.Dock = DockStyle.Fill;
            this.label1.Font = new Font("Microsoft Sans Serif", 11.25f, FontStyle.Italic, GraphicsUnit.Point, 204);
            this.label1.Location = new Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new Size(431, 81);
            this.label1.TabIndex = 0;
            this.button1.Location = new Point(353, 55);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Удалить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(661, 355);
            base.Controls.Add(this.splitContainer1);
            base.Name = "LogForm";
            this.Text = "Доступные файлы логов";
            base.FormClosing += new FormClosingEventHandler(this.LogForm_FormClosing);
            base.Load += new EventHandler(this.LogForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((ISupportInitialize)this.splitContainer1).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            base.ResumeLayout(false);
        }

        #endregion
    }
}