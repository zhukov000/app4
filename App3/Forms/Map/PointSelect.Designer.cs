namespace App3.Forms.Map
{
    partial class PointSelect
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.districtBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mapBox = new SharpMap.Forms.MapBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.buildBox = new System.Windows.Forms.CheckBox();
            this.plgBox = new System.Windows.Forms.CheckBox();
            this.roadBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.labelTime = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelTime);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.districtBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(753, 56);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Параметры поиска";
            // 
            // districtBox
            // 
            this.districtBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.districtBox.FormattingEnabled = true;
            this.districtBox.Location = new System.Drawing.Point(117, 20);
            this.districtBox.Name = "districtBox";
            this.districtBox.Size = new System.Drawing.Size(164, 24);
            this.districtBox.TabIndex = 1;
            this.districtBox.SelectedIndexChanged += new System.EventHandler(this.districtBox_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Район поиска";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.mapBox);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 56);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(753, 500);
            this.panel1.TabIndex = 1;
            // 
            // mapBox
            // 
            this.mapBox.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
            this.mapBox.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mapBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox.FineZoomFactor = 10D;
            this.mapBox.Location = new System.Drawing.Point(0, 49);
            this.mapBox.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.LayerByIndex;
            this.mapBox.Name = "mapBox";
            this.mapBox.PanOnClick = false;
            this.mapBox.PreviewMode = SharpMap.Forms.MapBox.PreviewModes.Fast;
            this.mapBox.QueryGrowFactor = 5F;
            this.mapBox.QueryLayerIndex = 0;
            this.mapBox.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox.ShowProgressUpdate = true;
            this.mapBox.Size = new System.Drawing.Size(753, 451);
            this.mapBox.TabIndex = 1;
            this.mapBox.Text = "mapBox1";
            this.mapBox.WheelZoomMagnitude = -2D;
            this.mapBox.ZoomToPointer = false;
            this.mapBox.MouseDown += new SharpMap.Forms.MapBox.MouseEventHandler(this.mapBox_MouseDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.button1);
            this.panel2.Controls.Add(this.roadBox);
            this.panel2.Controls.Add(this.plgBox);
            this.panel2.Controls.Add(this.buildBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(349, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 33);
            this.panel2.TabIndex = 2;
            this.panel2.Visible = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(753, 49);
            this.panel3.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(753, 49);
            this.label2.TabIndex = 0;
            this.label2.Text = "Выберите регион поиска, масштабируйте и двигайте карту, выбрав точку нажмите на н" +
    "ей правой клавишей мыши";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // buildBox
            // 
            this.buildBox.AutoSize = true;
            this.buildBox.Checked = true;
            this.buildBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.buildBox.Location = new System.Drawing.Point(50, 7);
            this.buildBox.Name = "buildBox";
            this.buildBox.Size = new System.Drawing.Size(61, 20);
            this.buildBox.TabIndex = 0;
            this.buildBox.Text = "Дома";
            this.buildBox.UseVisualStyleBackColor = true;
            // 
            // plgBox
            // 
            this.plgBox.AutoSize = true;
            this.plgBox.Checked = true;
            this.plgBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.plgBox.Location = new System.Drawing.Point(117, 7);
            this.plgBox.Name = "plgBox";
            this.plgBox.Size = new System.Drawing.Size(92, 20);
            this.plgBox.TabIndex = 0;
            this.plgBox.Text = "Полигоны";
            this.plgBox.UseVisualStyleBackColor = true;
            // 
            // roadBox
            // 
            this.roadBox.AutoSize = true;
            this.roadBox.Checked = true;
            this.roadBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.roadBox.Location = new System.Drawing.Point(215, 7);
            this.roadBox.Name = "roadBox";
            this.roadBox.Size = new System.Drawing.Size(74, 20);
            this.roadBox.TabIndex = 0;
            this.roadBox.Text = "Дороги";
            this.roadBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(295, 5);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(97, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Обновить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // labelTime
            // 
            this.labelTime.AutoSize = true;
            this.labelTime.Location = new System.Drawing.Point(298, 26);
            this.labelTime.Name = "labelTime";
            this.labelTime.Size = new System.Drawing.Size(0, 16);
            this.labelTime.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.panel4.Controls.Add(this.pictureBox1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 49);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(753, 451);
            this.panel4.TabIndex = 3;
            this.panel4.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::App3.Properties.Resources.loading;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(753, 451);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // PointSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 556);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PointSelect";
            this.Text = "Выбор точки";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PointSelect_FormClosing);
            this.Load += new System.EventHandler(this.PointSelect_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox districtBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private SharpMap.Forms.MapBox mapBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox buildBox;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox roadBox;
        private System.Windows.Forms.CheckBox plgBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label labelTime;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}