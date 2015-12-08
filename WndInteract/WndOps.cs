using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace WndInteract
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Rect
    {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    public class Win32Locator
    {
        [DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        private static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        internal static extern Boolean SetForegroundWindow(IntPtr hwnd);
        [DllImport("user32.dll", SetLastError = true)]
        internal static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);
        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hwnd, ref Rect rectangle);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassName(IntPtr hWnd,
                                    StringBuilder lpClassName,
                                    int nMaxCount
                                    );
        [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        internal static extern int GetWindowText(IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, string lParam);
        public const uint WM_SETTEXT = 0x000C;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool EnumWindows(CallBackPtr lpEnumFunc, IntPtr lParam);

        public delegate bool CallBackPtr(IntPtr hwnd, int lParam);
        static private CallBackPtr callBackPtr;
        static List<String> mAvailableWins = new List<string>();



        static Regex mNetReg = new Regex(@"WindowsForms10\.Window\.8\.app\.(.*)");  //detect .Net Windows
        static public String dotNet_Wnd_Postfix = null;

        static String wnd_trace; // for debug use
        static public String Dump()
        {
            return Environment.NewLine + "Latest window trace record: " + wnd_trace;
        }

        public static void KickOffEnumWindows()
        {
            callBackPtr = new CallBackPtr(GetWindowHandle);
            mAvailableWins.Clear();
            EnumWindows(callBackPtr, IntPtr.Zero);
        }

        public static List<String> GetWindows() {
            return mAvailableWins;
        }

        private static bool GetWindowHandle(IntPtr windowHandle, int windowHandles)
        {
            StringBuilder stringBuilder = new StringBuilder(256);
            GetWindowText(windowHandle, stringBuilder, stringBuilder.Capacity);
            mAvailableWins.Add(stringBuilder.ToString());
            return true;
        }


        static public Boolean SetForeGWindow(IntPtr hwnd)
        {
            Boolean res = false;
            if (hwnd != IntPtr.Zero)
            {
                res = SetForegroundWindow(hwnd);
                if (res == false)
                {
                    Trace.WriteLine("set to foreground failed");
                }
            }
            return res;
        }
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern IntPtr GetForegroundWindow();

        static public IntPtr GetActiveWnd() {
            return GetForegroundWindow();
        }

        static public Rect GetWinRect(IntPtr wnd) {
            Rect res = new Rect();
            if (!GetWindowRect(wnd, ref res)) {
                res.Bottom = res.Left = res.Top = res.Right = 0;
            }
            return res;
        }

        public static string GetWindowClassName(IntPtr hWnd)
        {
            StringBuilder buffer = new StringBuilder(128);

            GetClassName(hWnd, buffer, buffer.Capacity);

            return buffer.ToString();
        }


        static public Rect centerWindow(IntPtr wnd)
        {
            Rect wnd_rect = new Rect();
            wnd_rect.Bottom = wnd_rect.Left = wnd_rect.Top = wnd_rect.Right = 0;
            if (wnd != IntPtr.Zero)
            {

                //借这个机会检测下目标系统是什么平台
                String wnd_class = GetWindowClassName(wnd);
                if (wnd_class != null) {
                    Match m = mNetReg.Match(wnd_class);
                    if (m.Success)
                    {
                        dotNet_Wnd_Postfix = m.Groups[1].Value;
                    }
                    else
                    {
                        dotNet_Wnd_Postfix = null;
                    }
                }

                System.Drawing.Rectangle resolution = System.Windows.Forms.Screen.PrimaryScreen.Bounds;
                if (GetWindowRect(wnd, ref wnd_rect))
                {
                    int y = (resolution.Size.Height - (wnd_rect.Bottom - wnd_rect.Top)) / 2;
                    int x = (resolution.Size.Width - (wnd_rect.Right - wnd_rect.Left)) / 2;
                    if (MoveWindow(wnd, x, y, wnd_rect.Right - wnd_rect.Left, wnd_rect.Bottom - wnd_rect.Top, true))
                    {
                        GetWindowRect(wnd, ref wnd_rect);
                    }
                    else
                    {
                        Trace.WriteLine(String.Format("移动窗口:{0:X}到中央，操作失败", wnd));
                        wnd_rect.Bottom = wnd_rect.Left = wnd_rect.Top = wnd_rect.Right = 0;
                    }
                }
                else
                {
                    Trace.WriteLine(String.Format("获取窗口:{0:X}形状信息失败", wnd));
                    wnd_rect.Bottom = wnd_rect.Left = wnd_rect.Top = wnd_rect.Right = 0;
                }
            }
            return wnd_rect;
        }

        static public IntPtr locateWindow(String path, String ClassPostFix)
        {
            String[] nodes = path.Split(new Char[] { '/' });
            wnd_trace = "";
            IntPtr cur = IntPtr.Zero; // desktop
            int index = 0;
            if (nodes.Length == 0) return IntPtr.Zero;
            do
            {
                cur = NewFindWindow(cur, nodes[index], ClassPostFix);
                wnd_trace = String.Format("{0:s} -> {1:s}@{2:X00000} ", wnd_trace, nodes[index], cur.ToInt64());
                index++;
            } while (cur != IntPtr.Zero && index < nodes.Length);
            return cur;
        }
        static IntPtr NewFindWindow(IntPtr p, String f, String ClassPostFix)
        {
            Regex reg = new Regex(@"([S|T])\((.*)\)\x5b(\d+)\x5d");
            Match m = reg.Match(f);
            String WndClass = null;
            String WndName = null;
            IntPtr res = IntPtr.Zero;
            IntPtr cur_child = IntPtr.Zero;
            int WndIndex = 0;
            if (ClassPostFix == null) ClassPostFix = "";
            if (m.Success)
            {
                if (m.Groups[1].Value == "T")
                {
                    WndClass = m.Groups[2].Value == "" ? null : m.Groups[2].Value + ClassPostFix;
                }
                if (m.Groups[1].Value == "S")
                {
                    WndName = m.Groups[2].Value == "" ? null : m.Groups[2].Value;
                }
                int.TryParse(m.Groups[3].Value, out WndIndex);
                int cur_index = 0;
                do
                {
                    cur_child = FindWindowEx(p, cur_child, WndClass, WndName);
                    cur_index++;
                } while (cur_child != IntPtr.Zero && cur_index <= WndIndex);
                res = cur_child;
            }
            return res;
        }

        public static Boolean SendWndMessage(IntPtr wnd,String str) {
            try
            {
                SendMessage(wnd, WM_SETTEXT, IntPtr.Zero, str);
                int r = Marshal.GetLastWin32Error();
                if (r != 0)
                {
                    Trace.WriteLine(String.Format("sendmessage with error code:{0:d}", r), "error");
                    return false;
                }
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Send Text Failed with " + e.ToString(), "error");
                return false;
            }
        }

        public static Bitmap CaptureScreen()
        {
            try
            {
                Rectangle resolution = Screen.PrimaryScreen.Bounds;
                Bitmap bitmap = new Bitmap(resolution.Width, resolution.Height);
                Graphics g = Graphics.FromImage(bitmap);
                g.CopyFromScreen(new Point(0, 0), new Point(0, 0), resolution.Size);
                return bitmap;
            }
            catch (Exception e) {
                Trace.Write("截屏错误 " + e.Message);
                return null;
            }
        }
        public static bool HasChinese(string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fa5]");
        }
    }
}
