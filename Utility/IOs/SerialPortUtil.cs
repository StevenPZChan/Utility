using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Utility.IOs
{
    /// <summary>
    /// CRC模式
    /// </summary>
    public enum CRCMode
    {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
        CCITT,
        CCITT_FALSE,
        XMODEM,
        X25,
        MODBUS,
        IBM,
        MAXIM,
        USB,
        DNP
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
    }

    /// <summary>
    /// 串口开发辅助类
    /// </summary>
    [ToolboxBitmap(typeof(SerialPort))]
    public class SerialPortUtil : Component
    {
        #region SerialPortBaudRates Enum
        /// <summary>
        /// 串口波特率列表。 75,110,150,300,600,1200,2400,4800,9600,14400,19200,28800,38400,56000,57600, 115200,128000,230400,256000
        /// </summary>
        public enum SerialPortBaudRates
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            BaudRate_75 = 75,
            BaudRate_110 = 110,
            BaudRate_150 = 150,
            BaudRate_300 = 300,
            BaudRate_600 = 600,
            BaudRate_1200 = 1200,
            BaudRate_2400 = 2400,
            BaudRate_4800 = 4800,
            BaudRate_9600 = 9600,
            BaudRate_14400 = 14400,
            BaudRate_19200 = 19200,
            BaudRate_28800 = 28800,
            BaudRate_38400 = 38400,
            BaudRate_56000 = 56000,
            BaudRate_57600 = 57600,
            BaudRate_115200 = 115200,
            BaudRate_128000 = 128000,
            BaudRate_230400 = 230400,
            BaudRate_256000 = 256000
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        }
        #endregion

        #region SerialPortDatabits Enum
        /// <summary>
        /// 串口数据位列表（5,6,7,8）
        /// </summary>
        public enum SerialPortDatabits
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            FiveBits = 5,
            SixBits = 6,
            SeventBits = 7,
            EightBits = 8
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        }
        #endregion

        #region Fields
        /// <summary>
        /// 结束符比特
        /// </summary>
        [Description("结束符比特")] public byte EndByte = 0xFE;
        private readonly SerialPort comPort;
        //波特率
        private SerialPortBaudRates _baudRate;
        //数据位
        private SerialPortDatabits _dataBits;
        //校验位
        private Parity _parity;
        //串口号，默认COM1
        private string _portName;
        /// <summary>
        /// 接收事件是否有效 false表示有效
        /// </summary>
        private bool _receiveEventFlag;
        //停止位
        private StopBits _stopBits;
        #endregion

        #region Constructors
        /// <summary>
        /// 参数构造函数（使用枚举参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        public SerialPortUtil(string name, SerialPortBaudRates baud, Parity par, SerialPortDatabits dBits, StopBits sBits)
        {
            this._portName = name;
            this._baudRate = baud;
            this._parity = par;
            this._dataBits = dBits;
            this._stopBits = sBits;

            this.comPort = new SerialPort(this._portName, (int)this._baudRate, this._parity, (int)this._dataBits, this._stopBits)
                {WriteTimeout = 3000, ReadTimeout = 3000, ReceivedBytesThreshold = 1};

            this.comPort.DataReceived += comPort_DataReceived;
            this.comPort.ErrorReceived += comPort_ErrorReceived;
        }

        /// <summary>
        /// 参数构造函数（使用字符串参数构造）
        /// </summary>
        /// <param name="baud">波特率</param>
        /// <param name="par">奇偶校验位</param>
        /// <param name="sBits">停止位</param>
        /// <param name="dBits">数据位</param>
        /// <param name="name">串口号</param>
        public SerialPortUtil(string name, int baud, string par, int dBits, int sBits)
            : this(name, (SerialPortBaudRates)Enum.Parse(typeof(SerialPortBaudRates), baud.ToString()), (Parity)Enum.Parse(typeof(Parity), par, true),
                (SerialPortDatabits)Enum.Parse(typeof(SerialPortDatabits), dBits.ToString()), (StopBits)Enum.Parse(typeof(StopBits), sBits.ToString())) { }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerialPortUtil()
            : this("COM1", SerialPortBaudRates.BaudRate_9600, Parity.None, SerialPortDatabits.EightBits, StopBits.One) { }
        #endregion

        #region Properties
        //string End = "#";
        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return this._portName; }
            set
            {
                this._portName = value;
                this.comPort.PortName = value;
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public SerialPortBaudRates BaudRate
        {
            get { return this._baudRate; }
            set
            {
                this._baudRate = value;
                this.comPort.BaudRate = (int)value;
            }
        }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity
        {
            get { return this._parity; }
            set
            {
                this._parity = value;
                this.comPort.Parity = value;
            }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public SerialPortDatabits DataBits
        {
            get { return this._dataBits; }
            set
            {
                this._dataBits = value;
                this.comPort.DataBits = (int)value;
            }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return this._stopBits; }
            set
            {
                this._stopBits = value;
                this.comPort.StopBits = value;
            }
        }

        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen => this.comPort.IsOpen;
        #endregion

        #region Events
        /// <summary>
        /// 数据接收事件
        /// </summary>
        [Description("数据接收事件")]
        public event EventHandler<DataReceivedEventArgs> DataReceived;
        /// <summary>
        /// 错误处理事件
        /// </summary>
        [Description("错误处理事件")]
        public event SerialErrorReceivedEventHandler Error;
        #endregion

        #region Methods
        /// <summary>
        /// 转换字节数组到十六进制字符串
        /// </summary>
        /// <param name="comByte">待转换字节数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ByteToHex(byte[] comByte)
        {
            var builder = new StringBuilder(comByte.Length * 3);
            foreach (byte data in comByte)
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));

            return builder.ToString().ToUpper();
        }

        /// <summary>
        /// 16位CRC检验码生成器
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="mode">CRC模式</param>
        /// <returns>CRC检验码，高位在前</returns>
        public static byte[] CRC16(byte[] data, CRCMode mode)
        {
            ushort crcpoly, crcinit = 0x0000, crcxorout = 0x0000;
            bool refin = true, refout = true;
            switch (mode)
            {
                case CRCMode.CCITT:
                    crcpoly = 0x1021;
                    break;
                case CRCMode.CCITT_FALSE:
                    crcpoly = 0x1021;
                    crcinit = 0xFFFF;
                    refin = false;
                    refout = false;
                    break;
                case CRCMode.DNP:
                    crcpoly = 0x3D65;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMode.IBM:
                    crcpoly = 0x8005;
                    break;
                case CRCMode.MAXIM:
                    crcpoly = 0x8005;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMode.MODBUS:
                    crcpoly = 0x8005;
                    crcinit = 0xFFFF;
                    break;
                case CRCMode.USB:
                    crcpoly = 0x8005;
                    crcinit = 0xFFFF;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMode.X25:
                    crcpoly = 0x1021;
                    crcinit = 0xFFFF;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMode.XMODEM:
                    crcpoly = 0x1021;
                    refin = false;
                    refout = false;
                    break;
                default:
                    crcpoly = 0x8005;
                    crcinit = 0xFFFF;
                    break;
            }

            foreach (byte t in data)
            {
                byte temp = t;
                if (refin)
                {
                    char[] arr = Convert.ToString(temp, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(arr);
                    temp = Convert.ToByte(new string(arr), 2);
                }

                crcinit ^= (ushort)(temp << 8);
                for (var j = 0; j < 8; j++)
                    crcinit = (ushort)((crcinit & 0x8000) > 0 ? (crcinit << 1) ^ crcpoly : crcinit << 1);
            }

            if (refout)
            {
                char[] arr = Convert.ToString(crcinit, 2).PadLeft(16, '0').ToCharArray();
                Array.Reverse(arr);
                crcinit = Convert.ToUInt16(new string(arr), 2);
            }

            crcinit ^= crcxorout;
            var hi = (byte)((crcinit & 0xFF00) >> 8);
            var lo = (byte)(crcinit & 0x00FF);
            return new[] {hi, lo};
        }

        /// <summary>
        /// 检查端口名称是否存在
        /// </summary>
        /// <param name="portName"></param>
        /// <returns></returns>
        public static bool Exists(string portName) => SerialPort.GetPortNames().Any(port => port == portName);

        /// <summary>
        /// 格式化端口相关属性
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string Format(SerialPort port) => $"{port.PortName} ({port.BaudRate},{port.DataBits},{port.StopBits},{port.Parity},{port.Handshake}";

        /// <summary>
        /// 封装获取串口号列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames() => SerialPort.GetPortNames();

        /// <summary>
        /// 转换十六进制字符串到字节数组
        /// </summary>
        /// <param name="msg">待转换字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", ""); //移除空格

            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            var comBuffer = new byte[msg.Length / 2];
            for (var i = 0; i < msg.Length; i += 2)
                //convert each set of 2 characters to a byte and add to the array
                comBuffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 16);

            return comBuffer;
        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        public static void SetBauRateValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (SerialPortBaudRates rate in Enum.GetValues(typeof(SerialPortBaudRates)))
                obj.Items.Add(((int)rate).ToString());
        }

        /// <summary>
        /// 设置数据位
        /// </summary>
        public static void SetDataBitsValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (SerialPortDatabits databit in Enum.GetValues(typeof(SerialPortDatabits)))
                obj.Items.Add(((int)databit).ToString());
        }

        /// <summary>
        /// 设置校验位列表
        /// </summary>
        public static void SetParityValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(Parity)))
                obj.Items.Add(str);
            //foreach (Parity party in Enum.GetValues(typeof(Parity)))
            //{
            //    obj.Items.Add(((int)party).ToString());
            //}
        }

        /// <summary>
        /// 设置串口号
        /// </summary>
        /// <param name="obj"></param>
        public static void SetPortNameValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
                obj.Items.Add(str);
        }

        /// <summary>
        /// 设置停止位
        /// </summary>
        public static void SetStopBitValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(StopBits)))
                obj.Items.Add(str);
            //foreach (StopBits stopbit in Enum.GetValues(typeof(StopBits)))
            //{
            //    obj.Items.Add(((int)stopbit).ToString());
            //}
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            if (this.IsOpen)
                this.comPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            this.comPort.DiscardInBuffer();
            this.comPort.DiscardOutBuffer();
        }

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void OpenPort()
        {
            if (this.IsOpen)
                this.comPort.Close();

            this.comPort.Open();
        }

        /// <summary>
        /// 发送串口命令
        /// </summary>
        /// <param name="sendData">发送数据</param>
        /// <param name="receiveData">接收数据</param>
        /// <param name="overtime">重复次数</param>
        /// <returns></returns>
        public int SendCommand(byte[] sendData, ref byte[] receiveData, int overtime)
        {
            if (!this.IsOpen)
                this.comPort.Open();

            this._receiveEventFlag = true;  //关闭接收事件
            this.comPort.DiscardInBuffer(); //清空接收缓冲区
            this.comPort.Write(sendData, 0, sendData.Length);

            int num = 0, ret = 0;
            while (num++ < overtime)
            {
                if (this.comPort.BytesToRead >= receiveData.Length)
                    break;

                Thread.Sleep(1);
            }

            if (this.comPort.BytesToRead >= receiveData.Length)
                ret = this.comPort.Read(receiveData, 0, receiveData.Length);

            this._receiveEventFlag = false; //打开事件
            return ret;
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg"></param>
        public void WriteData(string msg) => WriteData(Encoding.Default.GetBytes(msg));

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg">写入端口的字节数组</param>
        public void WriteData(byte[] msg) => WriteData(msg, 0, msg.Length);

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg">包含要写入端口的字节数组</param>
        /// <param name="offset">参数从0字节开始的字节偏移量</param>
        /// <param name="count">要写入的字节数</param>
        public void WriteData(byte[] msg, int offset, int count)
        {
            if (!this.IsOpen)
                this.comPort.Open();

            this.comPort.Write(msg, offset, count);
        }

        /// <summary>
        /// 数据接收处理
        /// </summary>
        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (this._receiveEventFlag)
                return;

            #region 根据结束字节来判断是否全部获取完成
            var byteData = new List<byte>();
            //bool found = false;//是否检测到结束符号
            while (this.comPort.BytesToRead > 0)
            {
                var readBuffer = new byte[this.comPort.ReadBufferSize + 1];
                int count = this.comPort.Read(readBuffer, 0, this.comPort.ReadBufferSize);
                for (var i = 0; i < count; i++)
                    byteData.Add(readBuffer[i]);

                //if (readBuffer[i] == EndByte)
                //{
                //    found = true;
                //}
            }
            #endregion 根据结束字节来判断是否全部获取完成

            //字符转换
            string readString = Encoding.Default.GetString(byteData.ToArray(), 0, byteData.Count);

            //触发整条记录的处理
            DataReceived?.Invoke(sender, new DataReceivedEventArgs(readString));
        }

        /// <summary>
        /// 错误处理函数
        /// </summary>
        private void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) => Error?.Invoke(sender, e);
        #endregion
    }

    /// <summary>
    /// 接收数据处理事件
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        #region Fields
        /// <summary>
        /// 接收的数据
        /// </summary>
        public string DataReceived;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mDataReceived">接收的数据</param>
        public DataReceivedEventArgs(string mDataReceived)
        {
            this.DataReceived = mDataReceived;
        }
        #endregion
    }
}
