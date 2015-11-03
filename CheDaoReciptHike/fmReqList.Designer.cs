namespace CheDaoReciptHike
{
    partial class fmReqList
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lsReqs = new System.Windows.Forms.ListView();
            this.TransNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.GasPortNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClientInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.license = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tran_date = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbStatus = new System.Windows.Forms.Label();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lsDone = new System.Windows.Forms.ListView();
            this.done_TransNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done_GasPortNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done_ClientInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done_license = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done1_Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.print_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer_1min = new System.Windows.Forms.Timer(this.components);
            this.lbskStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.cbTopMost = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tcMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsReqs
            // 
            this.lsReqs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.TransNo,
            this.GasPortNo,
            this.ClientInfo,
            this.license,
            this.Amount,
            this.tran_date});
            this.lsReqs.FullRowSelect = true;
            this.lsReqs.GridLines = true;
            this.lsReqs.HideSelection = false;
            this.lsReqs.HoverSelection = true;
            this.lsReqs.Location = new System.Drawing.Point(6, 3);
            this.lsReqs.MultiSelect = false;
            this.lsReqs.Name = "lsReqs";
            this.lsReqs.Size = new System.Drawing.Size(1155, 574);
            this.lsReqs.TabIndex = 1;
            this.lsReqs.UseCompatibleStateImageBehavior = false;
            this.lsReqs.View = System.Windows.Forms.View.Details;
            this.lsReqs.DoubleClick += new System.EventHandler(this.lsReqs_DoubleClick);
            this.lsReqs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lsReqs_KeyPress);
            // 
            // TransNo
            // 
            this.TransNo.Text = "交易号";
            this.TransNo.Width = 110;
            // 
            // GasPortNo
            // 
            this.GasPortNo.Text = "油枪";
            this.GasPortNo.Width = 110;
            // 
            // ClientInfo
            // 
            this.ClientInfo.Text = "客户";
            this.ClientInfo.Width = 200;
            // 
            // license
            // 
            this.license.Text = "车牌号";
            this.license.Width = 110;
            // 
            // Amount
            // 
            this.Amount.Text = "金额";
            this.Amount.Width = 62;
            // 
            // tran_date
            // 
            this.tran_date.Text = "交易时间";
            this.tran_date.Width = 110;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(11, 10);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(152, 18);
            this.lbStatus.TabIndex = 5;
            this.lbStatus.Text = "通信状态：无连接";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabPage1);
            this.tcMain.Controls.Add(this.tabPage2);
            this.tcMain.Location = new System.Drawing.Point(18, 18);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1176, 616);
            this.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.tcMain.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lsReqs);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(1168, 584);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "待打印";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lsDone);
            this.tabPage2.Location = new System.Drawing.Point(4, 28);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage2.Size = new System.Drawing.Size(1168, 584);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "已打印";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lsDone
            // 
            this.lsDone.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.lsDone.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.done_TransNo,
            this.done_GasPortNo,
            this.done_ClientInfo,
            this.done_license,
            this.done1_Amount,
            this.print_time});
            this.lsDone.FullRowSelect = true;
            this.lsDone.GridLines = true;
            this.lsDone.Location = new System.Drawing.Point(4, 4);
            this.lsDone.Margin = new System.Windows.Forms.Padding(4);
            this.lsDone.MultiSelect = false;
            this.lsDone.Name = "lsDone";
            this.lsDone.Size = new System.Drawing.Size(1156, 572);
            this.lsDone.TabIndex = 0;
            this.lsDone.UseCompatibleStateImageBehavior = false;
            this.lsDone.View = System.Windows.Forms.View.Details;
            // 
            // done_TransNo
            // 
            this.done_TransNo.Text = "交易号";
            this.done_TransNo.Width = 110;
            // 
            // done_GasPortNo
            // 
            this.done_GasPortNo.Text = "油枪";
            this.done_GasPortNo.Width = 110;
            // 
            // done_ClientInfo
            // 
            this.done_ClientInfo.Text = "客户信息";
            this.done_ClientInfo.Width = 200;
            // 
            // done_license
            // 
            this.done_license.Text = "车牌";
            this.done_license.Width = 150;
            // 
            // done1_Amount
            // 
            this.done1_Amount.Text = "总价";
            this.done1_Amount.Width = 110;
            // 
            // print_time
            // 
            this.print_time.Text = "操作时间";
            this.print_time.Width = 150;
            // 
            // timer_1min
            // 
            this.timer_1min.Enabled = true;
            this.timer_1min.Interval = 60000;
            this.timer_1min.Tick += new System.EventHandler(this.timer_1min_Tick);
            // 
            // lbskStatus
            // 
            this.lbskStatus.AutoSize = true;
            this.lbskStatus.Location = new System.Drawing.Point(232, 10);
            this.lbskStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbskStatus.Name = "lbskStatus";
            this.lbskStatus.Size = new System.Drawing.Size(152, 18);
            this.lbskStatus.TabIndex = 7;
            this.lbskStatus.Text = "税控状态: 无连接";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(697, 4);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(112, 34);
            this.button1.TabIndex = 8;
            this.button1.Text = "模拟税控";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbTopMost
            // 
            this.cbTopMost.AutoSize = true;
            this.cbTopMost.Location = new System.Drawing.Point(445, 10);
            this.cbTopMost.Margin = new System.Windows.Forms.Padding(4);
            this.cbTopMost.Name = "cbTopMost";
            this.cbTopMost.Size = new System.Drawing.Size(135, 22);
            this.cbTopMost.TabIndex = 9;
            this.cbTopMost.Text = "保持在最前面";
            this.cbTopMost.UseVisualStyleBackColor = true;
            this.cbTopMost.CheckedChanged += new System.EventHandler(this.cbTopMost_CheckedChanged);
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.cbTopMost);
            this.panel1.Controls.Add(this.lbStatus);
            this.panel1.Controls.Add(this.lbskStatus);
            this.panel1.Controls.Add(this.button1);
            this.panel1.Location = new System.Drawing.Point(18, 638);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1172, 43);
            this.panel1.TabIndex = 7;
            // 
            // fmReqList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 692);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tcMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Name = "fmReqList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "车到发票辅助打印";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmReqList_FormClosed);
            this.Load += new System.EventHandler(this.fmReqList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fmReqList_KeyDown);
            this.Resize += new System.EventHandler(this.fmReqList_Resize);
            this.tcMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView lsReqs;
        private System.Windows.Forms.ColumnHeader TransNo;
        private System.Windows.Forms.ColumnHeader GasPortNo;
        private System.Windows.Forms.ColumnHeader ClientInfo;
        private System.Windows.Forms.ColumnHeader license;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView lsDone;
        private System.Windows.Forms.Timer timer_1min;
        private System.Windows.Forms.Label lbskStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ColumnHeader done_TransNo;
        private System.Windows.Forms.ColumnHeader done_GasPortNo;
        private System.Windows.Forms.ColumnHeader done_ClientInfo;
        private System.Windows.Forms.ColumnHeader done_license;
        private System.Windows.Forms.ColumnHeader done1_Amount;
        private System.Windows.Forms.CheckBox cbTopMost;
        private System.Windows.Forms.ColumnHeader print_time;
        private System.Windows.Forms.ColumnHeader tran_date;
        private System.Windows.Forms.Panel panel1;
    }
}

