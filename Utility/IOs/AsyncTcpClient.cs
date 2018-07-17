using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility.IOs
{
    /// <summary>
    /// 异步TCP客户端
    /// </summary>
    [ToolboxBitmap(typeof(WebBrowser))]
    public class AsyncTcpClient : Component, IDisposable
    {
        #region Fields
        private bool disposed;
        private int retries;
        private NetworkStream stream;
        private TcpClient tcpClient;
        #endregion

        #region Constructors
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AsyncTcpClient()
            : this("127.0.0.1", 0) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEp">远端服务器终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEp)
            : this(new[] {remoteEp.Address}, remoteEp.Port) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEp">远端服务器终结点</param>
        /// <param name="localEp">本地客户端终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEp, IPEndPoint localEp)
            : this(new[] {remoteEp.Address}, remoteEp.Port, localEp) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIpAddress">远端服务器IP地址</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress remoteIpAddress, int remotePort)
            : this(new[] {remoteIpAddress}, remotePort) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIpAddress">远端服务器IP地址</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEp">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress remoteIpAddress, int remotePort, IPEndPoint localEp)
            : this(new[] {remoteIpAddress}, remotePort, localEp) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(string remoteHostName, int remotePort)
            : this(Dns.GetHostAddresses(remoteHostName), remotePort) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEp">本地客户端终结点</param>
        public AsyncTcpClient(string remoteHostName, int remotePort, IPEndPoint localEp)
            : this(Dns.GetHostAddresses(remoteHostName), remotePort, localEp) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIpAddresses">远端服务器IP地址列表</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress[] remoteIpAddresses, int remotePort)
            : this(remoteIpAddresses, remotePort, null) { }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIpAddresses">远端服务器IP地址列表</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEp">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress[] remoteIpAddresses, int remotePort, IPEndPoint localEp)
        {
            this.Addresses = remoteIpAddresses;
            this.Port = remotePort;
            this.LocalIpEndPoint = localEp;
            this.Encoding = Encoding.Default;

            this.tcpClient = this.LocalIpEndPoint != null ? new TcpClient(this.LocalIpEndPoint) : new TcpClient();

            this.Retries = 3;
            this.RetryInterval = 5;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 远端服务器的IP地址列表
        /// </summary>
        [Category("TcpClient"), Description("远端服务器的IP地址列表")]
        public IPAddress[] Addresses { get; set; }
        /// <summary>
        /// 通信所使用的编码
        /// </summary>
        [Category("TcpClient"), Description("通信所使用的编码"), TypeConverter(typeof(EncodingTypeConverter))]
        public Encoding Encoding { get; set; }
        /// <summary>
        /// 远端服务器的端口
        /// </summary>
        [Category("TcpClient"), Description("远端服务器的端口")]
        public int Port { get; set; }
        /// <summary>
        /// 远端服务器终结点
        /// </summary>
        [Category("TcpClient"), Description("远端服务器终结点")]
        public IPEndPoint RemoteIpEndPoint => new IPEndPoint(this.Addresses[0], this.Port);
        /// <summary>
        /// 连接重试次数
        /// </summary>
        [Category("TcpClient"), DefaultValue(3), Description("连接重试次数")]
        public int Retries { get; set; }
        /// <summary>
        /// 连接重试间隔
        /// </summary>
        [Category("TcpClient"), DefaultValue(5), Description("连接重试间隔")]
        public int RetryInterval { get; set; }
        /// <summary>
        /// 是否已与服务器建立连接
        /// </summary>
        [Category("TcpClient"), Description("是否已与服务器建立连接")]
        public bool Connected => this.tcpClient.Client.Connected;
        /// <summary>
        /// 本地客户端终结点
        /// </summary>
        protected IPEndPoint LocalIpEndPoint { get; }
        #endregion

        #region Events
        /// <summary>
        /// 接收到数据报文事件
        /// </summary>
        public event EventHandler<TcpDatagramReceivedEventArgs> DataReceived;
        /// <summary>
        /// 与服务器的连接已建立事件
        /// </summary>
        public event EventHandler<TcpServerConnectedEventArgs> ServerConnected;
        /// <summary>
        /// 与服务器的连接已断开事件
        /// </summary>
        public event EventHandler<TcpServerDisconnectedEventArgs> ServerDisconnected;
        /// <summary>
        /// 与服务器的连接发生异常事件
        /// </summary>
        public event EventHandler<TcpServerExceptionOccurredEventArgs> ServerExceptionOccurred;
        #endregion

        #region Methods
        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public AsyncTcpClient Close()
        {
            if (!this.Connected)
                return this;

            this.retries = 0;
            this.tcpClient.Client.Shutdown(SocketShutdown.Both);
            this.tcpClient.Close();
            RaiseServerDisconnected(this.Addresses, this.Port);

            return this;
        }

        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public AsyncTcpClient Connect()
        {
            if (!this.Connected)
                // start the async connect operation
                this.tcpClient.BeginConnect(this.Addresses, this.Port, HandleTcpServerConnected, this.tcpClient);

            return this;
        }

        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(byte[] datagram)
        {
            if (datagram == null)
                throw new ArgumentNullException(nameof(datagram));

            if (!this.Connected)
            {
                RaiseServerDisconnected(this.Addresses, this.Port);
                throw new InvalidProgramException("This client has not connected to server.");
            }

            this.stream.BeginWrite(datagram, 0, datagram.Length, HandleDatagramWritten, this.tcpClient);
        }

        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(string datagram) => Send(this.Encoding.GetBytes(datagram));

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (this.disposed)
                return;

            if (disposing)
                try
                {
                    Close();
                    this.tcpClient = null;
                }
                catch (SocketException ex)
                {
                    Trace.WriteLine(ex.Message);
                }

            this.disposed = true;
        }

        private static void HandleDatagramWritten(IAsyncResult ar) => ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);

        private void HandleDatagramReceived(IAsyncResult ar)
        {
            if (!this.Connected || this.stream == null)
            {
                Close(); // receive buffer callback
                return;
            }

            int numberOfReadBytes;
            try
            {
                numberOfReadBytes = this.stream.EndRead(ar);
            }
            catch
            {
                numberOfReadBytes = 0;
            }

            if (numberOfReadBytes == 0)
            {
                // connection has been closed
                Close();
                return;
            }

            // received byte and trigger event notification
            var buffer = (byte[])ar.AsyncState;
            var receivedBytes = new byte[numberOfReadBytes];
            Buffer.BlockCopy(buffer, 0, receivedBytes, 0, numberOfReadBytes);
            RaiseDataReceived(this.tcpClient, receivedBytes);

            // then start reading from the network again
            this.stream.BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        }

        private void HandleTcpServerConnected(IAsyncResult ar)
        {
            try
            {
                this.tcpClient.EndConnect(ar);
                RaiseServerConnected(this.Addresses, this.Port);
                this.retries = 0;
            }
            catch (Exception ex)
            {
                //ExceptionHandler.Handle(ex);
                if (this.retries > 0)
                {
                    //Logger.Debug(string.Format(CultureInfo.InvariantCulture,
                    //  "Connect to server with retry {0} failed.", retries));
                }

                this.retries++;
                if (this.retries > this.Retries)
                {
                    // we have failed to connect to all the IP Addresses, connection has failed overall.
                    RaiseServerExceptionOccurred(this.Addresses, this.Port, ex);
                    return;
                }

                //Logger.Debug(string.Format(CultureInfo.InvariantCulture,
                //  "Waiting {0} seconds before retrying to connect to server.",
                //  RetryInterval));
                Task.Delay(TimeSpan.FromSeconds(this.RetryInterval)).Wait();
                Connect();
                return;
            }

            // we are connected successfully and start asyn read operation.
            this.stream = this.tcpClient.GetStream();
            var buffer = new byte[this.tcpClient.ReceiveBufferSize];
            this.stream.BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        }

        private void RaiseDataReceived(TcpClient sender, byte[] datagram) =>
            DataReceived?.Invoke(this, new TcpDatagramReceivedEventArgs(sender, datagram, this.Encoding));

        private void RaiseServerConnected(IPAddress[] ipAddresses, int port) =>
            ServerConnected?.Invoke(this, new TcpServerConnectedEventArgs(ipAddresses, port));

        private void RaiseServerDisconnected(IPAddress[] ipAddresses, int port) =>
            ServerDisconnected?.Invoke(this, new TcpServerDisconnectedEventArgs(ipAddresses, port));

        private void RaiseServerExceptionOccurred(IPAddress[] ipAddresses, int port, Exception innerException) =>
            ServerExceptionOccurred?.Invoke(this, new TcpServerExceptionOccurredEventArgs(ipAddresses, port, innerException));

        #region IDisposable Members
        /// <inheritdoc />
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// 与客户端的连接已建立事件参数
    /// </summary>
    public class TcpClientConnectedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// 与客户端的连接已建立事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        public TcpClientConnectedEventArgs(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException(nameof(tcpClient));

            this.TcpClient = tcpClient;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; }
        #endregion
    }

    /// <summary>
    /// 与客户端的连接已断开事件参数
    /// </summary>
    public class TcpClientDisconnectedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// 与客户端的连接已断开事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        public TcpClientDisconnectedEventArgs(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException(nameof(tcpClient));

            this.TcpClient = tcpClient;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; }
        #endregion
    }

    /// <summary>
    /// 接收到数据报文事件参数
    /// </summary>
    public class TcpDatagramReceivedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// 接收到数据报文事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        /// <param name="datagram">报文</param>
        /// <param name="encoding">数据编码</param>
        public TcpDatagramReceivedEventArgs(TcpClient tcpClient, byte[] datagram, Encoding encoding)
        {
            this.TcpClient = tcpClient;
            this.Datagram = datagram;
            this.PlainText = encoding.GetString(datagram);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 报文
        /// </summary>
        public byte[] Datagram { get; }
        /// <summary>
        /// 明文报文
        /// </summary>
        public string PlainText { get; }
        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; }
        #endregion
    }

    /// <summary>
    /// 与服务器的连接已建立事件参数
    /// </summary>
    public class TcpServerConnectedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// 与服务器的连接已建立事件参数
        /// </summary>
        /// <param name="ipAddresses">服务器IP地址列表</param>
        /// <param name="port">服务器端口</param>
        public TcpServerConnectedEventArgs(IPAddress[] ipAddresses, int port)
        {
            if (ipAddresses == null)
                throw new ArgumentNullException(nameof(ipAddresses));

            this.Addresses = ipAddresses;
            this.Port = port;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            string s = string.Join(",", this.Addresses.ToList());
            s = s + ":" + this.Port.ToString(CultureInfo.InvariantCulture);

            return s;
        }
        #endregion
    }

    /// <summary>
    /// 与服务器的连接已断开事件参数
    /// </summary>
    public class TcpServerDisconnectedEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// 与服务器的连接已断开事件参数
        /// </summary>
        /// <param name="ipAddresses">服务器IP地址列表</param>
        /// <param name="port">服务器端口</param>
        public TcpServerDisconnectedEventArgs(IPAddress[] ipAddresses, int port)
        {
            if (ipAddresses == null)
                throw new ArgumentNullException(nameof(ipAddresses));

            this.Addresses = ipAddresses;
            this.Port = port;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            string s = string.Join(",", this.Addresses.ToList());
            s = s + ":" + this.Port.ToString(CultureInfo.InvariantCulture);

            return s;
        }
        #endregion
    }

    /// <summary>
    /// 与服务器的连接发生异常事件参数
    /// </summary>
    public class TcpServerExceptionOccurredEventArgs : EventArgs
    {
        #region Constructors
        /// <summary>
        /// 与服务器的连接发生异常事件参数
        /// </summary>
        /// <param name="ipAddresses">服务器IP地址列表</param>
        /// <param name="port">服务器端口</param>
        /// <param name="innerException">内部异常</param>
        public TcpServerExceptionOccurredEventArgs(IPAddress[] ipAddresses, int port, Exception innerException)
        {
            if (ipAddresses == null)
                throw new ArgumentNullException(nameof(ipAddresses));

            this.Addresses = ipAddresses;
            this.Port = port;
            this.Exception = innerException;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; }
        /// <summary>
        /// 内部异常
        /// </summary>
        public Exception Exception { get; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; }
        #endregion

        #region Methods
        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            string s = string.Join(",", this.Addresses.ToList());
            s = s + ":" + this.Port.ToString(CultureInfo.InvariantCulture);

            return s;
        }
        #endregion
    }

    internal class EncodingTypeConverter : TypeConverter
    {
        #region Fields
        private readonly List<Encoding> arrValues;
        #endregion

        #region Constructors
        public EncodingTypeConverter()
        {
            // Initializes the standard values list with defaults.
            this.arrValues = new List<Encoding>
                {Encoding.ASCII, Encoding.BigEndianUnicode, Encoding.Default, Encoding.Unicode, Encoding.UTF32, Encoding.UTF7, Encoding.UTF8};
        }
        #endregion

        #region Methods
        // 是否可以转换
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) =>
            sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string))
                return true;

            return destinationType == typeof(InstanceDescriptor) || base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var s = value as string;
            return s != null ? Encoding.GetEncoding(s) : base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //将对象转换为字符串，如："Jonny,Sun,33"
            if (destinationType == typeof(string) && value is Encoding)
                return ((Encoding)value).BodyName;

            //生成设计时的构造器代码
            if (!((destinationType == typeof(InstanceDescriptor)) & value is Encoding))
                return base.ConvertTo(context, culture, value, destinationType);

            var p = (Encoding)value;
            MethodInfo method = typeof(Encoding).GetMethod("GetEncoding", new[] {typeof(string)});
            return method?.Invoke(null, new object[] {p.BodyName});
        }

        // 获取标准值集合
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            var svc = new StandardValuesCollection(this.arrValues);
            return svc;
        }

        // 是否支持标准值
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;
        #endregion
    }

    /// <summary>
    /// Internal class to join the TCP client and buffer together for easy management in the server
    /// </summary>
    internal class TcpClientState
    {
        #region Constructors
        /// <summary>
        /// Constructor for a new Client
        /// </summary>
        /// <param name="tcpClient">The TCP client</param>
        /// <param name="buffer">The byte array buffer</param>
        public TcpClientState(TcpClient tcpClient, byte[] buffer)
        {
            if (tcpClient == null)
                throw new ArgumentNullException(nameof(tcpClient));
            if (buffer == null)
                throw new ArgumentNullException(nameof(buffer));

            this.TcpClient = tcpClient;
            this.Buffer = buffer;
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets the Buffer.
        /// </summary>
        public byte[] Buffer { get; }
        /// <summary>
        /// Gets the network stream
        /// </summary>
        public NetworkStream NetworkStream => this.TcpClient.GetStream();
        /// <summary>
        /// Gets the TCP Client
        /// </summary>
        public TcpClient TcpClient { get; }
        #endregion
    }
}
