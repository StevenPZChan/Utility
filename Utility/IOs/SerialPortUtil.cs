using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace Utility.IOs
{
    /// <summary>
    /// 串口开发辅助类
    /// </summary>
    public class SerialPortUtil : Component
    {
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

        /// <summary>
        /// 结束符比特
        /// </summary>
        [Description("结束符比特")]
        public byte EndByte = 0xFE;//string End = "#";

        #region 变量属性
        private string _portName = "COM1";//串口号，默认COM1
        private SerialPortBaudRates _baudRate = SerialPortBaudRates.BaudRate_57600;//波特率
        private Parity _parity = Parity.None;//校验位
        private StopBits _stopBits = StopBits.One;//停止位
        private SerialPortDatabits _dataBits = SerialPortDatabits.EightBits;//数据位

        private SerialPort comPort;

        /// <summary>
        /// 接收事件是否有效 false表示有效
        /// </summary>
        private bool ReceiveEventFlag = false;

        /// <summary>
        /// 串口号
        /// </summary>
        public string PortName
        {
            get { return _portName; }
            set
            {
                _portName = value;
                comPort.PortName = value;
            }
        }

        /// <summary>
        /// 波特率
        /// </summary>
        public SerialPortBaudRates BaudRate
        {
            get { return _baudRate; }
            set
            {
                _baudRate = value;
                comPort.BaudRate = (int)value;
            }
        }

        /// <summary>
        /// 奇偶校验位
        /// </summary>
        public Parity Parity
        {
            get { return _parity; }
            set
            {
                _parity = value;
                comPort.Parity = value;
            }
        }

        /// <summary>
        /// 数据位
        /// </summary>
        public SerialPortDatabits DataBits
        {
            get { return _dataBits; }
            set
            {
                _dataBits = value;
                comPort.DataBits = (int)value;
            }
        }

        /// <summary>
        /// 停止位
        /// </summary>
        public StopBits StopBits
        {
            get { return _stopBits; }
            set
            {
                _stopBits = value;
                comPort.StopBits = value;
            }
        }

        /// <summary>
        /// 端口是否已经打开
        /// </summary>
        public bool IsOpen
        {
            get
            {
                return comPort.IsOpen;
            }
        }

        /// <summary>
        /// 串口数据位列表（5,6,7,8）
        /// </summary>
        public enum SerialPortDatabits : int
        {
#pragma warning disable CS1591 // 缺少对公共可见类型或成员的 XML 注释
            FiveBits = 5,
            SixBits = 6,
            SeventBits = 7,
            EightBits = 8
#pragma warning restore CS1591 // 缺少对公共可见类型或成员的 XML 注释
        }

        /// <summary>
        /// 串口波特率列表。
        /// 75,110,150,300,600,1200,2400,4800,9600,14400,19200,28800,38400,56000,57600,
        /// 115200,128000,230400,256000
        /// </summary>
        public enum SerialPortBaudRates : int
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

        #region 构造函数

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
            _portName = name;
            _baudRate = baud;
            _parity = par;
            _dataBits = dBits;
            _stopBits = sBits;

            comPort = new SerialPort(_portName, (int)_baudRate, _parity, (int)_dataBits, _stopBits);
            comPort.WriteTimeout = 3000;
            comPort.ReadTimeout = 3000;
            comPort.ReceivedBytesThreshold = 1;

            comPort.DataReceived += new SerialDataReceivedEventHandler(comPort_DataReceived);
            comPort.ErrorReceived += new SerialErrorReceivedEventHandler(comPort_ErrorReceived);
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
            : this(name, (SerialPortBaudRates)Enum.Parse(typeof(SerialPortBaudRates), baud.ToString()),
                (Parity)Enum.Parse(typeof(Parity), par.ToString(), true), (SerialPortDatabits)Enum.Parse(typeof(SerialPortDatabits), dBits.ToString()),
                (StopBits)Enum.Parse(typeof(StopBits), sBits.ToString()))
        { }

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SerialPortUtil()
            : this("COM1", SerialPortBaudRates.BaudRate_9600, Parity.None, SerialPortDatabits.EightBits, StopBits.One) { }
        #endregion

        /// <summary>
        /// 打开端口
        /// </summary>
        /// <returns></returns>
        public void OpenPort()
        {
            if (this.IsOpen)
                comPort.Close();

            comPort.Open();
        }

        /// <summary>
        /// 关闭端口
        /// </summary>
        public void ClosePort()
        {
            if (this.IsOpen)
                comPort.Close();
        }

        /// <summary>
        /// 丢弃来自串行驱动程序的接收和发送缓冲区的数据
        /// </summary>
        public void DiscardBuffer()
        {
            comPort.DiscardInBuffer();
            comPort.DiscardOutBuffer();
        }

        /// <summary>
        /// 数据接收处理
        /// </summary>
        private void comPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            //禁止接收事件时直接退出
            if (ReceiveEventFlag)
                return;

            #region 根据结束字节来判断是否全部获取完成
            List<byte> _byteData = new List<byte>();
            //bool found = false;//是否检测到结束符号
            while (comPort.BytesToRead > 0)
            {
                byte[] readBuffer = new byte[comPort.ReadBufferSize + 1];
                int count = comPort.Read(readBuffer, 0, comPort.ReadBufferSize);
                for (int i = 0; i < count; i++)
                {
                    _byteData.Add(readBuffer[i]);

                    //if (readBuffer[i] == EndByte)
                    //{
                    //    found = true;
                    //}
                }
            }
            #endregion

            //字符转换
            string readString = Encoding.Default.GetString(_byteData.ToArray(), 0, _byteData.Count);

            //触发整条记录的处理
            DataReceived?.Invoke(sender, new DataReceivedEventArgs(readString));
        }

        /// <summary>
        /// 错误处理函数
        /// </summary>
        private void comPort_ErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            Error?.Invoke(sender, e);
        }

        #region 数据写入操作

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg"></param>
        public void WriteData(string msg)
        {
            WriteData(Encoding.Default.GetBytes(msg));
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg">写入端口的字节数组</param>
        public void WriteData(byte[] msg)
        {
            WriteData(msg, 0, msg.Length);
        }

        /// <summary>
        /// 写入数据
        /// </summary>
        /// <param name="msg">包含要写入端口的字节数组</param>
        /// <param name="offset">参数从0字节开始的字节偏移量</param>
        /// <param name="count">要写入的字节数</param>
        public void WriteData(byte[] msg, int offset, int count)
        {
            if (!this.IsOpen)
                comPort.Open();

            comPort.Write(msg, offset, count);
        }

        /// <summary>
        /// 发送串口命令
        /// </summary>
        /// <param name="SendData">发送数据</param>
        /// <param name="ReceiveData">接收数据</param>
        /// <param name="Overtime">重复次数</param>
        /// <returns></returns>
        public int SendCommand(byte[] SendData, ref byte[] ReceiveData, int Overtime)
        {
            if (!this.IsOpen)
                comPort.Open();

            ReceiveEventFlag = true;        //关闭接收事件
            comPort.DiscardInBuffer();      //清空接收缓冲区                 
            comPort.Write(SendData, 0, SendData.Length);

            int num = 0, ret = 0;
            while (num++ < Overtime)
            {
                if (comPort.BytesToRead >= ReceiveData.Length) break;
                System.Threading.Thread.Sleep(1);
            }

            if (comPort.BytesToRead >= ReceiveData.Length)
            {
                ret = comPort.Read(ReceiveData, 0, ReceiveData.Length);
            }

            ReceiveEventFlag = false;       //打开事件
            return ret;
        }

        #endregion

        #region 常用的列表数据获取和绑定操作

        /// <summary>
        /// 封装获取串口号列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetPortNames()
        {
            return SerialPort.GetPortNames();
        }

        /// <summary>
        /// 设置串口号
        /// </summary>
        /// <param name="obj"></param>
        public static void SetPortNameValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in SerialPort.GetPortNames())
            {
                obj.Items.Add(str);
            }
        }

        /// <summary>
        /// 设置波特率
        /// </summary>
        public static void SetBauRateValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (SerialPortBaudRates rate in Enum.GetValues(typeof(SerialPortBaudRates)))
            {
                obj.Items.Add(((int)rate).ToString());
            }
        }

        /// <summary>
        /// 设置数据位
        /// </summary>
        public static void SetDataBitsValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (SerialPortDatabits databit in Enum.GetValues(typeof(SerialPortDatabits)))
            {
                obj.Items.Add(((int)databit).ToString());
            }
        }

        /// <summary>
        /// 设置校验位列表
        /// </summary>
        public static void SetParityValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(Parity)))
            {
                obj.Items.Add(str);
            }
            //foreach (Parity party in Enum.GetValues(typeof(Parity)))
            //{
            //    obj.Items.Add(((int)party).ToString());
            //}
        }

        /// <summary>
        /// 设置停止位
        /// </summary>
        public static void SetStopBitValues(ComboBox obj)
        {
            obj.Items.Clear();
            foreach (string str in Enum.GetNames(typeof(StopBits)))
            {
                obj.Items.Add(str);
            }
            //foreach (StopBits stopbit in Enum.GetValues(typeof(StopBits)))
            //{
            //    obj.Items.Add(((int)stopbit).ToString());
            //}   
        }

        #endregion

        #region 格式转换
        /// <summary>
        /// 转换十六进制字符串到字节数组
        /// </summary>
        /// <param name="msg">待转换字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] HexToByte(string msg)
        {
            msg = msg.Replace(" ", "");//移除空格

            //create a byte array the length of the
            //divided by 2 (Hex is 2 characters in length)
            byte[] comBuffer = new byte[msg.Length / 2];
            for (int i = 0; i < msg.Length; i += 2)
            {
                //convert each set of 2 characters to a byte and add to the array
                comBuffer[i / 2] = Convert.ToByte(msg.Substring(i, 2), 16);
            }

            return comBuffer;
        }

        /// <summary>
        /// 转换字节数组到十六进制字符串
        /// </summary>
        /// <param name="comByte">待转换字节数组</param>
        /// <returns>十六进制字符串</returns>
        public static string ByteToHex(byte[] comByte)
        {
            StringBuilder builder = new StringBuilder(comByte.Length * 3);
            foreach (byte data in comByte)
            {
                builder.Append(Convert.ToString(data, 16).PadLeft(2, '0').PadRight(3, ' '));
            }

            return builder.ToString().ToUpper();
        }

        /// <summary>
        /// 16位CRC检验码生成器
        /// </summary>
        /// <param name="data">数据</param>
        /// <param name="mode">CRC模式</param>
        /// <returns>CRC检验码，高位在前</returns>
        public static byte[] CRC16(byte[] data, CRCMODE mode)
        {
            ushort crcpoly, crcinit = 0x0000, crcxorout = 0x0000;
            bool refin = true, refout = true;
            switch (mode)
            {
                case CRCMODE.CCITT:
                    crcpoly = 0x1021;
                    break;
                case CRCMODE.CCITT_FALSE:
                    crcpoly = 0x1021;
                    crcinit = 0xFFFF;
                    refin = false;
                    refout = false;
                    break;
                case CRCMODE.DNP:
                    crcpoly = 0x3D65;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMODE.IBM:
                    crcpoly = 0x8005;
                    break;
                case CRCMODE.MAXIM:
                    crcpoly = 0x8005;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMODE.MODBUS:
                    crcpoly = 0x8005;
                    crcinit = 0xFFFF;
                    break;
                case CRCMODE.USB:
                    crcpoly = 0x8005;
                    crcinit = 0xFFFF;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMODE.X25:
                    crcpoly = 0x1021;
                    crcinit = 0xFFFF;
                    crcxorout = 0xFFFF;
                    break;
                case CRCMODE.XMODEM:
                    crcpoly = 0x1021;
                    refin = false;
                    refout = false;
                    break;
                default:
                    crcpoly = 0x8005;
                    crcinit = 0xFFFF;
                    break;
            }
            for (int i = 0; i < data.Length; i++)
            {
                byte temp = data[i];
                if (refin)
                {
                    char[] arr = Convert.ToString(temp, 2).PadLeft(8, '0').ToCharArray();
                    Array.Reverse(arr);
                    temp = Convert.ToByte(new string(arr), 2);
                }
                crcinit ^= (ushort)(temp << 8);
                for (int j = 0; j < 8; j++)
                    crcinit = (ushort)((crcinit & 0x8000) > 0 ? ((crcinit << 1) ^ crcpoly) : (crcinit << 1));
            }
            if (refout)
            {
                char[] arr = Convert.ToString(crcinit, 2).PadLeft(16, '0').ToCharArray();
                Array.Reverse(arr);
                crcinit = Convert.ToUInt16(new string(arr), 2);
            }
            crcinit ^= crcxorout;
            byte hi = (byte)((crcinit & 0xFF00) >> 8);
            byte lo = (byte)(crcinit & 0x00FF);
            return new byte[] { hi, lo };
        }
        #endregion

        /// <summary>
        /// 检查端口名称是否存在
        /// </summary>
        /// <param name="port_name"></param>
        /// <returns></returns>
        public static bool Exists(string port_name)
        {
            foreach (string port in SerialPort.GetPortNames()) if (port == port_name) return true;
            return false;
        }

        /// <summary>
        /// 格式化端口相关属性
        /// </summary>
        /// <param name="port"></param>
        /// <returns></returns>
        public static string Format(SerialPort port)
        {
            return string.Format("{0} ({1},{2},{3},{4},{5})",
                port.PortName, port.BaudRate, port.DataBits, port.StopBits, port.Parity, port.Handshake);
        }
    }

    /// <summary>
    /// CRC模式
    /// </summary>
    public enum CRCMODE
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
    /// 接收数据处理事件
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        /// <summary>
        /// 接收的数据
        /// </summary>
        public string DataReceived;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="m_DataReceived">接收的数据</param>
        public DataReceivedEventArgs(string m_DataReceived)
        {
            this.DataReceived = m_DataReceived;
        }
    }
}
