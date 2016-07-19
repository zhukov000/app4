namespace Monitor
{
    partial class SpoloxNode
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.panelOn = new System.Windows.Forms.Panel();
            this.panelOff = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // panelOn
            // 
            this.panelOn.BackgroundImage = global::Monitor.Properties.Resources.on;
            this.panelOn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOn.Location = new System.Drawing.Point(0, 26);
            this.panelOn.Name = "panelOn";
            this.panelOn.Size = new System.Drawing.Size(80, 69);
            this.panelOn.TabIndex = 0;
            this.panelOn.Visible = false;
            // 
            // panelOff
            // 
            this.panelOff.BackgroundImage = global::Monitor.Properties.Resources.off;
            this.panelOff.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOff.Location = new System.Drawing.Point(0, 26);
            this.panelOff.Name = "panelOff";
            this.panelOff.Size = new System.Drawing.Size(80, 69);
            this.panelOff.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Font = new System.Drawing.Font("Consolas", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Ростов-на-Дону";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // SpoloxNode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelOff);
            this.Controls.Add(this.panelOn);
            this.Controls.Add(this.label1);
            this.Name = "SpoloxNode";
            this.Size = new System.Drawing.Size(80, 95);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelOn;
        private System.Windows.Forms.Panel panelOff;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}
