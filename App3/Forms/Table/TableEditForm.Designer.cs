namespace App3.Forms
{
    partial class TableEditForm
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
            this.gisDataSet = new App3.gisDataSet();
            this.regionstatusBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.dbTable1 = new App3.Controls.DBTable();
            ((System.ComponentModel.ISupportInitialize)(this.gisDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regionstatusBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gisDataSet
            // 
            this.gisDataSet.DataSetName = "gisDataSet";
            this.gisDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // regionstatusBindingSource
            // 
            this.regionstatusBindingSource.DataMember = "region_status";
            this.regionstatusBindingSource.DataSource = this.gisDataSet;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 229);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(679, 33);
            this.panel1.TabIndex = 1;
            // 
            // button2
            // 
            this.button2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button2.Location = new System.Drawing.Point(93, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 33);
            this.button2.TabIndex = 0;
            this.button2.Text = "Отмена";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Location = new System.Drawing.Point(12, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 33);
            this.button1.TabIndex = 0;
            this.button1.Text = "Сохранить";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dbTable1
            // 
            this.dbTable1.CanAdd = true;
            this.dbTable1.CanDel = true;
            this.dbTable1.CanEdit = false;
            this.dbTable1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dbTable1.Location = new System.Drawing.Point(0, 0);
            this.dbTable1.Name = "dbTable1";
            this.dbTable1.Size = new System.Drawing.Size(679, 229);
            this.dbTable1.TabIndex = 2;
            // 
            // TableEditForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(679, 262);
            this.Controls.Add(this.dbTable1);
            this.Controls.Add(this.panel1);
            this.MinimizeBox = false;
            this.Name = "TableEditForm";
            this.Text = "TableEditForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TableEditForm_FormClosing);
            this.Load += new System.EventHandler(this.TableEditForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gisDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regionstatusBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.BindingSource regionstatusBindingSource;
        private gisDataSet gisDataSet;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private Controls.DBTable dbTable1;
    }
}