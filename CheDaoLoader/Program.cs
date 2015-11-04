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

namespace CheDaoLoader
{
    class Program
    {
        static void Main(string[] args)
        {
            if (CheckUpdate() == true)
            {
                StartAutoUpdate();
                StartApp();
            }
            else {
                StartApp();
            }
        }

        static bool StartApp() {
            String path = "unknown";
            String file = null;
            try
            {
                path = System.Configuration.ConfigurationManager.AppSettings["target_folder"];
                Directory.SetCurrentDirectory(path);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("指定的工作目录:" + path + " 没发现 ");
                return false;
            }
            try
            {
                file = System.Configuration.ConfigurationManager.AppSettings["target_exe"];
                Process.Start(file);
            }
            catch (Exception e)
            {
                System.Console.WriteLine("执行 " + file + " 失败:" + e.Message);
                return false;
            }
            return true;
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
