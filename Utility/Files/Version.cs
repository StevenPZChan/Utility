using System;
using System.Diagnostics;
using System.IO;

namespace Utility.Files
{
    /// <summary>
    /// 版本信息类
    /// </summary>
    public static class Version
    {
        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <param name="file">文件名</param>
        /// <param name="version">版本号</param>
        /// <param name="modifiedtime">修改时间</param>
        public static void GetVersion(this string file, out string version, out DateTime modifiedtime)
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(file);
            version = string.Join(".", info.ProductMajorPart, info.ProductMinorPart, info.ProductBuildPart, info.ProductPrivatePart);
            modifiedtime = new FileInfo(file).LastWriteTime;
        }
    }
}
