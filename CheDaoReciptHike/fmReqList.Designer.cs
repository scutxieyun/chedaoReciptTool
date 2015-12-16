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
            this.cmList = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsItemPrint = new System.Windows.Forms.ToolStripMenuItem();
            this.tsItemDelete = new System.Windows.Forms.ToolStripMenuItem();
            this.tsItemCopy = new System.Windows.Forms.ToolStripMenuItem();
            this.tsItemCopyClient = new System.Windows.Forms.ToolStripMenuItem();
            this.tsItemCopyLicense = new System.Windows.Forms.ToolStripMenuItem();
            this.lbStatus = new System.Windows.Forms.Label();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lsDone = new System.Windows.Forms.ListView();
            this.done_tran_info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done_ClientInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.done1_Amount = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.print_time = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.timer_1min = new System.Windows.Forms.Timer(this.components);
            this.lbskStatus = new System.Windows.Forms.Label();
            this.cbTopMost = new System.Windows.Forms.CheckBox();
            this.plInfo = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmList.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.plInfo.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lsReqs
            // 
            this.lsReqs.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.GasPortNo,
            this.ClientInfo,
            this.Amount});
            this.lsReqs.ContextMenuStrip = this.cmList;
            this.lsReqs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsReqs.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsReqs.FullRowSelect = true;
            this.lsReqs.GridLines = true;
            this.lsReqs.HideSelection = false;
            this.lsReqs.HoverSelection = true;
            this.lsReqs.Location = new System.Drawing.Point(3, 3);
            this.lsReqs.Margin = new System.Windows.Forms.Padding(2);
            this.lsReqs.MultiSelect = false;
            this.lsReqs.Name = "lsReqs";
            this.lsReqs.Size = new System.Drawing.Size(348, 155);
            this.lsReqs.TabIndex = 1;
            this.lsReqs.UseCompatibleStateImageBehavior = false;
            this.lsReqs.View = System.Windows.Forms.View.Details;
            this.lsReqs.DoubleClick += new System.EventHandler(this.lsReqs_DoubleClick);
            this.lsReqs.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.lsReqs_KeyPress);
            // 
            // GasPortNo
            // 
            this.GasPortNo.Text = "交易信息";
            this.GasPortNo.Width = 160;
            // 
            // ClientInfo
            // 
            this.ClientInfo.Text = "客户";
            this.ClientInfo.Width = 164;
            // 
            // Amount
            // 
            this.Amount.Text = "金额";
            this.Amount.Width = 62;
            // 
            // cmList
            // 
            this.cmList.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.cmList.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsItemPrint,
            this.tsItemDelete,
            this.tsItemCopy});
            this.cmList.Name = "cmList";
            this.cmList.Size = new System.Drawing.Size(161, 70);
            // 
            // tsItemPrint
            // 
            this.tsItemPrint.Name = "tsItemPrint";
            this.tsItemPrint.Size = new System.Drawing.Size(160, 22);
            this.tsItemPrint.Text = "推送到税控系统";
            this.tsItemPrint.Click += new System.EventHandler(this.tsItemPrint_Click);
            // 
            // tsItemDelete
            // 
            this.tsItemDelete.Name = "tsItemDelete";
            this.tsItemDelete.Size = new System.Drawing.Size(160, 22);
            this.tsItemDelete.Text = "删除";
            this.tsItemDelete.Click += new System.EventHandler(this.tsItemDelete_Click);
            // 
            // tsItemCopy
            // 
            this.tsItemCopy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsItemCopyClient,
            this.tsItemCopyLicense});
            this.tsItemCopy.Name = "tsItemCopy";
            this.tsItemCopy.Size = new System.Drawing.Size(160, 22);
            this.tsItemCopy.Text = "拷贝";
            // 
            // tsItemCopyClient
            // 
            this.tsItemCopyClient.Name = "tsItemCopyClient";
            this.tsItemCopyClient.Size = new System.Drawing.Size(100, 22);
            this.tsItemCopyClient.Text = "客户";
            this.tsItemCopyClient.Click += new System.EventHandler(this.tsItemCopyClient_Click);
            // 
            // tsItemCopyLicense
            // 
            this.tsItemCopyLicense.Name = "tsItemCopyLicense";
            this.tsItemCopyLicense.Size = new System.Drawing.Size(100, 22);
            this.tsItemCopyLicense.Text = "车牌";
            this.tsItemCopyLicense.Click += new System.EventHandler(this.tsItemCopyLicense_Click);
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.Location = new System.Drawing.Point(104, 8);
            this.lbStatus.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(71, 12);
            this.lbStatus.TabIndex = 5;
            this.lbStatus.Text = "通信:无连接";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabPage1);
            this.tcMain.Controls.Add(this.tabPage2);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
            this.tcMain.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(362, 185);
            this.tcMain.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tcMain.TabIndex = 6;
            this.tcMain.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tcMain_DrawItem);
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.Transparent;
            this.tabPage1.Controls.Add(this.lsReqs);
            this.tabPage1.Location = new System.Drawing.Point(4, 20);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(354, 161);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "待打印";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lsDone);
            this.tabPage2.Location = new System.Drawing.Point(4, 20);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(354, 161);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "已打印";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lsDone
            // 
            this.lsDone.Alignment = System.Windows.Forms.ListViewAlignment.SnapToGrid;
            this.lsDone.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.done_tran_info,
            this.done_ClientInfo,
            this.done1_Amount,
            this.print_time});
            this.lsDone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lsDone.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lsDone.FullRowSelect = true;
            this.lsDone.GridLines = true;
            this.lsDone.HideSelection = false;
            this.lsDone.Location = new System.Drawing.Point(3, 3);
            this.lsDone.MultiSelect = false;
            this.lsDone.Name = "lsDone";
            this.lsDone.Size = new System.Drawing.Size(348, 155);
            this.lsDone.TabIndex = 0;
            this.lsDone.UseCompatibleStateImageBehavior = false;
            this.lsDone.View = System.Windows.Forms.View.Details;
            this.lsDone.DoubleClick += new System.EventHandler(this.lsDone_DoubleClick);
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
            this.lbskStatus.Location = new System.Drawing.Point(5, 33);
            this.lbskStatus.Name = "lbskStatus";
            this.lbskStatus.Size = new System.Drawing.Size(65, 12);
            this.lbskStatus.TabIndex = 7;
            this.lbskStatus.Text = "最近操作: ";
            // 
            // cbTopMost
            // 
            this.cbTopMost.AutoSize = true;
            this.cbTopMost.Checked = true;
            this.cbTopMost.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbTopMost.Location = new System.Drawing.Point(7, 7);
            this.cbTopMost.Name = "cbTopMost";
            this.cbTopMost.Size = new System.Drawing.Size(96, 16);
            this.cbTopMost.TabIndex = 9;
            this.cbTopMost.Text = "保持在最前面";
            this.cbTopMost.UseVisualStyleBackColor = true;
            this.cbTopMost.CheckedChanged += new System.EventHandler(this.cbTopMost_CheckedChanged);
            // 
            // plInfo
            // 
            this.plInfo.Controls.Add(this.cbTopMost);
            this.plInfo.Controls.Add(this.lbStatus);
            this.plInfo.Controls.Add(this.lbskStatus);
            this.plInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plInfo.Location = new System.Drawing.Point(0, 185);
            this.plInfo.Name = "plInfo";
            this.plInfo.Size = new System.Drawing.Size(362, 54);
            this.plInfo.TabIndex = 7;
            // 
            // panel1
            // 
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.tcMain);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(362, 185);
            this.panel1.TabIndex = 7;
            // 
            // fmReqList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(362, 239);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.plInfo);
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "fmReqList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "车到发票辅助打印";
            this.TopMost = true;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.fmReqList_FormClosed);
            this.Load += new System.EventHandler(this.fmReqList_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.fmReqList_KeyDown);
            this.Resize += new System.EventHandler(this.fmReqList_Resize);
            this.cmList.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.plInfo.ResumeLayout(false);
            this.plInfo.PerformLayout();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.ColumnHeader done_tran_info;
        private System.Windows.Forms.ColumnHeader done_ClientInfo;
        private System.Windows.Forms.ColumnHeader done1_Amount;
        private System.Windows.Forms.CheckBox cbTopMost;
        private System.Windows.Forms.ColumnHeader print_time;
        private System.Windows.Forms.Panel plInfo;
        private System.Windows.Forms.ContextMenuStrip cmList;
        private System.Windows.Forms.ToolStripMenuItem tsItemPrint;
        private System.Windows.Forms.ToolStripMenuItem tsItemDelete;
        private System.Windows.Forms.ToolStripMenuItem tsItemCopy;
        private System.Windows.Forms.ToolStripMenuItem tsItemCopyClient;
        private System.Windows.Forms.ToolStripMenuItem tsItemCopyLicense;
        private System.Windows.Forms.Panel panel1;
    }
}

