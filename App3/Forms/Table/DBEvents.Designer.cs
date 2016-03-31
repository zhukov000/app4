namespace App3.Forms
{
    partial class DBEvents
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.panel5 = new System.Windows.Forms.Panel();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.panel4 = new System.Windows.Forms.Panel();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.endTimePicker = new System.Windows.Forms.DateTimePicker();
            this.begTimePicker = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(692, 185);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(692, 185);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(692, 77);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(692, 77);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Настройка выборки";
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Image = global::App3.Properties.Resources.FilteredObject_13400_32x;
            this.button1.Location = new System.Drawing.Point(631, 16);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(49, 52);
            this.button1.TabIndex = 9;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.comboBox2);
            this.panel5.Controls.Add(this.radioButton4);
            this.panel5.Controls.Add(this.radioButton3);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel5.Location = new System.Drawing.Point(390, 16);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(233, 58);
            this.panel5.TabIndex = 8;
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Enabled = false;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(68, 31);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(155, 21);
            this.comboBox2.TabIndex = 6;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(16, 32);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(46, 17);
            this.radioButton4.TabIndex = 5;
            this.radioButton4.TabStop = true;
            this.radioButton4.Text = "Для";
            this.radioButton4.UseVisualStyleBackColor = true;
            this.radioButton4.CheckedChanged += new System.EventHandler(this.radioButton4_CheckedChanged);
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Checked = true;
            this.radioButton3.Location = new System.Drawing.Point(16, 9);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(207, 17);
            this.radioButton3.TabIndex = 5;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "Вывести события для всех районов";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.checkBox2);
            this.panel4.Controls.Add(this.checkBox1);
            this.panel4.Controls.Add(this.endTimePicker);
            this.panel4.Controls.Add(this.begTimePicker);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(156, 16);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(234, 58);
            this.panel4.TabIndex = 7;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(7, 33);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(86, 17);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "Заканчивая";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(7, 9);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(69, 17);
            this.checkBox1.TabIndex = 5;
            this.checkBox1.Text = "Начиная";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // endTimePicker
            // 
            this.endTimePicker.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.endTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.endTimePicker.Location = new System.Drawing.Point(93, 32);
            this.endTimePicker.Name = "endTimePicker";
            this.endTimePicker.Size = new System.Drawing.Size(138, 20);
            this.endTimePicker.TabIndex = 4;
            // 
            // begTimePicker
            // 
            this.begTimePicker.CustomFormat = "dd.MM.yyyy HH:mm:ss";
            this.begTimePicker.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.begTimePicker.Location = new System.Drawing.Point(93, 6);
            this.begTimePicker.MinDate = new System.DateTime(2015, 1, 1, 0, 0, 0, 0);
            this.begTimePicker.Name = "begTimePicker";
            this.begTimePicker.Size = new System.Drawing.Size(138, 20);
            this.begTimePicker.TabIndex = 4;
            this.begTimePicker.Value = new System.DateTime(2015, 4, 24, 22, 34, 0, 0);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.comboBox1);
            this.panel3.Controls.Add(this.radioButton2);
            this.panel3.Controls.Add(this.radioButton1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(153, 58);
            this.panel3.TabIndex = 6;
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "10",
            "50",
            "100",
            "200",
            "500",
            "1000"});
            this.comboBox1.Location = new System.Drawing.Point(92, 31);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(54, 21);
            this.comboBox1.TabIndex = 5;
            this.comboBox1.Text = "200";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Checked = true;
            this.radioButton2.Location = new System.Drawing.Point(10, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(81, 17);
            this.radioButton2.TabIndex = 4;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "Последние";
            this.radioButton2.UseVisualStyleBackColor = true;
            this.radioButton2.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged_1);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(10, 9);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(136, 17);
            this.radioButton1.TabIndex = 3;
            this.radioButton1.Text = "Вывести все события";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // DBEvents
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 262);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "DBEvents";
            this.Text = "Полученные события";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DBEvents_FormClosing);
            this.Load += new System.EventHandler(this.DBEvents_Load);
            this.Enter += new System.EventHandler(this.DBEvents_Enter);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DateTimePicker begTimePicker;
        private System.Windows.Forms.DateTimePicker endTimePicker;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}