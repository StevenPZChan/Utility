using System;
using System.Threading;

namespace Utility.Software
{
    /// <summary>
    /// 控制函数执行时间,超时返回null不继续执行 调用方法 FuncTimeout.EventNeedRun action = delegate(object[] param) {
    /// //调用自定义函数 return Test(param[0].ToString(), param[1].ToString(), (DateTime)param[2]); };
    /// FuncTimeout ft = new FuncTimeout(action, 2000); var result = ft.doAction("1", "2", DateTime.Now);
    /// </summary>
    public class FuncTimeout
    {
        #region Delegates
        /// <summary>
        /// 定义一个委托 ，输入参数可选，输出object
        /// </summary>
        public delegate object EventNeedRun(params object[] param);
        #endregion

        #region Fields
        /// <summary>
        /// 要调用的方法的一个委托
        /// </summary>
        private readonly EventNeedRun _functionNeedRun;
        /// <summary>
        /// 信号量
        /// </summary>
        private readonly ManualResetEvent _manu = new ManualResetEvent(false);
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数，传入超时的时间以及运行的方法
        /// </summary>
        /// <param name="action">运行的方法</param>
        /// <param name="timeout">超时的时间</param>
        public FuncTimeout(EventNeedRun action, int timeout)
        {
            this._functionNeedRun = action;
            this.Timeout = timeout;
        }
        #endregion

        #region Properties
        /// <summary>
        /// 是否接受到信号
        /// </summary>
        public bool IsGetSignal { get; private set; }

        /// <summary>
        /// 设置超时时间
        /// </summary>
        public int Timeout { get; set; }
        #endregion

        #region Methods
        /// <summary>
        /// 调用函数
        /// </summary>
        /// <param name="input">可选个数的输入参数</param>
        /// <returns></returns>
        public object DoAction(params object[] input)
        {
            EventNeedRun whatTodo = CombineActionAndManuset;
            //通过BeginInvoke方法，在线程池上异步的执行方法。
            IAsyncResult r = whatTodo.BeginInvoke(input, MyAsyncCallback, null);
            //设置阻塞,如果上述的BeginInvoke方法在timeout之前运行完毕，则manu会收到信号。此时isGetSignal为true。
            //如果timeout时间内，还未收到信号，即异步方法还未运行完毕，则isGetSignal为false。
            this.IsGetSignal = this._manu.WaitOne(this.Timeout);

            return this.IsGetSignal ? whatTodo.EndInvoke(r) : null;
        }

        /// <summary>
        /// 把要传进来的方法，和 manu.Set()的方法合并到一个方法体。 action方法运行完毕后，设置信号量，以取消阻塞。
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns></returns>
        private object CombineActionAndManuset(params object[] input)
        {
            object output = this._functionNeedRun(input);
            this._manu.Set();

            return output;
        }

        /// <summary>
        /// 回调函数
        /// </summary>
        /// <param name="ar"></param>
        private void MyAsyncCallback(IAsyncResult ar)
        {
            //isGetSignal为false,表示异步方法其实已经超出设置的时间，此时不再需要执行回调方法。
            if (!this.IsGetSignal)
                Thread.CurrentThread.Abort();
        }
        #endregion
    }
}
