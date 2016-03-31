using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fias2pgsql
{
    public partial class ProgressDialog : Form
    {
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private ProgressBar progressBar1;
    
        public ProgressDialog()
        {
            InitializeComponent();
        }

        public ProgressDialog(int min, int max)
        {
            InitializeComponent();
            progressBar1.Minimum = min;
            progressBar1.Maximum = max;
            label1.Text = "0";
            UpdateProgress(min);
        }

        public void Inc()
        {
            UpdateProgress(progressBar1.Value + 1);
        }

        public void UpdateProgress(int progress)
        {
            if (progress > progressBar1.Maximum)
            {
                progress = 0;
                if (label1.InvokeRequired)
                    label1.BeginInvoke(new Action(() => label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString()));
                else
                    label1.Text = (Convert.ToInt32(label1.Text) + 1).ToString();
            }
            else
            {
                if (label3.InvokeRequired)
                    label3.BeginInvoke(new Action(() => label3.Text = progressBar1.Value.ToString()));
                else
                    label3.Text = progressBar1.Value.ToString();
            }

            if (progressBar1.InvokeRequired)
                progressBar1.BeginInvoke(new Action(() => progressBar1.Value = progress));
            else
                progressBar1.Value = progress;
            
        }

        public void SetIndeterminate(bool isIndeterminate)
        {
            if (progressBar1.InvokeRequired)
            {
                progressBar1.BeginInvoke(new Action(() =>
                    {
                        if (isIndeterminate)
                            progressBar1.Style = ProgressBarStyle.Marquee;
                        else
                            progressBar1.Style = ProgressBarStyle.Blocks;
                    }
                ));
            }
            else
            {
                if (isIndeterminate)
                    progressBar1.Style = ProgressBarStyle.Marquee;
                else
                    progressBar1.Style = ProgressBarStyle.Blocks;
            }
        }

        private void InitializeComponent()
        {
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progressBar1
            // 
            this.progressBar1.Dock = System.Windows.Forms.DockStyle.Top;
            this.progressBar1.Location = new System.Drawing.Point(0, 0);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(284, 23);
            this.progressBar1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(237, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(13, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(189, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "проход";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(13, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "значение";
            // 
            // ProgressDialog
            // 
            this.ClientSize = new System.Drawing.Size(284, 40);
            this.ControlBox = false;
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progressBar1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProgressDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
