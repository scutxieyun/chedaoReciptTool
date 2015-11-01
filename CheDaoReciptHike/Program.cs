using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using CheDaoReciptHike.Properties;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Reflection;

namespace CheDaoReciptHike
{
    static class Program
    {
        static fmReqList mainForm = null;
        static LogTrace log = null;
        static List<String> debug_cat;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                debug_cat = new List<String>(AppConfig.GetDebugLevel().Split(','));
                log = new LogTrace();
                Trace.WriteLine("start ...in " + Application.ExecutablePath);
                mainForm = new fmReqList();     
                Application.Run(mainForm);
                Trace.WriteLine("gracefully shutdown...");
                Trace.Flush();
            }
            catch (Exception e) {
                MessageBox.Show("系统初始化失败，请检查网络是否正常，或联系技术支持 错误信息：" + e.ToString());
                Trace.WriteLine("exception at start: " + e.ToString());
                Trace.Flush();
            }
        }
        public static void NewRequest(CheDaoInterface req) {

            mainForm.AddRequest(req);
        }
        public static void UpdateStatus(String info) {
            mainForm.UpdateStatus(info);
        }
        public static Boolean DebugCategory(string category) {
            return debug_cat.Exists(p => p == category);
        }
    }
    public static class AppConfig {
        static public int GetPort() {
            String str = System.Configuration.ConfigurationManager.AppSettings["port"];
            int port = 3344;
            if (str != null) {
                try
                {
                    port = int.Parse(str);
                }
                catch (Exception e) {
                    port = 3344;
                    Trace.WriteLine("read port information error. set default port 3344");
                }
            }else
            {
                Trace.WriteLine("no port found in configuration " + Application.ExecutablePath);
            }
            return port;
        }
        public static string GetDebugLevel() {
            String str = System.Configuration.ConfigurationManager.AppSettings["port"];
            if (str == null) {
                str = "1";
            }
            return str;
        }
        
    }
}
