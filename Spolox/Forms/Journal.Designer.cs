using App3.Controls;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace App3.Forms
{
    partial class Journal
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        private GanttChart ganttChart1;

        private Panel panel1;

        private Panel panel2;

        private DateTimePicker dateTimePicker2;

        private DateTimePicker dateTimePicker1;

        private Button button1;

        private Label label2;

        private Label label1;

        private Panel panel3;

        private Panel panel4;

        private DataGridView dataGridView1;

        private DataGridViewTextBoxColumn colStart;

        private DataGridViewTextBoxColumn colFinish;

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
            this.panel1 = new Panel();
            this.panel2 = new Panel();
            this.label2 = new Label();
            this.label1 = new Label();
            this.dateTimePicker2 = new DateTimePicker();
            this.dateTimePicker1 = new DateTimePicker();
            this.button1 = new Button();
            this.panel3 = new Panel();
            this.panel4 = new Panel();
            this.dataGridView1 = new DataGridView();
            this.colStart = new DataGridViewTextBoxColumn();
            this.colFinish = new DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((ISupportInitialize)this.dataGridView1).BeginInit();
            base.SuspendLayout();
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(446, 230);
            this.panel1.TabIndex = 0;
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dateTimePicker2);
            this.panel2.Controls.Add(this.dateTimePicker1);
            this.panel2.Controls.Add(this.button1);
            this.panel2.Dock = DockStyle.Bottom;
            this.panel2.Location = new Point(0, 230);
            this.panel2.Name = "panel2";
            this.panel2.Size = new Size(446, 64);
            this.panel2.TabIndex = 1;
            this.label2.AutoSize = true;
            this.label2.Location = new Point(162, 13);
            this.label2.Name = "label2";
            this.label2.Size = new Size(67, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Заканчивая";
            this.label1.AutoSize = true;
            this.label1.Location = new Point(19, 13);
            this.label1.Name = "label1";
            this.label1.Size = new Size(59, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Начиная с";
            this.dateTimePicker2.Location = new Point(156, 32);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new Size(130, 20);
            this.dateTimePicker2.TabIndex = 1;
            this.dateTimePicker1.Location = new Point(13, 32);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new Size(130, 20);
            this.dateTimePicker1.TabIndex = 1;
            this.button1.Location = new Point(302, 16);
            this.button1.Name = "button1";
            this.button1.Size = new Size(130, 36);
            this.button1.TabIndex = 0;
            this.button1.Text = "Показать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new EventHandler(this.button1_Click);
            this.panel3.Dock = DockStyle.Top;
            this.panel3.Location = new Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new Size(446, 87);
            this.panel3.TabIndex = 0;
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Dock = DockStyle.Fill;
            this.panel4.Location = new Point(0, 87);
            this.panel4.Name = "panel4";
            this.panel4.Size = new Size(446, 143);
            this.panel4.TabIndex = 1;
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new DataGridViewColumn[]
            {
                this.colStart,
                this.colFinish
            });
            this.dataGridView1.Dock = DockStyle.Fill;
            this.dataGridView1.Location = new Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new Size(446, 143);
            this.dataGridView1.TabIndex = 0;
            this.colStart.HeaderText = "Начало";
            this.colStart.Name = "colStart";
            this.colStart.ReadOnly = true;
            this.colStart.Width = 200;
            this.colFinish.HeaderText = "Конец";
            this.colFinish.Name = "colFinish";
            this.colFinish.ReadOnly = true;
            this.colFinish.Width = 200;
            base.AutoScaleDimensions = new SizeF(6f, 13f);
            base.AutoScaleMode = AutoScaleMode.Font;
            base.ClientSize = new Size(446, 294);
            base.Controls.Add(this.panel1);
            base.Controls.Add(this.panel2);
            base.MaximizeBox = false;
            base.MinimizeBox = false;
            base.Name = "Journal";
            this.Text = "Журнал работы программы";
            base.FormClosing += new FormClosingEventHandler(this.Journal_FormClosing);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((ISupportInitialize)this.dataGridView1).EndInit();
            base.ResumeLayout(false);
        }

        #endregion
    }
}