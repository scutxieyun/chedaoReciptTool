﻿namespace ReciptGen
{
    partial class fmMain
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
            this.tmInvoice = new System.Windows.Forms.Timer(this.components);
            this.tmPrint = new System.Windows.Forms.Timer(this.components);
            this.cbPrint = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // tmInvoice
            // 
            this.tmInvoice.Enabled = true;
            this.tmInvoice.Interval = 1000;
            this.tmInvoice.Tick += new System.EventHandler(this.tmInvoice_Tick);
            // 
            // tmPrint
            // 
            this.tmPrint.Enabled = true;
            this.tmPrint.Interval = 1000;
            this.tmPrint.Tick += new System.EventHandler(this.tmPrint_Tick);
            // 
            // cbPrint
            // 
            this.cbPrint.AutoSize = true;
            this.cbPrint.Checked = true;
            this.cbPrint.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbPrint.Location = new System.Drawing.Point(175, 88);
            this.cbPrint.Name = "cbPrint";
            this.cbPrint.Size = new System.Drawing.Size(79, 22);
            this.cbPrint.TabIndex = 0;
            this.cbPrint.Text = "Print";
            this.cbPrint.UseVisualStyleBackColor = true;
            // 
            // fmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(417, 366);
            this.Controls.Add(this.cbPrint);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "fmMain";
            this.Text = "Recipt Generation";
            this.Load += new System.EventHandler(this.fmMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer tmInvoice;
        private System.Windows.Forms.Timer tmPrint;
        private System.Windows.Forms.CheckBox cbPrint;
    }
}

