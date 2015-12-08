namespace CheDaoLoader
{
    partial class wndEnvCfg
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
            this.cbWndsList = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btReqCfg = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.btTest = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cbWndsList
            // 
            this.cbWndsList.FormattingEnabled = true;
            this.cbWndsList.Location = new System.Drawing.Point(81, 12);
            this.cbWndsList.Name = "cbWndsList";
            this.cbWndsList.Size = new System.Drawing.Size(191, 20);
            this.cbWndsList.TabIndex = 0;
            this.cbWndsList.DropDown += new System.EventHandler(this.cbWndsList_Refresh);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "税控窗口";
            // 
            // btReqCfg
            // 
            this.btReqCfg.Location = new System.Drawing.Point(14, 47);
            this.btReqCfg.Name = "btReqCfg";
            this.btReqCfg.Size = new System.Drawing.Size(75, 23);
            this.btReqCfg.TabIndex = 2;
            this.btReqCfg.Text = "请求配置";
            this.btReqCfg.UseVisualStyleBackColor = true;
            this.btReqCfg.Click += new System.EventHandler(this.btReqCfg_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(14, 76);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(258, 122);
            this.tbLog.TabIndex = 3;
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(104, 47);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 23);
            this.btTest.TabIndex = 4;
            this.btTest.Text = "测试";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Visible = false;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // wndEnvCfg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(287, 205);
            this.Controls.Add(this.btTest);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btReqCfg);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbWndsList);
            this.Name = "wndEnvCfg";
            this.Text = "wndEnvCfg";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cbWndsList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btReqCfg;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.Button btTest;
    }
}