using System;
using System.Linq;
using System.Windows.Forms;

namespace Utility.IOs
{
    /// <summary>
    /// DAM盒子串口命令类
    /// </summary>
    public static class DAMCommand
    {
        #region WorkMode Enum
        /// <summary>
        /// 工作模式
        /// </summary>
        public enum WorkMode
        {
            /// <summary>
            /// 正常模式
            /// </summary>
            _NORM,
            /// <summary>
            /// 自锁模式
            /// </summary>
            _SELFLOCK
        }
        #endregion

        #region Methods
        /// <summary>
        /// 查询DI通道状态
        /// </summary>
        /// <param name="sp">串口辅助类对象</param>
        /// <param name="start">开始通道（从1开始）</param>
        /// <param name="num">查询通道数量（应不大于8）</param>
        /// <returns>通道闭合状态数组</returns>
        public static int[] CheckInput(this SerialPortUtil sp, int start, int num)
        {
            var status = new int[num];
            var message = new byte[6];
            sp.SendCommand(CheckInputData(start, num), ref message, 100);
            int[] res = DataParse(message);
            Array.Copy(res, 0, status, 0, num);
            return status;
        }

        /// <summary>
        /// 使用定时器查询DI通道状态，但不返回结果，需要使用DataReceived事件获取结果
        /// </summary>
        /// <param name="sp">串口辅助类对象</param>
        /// <param name="start">开始通道（从1开始）</param>
        /// <param name="num">查询通道数量（应不大于8）</param>
        /// <param name="timer">Timer控件对象</param>
        public static void CheckInput(this SerialPortUtil sp, int start, int num, Timer timer) =>
            timer.Tick += (sender, e) => { sp.WriteData(CheckInputData(start, num)); };

        /// <summary>
        /// 控制所有继电器
        /// </summary>
        /// <param name="sp">串口辅助类对象</param>
        /// <param name="closing">闭合为true，断开为false</param>
        public static void ControlAllRelays(this SerialPortUtil sp, bool closing) =>
            sp.WriteData(closing ? SerialPortUtil.HexToByte("FE 0F 00 00 00 04 01 FF 31 D2") : SerialPortUtil.HexToByte("FE 0F 00 00 00 04 01 00 71 92"));

        /// <summary>
        /// 控制继电器
        /// </summary>
        /// <param name="sp">串口辅助类对象</param>
        /// <param name="channel">通道（从1开始）</param>
        /// <param name="closing">闭合为true，断开为false</param>
        public static void ControlRelay(this SerialPortUtil sp, int channel, bool closing)
        {
            byte[] msg, crc16;
            msg = SerialPortUtil.HexToByte("FE 05 00 00 00 00");
            msg[3] = (byte)(channel - 1);
            msg[4] = (byte)(closing ? 0xFF : 0x00);
            crc16 = SerialPortUtil.CRC16(msg, CRCMode.MODBUS);
            Array.Reverse(crc16);
            sp.WriteData(msg.Concat(crc16).ToArray());
        }

        /// <summary>
        /// 控制继电器
        /// </summary>
        /// <param name="sp">串口辅助类对象</param>
        /// <param name="channel">通道（从1开始）</param>
        /// <param name="closing">闪闭为true，闪开为false</param>
        /// <param name="time">时间，单位100ms</param>
        public static void ControlRelay(this SerialPortUtil sp, int channel, bool closing, int time)
        {
            byte[] msg, crc16;
            msg = SerialPortUtil.HexToByte("FE 10 00 00 00 02 04 00 00 00 00");
            msg[3] = (byte)(channel * 5 - 2);
            msg[8] = (byte)(closing ? 0x04 : 0x02);
            msg[10] = (byte)time;
            crc16 = SerialPortUtil.CRC16(msg, CRCMode.MODBUS);
            Array.Reverse(crc16);
            sp.WriteData(msg.Concat(crc16).ToArray());
        }

        /// <summary>
        /// 数据解析函数
        /// </summary>
        /// <param name="message">接收到的数据</param>
        /// <returns>解析结果</returns>
        public static int[] DataParse(byte[] message)
        {
            var status = new int[8];
            if (message[0] != 0xFE || message[1] != 0x02 || message[2] != 0x01)
                for (var i = 0; i < 8; i++)
                    status[i] = -1;
            else
                for (var i = 0; i < 8; i++)
                    status[i] = message[3] & (1 << i);
            return status;
        }

        /// <summary>
        /// 设置工作模式
        /// </summary>
        /// <param name="sp">串口辅助类对象</param>
        /// <param name="wm">工作模式</param>
        public static void SetWorkMode(this SerialPortUtil sp, WorkMode wm)
        {
            byte[] msg, crc16;
            msg = SerialPortUtil.HexToByte("FE 10 03 EB 00 01 02 00 00");
            msg[8] = (byte)(int)wm;
            crc16 = SerialPortUtil.CRC16(msg, CRCMode.MODBUS);
            Array.Reverse(crc16);
            sp.WriteData(msg.Concat(crc16).ToArray());
        }

        private static byte[] CheckInputData(int start, int num)
        {
            byte[] msg, crc16;
            msg = SerialPortUtil.HexToByte("FE 02 00 00 00 00");
            msg[3] = (byte)(start - 1);
            msg[5] = (byte)num;
            crc16 = SerialPortUtil.CRC16(msg, CRCMode.MODBUS);
            Array.Reverse(crc16);
            return msg.Concat(crc16).ToArray();
        }
        #endregion
    }
}
