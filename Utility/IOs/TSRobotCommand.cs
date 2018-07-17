using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using TsRemoteLib;

namespace Utility.IOs
{
    /// <summary>
    /// 东芝四轴机器人数据交换类
    /// </summary>
    public static class TSRobotCommand
    {
        #region Methods
        /// <summary>
        /// 获取工件集合的发送数据
        /// </summary>
        /// <param name="wps">工件集合</param>
        /// <returns>前最多10个工件的位置发送数据bytes</returns>
        public static byte[] GetSendingMessage(this IList<Workpiece> wps)
        {
            var num = (short)(wps.Count() > 10 ? 10 : wps.Count());
            IEnumerable<Workpiece> w = wps.Take(num);
            string message = num.ToString();
            message = w.Aggregate(message, (current, wp) => string.Concat(current, ",", wp.GetMessage()));
            return Encoding.ASCII.GetBytes(string.Concat(message, ",\r"));
        }

        /// <summary>
        /// 获取单个工件的发送数据
        /// </summary>
        /// <param name="wp">工件对象</param>
        /// <returns>工件位置发送数据</returns>
        public static byte[] GetSendingMessage(this Workpiece wp) => new[] {wp}.GetSendingMessage();

        /// <summary>
        /// 解析机器人返回数据
        /// </summary>
        /// <param name="message">返回数据</param>
        /// <param name="type">解析类型</param>
        /// <param name="response">回文</param>
        /// <returns>解析结果</returns>
        public static object Parse(this string message, string type, ref string response)
        {
            switch (type)
            {
                case "Status":
                    Match match = Regex.Match(message,
                        "FL,EE(\\d+) SE(\\d+) SC(\\d+) BC(\\d+) ES(\\d+) SS(\\d+) SV(\\d+) MM(\\d+) RM(\\d+) RS(\\d+) OV(\\d+) AL(\\d+) DC(\\d+) DS(\\d+)");
                    if (!match.Success)
                        return null;
                    else
                    {
                        var result = new List<int>();
                        for (var i = 1; i < 15; i++)
                            result.Add(int.Parse(match.Groups[i].Value));
                        response = "OK\r";
                        return result.ToArray();
                    }
                default: return null;
            }
        }
        #endregion
    }

    /// <summary>
    /// 东芝四轴机器人控制类
    /// </summary>
    public class TSRobotScara : TsRemoteS
    {
        #region Fields
        private readonly Dictionary<int, string> runmode = new Dictionary<int, string> {[0] = "CONT", [1] = "CYCLE", [2] = "STEP", [3] = "SEGMENT"},
            runstatus = new Dictionary<int, string> {[0] = "STOP(RESET)", [1] = "RUN", [2] = "STOP(RETRY)", [3] = "STOP(CONT)"};
        private TsStatusMonitor _status;
        #endregion

        #region Constructors
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public TSRobotScara() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ipAddress">IP地址</param>
        /// <param name="port">端口号</param>
        public TSRobotScara(string ipAddress, int port)
            : this()
        {
            SetIPaddr(1, ipAddress, port);
        }
        #endregion

        #region Properties
        /// <summary>
        /// 超弛
        /// </summary>
        public int Override
        {
            get { return this._status.Override; }
            set { SetOVRD(value); }
        }

        /// <summary>
        /// 程序运行状态
        /// </summary>
        public bool Running => this._status.RunStatus == 1;
        /// <summary>
        /// 运行状态缩写
        /// </summary>
        public string RunStatus => $"{this.Override}% {this.runmode[this._status.RunMode]}:{this.runstatus[this._status.RunStatus]}";
        /// <summary>
        /// 选择运行的程序
        /// </summary>
        public string SelectFile { get; private set; }

        /// <summary>
        /// 伺服状态
        /// </summary>
        public bool Servo
        {
            get { return this._status.ServoStatus == 1; }
            set
            {
                if (value == this.Servo)
                    return;

                if (value)
                    ServoOn();
                else
                    ServoOff();
            }
        }
        #endregion

        #region Events
        /// <summary>
        /// 有警报时事件
        /// </summary>
        public event EventHandler<string> Alarm;
        #endregion

        #region Methods
        /// <summary>
        /// 最小二乘法标定工具坐标系
        /// </summary>
        /// <param name="points">工具位于同一点时的世界坐标集合，SCARA机器人要求不同的C坐标至少两个点</param>
        /// <returns>工具坐标系</returns>
        public static TsTransS CalcTransTool(IEnumerable<TsPointS> points)
        {
            List<TsPointS> selectp = points.GroupBy(p => p.C).Select(p => p.First()).ToList();
            if (selectp.Count < 2)
                return null;

            var newTrans = new TsTransS();
            var r = new double[(selectp.Count - 1) * 2, 2];
            var T = new double[(selectp.Count - 1) * 2];
            for (var i = 1; i < selectp.Count; i++)
            {
                r[i * 2 - 2, 0] = Math.Cos(selectp[i].C * Math.PI / 180.0) - Math.Cos(selectp[0].C * Math.PI / 180.0);
                r[i * 2 - 2, 1] = -Math.Sin(selectp[i].C * Math.PI / 180.0) + Math.Sin(selectp[0].C * Math.PI / 180.0);
                r[i * 2 - 1, 0] = Math.Sin(selectp[i].C * Math.PI / 180.0) - Math.Sin(selectp[0].C * Math.PI / 180.0);
                r[i * 2 - 1, 1] = Math.Cos(selectp[i].C * Math.PI / 180.0) - Math.Cos(selectp[0].C * Math.PI / 180.0);
                T[i * 2 - 2] = selectp[0].X - selectp[i].X;
                T[i * 2 - 1] = selectp[0].Y - selectp[i].Y;
            }

            var a = new double[2, 2];
            var b = new double[2];
            for (var i = 0; i < 2; i++)
            {
                for (var j = 0; j < 2; j++)
                for (var k = 0; k < (selectp.Count - 1) * 2; k++)
                    a[i, j] += r[k, i] * r[k, j];
                for (var k = 0; k < (selectp.Count - 1) * 2; k++)
                    b[i] += r[k, i] * T[k];
            }

            newTrans.X = (a[1, 1] * b[0] - a[0, 1] * b[1]) / (a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0]);
            newTrans.Y = (-a[1, 0] * b[0] + a[0, 0] * b[1]) / (a[0, 0] * a[1, 1] - a[0, 1] * a[1, 0]);
            newTrans.Z = 0;
            newTrans.C = 0;
            return newTrans;
        }

        /// <summary>
        /// 两点法标定工件坐标系
        /// </summary>
        /// <param name="origin">工件坐标系原点</param>
        /// <param name="direction">工件坐标系中X正方向上任意一点</param>
        /// <returns>工件坐标系</returns>
        public static TsTransS CalcTransWork(TsPointS origin, TsPointS direction) =>
            new TsTransS {X = origin.X, Y = origin.Y, Z = origin.Z, C = 180.0 / Math.PI * Math.Atan2(direction.Y - origin.Y, direction.X - origin.X)};

        /// <summary>
        /// 连接控制器
        /// </summary>
        /// <returns>是否连接成功</returns>
        public bool Connect()
        {
            bool res = Connect(1);
            this._status = GetStatusMonitor();
            this.SelectFile = GetStatus().SelectFile;
            WatchDogStart(200, 0, 0, StatusChanged);
            return res;
        }

        /// <summary>
        /// 连接控制器
        /// </summary>
        /// <param name="type">连接类型</param>
        /// <returns>是否连接成功</returns>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public new bool Connect(int type) => base.Connect(type);

        /// <summary>
        /// 断开连接
        /// </summary>
        /// <returns>是否成功断开</returns>
        public new bool Disconnect()
        {
            WatchDogStop();
            return base.Disconnect();
        }

        /// <summary>
        /// 获取控制器中的所有程序名
        /// </summary>
        /// <returns>程序名数组</returns>
        public new string[] GetDir()
        {
            List<TsFileInfo> files = base.GetDir();
            var filenames = new List<string>();
            foreach (TsFileInfo f in files)
                filenames.Add(f.name);
            return filenames.ToArray();
        }

        /// <summary>
        /// 选择运行程序
        /// </summary>
        /// <param name="filename">程序名</param>
        public new void ProgramSelect(string filename)
        {
            base.ProgramSelect(filename);
            this.SelectFile = GetStatus().SelectFile;
        }

        private void StatusChanged(TsStatusMonitor status)
        {
            this._status = status;
            if (status.AlarmLevel > 0)
            {
                List<TsAlarm> currentAlarms = GetCurrentAlarm();
                var alarmmessage = "";
                foreach (TsAlarm alarm in currentAlarms)
                    alarmmessage += $"{alarm.Date} {alarm.AlarmNo} {alarm.AlarmMes}\r\n";
                Alarm?.Invoke(this, alarmmessage);
            }

            if (status.BreakCommand == 1 || status.StopCommand == 1) { }

            if (status.EmergencyStop == 1 || status.SafetyStop == 1) { }

            if (status.EtherNetConnection != 1)
                return;

            WatchDogStop();
            Task.Delay(1000).Wait();
            Connect(1);
        }
        #endregion
    }

    /// <summary>
    /// 单个工件结构
    /// </summary>
    public struct Workpiece
    {
        #region Fields
        private readonly double _c;
        private readonly int _opt1;
        private readonly int _opt2;
        private readonly int _opt3;
        private readonly double _x;
        private readonly double _y;
        #endregion Fields

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="x">X坐标</param>
        /// <param name="y">Y坐标</param>
        /// <param name="c">C坐标</param>
        /// <param name="opt1">附加数据1</param>
        /// <param name="opt2">附加数据2</param>
        /// <param name="opt3">附加数据3</param>
        public Workpiece(double x, double y, double c, int opt1 = 0, int opt2 = 0, int opt3 = 0)
        {
            this._x = x;
            this._y = y;
            this._c = c;
            this._opt1 = opt1;
            this._opt2 = opt2;
            this._opt3 = opt3;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// 获取该工件的发送数据
        /// </summary>
        /// <returns>发送数据bytes</returns>
        public string GetMessage()
        {
            string message = string.Join(",", this._x.ToString("f3"), this._y.ToString("f3"), this._c.ToString("f3"), this._opt1.ToString(),
                this._opt2.ToString(), this._opt3.ToString());
            return message;
        }
        #endregion Methods
    }
}
