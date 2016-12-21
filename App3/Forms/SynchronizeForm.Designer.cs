using System;
using System.Drawing;
using System.Windows.Forms;

namespace App3.Forms
{
    partial class SynchronizeForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            this.groupBox1 = new GroupBox();
            this.button1 = new Button();
            this.button2 = new Button();
            this.panel1 = new Panel();
            this.button3 = new Button();
            this.groupBox2 = new GroupBox();
            this.listBox1 = new ListBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            base.SuspendLayout();
            this.groupBox1.Dock = DockStyle.Left;
            this.groupBox1.Location = new Point(0, 0);
            this.groupBox1.Margin = new Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new Padding(4);
            this.groupBox1.Size = new Size(426, 140);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройки";
            this.button1.Location = new Point(468, 12);
            this.button1.Name = "button1";
            this.button1.Size = new Size(93, 30);
            this.button1.TabIndex = 1;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.button2.Location = new Point(468, 50);
            this.button2.Name = "button2";
            this.button2.Size = new Size(93, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new EventHandler(this.button2_Click);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = DockStyle.Top;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(606, 140);
            this.panel1.TabIndex = 2;
            this.button3.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Bold, GraphicsUnit.Point, 204);
            this.button3.Location = new Point(433, 104);
            this.button3.Name = "button3";
            this.button3.Size = new Size(159, 30);
            this.button3.TabIndex = 1;
            this.button3.Text = "Синхронизировать";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new EventHandler(this.button3_Click);
            this.groupBox2.Controls.Add(this.listBox1);
            this.groupBox2.Dock = DockStyle.Fill;
            this.groupBox2.Location = new Point(0, 140);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new Size(606, 182);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Журнал выполнения синхронизации";
            this.listBox1.Dock = DockStyle.Fill;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 16;
            this.listBox1.Location = new Point(3, 18);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new Size(600, 161);
            this.listBox1.TabIndex = 0;
            base.AutoScaleDimensions = new SizeF(8f, 16f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(606, 322);
            base.Controls.Add(this.groupBox2);
            base.Controls.Add(this.panel1);
            this.Font = new Font("Microsoft Sans Serif", 9.75f, FontStyle.Regular, GraphicsUnit.Point, 204);
            base.Margin = new Padding(4);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "SynchronizeForm";
            this.Text = "Параметры синхронизации";
            base.FormClosing += new FormClosingEventHandler(this.SynchronizeForm_FormClosing);
            base.Load += new EventHandler(this.SynchronizeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            base.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBox1;
        
    }
}