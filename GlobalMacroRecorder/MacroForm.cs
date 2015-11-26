using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;


using MouseKeyboardLibrary;
using WndInteract;
using System.Threading;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing.Imaging;

namespace GlobalMacroRecorder
{
    interface LogOutput {
        void write(String str);
    }

    public partial class MacroForm : Form, LogOutput
    {
        Dictionary<String, String> Field_Map = new Dictionary<string, string>();

        const string cfg_file_name = "chedaoshuikong_cfg.txt";

         ListViewItem createListItem(String content,String position) {
            String res;
            ListViewItem item;
            if (Field_Map.TryGetValue(content, out res) == false)
            {
                item = new ListViewItem(content);
                item.SubItems.Add(String.Format("Set({0:s})", content));
                item.SubItems.Add(position);
            }
            else {
                item = new ListViewItem(content);
                item.SubItems.Add(res);
                item.SubItems.Add(position);
            }
            return item;
        }

        List<MacroEvent> events = new List<MacroEvent>();
        int lastTimeRecorded = 0;

        MouseHook mouseHook = new MouseHook();
        KeyboardHook keyboardHook = new KeyboardHook();

        Rect main_wnd_rect = new Rect();
        String main_wnd_str = null;

        int cur_mouse_x = 0;
        int cur_mouse_y = 0;
        

        public MacroForm()
        {

            Win32Locator.KickOffEnumWindows();
            InitializeComponent();
            this.TopMost = true;

            Field_Map.Add("<客户名称>", "Copy([Customer_Text])");
            Field_Map.Add("<油号>", "Set([Product_Code])");
            Field_Map.Add("<单价>", "Set([Product_Price])");
            Field_Map.Add("<总价>", "Set([Amount])");
            Field_Map.Add("<数量>", "Set([Product_Number])");
            Field_Map.Add("[点击]", "<Click>");
            foreach (String str in Field_Map.Keys) {
                cbContent.Items.Add(str);
            }

            main_wnd_rect.Bottom = main_wnd_rect.Left = main_wnd_rect.Top = main_wnd_rect.Right = 0;

            TbLogTrace log = new TbLogTrace(this);
            Trace.Listeners.Add(log);

            mouseHook.MouseMove += new MouseEventHandler(mouseHook_MouseMove);
            //mouseHook.MouseDown += new MouseEventHandler(mouseHook_MouseDown_RecFieldPos);
            //mouseHook.MouseUp += new MouseEventHandler(mouseHook_MouseUp);

            keyboardHook.KeyDown += new KeyEventHandler(keyboardHook_KeyDown);
            //keyboardHook.KeyUp += new KeyEventHandler(keyboardHook_KeyUp);

            read_config(cfg_file_name);

        }

        void mouseHook_MouseMove(object sender, MouseEventArgs e)
        {

            cur_mouse_x = e.X;
            cur_mouse_y = e.Y;
            if (capture_status) {
                lbPosition.Text = String.Format("{0:d},{1:d}", cur_mouse_x, cur_mouse_y);
            }
            /*events.Add(
                new MacroEvent(
                    MacroEventType.MouseMove,
                    e,
                    Environment.TickCount - lastTimeRecorded
                ));

            lastTimeRecorded = Environment.TickCount;*/

        }

        void mouseHook_MouseDown(object sender, MouseEventArgs e)
        {

            events.Add(
                new MacroEvent(
                    MacroEventType.MouseDown,
                    e,
                    Environment.TickCount - lastTimeRecorded
                ));

            lastTimeRecorded = Environment.TickCount;

        }
        Boolean capture_status = false;
        void mouseHook_MouseDown_RecFieldPos(object sender, MouseEventArgs e) {
            if (!capture_status) return;
            if (lvFIelds.SelectedItems.Count == 0) return;
            ListViewItem sel = lvFIelds.SelectedItems[0];
            String pos = String.Format("{0:d},{1:d}", e.X, e.Y);
            sel.SubItems[2].Text = pos;
            capture_status = false;
        }

        void mouseHook_MouseUp(object sender, MouseEventArgs e)
        {

            events.Add(
                new MacroEvent(
                    MacroEventType.MouseUp,
                    e,
                    Environment.TickCount - lastTimeRecorded
                ));

            lastTimeRecorded = Environment.TickCount;

        }

        void keyboardHook_KeyDown(object sender, KeyEventArgs e)
        {
            if (capture_status == false)
            {
                this.recordStartButton.Enabled = true;
                return;
            }
            if (e.Control && e.KeyCode == Keys.F) {
                this.recordStartButton.Enabled = true;
                String position = String.Format("{0:d},{1:d}", cur_mouse_x, cur_mouse_y);
                this.lvFIelds.Items.Add(createListItem(cbContent.Text, position));
                capture_status = false;
                keyboardHook.Stop();
                mouseHook.Stop();
                Win32Locator.SetForeGWindow(this.Handle);
            }

            /*events.Add(
                new MacroEvent(
                    MacroEventType.KeyDown,
                    e,
                    Environment.TickCount - lastTimeRecorded
                ));

            lastTimeRecorded = Environment.TickCount;*/

        }

        

        void keyboardHook_KeyUp(object sender, KeyEventArgs e)
        {

            events.Add(
                new MacroEvent(
                    MacroEventType.KeyUp,
                    e,
                    Environment.TickCount - lastTimeRecorded
                ));

            lastTimeRecorded = Environment.TickCount;

        }

        private void recordStartButton_Click(object sender, EventArgs e)
        {
            this.recordStartButton.Enabled = false;
            if (capture_status == true)
            {
                return;
            }

            capture_status = true;

            events.Clear();
            lastTimeRecorded = Environment.TickCount;

            keyboardHook.Start();
            mouseHook.Start();

        }


        private void recordStopButton_Click(object sender, EventArgs e)
        {

            keyboardHook.Stop();
            mouseHook.Stop();

        }

        private void playBackMacroButton_Click(object sender, EventArgs e)
        {

            foreach (MacroEvent macroEvent in events)
            {

                Thread.Sleep(macroEvent.TimeSinceLastEvent);

                switch (macroEvent.MacroEventType)
                {
                    case MacroEventType.MouseMove:
                        {

                            MouseEventArgs mouseArgs = (MouseEventArgs)macroEvent.EventArgs;

                            MouseSimulator.X = mouseArgs.X;
                            MouseSimulator.Y = mouseArgs.Y;

                        }
                        break;
                    case MacroEventType.MouseDown:
                        {

                            MouseEventArgs mouseArgs = (MouseEventArgs)macroEvent.EventArgs;

                            MouseSimulator.MouseDown(mouseArgs.Button);

                        }
                        break;
                    case MacroEventType.MouseUp:
                        {

                            MouseEventArgs mouseArgs = (MouseEventArgs)macroEvent.EventArgs;

                            MouseSimulator.MouseUp(mouseArgs.Button);

                        }
                        break;
                    case MacroEventType.KeyDown:
                        {

                            KeyEventArgs keyArgs = (KeyEventArgs)macroEvent.EventArgs;

                            KeyboardSimulator.KeyDown(keyArgs.KeyCode);

                        }
                        break;
                    case MacroEventType.KeyUp:
                        {

                            KeyEventArgs keyArgs = (KeyEventArgs)macroEvent.EventArgs;

                            KeyboardSimulator.KeyUp(keyArgs.KeyCode);

                        }
                        break;
                    default:
                        break;
                }

            }

        }

        private void MacroForm_Load(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btLocateWnd_Click(object sender, EventArgs e)
        {
            if (cbMainWnd.Text.Trim().Length > 0) {
                IntPtr wnd = Win32Locator.locateWindow(String.Format("S({0:S})[0]", cbMainWnd.Text.Trim()), null);
                if (wnd == IntPtr.Zero) {
                    main_wnd_str = null;
                    MessageBox.Show(String.Format("定位窗口 {0:s} 失败", cbMainWnd.Text.Trim()));
                    lbLayout.Text = "...";
                    return;
                }
                main_wnd_str = cbMainWnd.Text.Trim();
                Win32Locator.SetForeGWindow(wnd);
                main_wnd_rect = Win32Locator.centerWindow(wnd);
                lbLayout.Text = String.Format("屏幕:{0:d}X{1:d},窗口({2:d},{3:d},{4:d},{5:d})", Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height,
                                        main_wnd_rect.Left,main_wnd_rect.Top,main_wnd_rect.Right,main_wnd_rect.Bottom);
                Trace.WriteLine(String.Format("Locate the Wnd at center"));
            }
        }

        private Boolean read_config(String fn) {
            System.IO.StreamReader s_file;
            try {
                s_file = new System.IO.StreamReader(new System.IO.FileStream(fn, FileMode.Open, FileAccess.Read));
                String line;
                lvFIelds.Items.Clear();
                while ((line = s_file.ReadLine()) != null) {
                    if (line.StartsWith("###")) {
                        String[] ps = line.Split('=');
                        if (ps.Length == 2) {
                            switch (ps[0].Trim()) {
                                case "###main_wnd_str":
                                    cbMainWnd.Text = ps[1];
                                    break;
                                case "###field":
                                    String[] row = ps[1].Split('|');
                                    ListViewItem item = new ListViewItem();
                                    item.Text = row[0];
                                    item.SubItems.Add(row[1]);
                                    item.SubItems.Add(row[2]);
                                    lvFIelds.Items.Add(item);
                                    break;
                                default:
                                    break;
                            }
                        }
                    };
                }
                s_file.Close();
                return true;
            }
            catch (Exception e) {
                return false;
            }
        }

        private void btSave_Click(object sender, EventArgs e)
        {
            System.IO.StreamWriter s_file;
            if (this.main_wnd_str == null || this.main_wnd_str.Trim() == "" || (this.main_wnd_rect.Top == this.main_wnd_rect.Bottom)) {
                MessageBox.Show("请选择税控软件输出窗口");
                return;
            }
            s_file = new System.IO.StreamWriter(new FileStream(cfg_file_name, FileMode.Create));
            s_file.WriteLine("#检查屏幕分辨率#");
            s_file.WriteLine(String.Format("CheckResolution({0:d},{1:d})", Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height));
            s_file.WriteLine("#主税控窗口标题# = ");
            s_file.WriteLine("###main_wnd_str=" + this.main_wnd_str);
            s_file.WriteLine(String.Format("ActiveWindow(S({0:s})[0])",this.main_wnd_str));
            s_file.WriteLine("#主窗口大小和位置#");
            s_file.WriteLine(String.Format("SetAndCheckWinRect({0:d},{1:d},{2:d},{3:d})", this.main_wnd_rect.Top, this.main_wnd_rect.Left, this.main_wnd_rect.Bottom, this.main_wnd_rect.Right));
            s_file.WriteLine("#字段填充#");
            foreach(ListViewItem item in lvFIelds.Items)
            {
                String cmd = item.SubItems[1].Text;
                s_file.WriteLine("###field={0:s}|{1:s}|{2:s}", item.Text, item.SubItems[1].Text, item.SubItems[2].Text);
                if (cmd == "<Click>")
                {
                    s_file.WriteLine(String.Format("#点击# = {0:s}", item.SubItems[2].Text));
                    s_file.WriteLine(String.Format("SetMouse({0:s})", item.SubItems[2].Text));
                    s_file.WriteLine(String.Format("sleep({0:s})", tbOpsDelay.Text));
                }
                else
                {
                    Regex reg = new Regex(@"(.*)\((.*)\)$");
                    Match m = reg.Match(cmd);
                    if (m.Success)
                    {
                        if (m.Groups[1].Value == "Copy")
                        {
                            s_file.WriteLine(String.Format("CopyClipboard({0:s})", m.Groups[2].Value));
                            s_file.WriteLine(String.Format("sleep({0:s})", tbOpsDelay.Text));
                            s_file.WriteLine(String.Format("SetMouse({0:s})", item.SubItems[2].Text));
                            s_file.WriteLine(String.Format("sleep({0:s})", tbOpsDelay.Text));
                            s_file.WriteLine("SendInput(^v)");
                            s_file.WriteLine(String.Format("sleep({0:s})", tbOpsDelay.Text));
                        }
                        if (m.Groups[1].Value == "Set") {
                            s_file.WriteLine(String.Format("SetMouse({0:s})", item.SubItems[2].Text));
                            s_file.WriteLine(String.Format("sleep({0:s})", tbOpsDelay.Text));
                            s_file.WriteLine(String.Format("SendInput({0:s})", m.Groups[2].Value));
                            s_file.WriteLine(String.Format("sleep({0:s})", tbOpsDelay.Text));
                        }
                    }
                    else {
                        s_file.WriteLine(String.Format("#数据错误 {0:s} with {1:s}", item.SubItems[0].Text, item.SubItems[1].Text));
                    }
                }
            }
            s_file.Close();
        }

        public void write(string str)
        {
            this.Invoke((MethodInvoker)delegate
            {
                this.tbLog.AppendText(str + Environment.NewLine);
            });
        }

        private void lvFIelds_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete) {
                if (lvFIelds.SelectedItems.Count > 0) {
                    lvFIelds.Items.Remove(lvFIelds.SelectedItems[0]);
                }
            }
        }

        private void cbContent_TextChanged(object sender, EventArgs e)
        {
            if (capture_status == true) {
                capture_status = false;
                this.recordStartButton.Enabled = true;
            }
            lbPosition.Text = "?";
        }

        private void btTest_Click(object sender, EventArgs e)
        {
            ScriptExecuter.init();
            Dictionary<String, String> rec = new Dictionary<string, string>();
            rec.Add("Customer_Text", "北京车到网络科技有限公司");
            rec.Add("Product_Code", "95#");
            rec.Add("Product_Price", "4.5");
            rec.Add("Amount", "300");
            rec.Add("Product_Number", "30");
            ScriptExecuter.readScript("chedaoshuikong_cfg.txt");
            int res = ScriptExecuter.execute(rec);
            if (res != 0) {
                Trace.WriteLine(String.Format("Execute res {0:d}", res));
            }
        }

        private void cbMainWnd_Click(object sender, EventArgs e)
        {
            cbMainWnd.Items.Clear();
            if (cbMainWnd.Items.Count == 0) {
                List<String> wnds = Win32Locator.GetWindows();
                foreach (String str in wnds) {
                    if(str.Trim() != "")
                        cbMainWnd.Items.Add(str);
                }
            }
            Win32Locator.KickOffEnumWindows();
        }

        private void btCapture_Click(object sender, EventArgs e)
        {
            Bitmap bp = Win32Locator.CaptureScreen();
            if (bp != null)
            {
                try
                {
                    bp.Save("shuikong-screen.bmp", ImageFormat.Bmp);
                    MessageBox.Show("屏幕信息保存在 shuikong-screen.bmp, 请发送给技术部门分析");
                }
                catch (Exception ex) {
                    MessageBox.Show("保存失败 " + ex.Message);
                }
            }
        }
    }

    class TbLogTrace : TextWriterTraceListener
    {
        LogOutput out_log;
        override public void WriteLine(String msg)
        {
            out_log.write(msg);
        }
        public TbLogTrace(LogOutput o) {
            out_log = o;
        }
    }

}
