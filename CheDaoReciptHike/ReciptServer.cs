using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace CheDaoReciptHike
{
    class ReciptServer
    {
        Socket mListener;
        ClientAgent mClient;
        //start the server at given port
        public int start(int port) {
            IPHostEntry ipHost = Dns.GetHostEntry("");
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, port);
            SocketPermission permission;
            permission = new SocketPermission(NetworkAccess.Accept,
                   TransportType.Tcp, "", SocketPermission.AllPorts);
            mListener = new Socket(IPAddress.Any.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            mListener.Bind(ipEndPoint);
            Trace.WriteLine("bind socket to " + port.ToString());
            mListener.Listen(2);    //only one working sessions. reserve one if current connection is hooked. 
            AsyncCallback aCallback = new AsyncCallback(AcceptCallback);

            mListener.BeginAccept(aCallback, mListener);

            return 0;
        }
        public void AcceptCallback(IAsyncResult ar) {
            if (mClient != null){
                mClient.close();//close working session
            }
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);
            mClient = new ClientAgent(handler); //only one working client
            mClient.startReceive();
            AsyncCallback aCallback = new AsyncCallback(AcceptCallback);
            mListener.BeginAccept(aCallback, mListener);
        }
    }
    class ClientAgent {
        const int max_fragment = 1024;
        const int header_length = 6;
        Socket peer = null;
        byte[] mBuffer = new byte[max_fragment - header_length];
        public static ChePacket gPacketHandle = new ChePacket();
        public ClientAgent(Socket s) {
            peer = s;
            Trace.WriteLine("new connect comming from " + peer.RemoteEndPoint.ToString());
            gPacketHandle.reset();
            Program.UpdateStatus(peer.RemoteEndPoint.ToString());
        }
        public void close() {
            if(peer != null) peer.Close();
            peer = null;
            Program.UpdateStatus("无连接");
        }
        public void startReceive() {
            peer.BeginReceive(mBuffer, 0, mBuffer.Length, SocketFlags.None, new AsyncCallback(receiveCallback), null);
        }
        public void receiveCallback(IAsyncResult ar) {
            if (peer == null) return;
            int bytesRead = 0;
            try
            {
                bytesRead = peer.EndReceive(ar);
                if (bytesRead > 0)
                {
                    Trace.WriteLine("new TCP data:" + bytesRead.ToString() + " bytes");
                    int res_len = 0;
                    Byte[] res = gPacketHandle.handle_incomming(mBuffer, bytesRead,out res_len);
                    if (res == null) {
                        throw new Exception("abort the connection because decode package failed");
                    }
                    if (res_len == 0)
                    {
                        Trace.WriteLine("报文处理失败");
                    }
                    else
                    {
                        peer.Send(res, res_len, SocketFlags.None);
                    }
                    startReceive();
                }
            }
            catch (Exception e) {
                //include the client close the connection
                Trace.WriteLine("连接异常中断 " + peer.RemoteEndPoint.ToString() + " 错误信息" + e.ToString());
                this.close();
            }

        }
        public class ChePacket
        {
            MemoryStream in_stream = new MemoryStream(max_fragment);
            MemoryStream out_stream = new MemoryStream(max_fragment);
            

            byte[] msg_body = new byte[max_fragment];//buffer for message body
            byte[] header_l_field = new byte[4];//length field
            byte[] header_t_field = new byte[2];//message type
            int buffer_point = 0; //the end of valid data in msg_body
            int expected_byte = 0;//the remained bytes for a package.
            short cur_type = -1; //message type

            public ChePacket() {
            }

            public void reset() {
                cur_type = -1; expected_byte = 0; buffer_point = 0;
            }
            public byte[] handle_incomming(byte[] data,int length,out int rsp_length) {//data is from stream
                int remainder_byte = length;
                Boolean abort = false;// notify if we need to abort the connection
                byte[] response = new byte[max_fragment];
                out_stream.Seek(0, SeekOrigin.Begin);
                in_stream.Write(data, 0, (int)length);
                in_stream.Seek(0, SeekOrigin.Begin);
                while (remainder_byte >= header_length) {//if there is no a valid header, process it when more data is available
                    if (expected_byte == 0) {//begin of a new message
                        in_stream.Read(header_l_field, 0, 4);
                        remainder_byte -= 4;
                        Array.Reverse(header_l_field);
                        expected_byte = BitConverter.ToInt32(header_l_field, 0);
                        in_stream.Read(header_t_field, 0, 2);
                        Array.Reverse(header_t_field);
                        remainder_byte -= 2;
                        cur_type = BitConverter.ToInt16(header_t_field, 0);
                    }
                    if (expected_byte > max_fragment || expected_byte <= 0) {//incorrect data
                        Trace.WriteLine("报文长度错误 丢弃 " + expected_byte.ToString());
                        this.reset();
                        remainder_byte = 0;
                        abort = true;
                        continue; 
                    }
                    int next_read = remainder_byte < expected_byte ? remainder_byte : expected_byte;
                    if (next_read == 0) continue; //Nothing to do beacause there is no data in stream
                    in_stream.Read(msg_body, buffer_point, next_read);
                    remainder_byte -= next_read;
                    expected_byte -= next_read;
                    buffer_point += next_read;
                    if (expected_byte == 0) {//get a new message
                        byte[] rsp_buffer = CheDaoFactory.HandlePackage(cur_type,msg_body,buffer_point);
                        if (rsp_buffer != null)
                        {
                            buffer_point = 0;
                            out_stream.Write(rsp_buffer, 0, rsp_buffer.Length);
                        }
                        else {
                            remainder_byte = 0; //ignore all package
                        }
                    }
                }
                if (remainder_byte > 0)
                {
                    byte[] b = new byte[header_length];
                    in_stream.Read(b, 0, remainder_byte);
                    in_stream.Seek(0, SeekOrigin.Begin);
                    in_stream.Write(b, 0, remainder_byte);// write byte for next bytes in stream later
                }
                else {
                    in_stream.Seek(0, SeekOrigin.Begin);
                }
                rsp_length = (int) out_stream.Position;
                if (abort) return null;
                return out_stream.GetBuffer();
            }

        }
    }
    

}
