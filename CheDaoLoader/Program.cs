using System;
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
using System.Net;

namespace CheDaoLoader
{
    class Program
    {
        static NotifyIcon mNotify;
        static int app_status = 0;
        static DateTime last_start_tick = DateTime.MinValue;
        static Timer mTimer;
        static String mAppCode;

        [STAThread]
        static void Main(string[] args)
        {
            if (PriorProcess() != null)
            {
                MessageBox.Show("打印辅助系统已经启动");
                return;// only one instance
            }

            if(Configure_Check() == false) return;

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
                PrepareUpload();
                mTimer = new Timer();
                mTimer.Interval = 10000; //execute the task per 10sec
                mTimer.Tick += MTimer_Tick;
                mTimer.Start();
                Application.Run();
                mNotify.Visible = false;
                mNotify.Dispose();
            }
        }
        static string[] log_files = null;
        static int cur_upload_index;
        static WebClient mWorkClient = new WebClient();
        private static void PrepareUpload()
        {
            mWorkClient.UploadDataCompleted += new UploadDataCompletedEventHandler(log_update_done);
            log_files = Directory.GetFiles("log", "*.log");
            cur_upload_index = 0;
            while(log_files != null && log_files.Length > cur_upload_index) {
                if (kickoff_upload(log_files[cur_upload_index]) == true) break; //success, upload in complete callback when it done.
                cur_upload_index++;
            }
        }
        private static Boolean kickoff_upload(String fn) {
            byte[] fileBytes = null;
            try
            {
                FileStream fs = new FileStream(fn, FileMode.Open, FileAccess.Read, FileShare.Read);
                fileBytes = new byte[fs.Length];
                fs.Read(fileBytes, 0, Convert.ToInt32(fs.Length));
                fs.Close();
                System.Uri url = new Uri(string.Format("{0:S}?client_id={1:s}&name={2:s}", ConfigurationManager.AppSettings["service_url"] + "log", mAppCode, fn));
                mWorkClient.UploadDataAsync(url, fileBytes);
            }
            catch (Exception e) {
                return false; // read file failure
            }
            return true;
        }

        private static void log_update_done(object sender, UploadDataCompletedEventArgs e)
        {
            if (e.Error == null) {
                try
                {
                    File.Delete(log_files[cur_upload_index]);
                    cur_upload_index++;
                    while (log_files != null && log_files.Length > cur_upload_index)
                    {
                        if (kickoff_upload(log_files[cur_upload_index]) == true) break; //success, upload in complete callback when it done.
                        cur_upload_index++;
                    }
                }
                catch (Exception ex){
                    return;
                }
            }
        }

        private static Boolean Configure_Check()
        {
            Configuration conf = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
            if (null == conf.AppSettings.Settings["client_id"])
            {
                fmConfigure fmConf = new fmConfigure();
                if (fmConf.ShowDialog() == DialogResult.OK)
                {
                    conf.AppSettings.Settings.Add("client_id", fmConf.AppCode);
                    conf.Save(ConfigurationSaveMode.Modified);
                    mAppCode = fmConf.AppCode;
                    return true;
                }
                return false;
            }
            mAppCode = conf.AppSettings.Settings["client_id"].Value;
            return true;
        }
        static int mNhr_count = 0;
        private static void MTimer_Tick(object sender, EventArgs e)
        {
            mNhr_count++;
            switch (app_status) {
                case 1:
                    //check log or ping the process
                    if (mNhr_count > 6 * 120) { //two hours
                        CheckUpdate();//reuse the function to ping server
                    }
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
                Application.Exit();
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
            String app_id = updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value;
            String app_version = updaterXmlFiles.GetNodeValue("//Application//Version");
            update_check.UpdaterUrl = String.Format("{0:s}{1:s}?client_id={2:s}&app_id={3:s}&app_ver={4:s}",updaterXmlFiles.GetNodeValue("//Url"),UpdateListFile,mAppCode,app_id,app_version);
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
