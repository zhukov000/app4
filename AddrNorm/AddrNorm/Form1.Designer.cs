namespace AddrNorm
{
    partial class Form1
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
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.kladrBox = new System.Windows.Forms.TextBox();
            this.houseBox = new System.Windows.Forms.TextBox();
            this.streetBox = new System.Windows.Forms.TextBox();
            this.locBox = new System.Windows.Forms.TextBox();
            this.disBox = new System.Windows.Forms.TextBox();
            this.regBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(194, 58);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = ">>>";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(169, 262);
            this.panel1.TabIndex = 3;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(0, 0);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(169, 262);
            this.textBox1.TabIndex = 1;
            this.textBox1.Text = "Г. БЕЛАЯ КАЛИТВА, УЛ. РОССИЙСКАЯ, 5";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.kladrBox);
            this.panel2.Controls.Add(this.houseBox);
            this.panel2.Controls.Add(this.streetBox);
            this.panel2.Controls.Add(this.locBox);
            this.panel2.Controls.Add(this.disBox);
            this.panel2.Controls.Add(this.regBox);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel2.Location = new System.Drawing.Point(296, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(372, 262);
            this.panel2.TabIndex = 4;
            // 
            // kladrBox
            // 
            this.kladrBox.Location = new System.Drawing.Point(29, 230);
            this.kladrBox.Name = "kladrBox";
            this.kladrBox.Size = new System.Drawing.Size(306, 20);
            this.kladrBox.TabIndex = 0;
            // 
            // houseBox
            // 
            this.houseBox.Location = new System.Drawing.Point(29, 187);
            this.houseBox.Name = "houseBox";
            this.houseBox.Size = new System.Drawing.Size(306, 20);
            this.houseBox.TabIndex = 0;
            // 
            // streetBox
            // 
            this.streetBox.Location = new System.Drawing.Point(29, 147);
            this.streetBox.Name = "streetBox";
            this.streetBox.Size = new System.Drawing.Size(306, 20);
            this.streetBox.TabIndex = 0;
            // 
            // locBox
            // 
            this.locBox.Location = new System.Drawing.Point(29, 101);
            this.locBox.Name = "locBox";
            this.locBox.Size = new System.Drawing.Size(306, 20);
            this.locBox.TabIndex = 0;
            // 
            // disBox
            // 
            this.disBox.Location = new System.Drawing.Point(29, 58);
            this.disBox.Name = "disBox";
            this.disBox.Size = new System.Drawing.Size(306, 20);
            this.disBox.TabIndex = 0;
            // 
            // regBox
            // 
            this.regBox.Location = new System.Drawing.Point(29, 12);
            this.regBox.Name = "regBox";
            this.regBox.Size = new System.Drawing.Size(306, 20);
            this.regBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(207, 107);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "0";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(194, 184);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(668, 262);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TextBox kladrBox;
        private System.Windows.Forms.TextBox houseBox;
        private System.Windows.Forms.TextBox streetBox;
        private System.Windows.Forms.TextBox locBox;
        private System.Windows.Forms.TextBox disBox;
        private System.Windows.Forms.TextBox regBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button2;

    }
}

