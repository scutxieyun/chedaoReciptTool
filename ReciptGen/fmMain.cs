using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CheDaoReciptHike;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace ReciptGen
{
    public partial class fmMain : Form
    {
        Socket mConnect;
        List<byte[]> InvoiceList = new List<byte[]>();
        public fmMain()
        {
            InitializeComponent();
        }

        private Boolean ConnectServer() {
            IPHostEntry ipRemoteHost = Dns.GetHostEntry(ConfigurationManager.AppSettings["address"]);
            IPAddress ipRemoteAddr = ipRemoteHost.AddressList[1];
            IPEndPoint ipEndPoint = new IPEndPoint(ipRemoteAddr, int.Parse(ConfigurationManager.AppSettings["port"]));
            mConnect = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            mConnect.Connect(ipEndPoint);
            return true;
        }

        private byte[] GenInvoicePackage() {
            CheRequest req = new CheRequest();
            String order_no = "RND"+ new Random().Next().ToString() ;
            req.Order_Number = order_no;
            req.Print_Number = "PR" + order_no;
            req.Product_Code = "94";
            req.Product_Price = "4.5";
            req.Product_Number = "10";
            req.Customer_Text = "中国石化";
            req.Amount = "100";
            req.LicenseNumber = "粤AXX434";
            req.Time = DateTime.Now.ToString();
            req.Pump_Numer = "3";
            ChePrintRequest p_req = new ChePrintRequest();
            p_req.Order_Number = order_no;
            p_req.Print_Type = "1";

            byte[] res = GenerateBytes(1, req);
            byte[] rsp = GenerateBytes(2, p_req);
            InvoiceList.Add(rsp);
            return res;
        }
        private byte[] GenerateBytes(short type, CheDaoInterface req) {
            XmlDocument doc = new XmlDocument();
            XmlSerializer xmlSerializer = new XmlSerializer((req.GetType()));
            MemoryStream body_str = new MemoryStream(1024);
            xmlSerializer.Serialize(body_str, req);
            int length = (int)body_str.Position;
            MemoryStream pkg_str = new MemoryStream(length + 6);
            byte[] tmp = System.BitConverter.GetBytes((int)length);
            Array.Reverse(tmp);
            pkg_str.Write(tmp, 0, 4);
            tmp = System.BitConverter.GetBytes(type);
            Array.Reverse(tmp);
            pkg_str.Write(tmp, 0, 2);
            pkg_str.Write(body_str.GetBuffer(), 0, length);
            return pkg_str.GetBuffer();
        }

        private void tmInvoice_Tick(object sender, EventArgs e)
        {
            if(mConnect.Connected) mConnect.Send(GenInvoicePackage());
        }

        private void tmPrint_Tick(object sender, EventArgs e)
        {
            if (InvoiceList.Count > 0 && cbPrint.Checked) {
                if(mConnect.Connected) mConnect.Send(InvoiceList[0]);
                InvoiceList.RemoveAt(0);
            }
        }

        private void fmMain_Load(object sender, EventArgs e)
        {
            ConnectServer();
        }
    }
}
