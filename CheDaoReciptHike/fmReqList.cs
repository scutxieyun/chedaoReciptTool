﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;
using CheDaoReciptHike.Properties;
using System.IO;
using System.Net;

namespace CheDaoReciptHike
{
    public partial class fmReqList : Form
    {
        fmLog log;
        int idx = 0;
        List<CheRequest> mActList = new List<CheRequest>();     //the transaction which is required the recipt
        List<CheRequest> mDoneList = new List<CheRequest>();    //the print is done
        int m60mCounter = 0;
        int m10mCounter = 0;
        const int upload_rec_period = 10;// min

        String mClientId = "unconfigured";
        String base_url = null;
        StringWriter mRecBuffer = null;
        String mPendingBuffer = null;
        WebClient mWebClient = new WebClient();
        public fmReqList(String[] args)
        {
            log = new fmLog();
            this.AddOwnedForm(log);
            InitializeComponent();
            if (args.Length >= 2) {
                mClientId = args[0];
                mWebClient.UploadStringCompleted += new UploadStringCompletedEventHandler(RecUploadDone);
                mWebClient.Encoding = System.Text.Encoding.UTF8;
                base_url = args[1] + "uploadrec?client_id=" + mClientId;
            }
            if(NewShuiKongInterface.init() != 0) MessageBox.Show("税控脚本初始化失败 " + NewShuiKongInterface.getLastError());
            /*ShuiKongFactory.init();
            if (ShuiKongFactory.DetectShuiKong() == true) {
                this.lbskStatus.Text = "税控状态:连接中";
            }*/
            
        }

        private void RecUploadDone(object sender, UploadStringCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                int res;
                if (int.TryParse(e.Result, out res) == false)
                {
                    CheDaoFactory.upload_err++;
                    CheDaoFactory.latest_upload_error = "return error:" + e.Result;
                }
                else
                {
                    if (res > 0) CheDaoFactory.upload_ok++;
                    else
                    {
                        CheDaoFactory.upload_err++;
                        CheDaoFactory.latest_upload_error = "format error";
                    }
                }
            }
            else {
                CheDaoFactory.upload_err++;
                CheDaoFactory.latest_upload_error = "network issue";
            }
            mPendingBuffer = null;
        }

        public void AddRequest(CheDaoInterface req) {
            switch (req.message_type) {
                case CheDaoInterface.data_validation:
                    CheRequest c_item = (CheRequest)req;
                    this.Invoke((MethodInvoker)delegate
                    {
                        this._AddRequest(c_item);// run in UI thread
                    });
                    break;
                case CheDaoInterface.print_confirm:
                    ChePActionRequest p_item = (ChePActionRequest)req;
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.MoveToDoneList(p_item.Order_Number,p_item.Printed_Time,p_item.message_type);// run in UI thread
                    });
                    break;
                case CheDaoInterface.delete_cmd:
                    CheDeleteActionRequest d_item = (CheDeleteActionRequest)req;
                    this.Invoke((MethodInvoker)delegate
                    {
                        this.MoveToDoneList(d_item.Order_Number, d_item.Printed_Time,d_item.message_type);// run in UI thread
                    });
                    break;
                default:
                    Trace.WriteLineIf(Program.trace_sw.TraceError, "incorrect message request received " + req.message_type.ToString());
                    break;
            }
        }
        public void UpdateStatus(String info) {
            this.Invoke((MethodInvoker)delegate
            {
                this._UpdateStatus(info);// run in UI thread
            });
        }
        private void _UpdateStatus(String info) {
            lbStatus.Text = "通信:" + info;
        }
        private void _AddRequest(CheRequest req) {
            Boolean ref_req = false;
            if (mActList.Count == 0)
                ref_req = true;
            mActList.Add(req);
            lsReqs.BeginUpdate();
            DateTime tran_date;
            if (DateTime.TryParse(req.Time,out tran_date) == false) {
                tran_date = DateTime.Now;
            }
            String brief_info = String.Format("{0:s} {1:s} 油枪{2:s}加{3:s}油{4:s}升", tran_date.ToShortTimeString(),req.LicenseNumber, req.Pump_Number, req.Product_Code, req.Product_Number);
            ListViewItem lvi = new ListViewItem(new String[] { brief_info});
            lvi.Name = req.Order_Number;    /** it is key to search the item*/
            //lvi.ImageIndex = i;     //todo
            //lvi.Text = req.Order_Number;
            lvi.SubItems.Add(req.Customer_Text);
            lvi.SubItems.Add(req.Amount);
            lvi.Tag = req;
            this.lsReqs.Items.Insert(0,lvi);
            this.lsReqs.EndUpdate();
            if (ref_req) this.Refresh();
        }

        private void fmReqList_FormClosed(object sender, FormClosedEventArgs e)
        {
            CheDaoFactory.close();
        }

        /*********************************************************************
        测试代码
        **/
        int mTestCounter = 0;
        private void timer_event() {
            Random r = new Random();
            mTestCounter++;
            if (mTestCounter > r.Next(10)) {
                mTestCounter = 0;
                if (lsReqs.Items.Count > 0)
                {
                    int index = r.Next(lsReqs.Items.Count);
                    ListViewItem act_item = lsReqs.Items[index];
                    CheRequest req = (CheRequest)act_item.Tag;
                    CheDaoFactory.Handle_Internal_Package(CheDaoInterface.delete_cmd, Encoding.UTF8.GetBytes(req.Order_Number));
                }
            }
        }
        /******************************************************************************/

        private void timer_1min_Tick(object sender, EventArgs e)
        {
            m60mCounter++;
            if (m60mCounter >= AppConfig.GetLifeTimeOfRec()/2) {
                m60mCounter = 0 ;
                CheDaoFactory.Handle_Internal_Package(CheDaoInterface.clean_cmd, Encoding.UTF8.GetBytes("Cleanup"));
            }
            m10mCounter++;
            if (m10mCounter >= upload_rec_period && base_url != null && !mWebClient.IsBusy && mRecBuffer != null && mPendingBuffer == null) {
                m10mCounter = 0;
                mPendingBuffer = "[" + mRecBuffer.ToString() + "]"; 
                mRecBuffer = null; //the operation is safe
                mWebClient.UploadStringAsync(new Uri(base_url),mPendingBuffer);
            }
        }

        /** just for the case: move one item to done list without print*/
        private void MoveToDoneList(String TransNo,String Printed_Time,int act_code) {
            ListViewItem[] list_items = lsReqs.Items.Find(TransNo,false);
            if (list_items.Length > 0) {
                lsReqs.Items.Remove(list_items[0]);
                this.lsDone.Items.Insert(0,list_items[0]).SubItems.Add(Printed_Time);
                this.mActList.Remove((CheRequest)list_items[0].Tag);
                this.mDoneList.Add((CheRequest)list_items[0].Tag);
                bool firstRec = false;
                if (mRecBuffer == null) {
                    firstRec = true;
                    mRecBuffer = new StringWriter();
                }
                CheRequest rec = (CheRequest)list_items[0].Tag;
                if(rec != null) mRecBuffer.Write((rec).toJsonString(act_code,firstRec));
                if (mActList.Count == 0) this.Refresh();
            }

        }
        


        private void lsReqs_DoubleClick(object sender, EventArgs e)
        {//note other functions call it directly, don't use sender or e
            if (lsReqs.SelectedItems.Count < 1) return;
            ListViewItem act_item = lsReqs.SelectedItems[0];
            CheRequest req = (CheRequest)act_item.Tag;
            if (req != null)
            {
                int res;
                if ((res = NewShuiKongInterface.SendRecipt(req)) != 0)
                {
                    MessageBox.Show(String.Format("税控脚本执行错误，错误码:{0:d} {1:s}", res,NewShuiKongInterface.getLastError()));
                    lbskStatus.Text = "最近操作：" + NewShuiKongInterface.getLastError();
                }
                else {
                    CheDaoFactory.Handle_Internal_Package(CheDaoInterface.print_confirm, Encoding.UTF8.GetBytes(req.Order_Number));
                    lbskStatus.Text = "最近操作：成功";
                }
            }
        }

        /**
        Init the internal message handle
        **/
        private void fmReqList_Load(object sender, EventArgs e)
        {
            CheDaoFactory.init();
            this.Text += " " + AppConfig.GetVersion();
            ReciptServer serv = new ReciptServer();
            serv.start(AppConfig.GetPort());
            _UpdateStatus(serv.ServiceInfo());
            layoutMainContent();
        }

        
        private void fmReqList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F12) {
                if (log.IsDisposed) { log = new fmLog(); }
                log.Show();
            }
            if (e.KeyCode == Keys.F11) {
                Process process = new Process();
                process.StartInfo.FileName = "notepad.exe";
                process.Start();
            }
        }

        private void cbTopMost_CheckedChanged(object sender, EventArgs e)
        {
            if (cbTopMost.Checked)
            {
                this.TopMost = true;
            }
            else {
                this.TopMost = false;
            }
        }

        private void fmReqList_Resize(object sender, EventArgs e)
        {
            /*Control con = (Control)sender;
            tcMain.Width = con.Width - 20;
            tcMain.Height = con.Width - 20;
            lsReqs.Width = con.Width - 20;
            lsReqs.Height = con.Height - 40;*/
        }

        private void lsReqs_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                lsReqs_DoubleClick(sender, e);
            }
            if (e.KeyChar == (char)Keys.Delete) {
                tsItemDelete_Click(sender, e);
            }
        }
        private void layoutMainContent() {
            /*if (!plBanner.Visible) tcMain.Height = flMain.Height - plInfo.Height - 30;
            else tcMain.Height = flMain.Height - plInfo.Height - this.plBanner.Height - 30;
            tcMain.Width = this.flMain.Width - 10;
            plInfo.Width = this.flMain.Width - 10;*/
            return;
        }

        private void flMain_Resize(object sender, EventArgs e)
        {
            layoutMainContent();
        }

        private void tcMain_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index == 0) {
                if (mActList.Count > 0) {
                    e.Graphics.FillRectangle(new SolidBrush(Color.Red), e.Bounds);
                }
            }
            // Then draw the current tab button text 
            Rectangle paddedBounds = e.Bounds;
            paddedBounds.Inflate(-2, -2);
            e.Graphics.DrawString(tcMain.TabPages[e.Index].Text, this.Font, SystemBrushes.MenuText, paddedBounds);

        }

        private void lsDone_DoubleClick(object sender, EventArgs e)
        {
            if (lsDone.SelectedItems.Count < 1) return;
            ListViewItem act_item = lsDone.SelectedItems[0];
            CheRequest req = (CheRequest)act_item.Tag;
            if (req != null)
            {
                if (MessageBox.Show("重新打印已打印记录", "重新打印", MessageBoxButtons.OKCancel) == DialogResult.Cancel) return;
                int res;
                if ((res = NewShuiKongInterface.SendRecipt(req)) != 0)
                {
                    MessageBox.Show(String.Format("税控脚本执行错误，错误码:{0:d} {1:s}", res, NewShuiKongInterface.getLastError()));
                    lbskStatus.Text = "最近操作：" + NewShuiKongInterface.getLastError();
                }
                else {
                    lbskStatus.Text = "最近操作：成功";
                }
               
            }
        }

        private void tsItemPrint_Click(object sender, EventArgs e)
        {
            if(lsReqs.SelectedItems.Count > 0)
                lsReqs_DoubleClick(sender, e);
        }

        private void tsItemDelete_Click(object sender, EventArgs e)
        {
            if (lsReqs.SelectedItems.Count < 1) return;
            ListViewItem act_item = lsReqs.SelectedItems[0];
            CheRequest req = (CheRequest)act_item.Tag;
            CheDaoFactory.Handle_Internal_Package(CheDaoInterface.delete_cmd, Encoding.UTF8.GetBytes(req.Order_Number));
        }

        private void tsItemCopyClient_Click(object sender, EventArgs e)
        {
            if (lsReqs.SelectedItems.Count < 1) return;
            ListViewItem act_item = lsReqs.SelectedItems[0];
            CheRequest req = (CheRequest)act_item.Tag;
            System.Windows.Forms.Clipboard.SetText(req.Customer_Text);
        }

        private void tsItemCopyLicense_Click(object sender, EventArgs e)
        {
            if (lsReqs.SelectedItems.Count < 1) return;
            ListViewItem act_item = lsReqs.SelectedItems[0];
            CheRequest req = (CheRequest)act_item.Tag;
            System.Windows.Forms.Clipboard.SetText(req.LicenseNumber);
        }
    }
}
