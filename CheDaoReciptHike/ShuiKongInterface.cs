using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using WndInteract;

namespace CheDaoReciptHike
{
    interface ShuiKongInterface
    {
        Boolean DetectShuiKong();
        Boolean SendRecipt(CheRequest req);
        String GetPattern(int target);
        String TestLocationFunction();

    }

    public static class ShuiKongFactory {
        static ShuiKongInterface mInterface = null;
        public static void init(){
            String shuikong_name = ConfigurationManager.AppSettings["shuikong_interface"];
            if (shuikong_name == null) shuikong_name = "CheDaoReciptHike.SendKeyShuiKong";
            //SndMsgShuiKong SendKeyShuiKong, JinSuiGenSendKeyImp, JinSuiGenSndMsgImp
            switch (shuikong_name) {
                case "CheDaoReciptHike.SendKeyShuiKong":
                    mInterface = (ShuiKongInterface)new CheDaoReciptHike.SendKeyShuiKong();
                    break;
                case "CheDaoReciptHike.SndMsgShuiKong":
                    mInterface = (ShuiKongInterface)new CheDaoReciptHike.SendKeyShuiKong();
                    break;
                case "JinSuiGenSendKeyImp":
                    mInterface = new CheDaoReciptHike.JinSuiGenSendKeyImp();
                    break;
                case "JinSuiGenSndMsgImp":
                    mInterface = new CheDaoReciptHike.JinSuiGenSndMsgImp();
                    break;
                default:
                    mInterface = null;
                    Trace.WriteLineIf(Program.trace_sw.TraceError, "Missed Shuikong Inteface [shuikong_interface]参数");
                    break;
            }
        }
        public static Boolean DetectShuiKong() {
            if (mInterface != null) return mInterface.DetectShuiKong();
            return false;
        }
        public static Boolean SendRecipt(CheRequest req) {
            if (mInterface != null) return mInterface.SendRecipt(req);
            return false;
        }
        public static String GetPattern(int target) {
            if (mInterface != null) return mInterface.GetPattern(target);
            return "Not defined";
        }
        public static String Test() {
            if (mInterface != null) return mInterface.TestLocationFunction();
            return "not defined";
        }
    }
    

    class SendKeyShuiKong : ShuiKongInterface
    {
        protected String anch_wnd_str = null;
        protected String first_editor_wnd_str = null;
        protected IntPtr anch_wnd_handle = IntPtr.Zero;
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
            this.first_editor_wnd_str = cfg["first_focus"];
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
        protected virtual void AnchWndChangeEvt(IntPtr wnd) {
            return;
        }
        bool ShuiKongInterface.DetectShuiKong()
        {
            IntPtr tmp = anch_wnd_handle;
            anch_wnd_handle = Win32Locator.locateWindow(anch_wnd_str,null);
            if (tmp != anch_wnd_handle)
            {
                Trace.WriteLineIf(Program.trace_sw.TraceVerbose,String.Format("locate {0:s} at {1:X00000}",anch_wnd_str,anch_wnd_handle));
                AnchWndChangeEvt(anch_wnd_handle);

            }
            //Trace.WriteLineIf(Program.trace_sw.TraceInfo, String.Format("locate {0:s} at {1:X00000}", anch_wnd_str, anch_wnd_handle));
            return anch_wnd_handle != IntPtr.Zero;
        }

        string ShuiKongInterface.GetPattern(int target)
        {
            return anch_wnd_str;
        }
        protected virtual String GetWndClassPostFix() {
            return null;
        }

        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern Boolean SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern IntPtr SetFocus(IntPtr hwnd);
        bool ShuiKongInterface.SendRecipt(CheRequest req)
        {
            if (anch_wnd_handle != IntPtr.Zero)
            {
                Win32Locator.SetForeGWindow(anch_wnd_handle);
                if (this.first_editor_wnd_str != null) {
                    IntPtr f_e = Win32Locator.locateWindow(this.first_editor_wnd_str, GetWndClassPostFix());
                    if (f_e != IntPtr.Zero)
                    {
                        Win32Locator.SetForeGWindow(f_e);
                        //SetFocus(f_e); the method doesn't work
                    }
                    else {
                        Trace.WriteLineIf(Program.trace_sw.TraceError,"did not find the window");
                    }
                }
                for (int i = 0; i < this.max_send_ops; i++)
                {
                        String str = actions[i].getValue(req);
                        SendKeys.Send(str);
                }
                return true;
            }
            return false;
        }

        String ShuiKongInterface.TestLocationFunction()
        {
            String res = "Test:\n Detect " + anch_wnd_str + "\n";
            ((ShuiKongInterface)this).DetectShuiKong();
            res += Win32Locator.Dump() + Environment.NewLine;
            if (this.first_editor_wnd_str != null)
            {
                String fix = GetWndClassPostFix() != null ? GetWndClassPostFix() : "";
                res += "find 1st Editor " + this.first_editor_wnd_str + "with postfix " + fix + Environment.NewLine;
                Win32Locator.locateWindow(this.first_editor_wnd_str, GetWndClassPostFix());
                res += Win32Locator.Dump();
            }
            return res;
        }
    }


    class SndMsgShuiKong : ShuiKongInterface
    {

        Dictionary<String, PropertyInfo> mFieldMap = new Dictionary<String, PropertyInfo>();
        String anch_wnd_str = null;
        IntPtr anch_wnd_handle = IntPtr.Zero;

        public SndMsgShuiKong()
        {
            Object o = ConfigurationManager.GetSection("shuikong_field_maps");
            NameValueCollection cfg = (NameValueCollection)o;
            foreach (String wnd_str in cfg.Keys) {
                PropertyInfo info = typeof(CheRequest).GetProperty(cfg[wnd_str]);
                if (info != null) {
                    mFieldMap.Add(wnd_str, info);
                    if (anch_wnd_str == null) anch_wnd_str = wnd_str; 
                }
                if (wnd_str == "shuikong_wnd_path") {
                    anch_wnd_str = cfg[wnd_str];
                }

            }
        }
        public bool DetectShuiKong()
        {
            IntPtr tmp = Win32Locator.locateWindow(anch_wnd_str, null);
            if (tmp != anch_wnd_handle) {
                anch_wnd_handle = tmp;
                AnchWndChangeEvt(tmp);
            }
            return tmp != IntPtr.Zero;
        }

        public string GetPattern(int target)
        {
            return anch_wnd_str;
        }
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern Boolean SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        public const uint WM_SETTEXT = 0x000C;
        public bool SendRecipt(CheRequest req)
        {
            Boolean res = false;
            IntPtr wnd_ptr;
            if (anch_wnd_handle != null)
            {
                Win32Locator.SetForeGWindow(anch_wnd_handle);
            }
            else {
                return false;
            }
            foreach (String wnd_str in mFieldMap.Keys) {
                res = false;
                if (wnd_str != null) {
                    wnd_ptr = Win32Locator.locateWindow(wnd_str, GetWndClassPostFix());
                    if (wnd_ptr == null) break;
                    String text = mFieldMap[wnd_str].GetValue(req, null).ToString();
                    try
                    {

                        SendMessage(wnd_ptr, WM_SETTEXT, IntPtr.Zero, text);
                        int r = Marshal.GetLastWin32Error();
                        if (r != 0)
                        {
                            Trace.WriteLineIf(Program.trace_sw.TraceError, String.Format("sendmessage with error code:{0:d}", res), "error");
                            break;
                        }
                    }
                    catch (Exception e)
                    {
                        Trace.WriteLineIf(Program.trace_sw.TraceError, "Send Text Failed with " + e.ToString(), "error");
                        break;
                    }
                }
                res = true;
            }
            return res;
        }

        public String TestLocationFunction()
        {
            String res = "Test:\n Detect " + anch_wnd_str + "\n";
            ((ShuiKongInterface)this).DetectShuiKong();
            res += Win32Locator.Dump() + Environment.NewLine;
            foreach (String wnd_str in mFieldMap.Keys) {
                String fix = GetWndClassPostFix() != null ? GetWndClassPostFix() : "";
                res += "Find " + wnd_str + " with postfix " + fix + Environment.NewLine;
                Win32Locator.locateWindow(wnd_str, GetWndClassPostFix());
                res += Win32Locator.Dump();
            }
            return res;
        }

        protected virtual void AnchWndChangeEvt(IntPtr wnd)
        {
            return;
        }

        protected virtual String GetWndClassPostFix()
        {
            return null;
        }

    }

    class JinSuiGenSendKeyImp : SendKeyShuiKong
    {
        Regex mNetReg = new Regex(@"WindowsForms10\.Window\.8\.app\.(.*)");
        String mPostFix = null;
        public JinSuiGenSendKeyImp() : base() {

        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd,
                                    StringBuilder lpClassName,
                                    int nMaxCount
                                    );
        public static string GetWindowClassName(IntPtr hWnd)
        {
            StringBuilder buffer = new StringBuilder(128);

            GetClassName(hWnd, buffer, buffer.Capacity);

            return buffer.ToString();
        }
        protected override void AnchWndChangeEvt(IntPtr wnd)
        {
            base.AnchWndChangeEvt(wnd);
            if (wnd == null) return;
            StringBuilder buffer = new StringBuilder(256);
            if (GetClassName(wnd, buffer, buffer.Capacity) == 0) return;
            Match m = mNetReg.Match(buffer.ToString());
            if (m.Success)
            {
                mPostFix = m.Groups[1].Value;
            }
            else {
                mPostFix = null;
            }
        }
        protected override string GetWndClassPostFix()
        {
            return mPostFix;
        }
    }
    class JinSuiGenSndMsgImp : SndMsgShuiKong
    {
        Regex mNetReg = new Regex(@"WindowsForms10\.Window\.8\.app\.(.*)");
        String mPostFix = null;
        public JinSuiGenSndMsgImp() : base()
        {

        }
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd,
                                    StringBuilder lpClassName,
                                    int nMaxCount
                                    );
        public static string GetWindowClassName(IntPtr hWnd)
        {
            StringBuilder buffer = new StringBuilder(128);

            GetClassName(hWnd, buffer, buffer.Capacity);

            return buffer.ToString();
        }
        protected override void AnchWndChangeEvt(IntPtr wnd)
        {
            base.AnchWndChangeEvt(wnd);
            if (wnd == null) return;
            StringBuilder buffer = new StringBuilder(256);
            if (GetClassName(wnd, buffer, buffer.Capacity) == 0) return;
            Match m = mNetReg.Match(buffer.ToString());
            if (m.Success)
            {
                mPostFix = m.Groups[1].Value;
            }
            else
            {
                mPostFix = null;
            }
        }
        protected override string GetWndClassPostFix()
        {
            return mPostFix;
        }
    }

}
