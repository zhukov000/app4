namespace App3.Forms
{
    partial class WarningForm
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.EventId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.District = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ObjName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phone = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.обработаноToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.отменитьТревогуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.тестоваяСработкаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.кОбъектуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.button5 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.Location = new System.Drawing.Point(12, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(118, 44);
            this.button1.TabIndex = 0;
            this.button1.Text = "Закрыть";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 263);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(954, 59);
            this.panel1.TabIndex = 2;
            // 
            // button6
            // 
            this.button6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button6.Location = new System.Drawing.Point(820, 9);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(120, 44);
            this.button6.TabIndex = 2;
            this.button6.Text = "Тестовая сработка";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button4
            // 
            this.button4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button4.Location = new System.Drawing.Point(696, 9);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(120, 44);
            this.button4.TabIndex = 2;
            this.button4.Text = "Отменить тревогу";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button3.Location = new System.Drawing.Point(571, 9);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(120, 44);
            this.button3.TabIndex = 2;
            this.button3.Text = "Подтвердить тревогу";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button2.Location = new System.Drawing.Point(260, 9);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(120, 44);
            this.button2.TabIndex = 2;
            this.button2.Text = "Отключить звук";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EventId,
            this.id,
            this.Number,
            this.District,
            this.ObjName,
            this.Message,
            this.Type,
            this.phone,
            this.time});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.Desktop;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 80;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black;
            this.dataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Size = new System.Drawing.Size(954, 263);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentClick);
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView1_MouseDown);
            // 
            // EventId
            // 
            this.EventId.HeaderText = "idсообщение";
            this.EventId.Name = "EventId";
            this.EventId.Visible = false;
            // 
            // id
            // 
            this.id.HeaderText = "idобъекта";
            this.id.Name = "id";
            this.id.Visible = false;
            // 
            // Number
            // 
            this.Number.FillWeight = 60F;
            this.Number.HeaderText = "Номер";
            this.Number.Name = "Number";
            this.Number.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Number.Width = 60;
            // 
            // District
            // 
            this.District.FillWeight = 150F;
            this.District.HeaderText = "Район";
            this.District.Name = "District";
            this.District.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.District.Width = 150;
            // 
            // ObjName
            // 
            this.ObjName.FillWeight = 150F;
            this.ObjName.HeaderText = "Название";
            this.ObjName.Name = "ObjName";
            this.ObjName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ObjName.Width = 150;
            // 
            // Message
            // 
            this.Message.FillWeight = 250F;
            this.Message.HeaderText = "Сообщение";
            this.Message.Name = "Message";
            this.Message.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Message.Width = 250;
            // 
            // Type
            // 
            this.Type.HeaderText = "Тип сообщения";
            this.Type.Name = "Type";
            this.Type.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Type.Width = 120;
            // 
            // phone
            // 
            this.phone.HeaderText = "Телефон";
            this.phone.Name = "phone";
            this.phone.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            // 
            // time
            // 
            this.time.HeaderText = "Дата/Время";
            this.time.Name = "time";
            this.time.Width = 120;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.обработаноToolStripMenuItem,
            this.отменитьТревогуToolStripMenuItem,
            this.тестоваяСработкаToolStripMenuItem,
            this.toolStripSeparator1,
            this.кОбъектуToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(190, 98);
            // 
            // обработаноToolStripMenuItem
            // 
            this.обработаноToolStripMenuItem.Name = "обработаноToolStripMenuItem";
            this.обработаноToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.обработаноToolStripMenuItem.Text = "Подтвердить тревогу";
            this.обработаноToolStripMenuItem.Click += new System.EventHandler(this.обработаноToolStripMenuItem_Click);
            // 
            // отменитьТревогуToolStripMenuItem
            // 
            this.отменитьТревогуToolStripMenuItem.Name = "отменитьТревогуToolStripMenuItem";
            this.отменитьТревогуToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.отменитьТревогуToolStripMenuItem.Text = "Отменить тревогу";
            // 
            // тестоваяСработкаToolStripMenuItem
            // 
            this.тестоваяСработкаToolStripMenuItem.Name = "тестоваяСработкаToolStripMenuItem";
            this.тестоваяСработкаToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.тестоваяСработкаToolStripMenuItem.Text = "Тестовая сработка";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(186, 6);
            // 
            // кОбъектуToolStripMenuItem
            // 
            this.кОбъектуToolStripMenuItem.Name = "кОбъектуToolStripMenuItem";
            this.кОбъектуToolStripMenuItem.Size = new System.Drawing.Size(189, 22);
            this.кОбъектуToolStripMenuItem.Text = "Карточка объекта";
            this.кОбъектуToolStripMenuItem.Click += new System.EventHandler(this.кОбъектуToolStripMenuItem_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(136, 9);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(118, 44);
            this.button5.TabIndex = 3;
            this.button5.Text = "Очистить экран";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // WarningForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(954, 322);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "WarningForm";
            this.Text = "Предупреждение";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WarningForm_FormClosing);
            this.Load += new System.EventHandler(this.WarningForm_Load);
            this.Shown += new System.EventHandler(this.WarningForm_Shown);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem обработаноToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem кОбъектуToolStripMenuItem;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ToolStripMenuItem отменитьТревогуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem тестоваяСработкаToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridViewTextBoxColumn EventId;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn District;
        private System.Windows.Forms.DataGridViewTextBoxColumn ObjName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Message;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn phone;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.Button button5;
    }
}