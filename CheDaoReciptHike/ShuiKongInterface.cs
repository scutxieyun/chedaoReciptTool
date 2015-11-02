using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheDaoReciptHike
{
    interface ShuiKongInterface
    {
        Boolean DetectShuiKong();
        Boolean SendRecipt(CheRequest req);
        String GetPattern(int target);
    }
    class Win32Locator {
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        static String wnd_trace; // for debug use
        static public String Dump() {
            return "Latest window trace record: " + wnd_trace;
        }
        static public IntPtr locateWindow(String path)
        {
            String[] nodes = path.Split(new Char[] { '/' });
            wnd_trace = "";
            IntPtr cur = IntPtr.Zero; // desktop
            int index = 0;
            if (nodes.Length == 0) return IntPtr.Zero;
            do {
                cur = NewFindWindow(cur, nodes[index]);
                wnd_trace = String.Format("{0:s} -> {1:X00000}", wnd_trace, cur.ToInt64());
                index++;
            } while (cur != null && index < nodes.Length);
            return cur;
        }
        static IntPtr NewFindWindow(IntPtr p, String f)
        {
            Regex reg = new Regex(@"([S|T])\((.*)\)\x5b(\d+)\x5d");
            Match m = reg.Match(f);
            String WndClass = null;
            String WndName = null;
            IntPtr res = IntPtr.Zero;
            IntPtr cur_child = IntPtr.Zero;
            int WndIndex = 0;
            if (m.Success) {
                if (m.Groups[1].Value == "T")
                {
                    WndClass = m.Groups[2].Value == "" ? null : m.Groups[2].Value;
                }
                if (m.Groups[1].Value == "S") {
                    WndName = m.Groups[2].Value == "" ? null : m.Groups[2].Value;
                }
                int.TryParse(m.Groups[3].Value, out WndIndex);
                int cur_index = 0;
                do
                {
                    cur_child = FindWindowEx(p, cur_child, WndClass, WndName);
                    cur_index++;
                } while (cur_child != null && cur_index <= WndIndex);
                res = cur_child;
            }
            return res;
        }
        static private String getQuotedString(String in_str)
        {
            Regex reg = new Regex(@"""([^""\\]*)""");
            Match m = reg.Match(in_str);
            if (m.Success)
            {
                return m.Groups[1].Value;
            }
            return null;
        }

    }

    class SendKeyShuiKong : ShuiKongInterface
    {
        String anch_wnd_str = null;
        IntPtr anch_wnd_handle = IntPtr.Zero;
        int max_send_ops;
        String next_key = "{ENTER}";
        public interface ValueToBeSent
        {
            String getValue(CheRequest req);
        }
        class FieldSend : ValueToBeSent
        {
            PropertyInfo info;
            String field_name;
            String next_key = "{ENTER}";
            public FieldSend(String field,String _next_key)
            {
                info = typeof(CheRequest).GetProperty(field);
                next_key = _next_key;
                field_name = field;
                if (info == null) {
                    Trace.WriteLineIf(Program.trace_sw.TraceError, "no field defined:" + field, "error");
                }
            }
            string ValueToBeSent.getValue(CheRequest req)
            {
                if (info != null)
                {
                    Object o = info.GetValue(req, null);
                    if (o != null) return o.ToString() + next_key;
                    else return next_key;
                }
                return "";
            }
        }
        class NextSend : ValueToBeSent
        {
            String next_key;
            public NextSend(String str)
            {
                next_key = str;
            }
            string ValueToBeSent.getValue(CheRequest req)
            {
                return next_key;
            }
        }
        class StringSend : ValueToBeSent
        {
            String text;
            String next_key = "{ENTER}";
            public StringSend(String str,String _next_key)
            {
                text = str;
                next_key = _next_key;
            }
            string ValueToBeSent.getValue(CheRequest req)
            {
                return text + next_key;
            }
        }
        List<ValueToBeSent> actions = new List<ValueToBeSent>();

        public ValueToBeSent createAction(String action)
        {
            Regex reg = new Regex(@"F\((.*)\)");
            if (action == null) return new NextSend(next_key);
            Match m = reg.Match(action);
            if (m.Success == true)
            {
                return new FieldSend(m.Groups[1].Value,next_key);
            }
            return new StringSend(action,next_key);
        }
        public SendKeyShuiKong()
        {
            Object o = ConfigurationManager.GetSection("shuikong_layout_cfg");
            NameValueCollection cfg = (NameValueCollection)o;
            anch_wnd_str = cfg["shuikong_wnd_path"];
            if (cfg["next_key"] != null) { next_key = cfg["next_key"]; }
            try
            {
                max_send_ops = int.Parse(cfg["max_send_ops"]);
            }
            catch (Exception)
            {
                throw new Exception("Parse Parameters failed in max_send_ops");
            }
            for (int i = 0; i < max_send_ops; i++)
            {
                String action = cfg[i.ToString()];
                actions.Add(createAction(action));
            }

        }
        bool ShuiKongInterface.DetectShuiKong()
        {
            IntPtr tmp = anch_wnd_handle;
            anch_wnd_handle = Win32Locator.locateWindow(anch_wnd_str);
            if (tmp != anch_wnd_handle)
            {
                Trace.WriteLineIf(Program.trace_sw.TraceInfo,String.Format("locate {0:s} at {1:X00000}",anch_wnd_str,anch_wnd_handle));
            }
            return anch_wnd_handle != IntPtr.Zero;
        }

        string ShuiKongInterface.GetPattern(int target)
        {
            return anch_wnd_str;
        }
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern Boolean SetForegroundWindow(IntPtr hwnd);
        bool ShuiKongInterface.SendRecipt(CheRequest req)
        {
            if (anch_wnd_handle != IntPtr.Zero)
            {
                SetForegroundWindow(anch_wnd_handle);
                for (int i = 0; i < this.max_send_ops; i++)
                {
                    String str = actions[i].getValue(req);
                    SendKeys.Send(str);
                }
                return true;
            }
            return false;
        }
    }
    class YiYeShuiKong:ShuiKongInterface{
        //static String edit_loc = "S((亿业)网络发票及管理系统)[0]/T(TPanel)[0]/T(TPanel)[0]/T(TF_write_jhd)[0]/T(TPageControl)[0]/S(发票明细)[0]/T(TPanel)[3]/T(TwwDBLookupCombo)[0]";
        static String edit_loc = "S((亿业)网络发票及管理系统)[0]";
        IntPtr edit_ptr = IntPtr.Zero;
        bool ShuiKongInterface.DetectShuiKong()
        {
            edit_ptr = Win32Locator.locateWindow(edit_loc);
            if (edit_ptr == IntPtr.Zero)
            {
                Trace.WriteLineIf(Program.trace_sw.TraceInfo, "locate " + edit_loc + " failed");
                return false;
            }
            else {
                Trace.WriteLineIf(Program.trace_sw.TraceInfo, String.Format("locate {0:s} at {1:x}",edit_loc, edit_ptr));
                return true;
            }

        }
        
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        public const uint WM_SETTEXT = 0x000C;
        bool SendText(int target, string text)
        {
            if (edit_ptr != IntPtr.Zero) {
                try {
                    SendMessage(edit_ptr, WM_SETTEXT, IntPtr.Zero, text);
                    int res = Marshal.GetLastWin32Error();
                    if (res != 0) {
                        Trace.WriteLineIf(Program.trace_sw.TraceError,String.Format("sendmessage with error code:{0:d}", res),"error");
                        return false;
                    }
                } catch (Exception e) {
                    Trace.WriteLineIf(Program.trace_sw.TraceError,"Send Text Failed with " + e.ToString(),"error");
                    return false;
                }
            }
            return true;
        }
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern Boolean SetForegroundWindow(IntPtr hwnd);
        Boolean ShuiKongInterface.SendRecipt(CheRequest req) {
            if (edit_ptr != IntPtr.Zero)
            {
                //SendMessage(edit_ptr, WM_SETTEXT, IntPtr.Zero, req.Client);
                //SetFocus(edit_ptr);
                SetForegroundWindow(edit_ptr);
                SendKeys.Send(req.Customer_Text);
                SendKeys.Send("{ENTER}");//地址
                SendKeys.Send("佛山市南海大道");//地址
                SendKeys.Send("{ENTER}");//行业分类
                SendKeys.Send("汽油");
                SendKeys.Send("{ENTER}");//开票人
                SendKeys.Send("{ENTER}");//收款人
                SendKeys.Send("{ENTER}");//币种
                SendKeys.Send("人民币");
                SendKeys.Send("{ENTER}");//备注
                SendKeys.Send("车到加油打印");
                SendKeys.Send("{ENTER}");//gride
                SendKeys.Send("{ENTER}");//税目代码
                SendKeys.Send("{ENTER}");//税率
                SendKeys.Send("{ENTER}");//项目
                SendKeys.Send(req.Product_Code);//收款人
                SendKeys.Send("{ENTER}");//单位
                SendKeys.Send("{ENTER}");//数量
                SendKeys.Send(req.Product_Number);
                SendKeys.Send("{ENTER}");//单价
                SendKeys.Send(req.Product_Price);//数量
                SendKeys.Send("{ENTER}");//金额
                SendKeys.Send(req.Amount);//数量
                int res = Marshal.GetLastWin32Error();
                Trace.WriteLine("SetFocus res " + res.ToString());
                //SendMessage(edit_ptr,)
            }
            return true;
        }

        public string GetPattern(int target)
        {
            return edit_loc;
        }
    }
}
