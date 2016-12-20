namespace App3.Dialog
{
    partial class AddressDialog
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
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.districtBox = new System.Windows.Forms.GroupBox();
            this.localityBox = new System.Windows.Forms.GroupBox();
            this.streetBox = new System.Windows.Forms.GroupBox();
            this.houseBox = new System.Windows.Forms.GroupBox();
            this.hosBox = new System.Windows.Forms.TextBox();
            this.strBox = new System.Windows.Forms.TextBox();
            this.locBox = new System.Windows.Forms.TextBox();
            this.distBox = new System.Windows.Forms.TextBox();
            this.regionBox = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.districtBox.SuspendLayout();
            this.localityBox.SuspendLayout();
            this.streetBox.SuspendLayout();
            this.houseBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 198);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(284, 35);
            this.panel1.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button2.Location = new System.Drawing.Point(197, 6);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 0;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.button1.Location = new System.Drawing.Point(13, 6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Применить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(284, 198);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.districtBox);
            this.groupBox1.Controls.Add(this.regionBox);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(284, 198);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Регион";
            // 
            // districtBox
            // 
            this.districtBox.Controls.Add(this.localityBox);
            this.districtBox.Controls.Add(this.distBox);
            this.districtBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.districtBox.Enabled = false;
            this.districtBox.Location = new System.Drawing.Point(3, 37);
            this.districtBox.Name = "districtBox";
            this.districtBox.Size = new System.Drawing.Size(278, 158);
            this.districtBox.TabIndex = 1;
            this.districtBox.TabStop = false;
            this.districtBox.Text = "Городской округ или район";
            // 
            // localityBox
            // 
            this.localityBox.Controls.Add(this.streetBox);
            this.localityBox.Controls.Add(this.locBox);
            this.localityBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.localityBox.Enabled = false;
            this.localityBox.Location = new System.Drawing.Point(3, 36);
            this.localityBox.Name = "localityBox";
            this.localityBox.Size = new System.Drawing.Size(272, 119);
            this.localityBox.TabIndex = 1;
            this.localityBox.TabStop = false;
            this.localityBox.Text = "Населенный пункт";
            // 
            // streetBox
            // 
            this.streetBox.Controls.Add(this.houseBox);
            this.streetBox.Controls.Add(this.strBox);
            this.streetBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.streetBox.Enabled = false;
            this.streetBox.Location = new System.Drawing.Point(3, 36);
            this.streetBox.Name = "streetBox";
            this.streetBox.Size = new System.Drawing.Size(266, 80);
            this.streetBox.TabIndex = 1;
            this.streetBox.TabStop = false;
            this.streetBox.Text = "Улица (проспект, площадь)";
            // 
            // houseBox
            // 
            this.houseBox.Controls.Add(this.hosBox);
            this.houseBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.houseBox.Enabled = false;
            this.houseBox.Location = new System.Drawing.Point(3, 36);
            this.houseBox.Name = "houseBox";
            this.houseBox.Size = new System.Drawing.Size(260, 41);
            this.houseBox.TabIndex = 1;
            this.houseBox.TabStop = false;
            this.houseBox.Text = "Дом";
            // 
            // hosBox
            // 
            this.hosBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.hosBox.Location = new System.Drawing.Point(3, 16);
            this.hosBox.Name = "hosBox";
            this.hosBox.Size = new System.Drawing.Size(254, 20);
            this.hosBox.TabIndex = 0;
            this.hosBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.hosBox_KeyDown);
            // 
            // strBox
            // 
            
            this.strBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.strBox.Location = new System.Drawing.Point(3, 16);
            
            this.strBox.Name = "strBox";
            
            this.strBox.Size = new System.Drawing.Size(260, 20);
            this.strBox.TabIndex = 0;
            this.strBox.Enter += new System.EventHandler(this.strBox_Enter);
            this.strBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.strBox_KeyDown);
            // 
            // locBox
            // 
            
            
            this.locBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.locBox.Location = new System.Drawing.Point(3, 16);
            
            this.locBox.Name = "locBox";
            
            this.locBox.Size = new System.Drawing.Size(266, 20);
            this.locBox.TabIndex = 0;
            this.locBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.locBox_KeyDown);
            // 
            // distBox
            // 
            
            this.distBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.distBox.Location = new System.Drawing.Point(3, 16);
            
            this.distBox.Name = "distBox";
            
            this.distBox.Size = new System.Drawing.Size(272, 20);
            this.distBox.TabIndex = 0;
            this.distBox.Enter += new System.EventHandler(this.distBox_Enter);
            this.distBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.distBox_KeyDown);
            // 
            // regionBox
            // 
            this.regionBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.regionBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.regionBox.FormattingEnabled = true;
            this.regionBox.Location = new System.Drawing.Point(3, 16);
            this.regionBox.Name = "regionBox";
            this.regionBox.Size = new System.Drawing.Size(278, 21);
            this.regionBox.TabIndex = 0;
            this.regionBox.SelectedValueChanged += new System.EventHandler(this.regionBox_SelectedValueChanged);
            // 
            // AddressForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 233);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "AddressForm";
            this.Text = "Введите адрес";
            this.Load += new System.EventHandler(this.AddressForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.districtBox.ResumeLayout(false);
            this.districtBox.PerformLayout();
            this.localityBox.ResumeLayout(false);
            this.localityBox.PerformLayout();
            this.streetBox.ResumeLayout(false);
            this.streetBox.PerformLayout();
            this.houseBox.ResumeLayout(false);
            this.houseBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox regionBox;
        private System.Windows.Forms.GroupBox districtBox;
        private System.Windows.Forms.TextBox distBox;
        private System.Windows.Forms.GroupBox localityBox;
        private System.Windows.Forms.TextBox locBox;
        private System.Windows.Forms.GroupBox streetBox;
        private System.Windows.Forms.TextBox strBox;
        private System.Windows.Forms.GroupBox houseBox;
        private System.Windows.Forms.TextBox hosBox;
    }
}