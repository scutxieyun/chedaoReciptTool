﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace CheDaoLoader
{
    class Program
    {
        static NotifyIcon mNotify;
        static int app_status = 0;
        static DateTime last_start_tick = DateTime.MinValue;
        static Timer mTimer;

        [STAThread]
        static void Main(string[] args)
        {
            if (PriorProcess() != null) return;// only one instance
            if (CheckUpdate() == true)
            {
                StartAutoUpdate();
            }
            if (StartApp() == true)
            {
                mNotify = new NotifyIcon();
                mNotify.Icon = Resource.trayicon;
                mNotify.Text = "车道加油辅助";
                mNotify.Visible = true;

                mTimer = new Timer();
                mTimer.Interval = 10000; //execute the task per 10sec
                mTimer.Tick += MTimer_Tick;
                mTimer.Start();
                Application.Run();
                mNotify.Visible = false;
                mNotify.Dispose();
            }
        }

        private static void MTimer_Tick(object sender, EventArgs e)
        {
            switch (app_status) {
                case 1:
                    //check log or ping the process
                    break;
                case 0:
                    //the app is required to restart
                    StartApp();
                    break;
                case 2://normal exit;
                    Application.Exit();
                    break;
                default:
                    //give up
                    Application.Exit();
                    break;
            }
            return;
        }

        public static Process PriorProcess()
        // Returns a System.Diagnostics.Process pointing to
        // a pre-existing process with the same name as the
        // current one, if any; or null if the current process
        // is unique.
        {
            Process curr = Process.GetCurrentProcess();
            Process[] procs = Process.GetProcessesByName(curr.ProcessName);
            foreach (Process p in procs)
            {
                if ((p.Id != curr.Id) &&
                    (p.MainModule.FileName == curr.MainModule.FileName))
                    return p;
            }
            return null;
        }

        static bool StartApp() {
            String path = "unknown";
            String file = null;
            app_status = -1; //unexpected error, in these status, the app can not be restarted.
            try
            {
                path = System.Configuration.ConfigurationManager.AppSettings["target_folder"];
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception e)
            {
                MessageBox.Show("指定的工作目录:" + path + " 没发现 ");
                return false;
            }
            try
            {
                if (last_start_tick != DateTime.MinValue) {
                    if (last_start_tick.AddMinutes(5) > DateTime.Now) {
                        //the application has been ran less than 10 mins
                        return false; //dont run it. 
                    }
                }
                file = System.Configuration.ConfigurationManager.AppSettings["target_exe"];
                Process p = Process.Start(file);
                p.EnableRaisingEvents = true;
                last_start_tick = DateTime.Now;
                app_status = 1; //working
                p.Exited += App_P_Exited;
            }
            catch (Exception e)
            {
                MessageBox.Show("执行 " + file + " 失败:" + e.Message);
                return false;
            }
            return true;
        }

        private static void App_P_Exited(object sender, EventArgs e)
        {
            Process p = (Process)sender;
            if (p.ExitCode == 0)
            {//normal exit
                app_status = 2;
            }
            else
            {
                //something wrong, try to restart the application
                app_status = 0;
            }
            //throw new NotImplementedException();
        }

        static bool CheckUpdate() {
            string currentDirectory = Environment.CurrentDirectory;
            String TempFilePath = "temp";
            String UpdateListFile = "UpdateList.xml";

            if (ConfigurationManager.AppSettings["update_file"] != null)
            {
                UpdateListFile = ConfigurationManager.AppSettings["update_file"];
            }

            if (ConfigurationManager.AppSettings["TempFilePath"] != null)
            {
                TempFilePath = ConfigurationManager.AppSettings["TempFilePath"];
            }

            string localXmlFile = currentDirectory + "\\" + UpdateListFile;
            string serverXmlFile = string.Empty;
            AutoUpdater.XmlFiles updaterXmlFiles;
            try
            {
                //从本地读取更新配置文件信息
                updaterXmlFiles = new AutoUpdater.XmlFiles(localXmlFile);
            }
            catch
            {
                System.Console.WriteLine("读配置文件出错 " + UpdateListFile);
                return false;
            }

            AutoUpdater.AppUpdater update_check = new AutoUpdater.AppUpdater();
            update_check.UpdaterUrl = updaterXmlFiles.GetNodeValue("//Url") + UpdateListFile;
            String tempUpdatePath = currentDirectory + "\\" + TempFilePath;
            try
            {
                update_check.DownAutoUpdateFile(tempUpdatePath);
            }
            catch
            {
                System.Console.WriteLine("与服务器连接失败,操作超时!");
                return false;

            }
            //获取更新文件列表
            Hashtable htUpdateFile = new Hashtable();

            serverXmlFile = tempUpdatePath + "\\UpdateList.xml";
            if (!File.Exists(serverXmlFile))
            {
                System.Console.WriteLine("不能够下载服务器文件");
                return false;
            }
            int availableUpdate = update_check.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
            if (availableUpdate > 0) return true;

            return false;

        }

        static bool StartAutoUpdate() {
            String path = "unknown";
            String file = null;
            try
            {
                path = System.Configuration.ConfigurationManager.AppSettings["updater_folder"];
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("指定的工作目录:" + path + " 没发现 ");
                return false;
            }
            try
            {
                file = System.Configuration.ConfigurationManager.AppSettings["updater_exe"];
                Process p = Process.Start(file);
                p.WaitForExit();

            }
            catch (Exception e)
            {
                System.Console.WriteLine("升级执行 " + file + " 失败:" + e.Message);
                return false;
            }
            return true;
        }
    }
}
