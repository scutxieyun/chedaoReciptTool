using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Diagnostics;
using CheDaoReciptHike.Properties;

namespace CheDaoReciptHike
{
    public partial class fmReqList : Form
    {
        fmLog log;
        int idx = 0;
        NotifyIcon mNotify;
        //ShuiKongInterface mShuiKongHandle = new YiYeShuiKong();
        ShuiKongInterface mSendKeyHandle = new SendKeyShuiKong();
        List<CheRequest> mActList = new List<CheRequest>();     //the transaction which is required the recipt
        List<CheRequest> mDoneList = new List<CheRequest>();    //the print is done
        public fmReqList()
        {
            log = new fmLog();
            mNotify = new NotifyIcon();
            mNotify.Icon = Resources.trayIcon;
            mNotify.Text = "车道加油辅助";
            mNotify.Visible = true;
            this.AddOwnedForm(log);
            InitializeComponent();
            if (mSendKeyHandle.DetectShuiKong() == true) {
                this.lbskStatus.Text = "税控状态:连接中";
            }
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
                        this.MoveToDoneList(p_item.Order_Number,p_item.Printed_Time);// run in UI thread
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
            lbStatus.Text = "通信状态：" + info;
        }
        private void _AddRequest(CheRequest req) {
            if (mActList.Count == 0)
                this.Refresh(); 
            mActList.Add(req);
            lsReqs.BeginUpdate();
            DateTime tran_date;
            if (DateTime.TryParse(req.Time,out tran_date) == false) {
                tran_date = DateTime.Now;
            }
            ListViewItem lvi = new ListViewItem(new String[] { tran_date.ToShortTimeString()});
            lvi.Name = req.Order_Number;    /** it is key to search the item*/
            //lvi.ImageIndex = i;     //todo
            //lvi.Text = req.Order_Number;
            String brief_info = String.Format("{0:s} 油枪{1:s}加{2:s}油{3:s}升", req.LicenseNumber, req.Pump_Numer, req.Product_Code, req.Product_Number);
            lvi.SubItems.Add(brief_info);
            lvi.SubItems.Add(req.Customer_Text);
            lvi.SubItems.Add(req.Amount);
            lvi.Tag = req;
            this.lsReqs.Items.Insert(0,lvi);
            this.lsReqs.EndUpdate();
        }

        private void fmReqList_FormClosed(object sender, FormClosedEventArgs e)
        {
            CheDaoFactory.close();
            mNotify.Visible = false;
            mNotify.Dispose();
        }

        private void timer_1min_Tick(object sender, EventArgs e)
        {
            if (mSendKeyHandle.DetectShuiKong() == true)
            {
                this.lbskStatus.Text = "税控状态:连接中";
            }
            else
            {
                this.lbskStatus.Text = "税控状态:无连接";
            }
        }

        /** just for the case: move one item to done list without print*/
        private void MoveToDoneList(String TransNo,String Printed_Time) {
            ListViewItem[] list_items = lsReqs.Items.Find(TransNo,false);
            if (list_items.Length > 0) {
                lsReqs.Items.Remove(list_items[0]);
                this.lsDone.Items.Insert(0,list_items[0]).SubItems.Add(Printed_Time);
                this.mActList.Remove((CheRequest)list_items[0].Tag);
                this.mDoneList.Add((CheRequest)list_items[0].Tag);

            }

        }

        private void lsReqs_DoubleClick(object sender, EventArgs e)
        {
            if (lsReqs.SelectedItems.Count < 1) return;
            ListViewItem act_item = lsReqs.SelectedItems[0];
            CheRequest req = (CheRequest)act_item.Tag;
            if (req != null)
            {
                if (mSendKeyHandle.DetectShuiKong() == true)
                {
                    mSendKeyHandle.SendRecipt(req);
                    CheDaoFactory.Handle_Internal_Package(CheDaoInterface.print_confirm, Encoding.UTF8.GetBytes(req.Order_Number));
                }
                else
                {
                    MessageBox.Show("无法检测到税控软件");
                }
            }
        }
        /**
        Init the internal message handle
        **/
        private void fmReqList_Load(object sender, EventArgs e)
        {
            CheDaoFactory.init();
            layoutMainContent();
            this.Text += " " + AppConfig.GetVersion();
            if (AppConfig.GetBanner() == null)
            {
                this.plBanner.Visible = false;
            }
            else {
                this.lbBanner.Text = AppConfig.GetBanner();
            }
            new ReciptServer().start(AppConfig.GetPort());
            layoutMainContent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process process = new Process();
            process.StartInfo.FileName = "notepad.exe";
            process.Start();
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
        }
        private void layoutMainContent() {
            if (plBanner.Visible)
                plBanner.Width = this.flMain.Width;
            if (!plBanner.Visible) tcMain.Height = flMain.Height - plInfo.Height - 30;
            else tcMain.Height = flMain.Height - plInfo.Height - this.plBanner.Height - 30;
            //tcMain.Width = this.flMain.Width;
            //plInfo.Width = this.flMain.Width;
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
    }
}
