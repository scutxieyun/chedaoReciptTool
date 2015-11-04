﻿using System;
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
                MessageBox.Show("系统初始化失败，请检查网络是否正常，或联系技术支持 错误信息：" + e.ToString());
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
                    Trace.WriteLineIf(Program.trace_sw.TraceWarning,"read port information error. set default port 3344");
                }
            }else
            {
                Trace.WriteLineIf(Program.trace_sw.TraceWarning, "no port found in configuration " + Application.ExecutablePath);
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
