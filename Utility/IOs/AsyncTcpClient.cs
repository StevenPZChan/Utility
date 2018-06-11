using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
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
        private TcpClient tcpClient;
        private NetworkStream stream;
        private bool disposed = false;
        private int retries = 0;
        #endregion

        #region Ctors
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public AsyncTcpClient()
            : this("127.0.0.1", 0) { }
        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEP">远端服务器终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEP)
          : this(new[] { remoteEP.Address }, remoteEP.Port)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteEP">远端服务器终结点</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPEndPoint remoteEP, IPEndPoint localEP)
          : this(new[] { remoteEP.Address }, remoteEP.Port, localEP)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddress">远端服务器IP地址</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress remoteIPAddress, int remotePort)
          : this(new[] { remoteIPAddress }, remotePort)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddress">远端服务器IP地址</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress remoteIPAddress, int remotePort, IPEndPoint localEP)
          : this(new[] { remoteIPAddress }, remotePort, localEP)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(string remoteHostName, int remotePort)
          : this(Dns.GetHostAddresses(remoteHostName), remotePort)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteHostName">远端服务器主机名</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(string remoteHostName, int remotePort, IPEndPoint localEP)
          : this(Dns.GetHostAddresses(remoteHostName), remotePort, localEP)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddresses">远端服务器IP地址列表</param>
        /// <param name="remotePort">远端服务器端口</param>
        public AsyncTcpClient(IPAddress[] remoteIPAddresses, int remotePort)
          : this(remoteIPAddresses, remotePort, null)
        {
        }

        /// <summary>
        /// 异步TCP客户端
        /// </summary>
        /// <param name="remoteIPAddresses">远端服务器IP地址列表</param>
        /// <param name="remotePort">远端服务器端口</param>
        /// <param name="localEP">本地客户端终结点</param>
        public AsyncTcpClient(IPAddress[] remoteIPAddresses, int remotePort, IPEndPoint localEP)
        {
            this.Addresses = remoteIPAddresses;
            this.Port = remotePort;
            this.LocalIPEndPoint = localEP;
            this.Encoding = Encoding.Default;

            if (this.LocalIPEndPoint != null)
                this.tcpClient = new TcpClient(this.LocalIPEndPoint);
            else
                this.tcpClient = new TcpClient();

            Retries = 3;
            RetryInterval = 5;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 是否已与服务器建立连接
        /// </summary>
        [Category("TcpClient"), Description("是否已与服务器建立连接")]
        public bool Connected { get { return tcpClient.Client.Connected; } }
        /// <summary>
        /// 远端服务器的IP地址列表
        /// </summary>
        [Category("TcpClient"), Description("远端服务器的IP地址列表")]
        public IPAddress[] Addresses { get; set; }
        /// <summary>
        /// 远端服务器的端口
        /// </summary>
        [Category("TcpClient"), Description("远端服务器的端口")]
        public int Port { get; set; }
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
        /// 远端服务器终结点
        /// </summary>
        [Category("TcpClient"), Description("远端服务器终结点")]
        public IPEndPoint RemoteIPEndPoint
        {
            get { return new IPEndPoint(Addresses[0], Port); }
        }
        /// <summary>
        /// 本地客户端终结点
        /// </summary>
        protected IPEndPoint LocalIPEndPoint { get; private set; }
        /// <summary>
        /// 通信所使用的编码
        /// </summary>
        [Category("TcpClient"), Description("通信所使用的编码"), TypeConverter(typeof(EncodingTypeConverter))]
        public Encoding Encoding { get; set; }
        #endregion

        #region Connect
        /// <summary>
        /// 连接到服务器
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public AsyncTcpClient Connect()
        {
            if (!Connected)
                // start the async connect operation
                tcpClient.BeginConnect(Addresses, Port, HandleTcpServerConnected, tcpClient);

            return this;
        }

        /// <summary>
        /// 关闭与服务器的连接
        /// </summary>
        /// <returns>异步TCP客户端</returns>
        public AsyncTcpClient Close()
        {
            if (Connected)
            {
                retries = 0;
                tcpClient.Client.Shutdown(SocketShutdown.Both);
                tcpClient.Close();
                RaiseServerDisconnected(Addresses, Port);
            }

            return this;
        }
        #endregion

        #region Receive
        private void HandleTcpServerConnected(IAsyncResult ar)
        {
            try
            {
                tcpClient.EndConnect(ar);
                RaiseServerConnected(Addresses, Port);
                retries = 0;
            }
            catch (Exception ex)
            {
                //ExceptionHandler.Handle(ex);
                if (retries > 0)
                {
                    //Logger.Debug(string.Format(CultureInfo.InvariantCulture,
                    //  "Connect to server with retry {0} failed.", retries));
                }

                retries++;
                if (retries > Retries)
                {
                    // we have failed to connect to all the IP Addresses, 
                    // connection has failed overall.
                    RaiseServerExceptionOccurred(Addresses, Port, ex);
                    return;
                }
                else
                {
                    //Logger.Debug(string.Format(CultureInfo.InvariantCulture,
                    //  "Waiting {0} seconds before retrying to connect to server.",
                    //  RetryInterval));
                    Thread.Sleep(TimeSpan.FromSeconds(RetryInterval));
                    Connect();
                    return;
                }
            }

            // we are connected successfully and start asyn read operation.
            stream = tcpClient.GetStream();
            byte[] buffer = new byte[tcpClient.ReceiveBufferSize];
            stream.BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        }

        private void HandleDatagramReceived(IAsyncResult ar)
        {
            if (!Connected || stream == null)
            {
                Close(); // receive buffer callback
                return;
            }

            int numberOfReadBytes = 0;
            try
            {
                numberOfReadBytes = stream.EndRead(ar);
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
            byte[] buffer = (byte[])ar.AsyncState;
            byte[] receivedBytes = new byte[numberOfReadBytes];
            Buffer.BlockCopy(buffer, 0, receivedBytes, 0, numberOfReadBytes);
            RaiseDataReceived(tcpClient, receivedBytes);

            // then start reading from the network again
            stream.BeginRead(buffer, 0, buffer.Length, HandleDatagramReceived, buffer);
        }
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

        private void RaiseDataReceived(TcpClient sender, byte[] datagram)
        {
            DataReceived?.Invoke(this, new TcpDatagramReceivedEventArgs(sender, datagram, Encoding));
        }

        private void RaiseServerConnected(IPAddress[] ipAddresses, int port)
        {
            ServerConnected?.Invoke(this, new TcpServerConnectedEventArgs(ipAddresses, port));
        }

        private void RaiseServerDisconnected(IPAddress[] ipAddresses, int port)
        {
            ServerDisconnected?.Invoke(this, new TcpServerDisconnectedEventArgs(ipAddresses, port));
        }

        private void RaiseServerExceptionOccurred(IPAddress[] ipAddresses, int port, Exception innerException)
        {
            ServerExceptionOccurred?.Invoke(this, new TcpServerExceptionOccurredEventArgs(ipAddresses, port, innerException));
        }
        #endregion

        #region Send
        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(byte[] datagram)
        {
            if (datagram == null)
                throw new ArgumentNullException("datagram");

            if (!Connected)
            {
                RaiseServerDisconnected(Addresses, Port);
                throw new InvalidProgramException("This client has not connected to server.");
            }

            stream.BeginWrite(datagram, 0, datagram.Length, HandleDatagramWritten, tcpClient);
        }

        private void HandleDatagramWritten(IAsyncResult ar)
        {
            ((TcpClient)ar.AsyncState).GetStream().EndWrite(ar);
        }

        /// <summary>
        /// 发送报文
        /// </summary>
        /// <param name="datagram">报文</param>
        public void Send(string datagram)
        {
            Send(this.Encoding.GetBytes(datagram));
        }
        #endregion

        #region IDisposable Members
        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public new void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed 
        /// and unmanaged resources; <c>false</c> 
        /// to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    try
                    {
                        Close();

                        if (tcpClient != null)
                            tcpClient = null;
                    }
                    catch (SocketException ex)
                    {
                        System.Diagnostics.Trace.WriteLine(ex.Message);
                    }
                }

                disposed = true;
            }
        }
        #endregion
    }

    /// <summary>
    /// Internal class to join the TCP client and buffer together
    /// for easy management in the server
    /// </summary>
    internal class TcpClientState
    {
        /// <summary>
        /// Constructor for a new Client
        /// </summary>
        /// <param name="tcpClient">The TCP client</param>
        /// <param name="buffer">The byte array buffer</param>
        public TcpClientState(TcpClient tcpClient, byte[] buffer)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");
            if (buffer == null)
                throw new ArgumentNullException("buffer");

            this.TcpClient = tcpClient;
            this.Buffer = buffer;
        }

        /// <summary>
        /// Gets the TCP Client
        /// </summary>
        public TcpClient TcpClient { get; private set; }

        /// <summary>
        /// Gets the Buffer.
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// Gets the network stream
        /// </summary>
        public NetworkStream NetworkStream
        {
            get { return TcpClient.GetStream(); }
        }
    }

    /// <summary>
    /// 与客户端的连接已建立事件参数
    /// </summary>
    public class TcpClientConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 与客户端的连接已建立事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        public TcpClientConnectedEventArgs(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");

            this.TcpClient = tcpClient;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; private set; }
    }

    /// <summary>
    /// 与客户端的连接已断开事件参数
    /// </summary>
    public class TcpClientDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 与客户端的连接已断开事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        public TcpClientDisconnectedEventArgs(TcpClient tcpClient)
        {
            if (tcpClient == null)
                throw new ArgumentNullException("tcpClient");

            this.TcpClient = tcpClient;
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; private set; }
    }

    /// <summary>
    /// 与服务器的连接发生异常事件参数
    /// </summary>
    public class TcpServerExceptionOccurredEventArgs : EventArgs
    {
        /// <summary>
        /// 与服务器的连接发生异常事件参数
        /// </summary>
        /// <param name="ipAddresses">服务器IP地址列表</param>
        /// <param name="port">服务器端口</param>
        /// <param name="innerException">内部异常</param>
        public TcpServerExceptionOccurredEventArgs(
          IPAddress[] ipAddresses, int port, Exception innerException)
        {
            if (ipAddresses == null)
                throw new ArgumentNullException("ipAddresses");

            this.Addresses = ipAddresses;
            this.Port = port;
            this.Exception = innerException;
        }

        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; private set; }
        /// <summary>
        /// 内部异常
        /// </summary>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = string.Empty;
            foreach (var item in Addresses)
            {
                s = s + item.ToString() + ',';
            }
            s = s.TrimEnd(',');
            s = s + ":" + Port.ToString(CultureInfo.InvariantCulture);

            return s;
        }
    }

    /// <summary>
    /// 接收到数据报文事件参数
    /// </summary>
    public class TcpDatagramReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 接收到数据报文事件参数
        /// </summary>
        /// <param name="tcpClient">客户端</param>
        /// <param name="datagram">报文</param>
        /// <param name="encoding">数据编码</param>
        public TcpDatagramReceivedEventArgs(TcpClient tcpClient, byte[] datagram, Encoding encoding)
        {
            TcpClient = tcpClient;
            Datagram = datagram;
            PlainText = encoding.GetString(datagram);
        }

        /// <summary>
        /// 客户端
        /// </summary>
        public TcpClient TcpClient { get; private set; }
        /// <summary>
        /// 报文
        /// </summary>
        public byte[] Datagram { get; private set; }
        /// <summary>
        /// 明文报文
        /// </summary>
        public string PlainText { get; private set; }
    }

    /// <summary>
    /// 与服务器的连接已建立事件参数
    /// </summary>
    public class TcpServerConnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 与服务器的连接已建立事件参数
        /// </summary>
        /// <param name="ipAddresses">服务器IP地址列表</param>
        /// <param name="port">服务器端口</param>
        public TcpServerConnectedEventArgs(IPAddress[] ipAddresses, int port)
        {
            if (ipAddresses == null)
                throw new ArgumentNullException("ipAddresses");

            this.Addresses = ipAddresses;
            this.Port = port;
        }

        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = string.Empty;
            foreach (var item in Addresses)
            {
                s = s + item.ToString() + ',';
            }
            s = s.TrimEnd(',');
            s = s + ":" + Port.ToString(CultureInfo.InvariantCulture);

            return s;
        }
    }

    /// <summary>
    /// 与服务器的连接已断开事件参数
    /// </summary>
    public class TcpServerDisconnectedEventArgs : EventArgs
    {
        /// <summary>
        /// 与服务器的连接已断开事件参数
        /// </summary>
        /// <param name="ipAddresses">服务器IP地址列表</param>
        /// <param name="port">服务器端口</param>
        public TcpServerDisconnectedEventArgs(IPAddress[] ipAddresses, int port)
        {
            if (ipAddresses == null)
                throw new ArgumentNullException("ipAddresses");

            this.Addresses = ipAddresses;
            this.Port = port;
        }

        /// <summary>
        /// 服务器IP地址列表
        /// </summary>
        public IPAddress[] Addresses { get; private set; }
        /// <summary>
        /// 服务器端口
        /// </summary>
        public int Port { get; private set; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string s = string.Empty;
            foreach (var item in Addresses)
            {
                s = s + item.ToString() + ',';
            }
            s = s.TrimEnd(',');
            s = s + ":" + Port.ToString(CultureInfo.InvariantCulture);

            return s;
        }
    }

    internal class EncodingTypeConverter : TypeConverter
    {
        private List<Encoding> arrValues;

        public EncodingTypeConverter()
        {
            // Initializes the standard values list with defaults.
            arrValues = new List<Encoding>() {
                Encoding.ASCII,
                Encoding.BigEndianUnicode,
                Encoding.Default,
                Encoding.Unicode,
                Encoding.UTF32,
                Encoding.UTF7,
                Encoding.UTF8
            };
        }

        // 是否支持标准值
        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            return true;
        }

        // 获取标准值集合
        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            StandardValuesCollection svc = new StandardValuesCollection(arrValues);
            return svc;
        }

        // 是否可以转换
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
                return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            if (destinationType == typeof(string)) return true;
            if (destinationType == typeof(InstanceDescriptor)) return true;

            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
                return Encoding.GetEncoding((string)value);

            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            //将对象转换为字符串，如："Jonny,Sun,33" 
            if ((destinationType == typeof(string)) && (value is Encoding))
                return ((Encoding)value).BodyName;

            //生成设计时的构造器代码 
            if (destinationType == typeof(InstanceDescriptor) & value is Encoding)
            {
                Encoding p = (Encoding)value;
                MethodInfo method = typeof(Encoding).GetMethod("GetEncoding", new Type[] { typeof(string) });
                return method.Invoke(null, new object[] { p.BodyName });
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
