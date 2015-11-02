using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace CheDaoReciptHike
{
    public abstract class CheDaoInterface {
        public const short data_validation = 1;
        public const short scan_print = 2;
        public const short print_confirm = 10;  //internal use only
        public const short offline_msg = 100;
        public abstract string Response();
        int result = 0;
        public int Result {
            get {
                return result;
            }
            set {
                result = value;
            }
        }
        int m_t;
        public int message_type {
            get {
                return m_t;
            }
            set {
                m_t = value;
            }
        }
    }
    [XmlRootAttribute("Invoice_Verify", Namespace = "", IsNullable = false)]
    public class CheRequest:CheDaoInterface
    {
        [XmlAttribute("Order_Number")]
        public string Order_Number {
            get; set;
        }
        [XmlElementAttribute("Print_Numer")]
        public string Print_Number {
            get; set;
        }
        [XmlElementAttribute("Organ_Numer")]
        public string Organ_Number { get; set; }
        [XmlElementAttribute("Pump_Number")]
        public string Pump_Numer { get; set; }
        [XmlElementAttribute("Amount")]
        public string Amount { get; set; }
        [XmlElementAttribute("Discount_Type")]
        public string Discount_Type { get; set; }
        [XmlElementAttribute("Discount")]
        public string Discount { get; set; }
        [XmlElementAttribute("Product_Type")]
        public string Product_Type { get; set; }
        [XmlElementAttribute("Product_Code")]
        public string Product_Code { get; set; }
        [XmlElementAttribute("Customer_ID")]
        public string Customer_ID { get; set; }
        [XmlElementAttribute("Customer_Text")]
        public string Customer_Text { get; set; }
        [XmlElementAttribute("LicenseNumber")]
        public string LicenseNumber { get; set; }
        [XmlElementAttribute("Time")]
        public string Time  { get; set; }
        [XmlElementAttribute("PhoneNumber")]
        public string PhoneNumber { get; set; }
        [XmlElementAttribute("MergerPrint")]
        public string MergerPrint { get; set; }
        [XmlElementAttribute("Product_Price")]
        public string Product_Price { get; set; }
        [XmlElementAttribute("Product_Number")]
        public string Product_Number { get; set; }

        public CheRequest() {
            message_type = CheDaoInterface.data_validation;
        }
        public static CheRequest create(string str) {
            XmlDocument doc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(CheRequest));
            try
            {
                CheRequest o = (CheRequest) xmlSerializer.Deserialize(new StringReader(str));
                return o;
            }
            catch (Exception e) {
                Trace.WriteLineIf(Program.trace_sw.TraceError," decode XML error " + e.ToString());
                return null;
            }
        }
        public static string get_value(XmlNode recipt, string item) {
            XmlNode node = recipt.SelectSingleNode(item);
            if (node != null) {
                return node.InnerText;
            }
            return "";
        }
        override public string ToString() {
            string res = "";
            res = "no:" + this.Order_Number + " license_no:" + this.LicenseNumber;
            return res;
        }
        public override string Response() {
           return String.Format("<?xml version=\"1.0\" encoding=\"utf-8\" ?> <Invoice_Response Order_Number=" + 
               "\"{0:s}\" Result=\"{1:d}\"><Print_Number>{2:s}</Print_Number><Time>{3:s}</Time>" + 
               "<Result_Detail></Result_Detail></Invoice_Response>",this.Order_Number,this.Result,this.Print_Number,DateTime.Now);
        }
    }
    [XmlRootAttribute("Invoice_Print", Namespace = "", IsNullable = false)]
    public class ChePrintRequest : CheDaoInterface
    {
        public ChePrintRequest() {
            message_type = CheDaoInterface.scan_print;
        }
        [XmlAttribute("Order_Number")]
        public string Order_Number
        {
            get; set;
        }
        [XmlElementAttribute("Print_Type")]
        public string Print_Type
        {
            get; set;
        }
        public override string Response()
        {
            return String.Format("<? xml version =\"1.0\" encoding=\"utf-8\" ?>" + 
                                "<Invoice_Response Order_Number=\"{0:s}\" Result=\"{1:d}\">" + 
                                "<Time>{2:s}</Time ><Result_Detail></Result_Detail></Invoice_Response>",
                                this.Order_Number,this.Result,DateTime.Now);
        }
        public static ChePrintRequest create(string str)
        {
            XmlDocument doc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ChePrintRequest));
            try
            {
                ChePrintRequest o = (ChePrintRequest)xmlSerializer.Deserialize(new StringReader(str));
                return o;
            }
            catch (Exception e)
            {
                Trace.WriteLineIf(Program.trace_sw.TraceError,"decode XML error " + e.ToString());
                return null;
            }
        }
    }
    public class ChePActionRequest : CheDaoInterface
    {
        String _Order_Number;
        ChePActionRequest(String str,String printed_time) {
            this.message_type = CheDaoInterface.print_confirm;
            _Order_Number = str;
            _Printed_Time = printed_time;
        }
        String _Printed_Time;
        public String Printed_Time {
            get { return _Printed_Time; }
        }
        public String Order_Number {
            get { return _Order_Number; }
        }
        public override string Response()
        {
            return null;
        }
        public static ChePActionRequest create(string str,String printed_time) {
            return new ChePActionRequest(str,printed_time);
        }
    }
    static public class CheDaoFactory {

        static FileStream bk_stream = null; //based on requirement, the tool need to buffer the package received in background
        static FileStream rst_stream = null;// the stream for restore;
                                     /** bk stream stategy:
                                         use two file to save the bk package, one is active, another is standby. call cehdao-bk-A.dat
                                     **/
        static String bk_fn = "chedao-bk.dat";
        static String active_a = "A-";
        static int Invoice_Request_Count = 0;
        static int Print_Request_Count = 0;
        static int Print_Act_Count = 0;
        static int save_error_count = 0;

        static Dictionary<String, CheRequest> mPendingList = new Dictionary<String, CheRequest>(); //dont send to UI. until the confirm message arrive.

        public static void init() {
            try
            {
                rst_stream = new FileStream(active_a + bk_fn, FileMode.Open, FileAccess.Read);
                active_a = "B-";
            }
            catch (Exception e)
            {
                active_a = "B-";
            }
            if (rst_stream == null)
            {
                try
                {
                    rst_stream = new FileStream(active_a + bk_fn, FileMode.Open, FileAccess.Read);

                }
                catch (Exception e)
                {
                    rst_stream = null;
                }
                active_a = "A-";
            }
            //now open a stream for write
            bk_stream = new FileStream(active_a + bk_fn, FileMode.Create);
            try
            {
                restore();
            }
            catch (Exception e) {
                Trace.WriteLineIf(Program.trace_sw.TraceError,"Restore found a error " + e.ToString());
            }
        }
        public static void close() {
            try
            {
                lock (bk_stream)
                {
                    if (bk_stream != null)
                    {
                        bk_stream.Close();
                    }
                }
            }
            catch (Exception e) {
                Trace.WriteLineIf(Program.trace_sw.TraceError, "Close bk stream error with " + e.Message);
            }
        }

        public static string Dump() {
            String res = "Active File bk: " + active_a + bk_fn + Environment.NewLine + "dict cache has " + mPendingList.Keys.Count.ToString() + Environment.NewLine;
            res += String.Format("Invoice Request:{0:d}  Print Request:{1:d} Print Act {2:d} save err:{3:d}", Invoice_Request_Count, Print_Request_Count, Print_Act_Count, save_error_count) + Environment.NewLine;
            foreach (String key in mPendingList.Keys) {
                res += key + Environment.NewLine;
            }
            
            return res;
        }

        public static void restore() {
            const int max_fragment = 1024;
            byte[] buffer = new byte[max_fragment];
            int read_len = 0;
            if (rst_stream != null)
            {
                int msg_count = 0;
                int discard_count = 0;
                Trace.WriteLineIf(Program.trace_sw.TraceVerbose,"restore dumped package","info");
                Boolean read_ok = false;
                do
                {
                    read_ok = false;
                    read_len = rst_stream.Read(buffer, 0, sizeof(int));
                    if (read_len != sizeof(int)) { continue; }
                    int msg_len = BitConverter.ToInt32(buffer, 0);
                    read_len = rst_stream.Read(buffer, 0, sizeof(short));
                    if (read_len != sizeof(short)) { continue; }
                    short msg_type = BitConverter.ToInt16(buffer, 0);
                    read_len = rst_stream.Read(buffer, 0, sizeof(long));
                    if (read_len != sizeof(long)) { continue; }
                    DateTime c_t = new DateTime(BitConverter.ToInt64(buffer, 0));
                    read_len = rst_stream.Read(buffer, 0, msg_len);
                    if (read_len != msg_len) { continue; }
                    read_ok = true;
                    msg_count++;
                    if (c_t.AddHours(1) < DateTime.Now)
                    {
                        discard_count++;
                        continue; // skip the message because it is too old.
                    }
                    LocalChePackage p = new LocalChePackage(msg_type, buffer, msg_len, c_t.Ticks);
                    _HandlePackage(p);
                } while (read_ok);
                Trace.WriteLineIf(Program.trace_sw.TraceInfo,String.Format("restore done found msg {0:d} discard {1:d}",msg_count,discard_count),"info");
                String obsolete_fn = active_a == "A-" ? "B-" : "A-";
                rst_stream.Close();
                try
                {
                    File.Delete(obsolete_fn + bk_fn);
                }
                catch (Exception e) {
                    Trace.WriteLineIf(Program.trace_sw.TraceError,"delete backup file " + obsolete_fn + bk_fn + " failed","error");
                }
            }
            return;
        }



        static CheDaoInterface create(LocalChePackage p) {
            Trace.WriteLineIf(Program.trace_sw.TraceVerbose,String.Format("incomming package {0:d} {1:s}", p.msg_type, p.rawXML),"message");
            if (p.msg_type == CheDaoInterface.data_validation)
            {
                Invoice_Request_Count++;
                return CheRequest.create(p.rawXML);
            }
            if (p.msg_type == CheDaoInterface.scan_print) {
                Print_Request_Count++;
                return ChePrintRequest.create(p.rawXML);
            }
            if (p.msg_type == CheDaoInterface.print_confirm) {
                Print_Act_Count++;
                return ChePActionRequest.create(p.rawXML,p.created_time);//it is just a string
            }
            return null;
        }

        static CheDaoInterface _HandlePackage(LocalChePackage p) {
            CheDaoInterface msg = create(p);
            if (msg != null) {
                switch(msg.message_type){
                    case CheDaoInterface.data_validation:
                        CheRequest item = (CheRequest)msg;
                        //put to pending list
                        try
                        {
                            mPendingList.Add(item.Order_Number, item);
                        }
                       catch (Exception e)
                        {
                            Trace.WriteLineIf(Program.trace_sw.TraceError,"Insert CheRequest Failed, maybe the key already exist " + e.Message,"error");
                            msg.Result = 2; //2:其它不符;
                        }
                        break;
                    case CheDaoInterface.scan_print:
                        ChePrintRequest p_item = (ChePrintRequest)msg;
                        if (mPendingList.ContainsKey(p_item.Order_Number))
                        {
                            CheRequest w_item = mPendingList[p_item.Order_Number];
                            mPendingList.Remove(p_item.Order_Number);
                            Program.NewRequest(w_item); // to be print
                        }
                        else
                        {
                            Trace.WriteLineIf(Program.trace_sw.TraceError,"invalide print request, no key exist","error");
                            msg.Result = 2;//2:已开票;
                        }
                        break;
                    case CheDaoInterface.print_confirm:
                        Program.NewRequest(msg);
                        break;
                    default:
                        msg = null;
                        break;
                }
                try
                {
                    lock (bk_stream)
                    {
                        if (msg != null) p.save(bk_stream); // just save the valid msg
                    }
                }
                catch (Exception e)
                {
                    save_error_count++;
                    Trace.WriteLineIf(Program.trace_sw.TraceError,"Write backup stream failed " + e.Message,"error");
                }
            }
            return msg;
        }

        internal static byte[] HandlePackage(short cur_type, byte[] msg_body, int buffer_point)
        {
            LocalChePackage p = new LocalChePackage(cur_type, msg_body, buffer_point);
            CheDaoInterface msg = _HandlePackage(p);
            if (msg != null) {
                string rsp_msg = msg.Response();
                Trace.WriteLineIf(Program.trace_sw.TraceVerbose,"send response: " + rsp_msg,"message");
                byte[] body_buffer = Encoding.UTF8.GetBytes(rsp_msg);
                MemoryStream rsp_str = new MemoryStream(body_buffer.Length + 6);
                byte[] tmp = System.BitConverter.GetBytes((int)rsp_msg.Length);
                Array.Reverse(tmp);
                rsp_str.Write(tmp, 0, sizeof(int));
                tmp = System.BitConverter.GetBytes((short)p.msg_type);
                Array.Reverse(tmp);
                rsp_str.Write(tmp, 0, sizeof(short));
                rsp_str.Write(body_buffer,0,body_buffer.Length);
                return rsp_str.GetBuffer();
            }
            return null;
        }
        /* the package is from GUI*/
        internal static void Handle_Internal_Package(short msg_type, byte[] msg_body) {
            LocalChePackage p = new LocalChePackage(msg_type, msg_body, msg_body.Length);
            _HandlePackage(p);
        }

        class LocalChePackage {
            int body_len;
            short inner_msg_type;
            long time;
            byte[] msg_body;
            public LocalChePackage(short _msg_type,byte[] _msg_body,int _body_length) {
                body_len = _body_length;
                inner_msg_type = _msg_type;
                msg_body = _msg_body;
                time = DateTime.Now.Ticks;
            }
            public LocalChePackage(short _msg_type, byte[] _msg_body, int _body_length,long _create_time)
            {
                body_len = _body_length;
                inner_msg_type = _msg_type;
                msg_body = _msg_body;
                time = _create_time;
            }
            public void save(Stream str) {
                str.Write(BitConverter.GetBytes(body_len), 0, sizeof(int));
                str.Write(BitConverter.GetBytes(inner_msg_type), 0, sizeof(short));
                str.Write(BitConverter.GetBytes(time), 0, sizeof(long));
                str.Write(msg_body, 0, body_len);
            }
            public String rawXML {
                get {
                    return Encoding.UTF8.GetString(msg_body, 0, body_len);
                }
            }
            public String created_time {
                get {
                    return new DateTime(this.time).ToString();
                }
            }
            public short msg_type {
                get {
                    if (inner_msg_type > CheDaoInterface.offline_msg)
                    {
                        return (short) (inner_msg_type - CheDaoInterface.offline_msg);
                    }
                    else {
                        return inner_msg_type;
                    }
                }
            }
        }
    }

}