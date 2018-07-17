using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Services;
using System.Xml;

namespace Utility.Software
{
    /// <summary>
    /// 自动更新类
    /// </summary>
    public class AutoUpdate
    {
        #region Fields
        private readonly DataTable _neededToDownload = new DataTable();
        private XmlDocument xml;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        public AutoUpdate()
        {
            this.Remains = new List<string>();
            this.Remains.Clear();
            this._neededToDownload.Clear();
            this._neededToDownload.Columns.Add("FileName", typeof(string));
            this._neededToDownload.Columns.Add("SourceDir", typeof(string));
            this._neededToDownload.Columns.Add("DestDir", typeof(string));
        }
        #endregion

        #region Properties
        /// <summary>
        /// 剩余没更新的文件
        /// </summary>
        public List<string> Remains { get; }
        #endregion

        #region Methods
        /// <summary>
        /// 下载更新文件到update目录并更新原文件
        /// </summary>
        /// <param name="destPath">本地程序目录</param>
        /// <param name="webServiceAddress">服务器程序目录</param>
        /// <returns>是否更新成功，如不成功，可检查update目录并手动更新</returns>
        public bool UpdateFiles(string destPath, string webServiceAddress)
        {
            if (!destPath.EndsWith(@"/"))
                destPath += @"/";
            if (!webServiceAddress.EndsWith(@"/"))
                webServiceAddress += @"/";

            if (!Directory.Exists(destPath))
                Directory.CreateDirectory(destPath);

            string tempPath = destPath + @"update/";

            if (Directory.Exists(tempPath))
            {
                Directory.Delete(tempPath, true);
                Directory.CreateDirectory(tempPath);
            }
            else
                Directory.CreateDirectory(tempPath);

            try
            {
                //写入新UpdateConfig.xml升级完毕后替换
                //xml.Save(tempPath + "UpdateConfig.xml");
                //if (DialogResult.Yes == MessageBox.Show("有新版本，升级否", "提示", MessageBoxButtons.YesNo))
                //{
                foreach (DataRow dr in this._neededToDownload.Rows)
                    DownloadFile(webServiceAddress + dr[1], tempPath + dr[2]);

                string[] dirs = Directory.GetFiles(tempPath, "*.*", SearchOption.AllDirectories);
                foreach (string dir in dirs)
                {
                    string fileName = dir.Replace(tempPath, "");
                    try
                    {
                        if (File.Exists(destPath + fileName))
                            File.Delete(destPath + fileName);
                        //TODO:如果系统正在运行，您得停止系统，至于如何停止，也许可以使用System.Diagnostics.Process

                        File.Copy(dir, destPath + fileName);
                    }
                    catch
                    {
                        this.Remains.Add(fileName);
                    }
                }

                this.xml.Save(destPath + "UpdateConfig.xml");
                //MessageBox.Show("升级完毕");
                //}
            }
            catch (Exception ex)
            {
                throw new Exception($"升级失败，原因是：{ex.Message}。请重试！", ex);
            }

            return true;
        }

        /// <summary>
        /// 检查版本更新
        /// </summary>
        /// <param name="xmlPath">本地版本信息xml所在目录</param>
        /// <param name="webServiceAddress">服务器版本信息xml的所在目录URL</param>
        /// <returns>是否有更新</returns>
        public bool VersionCheck(string xmlPath, string webServiceAddress) //版本号校验
        {
            var newVersionExist = false;
            if (!xmlPath.EndsWith(@"/"))
                xmlPath += @"/";
            if (!File.Exists(xmlPath + "UpdateConfig.xml"))
            {
                var updateConfig = new XmlDocument();
                updateConfig.LoadXml(@"<root></root>");
                updateConfig.Save(xmlPath + "UpdateConfig.xml");
            }

            try
            {
                var appUpdate = new MyService(webServiceAddress);
                XmlDocument serverXmlDoc = appUpdate.AppUpdateVersion();
                var localXmlDoc = new XmlDocument();
                localXmlDoc.Load(xmlPath + @"UpdateConfig.xml");
                if (serverXmlDoc.DocumentElement != null && localXmlDoc.DocumentElement != null)
                {
                    XmlNodeList serverNodes = serverXmlDoc.DocumentElement.GetElementsByTagName("File");
                    XmlNodeList localNodes = localXmlDoc.DocumentElement.GetElementsByTagName("File");
                    foreach (XmlNode serverNode in serverNodes)
                    {
                        var moduleExist = false;
                        foreach (XmlNode localNode in localNodes)
                        {
                            //找到对应模块
                            if (((XmlElement)localNode).GetElementsByTagName("FileName")[0].InnerText
                             != ((XmlElement)serverNode).GetElementsByTagName("FileName")[0].InnerText)
                                continue;

                            moduleExist = true;
                            bool isNew;
                            try
                            {
                                isNew = new Version(((XmlElement)localNode).GetElementsByTagName("Version")[0].InnerText).CompareTo(
                                            new Version(((XmlElement)serverNode).GetElementsByTagName("Version")[0].InnerText)) < 0;
                            }
                            catch
                            {
                                isNew = string.Compare(((XmlElement)localNode).GetElementsByTagName("Version")[0].InnerText,
                                            ((XmlElement)serverNode).GetElementsByTagName("Version")[0].InnerText, StringComparison.Ordinal) < 0;
                            }

                            //版本号判断
                            if (isNew)
                            {
                                newVersionExist = true;
                                DataRow dr = this._neededToDownload.NewRow();
                                dr[0] = ((XmlElement)serverNode).GetElementsByTagName("FileName")[0].InnerText;
                                dr[1] = ((XmlElement)serverNode).GetElementsByTagName("SourceDir")[0].InnerText;
                                dr[2] = ((XmlElement)serverNode).GetElementsByTagName("DestDir")[0].InnerText;
                                this._neededToDownload.Rows.Add(dr);
                            }

                            break;
                        }

                        //没找到对应模块
                        if (moduleExist)
                            continue;

                        {
                            newVersionExist = true;
                            DataRow dr = this._neededToDownload.NewRow();
                            dr[0] = ((XmlElement)serverNode).GetElementsByTagName("FileName")[0].InnerText;
                            dr[1] = ((XmlElement)serverNode).GetElementsByTagName("SourceDir")[0].InnerText;
                            dr[2] = ((XmlElement)serverNode).GetElementsByTagName("DestDir")[0].InnerText;
                            this._neededToDownload.Rows.Add(dr);
                        }
                    }
                }

                this.xml = (XmlDocument)serverXmlDoc.Clone();
            }
            catch
            {
                return false;
            }

            return newVersionExist;
        }

        private static void DownloadFile(string source, string fileName) //下载更新文件
        {
            try
            {
                new WebClient().DownloadFile(source, fileName);
            }
            catch
            {
                // ignored
            }
        }
        #endregion

        #region Nested type: MyService
        private class MyService : WebService
        {
            #region Fields
            private readonly string url;
            #endregion

            #region Constructors
            public MyService(string address)
            {
                this.url = address;
            }
            #endregion

            #region Methods
            [WebMethod(Description = "读服务器上关于程序各个版本信息的配置文件UpdateConfig.xml")]
            public XmlDocument AppUpdateVersion()
            {
                var xml = new XmlDocument();
                xml.Load(this.url + @"/UpdateConfig.xml");

                return xml;
            }

            private string CurrentPath(string virtualPath) => HttpContext.Current.Server.MapPath(virtualPath);
            #endregion
        }
        #endregion
    }
}
