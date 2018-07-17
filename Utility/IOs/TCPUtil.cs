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
        #region Fields
        private readonly string address;
        private readonly int port;
        private readonly string type;
        private TcpClient _client;
        private NetworkStream _ns;
        private bool closing;
        private Thread datareceive;
        private TcpListener listener;
        private Thread listeningthread;
        private Socket socket;
        #endregion

        #region Constructors
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
        #endregion

        #region Events
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
        #endregion

        #region Methods
        /// <summary>
        /// tcp连接关闭
        /// </summary>
        public void Close()
        {
            this.closing = true;
            Thread.Sleep(3000);
            this._ns?.Close();
            this._client?.Close();
            if (this.listener != null)
            {
                this.listener.Stop();
                this.listener = null;
            }

            if (this.socket == null)
                return;

            this.socket.Shutdown(SocketShutdown.Both);
            this.socket.Close();
        }

        /// <summary>
        /// 获取client的networkstream
        /// </summary>
        public void GetStream() => this._ns = GetStream(this._client);

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
                Error?.Invoke(this, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        /// <returns>初始化是否成功</returns>
        public bool Initialize()
        {
            IPAddress ip = Dns.GetHostAddresses(this.address)[0];
            try
            {
                switch (this.type)
                {
                    case "client":
                        this._client = new TcpClient();
                        this._client.Connect(ip, this.port);
                        GetStream();
                        this.datareceive = new Thread(DataReceive);
                        this.datareceive.Start();
                        break;
                    case "server":
                        this.listener = new TcpListener(ip, this.port);
                        this.listener.Start();
                        this.listeningthread = new Thread(NewConnectIn);
                        this.listeningthread.Start();
                        break;
                    case "socket":
                        this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                        this.socket.Connect(ip, this.port);
                        this.datareceive = new Thread(DataReceive);
                        this.datareceive.Start();
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 读取数据
        /// </summary>
        /// <param name="ns">networkstream对象</param>
        /// <returns>读取到的数据</returns>
        public byte[] Read(NetworkStream ns)
        {
            var message = new byte[1024];
            try
            {
                int received = ns.Read(message, 0, 1024);
                return message.Take(received).ToArray();
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex.Message);
                return null;
            }
        }

        /// <summary>
        /// socket读取数据
        /// </summary>
        /// <returns>读取到的数据</returns>
        public byte[] Receive()
        {
            var message = new byte[1024];
            try
            {
                if (this.type == "socket")
                {
                    int received = this.socket.Receive(message, 0, 1024, SocketFlags.None);
                    return message.Take(received).ToArray();
                }

                return Read(this._ns);
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex.Message);
                return null;
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
                if (this.type != "socket")
                    return Write(this._ns, message);

                this.socket.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                Error?.Invoke(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// socket发送数据
        /// </summary>
        /// <param name="message">数据</param>
        /// <returns>是否发送成功</returns>
        public bool Send(string message) => Send(Encoding.ASCII.GetBytes(message));

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
                Error?.Invoke(this, ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="ns">networkstream对象</param>
        /// <param name="message">数据</param>
        /// <returns>是否发送成功</returns>
        public bool Write(NetworkStream ns, string message) => Write(ns, Encoding.ASCII.GetBytes(message));

        private void DataReceive()
        {
            while (!this.closing)
            {
                byte[] message = Receive();
                if (message.Length > 0)
                    DataReceived?.Invoke(this, Encoding.ASCII.GetString(message));
            }
        }

        private void NewConnectIn()
        {
            while (!this.closing)
                ConnectionAdded?.BeginInvoke(this, this.listener.AcceptTcpClient(), null, null);
        }
        #endregion
    }
}
