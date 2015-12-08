using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using MouseKeyboardLibrary;

namespace WndInteract
{
    static public class ScriptExecuter
    {
        static Dictionary<String, String> record = new Dictionary<string, string>();
        static List<CmdDefintion> mCmdList = new List<CmdDefintion>();
        static String Last_Error = "NO";
        static public TraceLevel debug_level = TraceLevel.Error;
        public const String default_script_fn = "chedaoshuikong_cfg.txt";
        static public String getLastError() {
            return Last_Error;
        }
        static public void init() {
            /*function_table.Clear();
            function_table.Add("ActiveWindow", new ActiveWindowFunction());
            function_table.Add("CheckWinRect", new CheckWinRectFunction());
            function_table.Add("CopyClipboard", new CopyClipboardFunction());
            function_table.Add("sleep", new sleepFunction());
            function_table.Add("SendInput", new SendInputFunction());
            function_table.Add("SetMouse", new SetMouseFunction());*/
        }

        static public void PutFieldValue(String field, String value) {
            if (record.ContainsKey(field)) record[field] = value;
            else record.Add(field, value);
        }

        static public String GetFieldValue(String field) {
            String value = null;
            if (record.TryGetValue(field, out value)) return value;
            else return null;
        }

        static public Boolean readScript(String fn) {
            StreamReader reader;
            mCmdList.Clear();
            try
            {
                 reader = new System.IO.StreamReader(new System.IO.FileStream(fn, FileMode.Open, FileAccess.Read));
            }
            catch (Exception e) {
                Trace.WriteLine(String.Format("打开文件{0:s} 错误:{1:s}", fn, e.Message));
                Last_Error = "打开脚本文件错误";
                return false;
            }
            String line = null;
            Regex reg = new Regex(@"([^\(]*)\((.*)\)$");
            while ((line = reader.ReadLine()) != null){
                if (line.Trim().Length == 0) continue;
                if (line.StartsWith("#")) continue; //commens
                Match m = reg.Match(line);
                if (m.Success) {
                    ScriptFunction func = createFuncObj(m.Groups[1].Value, m.Groups[2].Value);
                    if (func != null)
                    {
                        mCmdList.Add(new CmdDefintion(m.Groups[1].Value, func, m.Groups[2].Value));
                    }
                    else {
                        Trace.WriteLine(String.Format("No the function {0:s} defined ", m.Groups[1].Value));
                    }
                }
            }
            reader.Close();
            Last_Error = "";
            return true;
        }
        static public int execute(Dictionary<String, String> record) {
            ScriptExecuter.record = record;
            Boolean exec_res = true;
            int cmd_idx = 1;
            if (mCmdList.Count == 0) return -1;
            foreach (CmdDefintion cmd in mCmdList) {
                exec_res = cmd.execute(record);
                if (exec_res == false) break;
                cmd_idx++;
            }
            if (exec_res == true)
            {
                Last_Error = "";
                return 0;
            }
            return cmd_idx;
        }
        static public ScriptFunction createFuncObj(String func_name, String args) {
            ScriptFunction func = null;
            switch (func_name) {
                case "ActiveWindow":
                    func = new ActiveWindowFunction(args);
                    break;
                case "SetAndCheckWinRect":
                    func = new SetAndCheckWinRectFunction(args);
                    break;
                case "CopyClipboard":
                    func = new CopyClipboardFunction(args);
                    break;
                case "sleep":
                    func = new sleepFunction(args);
                    break;
                case "SendInput":
                    func = new SendInputFunction(args);
                    break;
                case "SetMouse":
                    func = new SetMouseFunction(args);
                    break;
                case "SendMessage":
                    func = new SendMessageFunction(args);
                    break;
                case "CheckResolution":
                    func = new CheckResolutionFunction(args);
                    break;
                case "ReplaceString":
                    func = new ReplaceStringFunction(args);
                    break;
                default:
                    Trace.WriteLine("no cmd know" + func_name);
                    break;
            }
            return func;
        }
        public interface ScriptFunction {
            Boolean execute(Dictionary<string, String> pv);
            
        }
        class sleepFunction : ScriptFunction
        {
            int msec = 0;
            public sleepFunction(String args) {
                if (int.TryParse(args, out msec) == false)
                {
                    Trace.WriteLine("参数错误  " + args);
                    msec = 0;
                }
            }
            public bool execute(Dictionary<string, String> pv)
            {
                if (msec != 0)
                {
                    try
                    {
                        Thread.Sleep(msec);
                    }
                    catch(Exception e) {
                        Last_Error = "Sleep with 错误 " + e.Message;
                        Trace.WriteLine(Last_Error);
                        return false;
                    }
                    return true;
                }
                Last_Error = "Sleep 参数错误";
                Trace.WriteLine(Last_Error);
                return false;
            }
        }
        class SetMouseFunction : ScriptFunction {
            int x, y;
            Boolean data_ready = false;
            public SetMouseFunction(String args) {
                String[] cor = args.Split(',');
                if (cor.Length == 2 && int.TryParse(cor[0], out x) && int.TryParse(cor[1], out y))
                {
                    data_ready = true;
                }
                else {
                    Trace.WriteLine("参数错误 " + args);
                }
            }
            public bool execute(Dictionary<string, String> pv) {
                if (data_ready) {
                    try
                    {
                        System.Windows.Forms.Cursor.Position = new System.Drawing.Point(x, y);
                        Thread.Sleep(10);
                        MouseSimulator.Click(MouseButtons.Left);
                    }
                    catch (Exception e){
                        Last_Error = "执行鼠标操作错误 " + e.Message;
                        return false;
                    }
                    return true;
                }
                Last_Error = "SetMouse参数错误";
                return false;
            }
        }
        class SendInputFunction : ScriptFunction {
            String con = null;
            String mField = null;
            public SendInputFunction(String args) {
                Regex reg = new Regex(@"\[(.*)\]");
                Match m = reg.Match(args);
                if (m.Success)
                {
                    mField = m.Groups[1].Value;
                }
                else
                {
                    con = args;
                }
            }
            public Boolean execute(Dictionary<string, String> pv) {
                String value = con;
                if (value == null && mField != null)
                {
                    value = ScriptExecuter.GetFieldValue(mField);

                }
                if (value != null)
                {
                    try
                    {
                        SendKeys.SendWait(value);
                        Trace.WriteLineIf(ScriptExecuter.debug_level >= TraceLevel.Info,"发送 " + value);
                    }
                    catch (Exception e) {
                        Last_Error = "SendInput 错误 " + e.Message;
                        return false;
                    }
                }
                else {
                    Trace.WriteLine("无数据可发送");
                }
                return true;
            }
        }
        class CopyClipboardFunction : ScriptFunction {
            Regex reg = new Regex(@"\[(.*)\]");
            String mField = null;
            String con = null;
            public CopyClipboardFunction(String args) {
                Match m = reg.Match(args);
                if (m.Success)
                {
                    mField = m.Groups[1].Value;
                }
                else {
                    con = args;
                }
            }
            public Boolean execute(Dictionary<string,String> pv) {
                String value = con;
                if (value == null && mField != null)
                {
                    value = ScriptExecuter.GetFieldValue(mField);
                }
                if (value != null)
                {
                    try
                    {
                        System.Windows.Forms.Clipboard.Clear();
                        Thread.Sleep(10);
                        System.Windows.Forms.Clipboard.SetText(value);
                        Trace.WriteLineIf(ScriptExecuter.debug_level >= TraceLevel.Info,"拷贝 " + value);
                    }
                    catch (Exception e) {
                        Last_Error = "CopyClipBoard 错误 " + e.Message;
                        return false;
                    }
                    return true;
                }
                else {
                    Last_Error = "CopyClipBoard 不能找到指定的字段 " + mField;
                    Trace.WriteLine(Last_Error);
                }
                return false;
            }
        }
        class SetAndCheckWinRectFunction: ScriptFunction
        {
            int top, left, bottom, right;
            Boolean data_ready = false;
            public SetAndCheckWinRectFunction(String args)
            {
                String[] ps = args.Split(',');
                if (ps.Length == 4 && int.TryParse(ps[0], out top) && int.TryParse(ps[1], out left) && int.TryParse(ps[2], out bottom) && int.TryParse(ps[3], out right))
                {
                    data_ready = true;
                }
                else {
                    Trace.WriteLine("参数错误 " + args);
                }
            }
            public Boolean execute(Dictionary<string, String> pv) {
                if (!data_ready) return false;
                IntPtr wnd = Win32Locator.GetActiveWnd();
                Rect rect = Win32Locator.GetWinRect(wnd);
                if (Win32Locator.MoveWindow(wnd, left, top, right - left, bottom - top, true))
                {
                    return true;
                }
                else {
                    Last_Error = String.Format("调整窗口失败 {0:d} {1:d} {2:d} {3:d}",rect.Top, rect.Left, rect.Bottom, rect.Right);
                    Trace.WriteLine(Last_Error);
                }
                return false;
            }
        }
        class ActiveWindowFunction : ScriptFunction {
            String mTitle = null;
            public ActiveWindowFunction(String args) {
                mTitle = args;
            }
            public Boolean execute(Dictionary<string, String> pv) {
                IntPtr wnd = Win32Locator.locateWindow(mTitle,null);
                if (Win32Locator.SetForeGWindow(wnd)) return true;
                Last_Error = "激活窗口失败，请检查税控窗口是否打开";
                Trace.WriteLine(Last_Error);
                return false;
            }
        }
        class SendMessageFunction : ScriptFunction {
            String wnd_str = null;
            String mField = null;
            String con = null;
            public SendMessageFunction(String args) {
                String[] ps = args.Split(',');
                if (ps.Length == 2) {
                    wnd_str = ps[0];
                    Regex reg = new Regex(@"\[(.*)\]");
                    Match m = reg.Match(ps[1]);
                    if (m.Success)
                    {
                        mField = m.Groups[1].Value;
                    }
                    else {
                        con = ps[1];
                    }
                }
            }
            public Boolean execute(Dictionary<string, String> pv) {
                IntPtr wnd_ptr = Win32Locator.locateWindow(wnd_str, Win32Locator.dotNet_Wnd_Postfix);
                if (wnd_ptr == IntPtr.Zero) return false;
                String value = con;
                if (value == null && mField != null)
                {
                    value = ScriptExecuter.GetFieldValue(mField);
                }
                if (value != null)
                {
                    Trace.WriteLineIf(ScriptExecuter.debug_level > TraceLevel.Info,"Send Message " + value);
                    if(Win32Locator.SendWndMessage(wnd_ptr, value)) return true;
                    Last_Error = "发送消息失败";
                    Trace.WriteLine(Last_Error);
                    return false;
                }
                else
                {
                    Last_Error = ("不能找到指定的字段 " + mField);
                    Trace.WriteLine(Last_Error);
                }
                return false;

            }
        }

        class CmdDefintion {
            ScriptFunction mFunction;
            String param;
            String name;
            public CmdDefintion(String _name, ScriptFunction fun, String args) {
                name = _name;
                mFunction = fun;
                param = args;

            }
            public Boolean execute(Dictionary<string, String> pv) {
                Trace.WriteLineIf(debug_level >= TraceLevel.Info,"Exec " + name);
                return mFunction.execute(pv);
            }
        }

        private class CheckResolutionFunction : ScriptFunction
        {
            int width = 0;
            int height = 0;
            Boolean data_ready = false;
            public CheckResolutionFunction(string args)
            {
                String[] ps = args.Split(',');
                if (ps.Length == 2 && int.TryParse(ps[0], out width) && int.TryParse(ps[1], out height)) {
                    data_ready = true;
                }
            }

            public bool execute(Dictionary<string, string> pv)
            {
                if (data_ready)
                {
                    if (width == Screen.PrimaryScreen.Bounds.Width && height == Screen.PrimaryScreen.Bounds.Height)
                    {
                        return true;
                    }
                    Last_Error = String.Format("屏幕分辨率不匹配 期望{0:d} X {1:d}", width, height);
                    Trace.WriteLine(Last_Error);
                    return false;
                }
                else {
                    Last_Error = "屏幕参数错误";
                    return false;
                }
            }
        }
        private class ReplaceStringFunction : ScriptFunction {
            String field;
            String source_string;
            string dest_string;
            public ReplaceStringFunction(String args) {
                String[] pv = args.Split(',');
                if (pv.Length == 3) {
                    field = pv[0];
                    source_string = pv[1];
                    dest_string = pv[2];
                }
            }
            public bool execute(Dictionary<String, String> pv) {
                if (pv.ContainsKey(field)) {
                    if (pv[field] == source_string) {
                        pv[field] = dest_string;
                    }
                }
                return true;
            }
        }
    }
}
