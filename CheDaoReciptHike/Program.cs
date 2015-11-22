using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using CheDaoReciptHike.Properties;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Reflection;
using System.Configuration;

namespace CheDaoReciptHike
{
    static class Program
    {
        static fmReqList mainForm = null;
        public static LogTrace log = null;
        public static TraceSwitch trace_sw;
        public static TextWriterTraceListener st_log = null;
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppConfig.init();
            try
            {
                trace_sw = new TraceSwitch("General_Log_SW", "for all trace");
                log = new LogTrace();
                //String fn = String.Format("log\\log-{0:d}-{1:d}-{2:d}.txt", DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
                //st_log = new TextWriterTraceListener(fn);
                //Trace.Listeners.Add(st_log);
                Trace.WriteLineIf(trace_sw.TraceInfo,"start ...in " + Application.ExecutablePath);
                mainForm = new fmReqList();     
                Application.Run(mainForm);
                Trace.WriteLineIf(trace_sw.TraceInfo,"gracefully shutdown...");
                Trace.Flush();
                log.Close();
            }
            catch (Exception e) {
                MessageBox.Show("系统失败，我们将试图修复，如果修复失败，请联系技术支持 错误信息：" + e.ToString());
                Environment.ExitCode = -1; //
            }
        }
        public static void NewRequest(CheDaoInterface req) {

            mainForm.AddRequest(req);
        }
        public static void UpdateStatus(String info) {
            mainForm.UpdateStatus(info);
        }
    }
    public static class AppConfig {
        static int lifetime_of_req = 60;
        static int port;
        static public void init() {
            String str = "3344";// System.Configuration.ConfigurationManager.AppSettings["port"];
            if (str != null)
            {
                if (!int.TryParse(str, out port)) port = 3344;
            }
            str = System.Configuration.ConfigurationManager.AppSettings["lifetime_of_rec"];
            if (str != null) {
                if (!int.TryParse(str, out lifetime_of_req)) lifetime_of_req = 60;
            }
        }
        static public int GetPort() {
            return port;
        }

        static public int GetLifeTimeOfRec() {
            return lifetime_of_req; //minutes
        }
        public static string GetDebugLevel() {
            String str = System.Configuration.ConfigurationManager.AppSettings["port"];
            if (str == null) {
                str = "1";
            }
            return str;
        }

        internal static string GetVersion()
        {
            const String version = "v1.0";
            if (ConfigurationManager.AppSettings["user_info"] != null) {
                return version + " for " + ConfigurationManager.AppSettings["user_info"];
            }
            return version;
        }
        internal static string GetBanner() {
            return ConfigurationManager.AppSettings["banner"];
        }
    }
}
