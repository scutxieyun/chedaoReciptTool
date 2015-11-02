namespace CheDaoReciptHike
{
    partial class fmLog
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
            this.btInfo = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btClearLog = new System.Windows.Forms.Button();
            this.cbRealtimeLog = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // btInfo
            // 
            this.btInfo.Location = new System.Drawing.Point(9, 9);
            this.btInfo.Name = "btInfo";
            this.btInfo.Size = new System.Drawing.Size(75, 23);
            this.btInfo.TabIndex = 1;
            this.btInfo.Text = "SystemInfo";
            this.btInfo.UseVisualStyleBackColor = true;
            this.btInfo.Click += new System.EventHandler(this.btInfo_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(8, 36);
            this.tbLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(544, 332);
            this.tbLog.TabIndex = 2;
            // 
            // btClearLog
            // 
            this.btClearLog.Location = new System.Drawing.Point(88, 9);
            this.btClearLog.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btClearLog.Name = "btClearLog";
            this.btClearLog.Size = new System.Drawing.Size(50, 23);
            this.btClearLog.TabIndex = 3;
            this.btClearLog.Text = "Clear";
            this.btClearLog.UseVisualStyleBackColor = true;
            this.btClearLog.Click += new System.EventHandler(this.btClearLog_Click);
            // 
            // cbRealtimeLog
            // 
            this.cbRealtimeLog.AutoSize = true;
            this.cbRealtimeLog.Location = new System.Drawing.Point(198, 9);
            this.cbRealtimeLog.Name = "cbRealtimeLog";
            this.cbRealtimeLog.Size = new System.Drawing.Size(79, 21);
            this.cbRealtimeLog.TabIndex = 4;
            this.cbRealtimeLog.Text = "实时日志";
            this.cbRealtimeLog.UseVisualStyleBackColor = true;
            this.cbRealtimeLog.CheckedChanged += new System.EventHandler(this.cbRealtimeLog_CheckedChanged);
            // 
            // fmLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(562, 387);
            this.Controls.Add(this.cbRealtimeLog);
            this.Controls.Add(this.btClearLog);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btInfo);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "fmLog";
            this.ShowInTaskbar = false;
            this.Text = "fmLog";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmLog_FormClosed);
            this.Load += new System.EventHandler(this.fmLog_Load);
            this.Resize += new System.EventHandler(this.fmLog_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btInfo;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btClearLog;
        private System.Windows.Forms.CheckBox cbRealtimeLog;
    }
}