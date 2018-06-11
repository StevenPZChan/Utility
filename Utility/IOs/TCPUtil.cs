using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Utility.IOs
{
    /// <summary>
    /// TCP通信辅助类
    /// </summary>
    public class TCPUtil
    {
        /// <summary>
        /// 监听到新连接时事件
        /// </summary>
        public event EventHandler<TcpClient> ConnectionAdded;
        /// <summary>
        /// 数据接收时事件
        /// </summary>
        public event EventHandler<string> DataReceived;
        /// <summary>
        /// 错误引发时事件
        /// </summary>
        public event EventHandler<string> Error;

        private TcpClient client;
        private TcpListener listener;
        private Thread listeningthread;
        private Thread datareceive;
        private Socket socket;
        private NetworkStream ns;
        private string address;
        private int port;
        private string type;
        private bool closing = false;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="address">主机地址或域名</param>
        /// <param name="port">端口号</param>
        /// <param name="type">使用方式：client, server, socket</param>
        public TCPUtil(string address, int port, string type = "client")
        {
            this.address = address;
            this.port = port;
            this.type = type;
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public bool Initialize()
        {
            IPAddress ip = Dns.GetHostAddresses(address)[0];
            try
            {
                switch (type)
                {
                    case "client":
                        client = new TcpClient();
                        client.Connect(ip, port);
                        GetStream();
                        datareceive = new Thread(new ThreadStart(DataReceive));
                        datareceive.Start();
                        break;
                    case "server":
                        listener = new TcpListener(ip, port);
                        listener.Start();
                        listeningthread = new Thread(new ThreadStart(NewConnectIn));
                        listeningthread.Start();
                        break;
                    case "socket":
                        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        socket.Connect(ip, port);
                        datareceive = new Thread(new ThreadStart(DataReceive));
                        datareceive.Start();
                        break;
                }
                return true;
            }
            catch (Exception ex)
            {
                Error(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取client的networkstream
        /// </summary>
        public void GetStream()
        {
            ns = GetStream(client);
        }

        /// <summary>
        /// 获取client的networkstream
        /// </summary>
        /// <param name="client">client对象</param>
        /// <returns>networkstream对象</returns>
        public NetworkStream GetStream(TcpClient client)
        {
            try
            {
                NetworkStream ns = client.GetStream();
                ns.WriteTimeout = 3000;
                ns.ReadTimeout = 3000;
                return ns;
            }
            catch (Exception ex)
            {
                Error(this, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// tcp连接关闭
        /// </summary>
        public void Close()
        {
            closing = true;
            Thread.Sleep(3000);
            if (ns != null)
                ns.Close();
            if (client != null)
                client.Close();
            if (listener != null)
            {
                listener.Stop();
                listener = null;
            }
            if (socket != null)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>是否发送成功</returns>
        public bool Send(byte[] message)
        {
            try
            {
                if (type == "socket")
                {
                    socket.Send(message);
                    return true;
                }
                else
                    return Write(ns, message);
            }
            catch (Exception ex)
            {
                Error(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// socket发送数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string message)
        {
            return Send(Encoding.ASCII.GetBytes(message));
        }

        /// <summary>
        /// socket读取数据
        /// </summary>
        /// <returns>读取到的数据</returns>
        public byte[] Receive()
        {
            byte[] message = new byte[1024];
            try
            {
                if (type == "socket")
                {
                    int received = socket.Receive(message, 0, 1024, SocketFlags.None);
                    return message.Take(received).ToArray();
                }
                else
                    return Read(ns);
            }
            catch (Exception ex)
            {
                Error(this, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ns">networkstream对象</param>
        /// <param name="message">数据</param>
        /// <returns>是否发送成功</returns>
        public bool Write(NetworkStream ns, byte[] message)
        {
            try
            {
                ns.Write(message, 0, message.Length);
                return true;
            }
            catch (Exception ex)
            {
                Error(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ns">networkstream对象</param>
        /// <param name="message">数据</param>
        /// <returns>是否发送成功</returns>
        public bool Write(NetworkStream ns, string message)
        {
            return Write(ns, Encoding.ASCII.GetBytes(message));
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="ns">networkstream对象</param>
        /// <returns>读取到的数据</returns>
        public byte[] Read(NetworkStream ns)
        {
            byte[] message = new byte[1024];
            try
            {
                int received = ns.Read(message, 0, 1024);
                return message.Take(received).ToArray();
            }
            catch (Exception ex)
            {
                Error(this, ex.Message);
                return null;
            }
        }

        private void NewConnectIn()
        {
            while (!closing)
                ConnectionAdded.BeginInvoke(this, listener.AcceptTcpClient(), null, null);
        }

        private void DataReceive()
        {
            while (!closing)
            {
                byte[] message = Receive();
                if (message.Length > 0)
                    DataReceived(this, Encoding.ASCII.GetString(message));
            }
        }
    }
}
