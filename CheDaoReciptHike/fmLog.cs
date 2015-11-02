using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace CheDaoReciptHike
{
    public partial class fmLog : Form
    {
        class fmTrace: TextWriterTraceListener {
            public fmLog con;
            override public void WriteLine(String msg) {
                con.WriteLine(msg);
            }

        }
        fmTrace source;
        public fmLog()
        {
            InitializeComponent();
        }
        public void refresh() {
        }
        public void WriteLine(String msg) {

            if (this.Visible)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    tbLog.AppendText(DateTime.Now.ToString() + ":" + msg + Environment.NewLine);// run in UI thread
                });
            }
        }
        private void btInfo_Click(object sender, EventArgs e)
        {
            tbLog.AppendText(CheDaoFactory.Dump() + Environment.NewLine);
            tbLog.AppendText(Win32Locator.Dump() + Environment.NewLine);
            tbLog.AppendText(Program.log.Dump() + Environment.NewLine);
        }

        private void fmLog_Load(object sender, EventArgs e)
        {
            source = new fmTrace();
            source.con = this;
            fmLog_Resize(sender, e);
        }

        private void fmLog_FormClosed(object sender, FormClosedEventArgs e)
        {
            if(cbRealtimeLog.Checked) Trace.Listeners.Remove(source);
        }

        private void fmLog_Resize(object sender, EventArgs e)
        {
            Control con = (Control)sender;
            tbLog.Height = con.Size.Height - 100;
            tbLog.Width = con.Size.Width - 40;
        }

        private void btClearLog_Click(object sender, EventArgs e)
        {
            tbLog.Clear();
        }

        private void cbRealtimeLog_CheckedChanged(object sender, EventArgs e)
        {
            if (cbRealtimeLog.Checked)
            {
                Trace.Listeners.Add(source); //the trace listener will introduce a deadlock, suppress it until a good solution.
            }
        }
    }
    public class LogTrace:TextWriterTraceListener{
        System.IO.StreamWriter log_file = null;
        int error_count;
        String fn;
        //int smart_flush = 0;
        public LogTrace() {
            String fn = String.Format("log\\log-{0:d}-{1:d}-{2:d}.log", DateTime.Now.Year,DateTime.Now.Month,DateTime.Now.Day);
            log_file = new System.IO.StreamWriter(new FileStream(fn,FileMode.Append));
            Trace.Listeners.Clear();
            //this.Writer = log_file;
            Trace.Listeners.Add(this);
        }
        public override void WriteLine(string message)
        {
            //base.WriteLine(DateTime.Now.ToString() + ":" + message);
            lock (log_file)
            {
                if(log_file != null) log_file.WriteLine(DateTime.Now.ToString() + ":" + message);
            }
        }
        public override void Flush()
        {
            /*smart_flush++;
            if (smart_flush < 10) {
                return;
            }
            smart_flush = 0;*/
            try
            {
                //base.Flush();
                lock(log_file){
                    if(log_file != null) log_file.Flush();
                }
            }
            catch (Exception e) {
                error_count++;
                if (error_count > 10) log_file = null;
                else  restore();
                return;
            }
        }
        public override void Close()
        {
            //base.Close();
            lock (log_file)
            {
                if(log_file != null) log_file.Close();
                log_file = null;
            }
        }
        private void restore() {
            lock (log_file)
            {
                log_file = new System.IO.StreamWriter(new FileStream(fn, FileMode.Append));
            }
        }
        public string Dump() {
            return "Error Flush happened " + error_count.ToString();
        }

    }
}
