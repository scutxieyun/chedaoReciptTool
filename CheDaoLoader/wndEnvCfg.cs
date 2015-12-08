using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Windows.Forms;
using WndInteract;

namespace CheDaoLoader
{
    interface LogOutput
    {
        void write(String str);
    }

    public partial class wndEnvCfg : Form ,LogOutput
    {
        public wndEnvCfg()
        {
            Win32Locator.KickOffEnumWindows();
            InitializeComponent();
            TbLogTrace log = new TbLogTrace(this);
            Trace.Listeners.Add(log);
            this.TopMost = true;
        }

        private void btReqCfg_Click(object sender, EventArgs e)
        {
            if (cbWndsList.Text.Trim() == "") {
                MessageBox.Show("请选择税控窗口");
                return;
            }
            System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
            String url = string.Format("{0:s}cfgfile?wndtitle={1:s}&x_res={2:d}&y_res={3:d}", ConfigurationManager.AppSettings["service_url"], cbWndsList.Text.Trim(), resolution.Width, resolution.Height);
            WebClient w_cli = new WebClient();
            try
            {
                Trace.WriteLine("获取配置文件" + url + Environment.NewLine);
                w_cli.DownloadFile(url, "tmp_" + ScriptExecuter.default_script_fn);
                Trace.WriteLine("写入配置文件tmp_" + ScriptExecuter.default_script_fn + Environment.NewLine);
                Trace.WriteLine("结束" + Environment.NewLine);
                btTest.Visible = true;
            }
            catch (IOException io_ex)
            {
                MessageBox.Show("写脚本文件错误" + ScriptExecuter.default_script_fn + io_ex.Message);
            }
            catch (WebException web_ex)
            {
                if (((HttpWebResponse)web_ex.Response).StatusCode == HttpStatusCode.NotFound)
                {
                    Trace.WriteLine("服务器没有该税控软件的配置，收集信息中。。。。" + Environment.NewLine);
                    if (collectWndEnv(cbWndsList.Text.Trim()) == false)
                    {
                        MessageBox.Show("收集配置失败，请联系技术支持");
                        return;
                    }
                    else
                    {
                        Trace.WriteLine("数据已经提交服务器，请稍后尝试获取配置文件");
                    }
                }
                else
                {
                    MessageBox.Show("网络故障" + web_ex.Message + " 请联系技术支持");
                    return;
                }

            }
            catch (Exception ex) {
                MessageBox.Show("系统故障");
                return;
            }
        }

        private Boolean collectWndEnv(string v)
        {
            IntPtr wnd = Win32Locator.locateWindow(String.Format("S({0:S})[0]", v), null);
            if (wnd == IntPtr.Zero)
            {
                MessageBox.Show(String.Format("定位窗口 {0:s} 失败", v));
                return false;
            }
            try
            {
                String fn = "shuikong-screen.jpg";
                Win32Locator.SetForeGWindow(wnd);
                Win32Locator.centerWindow(wnd);
                Bitmap pic = Win32Locator.CaptureScreen();
                MemoryStream mStream = new MemoryStream();
                pic.Save(mStream, ImageFormat.Jpeg);
                WebClient w_cli = new WebClient();
                String url = string.Format("{0:s}log?client_id={1:s}&name={2:d}", ConfigurationManager.AppSettings["service_url"], Program.mAppCode, fn);
                w_cli.UploadData(new Uri(url), mStream.ToArray());
                MessageBox.Show("上载文件成功，请联系技术支持生成配置文件");
            }
            catch (IOException e)
            {
                MessageBox.Show("写文件错误，请检查磁盘空间");
                return false;
            }
            catch (WebException e)
            {
                MessageBox.Show("上载文件失败，请联系技术支持");
                return false;
            }
            catch (Exception e) {
                MessageBox.Show("系统异常");
                return false;
            }
            return true;
        }

        private void cbWndsList_Refresh(object sender, EventArgs e)
        {
            cbWndsList.Items.Clear();
            if (cbWndsList.Items.Count == 0)
            {
                List<String> wnds = Win32Locator.GetWindows();
                foreach (String str in wnds)
                {
                    if (str.Trim() != "" && Win32Locator.HasChinese(str))
                        cbWndsList.Items.Add(str);
                }
            }
            Win32Locator.KickOffEnumWindows();
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            ScriptExecuter.init();
            Dictionary<String, String> rec = new Dictionary<string, string>();
            rec.Add("Customer_Text", "北京车到网络科技有限公司");
            rec.Add("Product_Code", "95#");
            rec.Add("Product_Price", "4.5");
            rec.Add("Amount", "300");
            rec.Add("Product_Number", "30");
            ScriptExecuter.readScript("tmp_" + ScriptExecuter.default_script_fn);
            int res = ScriptExecuter.execute(rec);
            if (res != 0)
            {
                Trace.WriteLine(String.Format("Execute res {0:d}", res));
                MessageBox.Show("执行失败，请联系技术支持");
            }
            else {
                if (MessageBox.Show(this, "脚本执行完成，请检查发票信息是否成功注入，如没有，请联系技术支持", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    this.DialogResult = DialogResult.OK;
                    File.Copy("tmp_" + ScriptExecuter.default_script_fn, ScriptExecuter.default_script_fn);
                    this.Close();
                }
            }
        }

        public void write(string str)
        {
            this.Invoke((MethodInvoker)delegate
            {
                tbLog.AppendText(str + Environment.NewLine);
            });
        }
    }

    class TbLogTrace : TextWriterTraceListener
    {
        LogOutput out_log;
        override public void WriteLine(String msg)
        {
            out_log.write(msg);
        }
        public TbLogTrace(LogOutput o)
        {
            out_log = o;
        }
    }
}
