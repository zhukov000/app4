namespace App3.Forms
{
    partial class OneDistrictForm
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
            this.mapBox1 = new SharpMap.Forms.MapBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.gisDataSet1 = new App3.gisDataSet();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.selectedObject = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.addressLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gisDataSet1)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mapBox1
            // 
            this.mapBox1.ActiveTool = SharpMap.Forms.MapBox.Tools.Pan;
            this.mapBox1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mapBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mapBox1.FineZoomFactor = 10D;
            this.mapBox1.Location = new System.Drawing.Point(0, 0);
            this.mapBox1.MapQueryMode = SharpMap.Forms.MapBox.MapQueryType.AllLayers;
            this.mapBox1.Name = "mapBox1";
            this.mapBox1.PanOnClick = false;
            this.mapBox1.PreviewMode = SharpMap.Forms.MapBox.PreviewModes.Fast;
            this.mapBox1.QueryGrowFactor = 2F;
            this.mapBox1.QueryLayerIndex = 0;
            this.mapBox1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(210)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(244)))), ((int)(((byte)(244)))));
            this.mapBox1.ShowProgressUpdate = true;
            this.mapBox1.Size = new System.Drawing.Size(404, 399);
            this.mapBox1.TabIndex = 0;
            this.mapBox1.Text = "mapBox1";
            this.mapBox1.WheelZoomMagnitude = -2D;
            this.mapBox1.MouseMove += new SharpMap.Forms.MapBox.MouseEventHandler(this.mapBox1_MouseMove);
            this.mapBox1.MapRefreshed += new System.EventHandler(this.mapBox1_MapRefreshed);
            this.mapBox1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.mapBox1_MouseClick);
            this.mapBox1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mapBox1_MouseDoubleClick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.mapBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.groupBox1);
            this.splitContainer1.Size = new System.Drawing.Size(673, 403);
            this.splitContainer1.SplitterDistance = 408;
            this.splitContainer1.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.treeView1);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(257, 399);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Объекты в районе";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 16);
            this.treeView1.Name = "treeView1";
            this.treeView1.ShowNodeToolTips = true;
            this.treeView1.Size = new System.Drawing.Size(251, 332);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterExpand += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterExpand);
            this.treeView1.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseClick);
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.button1);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 348);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(251, 48);
            this.panel1.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.button1.Location = new System.Drawing.Point(0, 25);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(251, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Обновить карту";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(3, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(182, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "переход к объекту при выборе";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // gisDataSet1
            // 
            this.gisDataSet1.DataSetName = "gisDataSet";
            this.gisDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectedObject,
            this.toolStripSeparator1,
            this.addressLabel,
            this.toolStripSeparator2,
            this.StatusLabel});
            this.statusStrip1.Location = new System.Drawing.Point(0, 403);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(673, 23);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // selectedObject
            // 
            this.selectedObject.Name = "selectedObject";
            this.selectedObject.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // addressLabel
            // 
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(0, 18);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(0, 18);
            // 
            // OneDistrictForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(673, 426);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "OneDistrictForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "OneDistrictForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OneDistrictForm_FormClosing);
            this.Load += new System.EventHandler(this.OneDistrictForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gisDataSet1)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private SharpMap.Forms.MapBox mapBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        private gisDataSet gisDataSet1;
        private System.Windows.Forms.ToolStripStatusLabel selectedObject;
        private System.Windows.Forms.ToolStripStatusLabel addressLabel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
    }
}