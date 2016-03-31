namespace App3.Forms
{
    partial class ConfigForm
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
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.panel8 = new System.Windows.Forms.Panel();
            this.button3 = new System.Windows.Forms.Button();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.MGruop = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.statusTable = new App3.Controls.DBTable();
            this.stateTable = new App3.Controls.DBTable();
            this.panel7 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dbTable1 = new App3.Controls.DBTable();
            this.panel6 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.panel9 = new System.Windows.Forms.Panel();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.dbTable2 = new App3.Controls.DBTable();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel7.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.panel6.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.panel9.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.checkBox4);
            this.panel1.Controls.Add(this.checkBox3);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(683, 211);
            this.panel1.TabIndex = 0;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox2.Location = new System.Drawing.Point(16, 176);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(274, 20);
            this.checkBox2.TabIndex = 5;
            this.checkBox2.Text = "Журналирование системных событий";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox1.Location = new System.Drawing.Point(16, 150);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(372, 20);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "Принимать соединения ОКО (требуется перезапуск)";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(282, 76);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(96, 29);
            this.button2.TabIndex = 3;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(180, 76);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(96, 29);
            this.button1.TabIndex = 3;
            this.button1.Text = "Записать";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox1.Location = new System.Drawing.Point(132, 48);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(246, 22);
            this.textBox1.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(132, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(246, 24);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(83, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Значение";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(12, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Имя свойства";
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(697, 246);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(689, 217);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Цвета";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.panel8);
            this.groupBox1.Controls.Add(this.button3);
            this.groupBox1.Controls.Add(this.numericUpDown2);
            this.groupBox1.Controls.Add(this.numericUpDown1);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.panel5);
            this.groupBox1.Controls.Add(this.panel4);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(133, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(553, 211);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Состояние региона";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label6.Location = new System.Drawing.Point(355, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(153, 22);
            this.label6.TabIndex = 6;
            this.label6.Text = "нет работающих";
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel8.Location = new System.Drawing.Point(288, 34);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(41, 40);
            this.panel8.TabIndex = 5;
            this.panel8.DoubleClick += new System.EventHandler(this.colorPanel_DoubleClick);
            // 
            // button3
            // 
            this.button3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button3.Location = new System.Drawing.Point(162, 161);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 4;
            this.button3.Text = "Сохранить";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(178, 106);
            this.numericUpDown2.Maximum = new decimal(new int[] {
            98,
            0,
            0,
            0});
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(46, 20);
            this.numericUpDown2.TabIndex = 3;
            this.numericUpDown2.ValueChanged += new System.EventHandler(this.numericUpDown2_ValueChanged);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(178, 45);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(46, 20);
            this.numericUpDown1.TabIndex = 3;
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(64, 162);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 22);
            this.label5.TabIndex = 2;
            this.label5.Text = "плохо";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(65, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(99, 22);
            this.label4.TabIndex = 2;
            this.label4.Text = "проблемы";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(65, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "норма";
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Location = new System.Drawing.Point(17, 154);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(41, 40);
            this.panel5.TabIndex = 1;
            this.panel5.DoubleClick += new System.EventHandler(this.colorPanel_DoubleClick);
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Location = new System.Drawing.Point(17, 94);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(41, 40);
            this.panel4.TabIndex = 1;
            this.panel4.DoubleClick += new System.EventHandler(this.colorPanel_DoubleClick);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Location = new System.Drawing.Point(17, 34);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(41, 40);
            this.panel3.TabIndex = 0;
            this.panel3.DoubleClick += new System.EventHandler(this.colorPanel_DoubleClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(130, 211);
            this.panel2.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MGruop,
            this.id});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.Size = new System.Drawing.Size(130, 211);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // MGruop
            // 
            this.MGruop.HeaderText = "Группа сообщений";
            this.MGruop.Name = "MGruop";
            this.MGruop.ReadOnly = true;
            this.MGruop.Width = 125;
            // 
            // id
            // 
            this.id.HeaderText = "Column1";
            this.id.Name = "id";
            this.id.ReadOnly = true;
            this.id.Visible = false;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(689, 217);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Настройки соединения";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.statusTable);
            this.tabPage3.Controls.Add(this.stateTable);
            this.tabPage3.Controls.Add(this.panel7);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(689, 217);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Статистика";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // statusTable
            // 
            this.statusTable.CanAdd = false;
            this.statusTable.CanDel = false;
            this.statusTable.CanEdit = false;
            this.statusTable.Dock = System.Windows.Forms.DockStyle.Right;
            this.statusTable.Filter = "";
            this.statusTable.Location = new System.Drawing.Point(467, 0);
            this.statusTable.Name = "statusTable";
            this.statusTable.Size = new System.Drawing.Size(222, 179);
            this.statusTable.TabIndex = 4;
            this.statusTable.TableName = "oko.tstatus";
            // 
            // stateTable
            // 
            this.stateTable.CanAdd = false;
            this.stateTable.CanDel = false;
            this.stateTable.CanEdit = false;
            this.stateTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.stateTable.Filter = "";
            this.stateTable.Location = new System.Drawing.Point(0, 0);
            this.stateTable.Name = "stateTable";
            this.stateTable.Size = new System.Drawing.Size(689, 179);
            this.stateTable.TabIndex = 3;
            this.stateTable.TableName = "oko.tstate";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.button6);
            this.panel7.Controls.Add(this.button7);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel7.Location = new System.Drawing.Point(0, 179);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(689, 38);
            this.panel7.TabIndex = 1;
            // 
            // button6
            // 
            this.button6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button6.Location = new System.Drawing.Point(100, 7);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 23);
            this.button6.TabIndex = 1;
            this.button6.Text = "Отмена";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button7.Location = new System.Drawing.Point(19, 7);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(75, 23);
            this.button7.TabIndex = 0;
            this.button7.Text = "Сохранить";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.dbTable1);
            this.tabPage4.Controls.Add(this.panel6);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(689, 217);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "События";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dbTable1
            // 
            this.dbTable1.CanAdd = false;
            this.dbTable1.CanDel = false;
            this.dbTable1.CanEdit = true;
            this.dbTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTable1.Filter = "";
            this.dbTable1.Location = new System.Drawing.Point(0, 0);
            this.dbTable1.Name = "dbTable1";
            this.dbTable1.Size = new System.Drawing.Size(689, 179);
            this.dbTable1.TabIndex = 1;
            this.dbTable1.TableName = "oko.tstate";
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.button5);
            this.panel6.Controls.Add(this.button4);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel6.Location = new System.Drawing.Point(0, 179);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(689, 38);
            this.panel6.TabIndex = 0;
            // 
            // button5
            // 
            this.button5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button5.Location = new System.Drawing.Point(100, 7);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 23);
            this.button5.TabIndex = 1;
            this.button5.Text = "Отмена";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button4
            // 
            this.button4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button4.Location = new System.Drawing.Point(19, 7);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 0;
            this.button4.Text = "Сохранить";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.panel9);
            this.tabPage5.Controls.Add(this.dbTable2);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(689, 217);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Константы";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.button8);
            this.panel9.Controls.Add(this.button9);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(0, 179);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(689, 38);
            this.panel9.TabIndex = 1;
            // 
            // button8
            // 
            this.button8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button8.Location = new System.Drawing.Point(100, 7);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(75, 23);
            this.button8.TabIndex = 1;
            this.button8.Text = "Отмена";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button9.Location = new System.Drawing.Point(19, 7);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 0;
            this.button9.Text = "Сохранить";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // dbTable2
            // 
            this.dbTable2.CanAdd = false;
            this.dbTable2.CanDel = false;
            this.dbTable2.CanEdit = true;
            this.dbTable2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTable2.Filter = "";
            this.dbTable2.Location = new System.Drawing.Point(0, 0);
            this.dbTable2.Name = "dbTable2";
            this.dbTable2.Size = new System.Drawing.Size(689, 217);
            this.dbTable2.TabIndex = 0;
            this.dbTable2.TableName = "settings";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox3.Location = new System.Drawing.Point(406, 11);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(260, 20);
            this.checkBox3.TabIndex = 6;
            this.checkBox3.Text = "Автоматическая синхронизация БД";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.checkBox4.Location = new System.Drawing.Point(406, 37);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(259, 20);
            this.checkBox4.TabIndex = 7;
            this.checkBox4.Text = "Помещение событий в архив через:";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // textBox2
            // 
            this.textBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBox2.Location = new System.Drawing.Point(567, 66);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(41, 22);
            this.textBox2.TabIndex = 8;
            this.textBox2.Text = "60";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label7.Location = new System.Drawing.Point(614, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 16);
            this.label7.TabIndex = 9;
            this.label7.Text = "дней";
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(697, 246);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ConfigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Настройки приложения";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ConfigForm_FormClosing);
            this.Load += new System.EventHandler(this.ConfigForm_Load);
            this.Enter += new System.EventHandler(this.ConfigForm_Enter);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel7.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridViewTextBoxColumn MGruop;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private Controls.DBTable dbTable1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private Controls.DBTable statusTable;
        private Controls.DBTable stateTable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel8;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Panel panel9;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private Controls.DBTable dbTable2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
    }
}