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
            this.GasPortNo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.ClientInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.lbStatus = new System.Windows.Forms.Label();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lsDone = new System.Windows.Forms.ListView();
            this.done_tran_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done_tran_info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done_ClientInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done1_Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.print_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer_1min = new System.Windows.Forms.Timer(this.components);
            this.lbskStatus = new System.Windows.Forms.Label();
            this.cbTopMost = new System.Windows.Forms.CheckBox();
            this.plInfo = new System.Windows.Forms.Panel();
            this.flMain = new System.Windows.Forms.FlowLayoutPanel();
            this.plBanner = new System.Windows.Forms.Panel();
            this.lbBanner = new System.Windows.Forms.Label();
            this.tran_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tcMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.plInfo.SuspendLayout();
            this.flMain.SuspendLayout();
            this.plBanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsReqs
            // 
            this.lsReqs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsReqs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.tran_time,
            this.GasPortNo,
            this.ClientInfo,
            this.Amount});
            this.lsReqs.FullRowSelect = true;
            this.lsReqs.HideSelection = false;
            this.lsReqs.HoverSelection = true;
            this.lsReqs.Location = new System.Drawing.Point(7, 7);
            this.lsReqs.MultiSelect = false;
            this.lsReqs.Name = "lsReqs";
            this.lsReqs.Size = new System.Drawing.Size(657, 230);
            this.lsReqs.TabIndex = 1;
            this.lsReqs.UseCompatibleStateImageBehavior = false;
            this.lsReqs.View = System.Windows.Forms.View.Details;
            this.lsReqs.DoubleClick += new System.EventHandler(this.lsReqs_DoubleClick);
            this.lsReqs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lsReqs_KeyPress);
            // 
            // GasPortNo
            // 
            this.GasPortNo.Text = "交易信息";
            this.GasPortNo.Width = 245;
            // 
            // ClientInfo
            // 
            this.ClientInfo.Text = "客户";
            this.ClientInfo.Width = 200;
            // 
            // Amount
            // 
            this.Amount.Text = "金额";
            this.Amount.Width = 62;
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
            this.tcMain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tcMain.Controls.Add(this.tabPage1);
            this.tcMain.Controls.Add(this.tabPage2);
            this.tcMain.Location = new System.Drawing.Point(4, 110);
            this.tcMain.Margin = new System.Windows.Forms.Padding(4);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(679, 276);
            this.tcMain.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lsReqs);
            this.tabPage1.Location = new System.Drawing.Point(4, 28);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(671, 244);
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
            this.tabPage2.Size = new System.Drawing.Size(671, 244);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "已打印";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lsDone
            // 
            this.lsDone.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.lsDone.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lsDone.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.done_tran_time,
            this.done_tran_info,
            this.done_ClientInfo,
            this.done1_Amount,
            this.print_time});
            this.lsDone.FullRowSelect = true;
            this.lsDone.GridLines = true;
            this.lsDone.Location = new System.Drawing.Point(10, 4);
            this.lsDone.Margin = new System.Windows.Forms.Padding(4);
            this.lsDone.MultiSelect = false;
            this.lsDone.Name = "lsDone";
            this.lsDone.Size = new System.Drawing.Size(653, 223);
            this.lsDone.TabIndex = 0;
            this.lsDone.UseCompatibleStateImageBehavior = false;
            this.lsDone.View = System.Windows.Forms.View.Details;
            // 
            // done_tran_time
            // 
            this.done_tran_time.Text = "交易时间";
            this.done_tran_time.Width = 110;
            // 
            // done_tran_info
            // 
            this.done_tran_info.Text = "交易内容";
            this.done_tran_info.Width = 110;
            // 
            // done_ClientInfo
            // 
            this.done_ClientInfo.Text = "客户信息";
            this.done_ClientInfo.Width = 200;
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
            // plInfo
            // 
            this.plInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plInfo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.plInfo.Controls.Add(this.cbTopMost);
            this.plInfo.Controls.Add(this.lbStatus);
            this.plInfo.Controls.Add(this.lbskStatus);
            this.plInfo.Location = new System.Drawing.Point(4, 394);
            this.plInfo.Margin = new System.Windows.Forms.Padding(4);
            this.plInfo.Name = "plInfo";
            this.plInfo.Size = new System.Drawing.Size(679, 43);
            this.plInfo.TabIndex = 7;
            // 
            // flMain
            // 
            this.flMain.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flMain.Controls.Add(this.plBanner);
            this.flMain.Controls.Add(this.tcMain);
            this.flMain.Controls.Add(this.plInfo);
            this.flMain.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flMain.Location = new System.Drawing.Point(12, 12);
            this.flMain.Name = "flMain";
            this.flMain.Size = new System.Drawing.Size(684, 472);
            this.flMain.TabIndex = 8;
            this.flMain.Resize += new System.EventHandler(this.flMain_Resize);
            // 
            // plBanner
            // 
            this.plBanner.Controls.Add(this.lbBanner);
            this.plBanner.Location = new System.Drawing.Point(3, 3);
            this.plBanner.Name = "plBanner";
            this.plBanner.Size = new System.Drawing.Size(681, 100);
            this.plBanner.TabIndex = 8;
            // 
            // lbBanner
            // 
            this.lbBanner.AutoSize = true;
            this.lbBanner.Font = new System.Drawing.Font("宋体", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBanner.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.lbBanner.Location = new System.Drawing.Point(12, 35);
            this.lbBanner.Name = "lbBanner";
            this.lbBanner.Size = new System.Drawing.Size(82, 24);
            this.lbBanner.TabIndex = 0;
            this.lbBanner.Text = "label1";
            // 
            // tran_time
            // 
            this.tran_time.Text = "交易时间";
            this.tran_time.Width = 124;
            // 
            // fmReqList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 496);
            this.Controls.Add(this.flMain);
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
            this.plInfo.ResumeLayout(false);
            this.plInfo.PerformLayout();
            this.flMain.ResumeLayout(false);
            this.plBanner.ResumeLayout(false);
            this.plBanner.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ListView lsReqs;
        private System.Windows.Forms.ColumnHeader GasPortNo;
        private System.Windows.Forms.ColumnHeader ClientInfo;
        private System.Windows.Forms.ColumnHeader Amount;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ListView lsDone;
        private System.Windows.Forms.Timer timer_1min;
        private System.Windows.Forms.Label lbskStatus;
        private System.Windows.Forms.ColumnHeader done_tran_time;
        private System.Windows.Forms.ColumnHeader done_tran_info;
        private System.Windows.Forms.ColumnHeader done_ClientInfo;
        private System.Windows.Forms.ColumnHeader done1_Amount;
        private System.Windows.Forms.CheckBox cbTopMost;
        private System.Windows.Forms.ColumnHeader print_time;
        private System.Windows.Forms.Panel plInfo;
        private System.Windows.Forms.FlowLayoutPanel flMain;
        private System.Windows.Forms.Panel plBanner;
        private System.Windows.Forms.Label lbBanner;
        private System.Windows.Forms.ColumnHeader tran_time;
    }
}

