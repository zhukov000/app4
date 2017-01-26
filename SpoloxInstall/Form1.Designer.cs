namespace SpoloxInstall
{
    partial class Settings
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.postgresqlPath = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.postgisPath = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.backupPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button4 = new System.Windows.Forms.Button();
            this.arhivePath = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.dirPath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.municName = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dbName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.dbPass = new System.Windows.Forms.TextBox();
            this.portName = new System.Windows.Forms.TextBox();
            this.ipPort = new System.Windows.Forms.TextBox();
            this.ipAddress = new System.Windows.Forms.TextBox();
            this.dbUser = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.ListBox();
            this.button11 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.pgbinPath = new System.Windows.Forms.TextBox();
            this.button12 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(169, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Путь к установщику PostgreSQL";
            // 
            // postgresqlPath
            // 
            this.postgresqlPath.Location = new System.Drawing.Point(16, 30);
            this.postgresqlPath.Name = "postgresqlPath";
            this.postgresqlPath.Size = new System.Drawing.Size(406, 20);
            this.postgresqlPath.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(428, 28);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "Обзор";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(427, 77);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 5;
            this.button2.Text = "Обзор";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // postgisPath
            // 
            this.postgisPath.Location = new System.Drawing.Point(15, 79);
            this.postgisPath.Name = "postgisPath";
            this.postgisPath.Size = new System.Drawing.Size(406, 20);
            this.postgisPath.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(151, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Путь к установщику PostGIS";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(427, 126);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 8;
            this.button3.Text = "Обзор";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // backupPath
            // 
            this.backupPath.Location = new System.Drawing.Point(15, 128);
            this.backupPath.Name = "backupPath";
            this.backupPath.Size = new System.Drawing.Size(406, 20);
            this.backupPath.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Путь к бэкапу БД";
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(427, 220);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(75, 23);
            this.button4.TabIndex = 11;
            this.button4.Text = "Обзор";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // arhivePath
            // 
            this.arhivePath.Location = new System.Drawing.Point(15, 222);
            this.arhivePath.Name = "arhivePath";
            this.arhivePath.Size = new System.Drawing.Size(406, 20);
            this.arhivePath.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 205);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(139, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Путь к архиву программы";
            // 
            // button5
            // 
            this.button5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button5.Location = new System.Drawing.Point(15, 298);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(148, 40);
            this.button5.TabIndex = 12;
            this.button5.Text = "1. PostgreSQL";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button6.Location = new System.Drawing.Point(15, 344);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(148, 40);
            this.button6.TabIndex = 12;
            this.button6.Text = "2. PostGIS";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button7.Location = new System.Drawing.Point(15, 390);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(148, 40);
            this.button7.TabIndex = 12;
            this.button7.Text = "3. БД";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button8.Location = new System.Drawing.Point(15, 436);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(148, 40);
            this.button8.TabIndex = 12;
            this.button8.Text = "4. Копирование";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
            // 
            // button9
            // 
            this.button9.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button9.Location = new System.Drawing.Point(15, 482);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(148, 40);
            this.button9.TabIndex = 12;
            this.button9.Text = "5. Конфигурация";
            this.button9.UseVisualStyleBackColor = true;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(427, 262);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(75, 23);
            this.button10.TabIndex = 15;
            this.button10.Text = "Обзор";
            this.button10.UseVisualStyleBackColor = true;
            this.button10.Click += new System.EventHandler(this.button10_Click);
            // 
            // dirPath
            // 
            this.dirPath.Location = new System.Drawing.Point(15, 264);
            this.dirPath.Name = "dirPath";
            this.dirPath.Size = new System.Drawing.Size(406, 20);
            this.dirPath.TabIndex = 14;
            this.dirPath.Text = "C:\\Spolox";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 247);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(159, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Целевая папка с программой";
            // 
            // municName
            // 
            this.municName.FormattingEnabled = true;
            this.municName.Location = new System.Drawing.Point(168, 137);
            this.municName.Name = "municName";
            this.municName.Size = new System.Drawing.Size(153, 21);
            this.municName.TabIndex = 16;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 140);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "Муниципалитет";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dbName);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.dbPass);
            this.groupBox1.Controls.Add(this.portName);
            this.groupBox1.Controls.Add(this.ipPort);
            this.groupBox1.Controls.Add(this.ipAddress);
            this.groupBox1.Controls.Add(this.dbUser);
            this.groupBox1.Controls.Add(this.municName);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Location = new System.Drawing.Point(179, 298);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 165);
            this.groupBox1.TabIndex = 17;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Конфигурация";
            // 
            // dbName
            // 
            this.dbName.Location = new System.Drawing.Point(168, 118);
            this.dbName.Name = "dbName";
            this.dbName.Size = new System.Drawing.Size(153, 20);
            this.dbName.TabIndex = 19;
            this.dbName.Text = "gis";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(16, 121);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Название БД";
            // 
            // dbPass
            // 
            this.dbPass.Location = new System.Drawing.Point(168, 43);
            this.dbPass.Name = "dbPass";
            this.dbPass.Size = new System.Drawing.Size(153, 20);
            this.dbPass.TabIndex = 17;
            this.dbPass.Text = "s567507";
            // 
            // portName
            // 
            this.portName.Location = new System.Drawing.Point(168, 99);
            this.portName.Name = "portName";
            this.portName.Size = new System.Drawing.Size(153, 20);
            this.portName.TabIndex = 17;
            this.portName.Text = "COM3";
            // 
            // ipPort
            // 
            this.ipPort.Location = new System.Drawing.Point(168, 80);
            this.ipPort.Name = "ipPort";
            this.ipPort.Size = new System.Drawing.Size(153, 20);
            this.ipPort.TabIndex = 17;
            this.ipPort.Text = "9090";
            // 
            // ipAddress
            // 
            this.ipAddress.Location = new System.Drawing.Point(168, 61);
            this.ipAddress.Name = "ipAddress";
            this.ipAddress.Size = new System.Drawing.Size(153, 20);
            this.ipAddress.TabIndex = 17;
            this.ipAddress.Text = "80.254.127.230";
            // 
            // dbUser
            // 
            this.dbUser.Location = new System.Drawing.Point(168, 24);
            this.dbUser.Name = "dbUser";
            this.dbUser.Size = new System.Drawing.Size(153, 20);
            this.dbUser.TabIndex = 17;
            this.dbUser.Text = "postgres";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(16, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(57, 13);
            this.label11.TabIndex = 13;
            this.label11.Text = "COM-порт";
            this.label11.Click += new System.EventHandler(this.label6_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(16, 83);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 13);
            this.label10.TabIndex = 13;
            this.label10.Text = "Порт сервера";
            this.label10.Click += new System.EventHandler(this.label6_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(16, 64);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 13);
            this.label9.TabIndex = 13;
            this.label9.Text = "IP адрес сервера";
            this.label9.Click += new System.EventHandler(this.label6_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(64, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Пароль БД";
            this.label8.Click += new System.EventHandler(this.label6_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "Пользователь БД";
            this.label7.Click += new System.EventHandler(this.label6_Click);
            // 
            // log
            // 
            this.log.FormattingEnabled = true;
            this.log.Location = new System.Drawing.Point(179, 469);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(323, 95);
            this.log.TabIndex = 18;
            // 
            // button11
            // 
            this.button11.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button11.Location = new System.Drawing.Point(15, 528);
            this.button11.Name = "button11";
            this.button11.Size = new System.Drawing.Size(148, 40);
            this.button11.TabIndex = 12;
            this.button11.Text = "6. Запуск";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 158);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 13);
            this.label13.TabIndex = 6;
            this.label13.Text = "Путь к psql";
            // 
            // pgbinPath
            // 
            this.pgbinPath.Location = new System.Drawing.Point(15, 175);
            this.pgbinPath.Name = "pgbinPath";
            this.pgbinPath.Size = new System.Drawing.Size(406, 20);
            this.pgbinPath.TabIndex = 7;
            this.pgbinPath.Text = "C:\\Program Files\\PostgreSQL\\9.6\\bin\\psql.exe";
            // 
            // button12
            // 
            this.button12.Location = new System.Drawing.Point(427, 173);
            this.button12.Name = "button12";
            this.button12.Size = new System.Drawing.Size(75, 23);
            this.button12.TabIndex = 8;
            this.button12.Text = "Обзор";
            this.button12.UseVisualStyleBackColor = true;
            this.button12.Click += new System.EventHandler(this.button12_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(518, 577);
            this.Controls.Add(this.log);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.dirPath);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.arhivePath);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button12);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.pgbinPath);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.backupPath);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.postgisPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.postgresqlPath);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "Settings";
            this.Text = "Установка Spolox";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox postgresqlPath;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox postgisPath;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.TextBox backupPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.TextBox arhivePath;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.TextBox dirPath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox municName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox dbName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox dbPass;
        private System.Windows.Forms.TextBox portName;
        private System.Windows.Forms.TextBox ipPort;
        private System.Windows.Forms.TextBox ipAddress;
        private System.Windows.Forms.TextBox dbUser;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListBox log;
        private System.Windows.Forms.Button button11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox pgbinPath;
        private System.Windows.Forms.Button button12;
    }
}

