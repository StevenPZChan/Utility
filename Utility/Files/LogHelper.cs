using System;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Utility.Files
{
    /// <summary>
    /// 日志记录类
    /// 在Program.cs中插入LogHelper.BindExceptionHandler();即可实现未捕捉异常的日志记录功能
    /// </summary>
    public class LogHelper
    {
        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="ex">异常类</param>
        /// <param name="path">日志路径</param>
        /// <returns>日志文件名</returns>
        public static string CreateLog(Exception ex, string path)
        {
            if (path == "")
                path = Application.StartupPath + "\\log";
            if (!Directory.Exists(path))
            {
                //创建日志文件夹
                Directory.CreateDirectory(path);
            }
            //发生异常每天都创建一个单独的日子文件[*.log],每天的错误信息都在这一个文件里。方便查找
            path += "\\" + DateTime.Now.ToString("yyyyMMdd") + ".log";
            return path;
        }

        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="ex">异常类</param>
        /// <param name="path">日志文件存放路径</param>
        private static void WriteLogInfo(Exception ex, string path = "")
        {
            string logAddress = CreateLog(ex, path);
            using (StreamWriter sw = new StreamWriter(logAddress, true, Encoding.Default))
            {
                sw.WriteLine("*****************************************【"
                               + DateTime.Now.ToLongTimeString()
                               + "】*****************************************");
                if (ex != null)
                {
                    sw.WriteLine("【ErrorType】" + ex.GetType());
                    sw.WriteLine("【TargetSite】" + ex.TargetSite);
                    sw.WriteLine("【Message】" + ex.Message);
                    sw.WriteLine("【Source】" + ex.Source);
                    sw.WriteLine("【StackTrace】" + ex.StackTrace.Trim());
                }
                else
                {
                    sw.WriteLine("Exception is NULL");
                }
                sw.WriteLine();
            }
        }

        /// <summary>
        /// 绑定全局异常处理
        /// </summary>
        public static void BindExceptionHandler()
        {
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += Application_ThreadException;
            //处理未捕获的异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        /// <summary>
        /// 处理UI线程异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            WriteLogInfo(e.Exception as Exception);
            MessageBox.Show("发生错误：" + e.Exception.Message + "，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            Environment.Exit(0);
        }

        /// <summary>
        /// 处理未捕获的异常
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteLogInfo(e.ExceptionObject as Exception);
            MessageBox.Show("发生错误：" + ((Exception)e.ExceptionObject).Message + "，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            Environment.Exit(0);
        }
    }
}
