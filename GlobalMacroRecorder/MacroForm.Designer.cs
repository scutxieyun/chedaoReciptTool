namespace GlobalMacroRecorder
{
    partial class MacroForm
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
            this.recordStartButton = new System.Windows.Forms.Button();
            this.lvFIelds = new System.Windows.Forms.ListView();
            this.clTitle = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clFieldName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clPosition = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label3 = new System.Windows.Forms.Label();
            this.btLocateWnd = new System.Windows.Forms.Button();
            this.btSave = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.cbContent = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPosition = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tbOpsDelay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btTest = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbMainWnd = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbLayout = new System.Windows.Forms.Label();
            this.btCapture = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // recordStartButton
            // 
            this.recordStartButton.Location = new System.Drawing.Point(204, 82);
            this.recordStartButton.Name = "recordStartButton";
            this.recordStartButton.Size = new System.Drawing.Size(45, 21);
            this.recordStartButton.TabIndex = 0;
            this.recordStartButton.Text = "跟踪";
            this.recordStartButton.UseVisualStyleBackColor = true;
            this.recordStartButton.Click += new System.EventHandler(this.recordStartButton_Click);
            // 
            // lvFIelds
            // 
            this.lvFIelds.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clTitle,
            this.clFieldName,
            this.clPosition});
            this.lvFIelds.FullRowSelect = true;
            this.lvFIelds.HideSelection = false;
            this.lvFIelds.Location = new System.Drawing.Point(13, 123);
            this.lvFIelds.Name = "lvFIelds";
            this.lvFIelds.Size = new System.Drawing.Size(335, 129);
            this.lvFIelds.TabIndex = 3;
            this.lvFIelds.UseCompatibleStateImageBehavior = false;
            this.lvFIelds.View = System.Windows.Forms.View.Details;
            this.lvFIelds.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
            this.lvFIelds.KeyDown += new System.Windows.Forms.KeyEventHandler(this.lvFIelds_KeyDown);
            // 
            // clTitle
            // 
            this.clTitle.Text = "字段名";
            this.clTitle.Width = 68;
            // 
            // clFieldName
            // 
            this.clFieldName.Text = "字段标识";
            this.clFieldName.Width = 87;
            // 
            // clPosition
            // 
            this.clPosition.Text = "位置";
            this.clPosition.Width = 155;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "税控窗口名";
            // 
            // btLocateWnd
            // 
            this.btLocateWnd.Location = new System.Drawing.Point(256, 6);
            this.btLocateWnd.Name = "btLocateWnd";
            this.btLocateWnd.Size = new System.Drawing.Size(52, 23);
            this.btLocateWnd.TabIndex = 6;
            this.btLocateWnd.Text = "定位";
            this.btLocateWnd.UseVisualStyleBackColor = true;
            this.btLocateWnd.Click += new System.EventHandler(this.btLocateWnd_Click);
            // 
            // btSave
            // 
            this.btSave.Location = new System.Drawing.Point(271, 417);
            this.btSave.Name = "btSave";
            this.btSave.Size = new System.Drawing.Size(75, 23);
            this.btSave.TabIndex = 7;
            this.btSave.Text = "保存";
            this.btSave.UseVisualStyleBackColor = true;
            this.btSave.Click += new System.EventHandler(this.btSave_Click);
            // 
            // tbLog
            // 
            this.tbLog.Location = new System.Drawing.Point(12, 256);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.tbLog.Size = new System.Drawing.Size(335, 157);
            this.tbLog.TabIndex = 8;
            // 
            // cbContent
            // 
            this.cbContent.FormattingEnabled = true;
            this.cbContent.Location = new System.Drawing.Point(77, 82);
            this.cbContent.Name = "cbContent";
            this.cbContent.Size = new System.Drawing.Size(121, 20);
            this.cbContent.TabIndex = 9;
            this.cbContent.TextChanged += new System.EventHandler(this.cbContent_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(42, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "内容";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(255, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "位置";
            // 
            // lbPosition
            // 
            this.lbPosition.AutoSize = true;
            this.lbPosition.Location = new System.Drawing.Point(294, 85);
            this.lbPosition.Name = "lbPosition";
            this.lbPosition.Size = new System.Drawing.Size(17, 12);
            this.lbPosition.TabIndex = 12;
            this.lbPosition.Text = "？";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 13;
            this.label1.Text = "操作延迟";
            // 
            // tbOpsDelay
            // 
            this.tbOpsDelay.Location = new System.Drawing.Point(77, 58);
            this.tbOpsDelay.Name = "tbOpsDelay";
            this.tbOpsDelay.Size = new System.Drawing.Size(121, 21);
            this.tbOpsDelay.TabIndex = 14;
            this.tbOpsDelay.Text = "200";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "毫秒";
            // 
            // btTest
            // 
            this.btTest.Location = new System.Drawing.Point(164, 417);
            this.btTest.Name = "btTest";
            this.btTest.Size = new System.Drawing.Size(75, 23);
            this.btTest.TabIndex = 16;
            this.btTest.Text = "测试";
            this.btTest.UseVisualStyleBackColor = true;
            this.btTest.Click += new System.EventHandler(this.btTest_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(75, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 12);
            this.label6.TabIndex = 17;
            this.label6.Text = "移动到内容的输入窗口，按Ctrl-F固定坐标";
            // 
            // cbMainWnd
            // 
            this.cbMainWnd.FormattingEnabled = true;
            this.cbMainWnd.Location = new System.Drawing.Point(77, 8);
            this.cbMainWnd.Name = "cbMainWnd";
            this.cbMainWnd.Size = new System.Drawing.Size(173, 20);
            this.cbMainWnd.TabIndex = 18;
            this.cbMainWnd.Click += new System.EventHandler(this.cbMainWnd_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 35);
            this.label7.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "布局分析";
            // 
            // lbLayout
            // 
            this.lbLayout.AutoSize = true;
            this.lbLayout.Location = new System.Drawing.Point(75, 35);
            this.lbLayout.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbLayout.Name = "lbLayout";
            this.lbLayout.Size = new System.Drawing.Size(23, 12);
            this.lbLayout.TabIndex = 20;
            this.lbLayout.Text = "...";
            // 
            // btCapture
            // 
            this.btCapture.Location = new System.Drawing.Point(8, 419);
            this.btCapture.Name = "btCapture";
            this.btCapture.Size = new System.Drawing.Size(75, 23);
            this.btCapture.TabIndex = 21;
            this.btCapture.Text = "截屏";
            this.btCapture.UseVisualStyleBackColor = true;
            this.btCapture.Click += new System.EventHandler(this.btCapture_Click);
            // 
            // MacroForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(355, 443);
            this.Controls.Add(this.btCapture);
            this.Controls.Add(this.lbLayout);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbMainWnd);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btTest);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbOpsDelay);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbPosition);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbContent);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.btSave);
            this.Controls.Add(this.btLocateWnd);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lvFIelds);
            this.Controls.Add(this.recordStartButton);
            this.Name = "MacroForm";
            this.Text = "车到发票打印配置工具";
            this.Load += new System.EventHandler(this.MacroForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button recordStartButton;
        private System.Windows.Forms.ListView lvFIelds;
        private System.Windows.Forms.ColumnHeader clTitle;
        private System.Windows.Forms.ColumnHeader clFieldName;
        private System.Windows.Forms.ColumnHeader clPosition;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btLocateWnd;
        private System.Windows.Forms.Button btSave;
        private System.Windows.Forms.TextBox tbLog;
        private System.Windows.Forms.ComboBox cbContent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbPosition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbOpsDelay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btTest;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbMainWnd;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbLayout;
        private System.Windows.Forms.Button btCapture;
    }
}

