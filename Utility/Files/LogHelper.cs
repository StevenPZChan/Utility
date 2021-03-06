﻿using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utility.Files
{
    /// <summary>
    /// 日志记录类 在Program.cs中插入LogHelper.BindExceptionHandler();即可实现未捕捉异常的日志记录功能
    /// </summary>
    public static class LogHelper
    {
        #region Methods
        /// <summary>
        /// 绑定全局异常处理
        /// </summary>
        public static void BindExceptionHandler()
        {
            //设置应用程序处理异常方式：ThreadException处理
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += Application_ThreadException;
            //处理其他线程未捕获的异常
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            //处理Task未捕获的异常，GC调用时才会触发
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;
        }

        /// <summary>
        /// 写日志信息
        /// </summary>
        /// <param name="ex">异常类</param>
        /// <param name="path">日志文件存放路径</param>
        public static void WriteLogInfo(Exception ex, string path = "")
        {
            string logAddress = CreateLog(path);
            using (var sw = new StreamWriter(logAddress, true, Encoding.Default))
            {
                sw.WriteLine($"*****************************************【{DateTime.Now.ToLongTimeString()}】*****************************************");
                if (ex != null)
                {
                    sw.WriteLine("【ErrorType】" + ex.GetType());
                    sw.WriteLine("【TargetSite】" + ex.TargetSite);
                    sw.WriteLine("【Message】" + ex.Message);
                    sw.WriteLine("【Source】" + ex.Source);
                    sw.WriteLine("【StackTrace】" + ex.StackTrace.Trim());
                }
                else
                    sw.WriteLine("Exception is NULL");

                sw.WriteLine();
            }
        }

        private static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            WriteLogInfo(e.Exception);
            MessageBox.Show($"发生错误：{e.Exception.Message}，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            Environment.Exit(0);
        }

        /// <summary>
        /// 创建日志文件
        /// </summary>
        /// <param name="path">日志路径</param>
        /// <returns>日志文件名</returns>
        private static string CreateLog(string path)
        {
            if (path == "")
                path = Application.StartupPath + @"\log";
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //发生异常每天都创建一个单独的日子文件[*.log],每天的错误信息都在这一个文件里。方便查找
            return $@"{path}\{DateTime.Now:yyyyMMdd}.log";
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteLogInfo(e.ExceptionObject as Exception);

            //发生致命错误时才反馈信息并关闭程序
            if (!e.IsTerminating)
                return;

            MessageBox.Show($"发生错误：{((Exception)e.ExceptionObject).Message}，请查看程序日志！", "系统错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            Application.Exit();
            Environment.Exit(0);
        }

        private static void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            foreach (Exception ex in e.Exception.InnerExceptions)
                WriteLogInfo(ex);
            //将异常标识为已经观察到
            e.SetObserved();
        }
        #endregion
    }
}
