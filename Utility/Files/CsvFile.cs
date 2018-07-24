using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;

namespace Utility.Files
{
    /// <summary>
    /// csv文件操作类
    /// </summary>
    public static class CsvFile
    {
        #region Methods
        /// <summary>
        /// 修改文件名称
        /// </summary>
        /// <param name="oldPath">旧路径</param>
        /// <param name="newPath">新路径</param>
        /// <returns>返回成功与否</returns>
        public static bool ChangeFileName(string oldPath, string newPath)
        {
            var re = false;
            try
            {
                if (File.Exists(oldPath))
                {
                    File.Move(oldPath, newPath);
                    re = true;
                }
            }
            catch
            {
                re = false;
            }

            return re;
        }

        /// 给定文件的路径，读取文件的二进制数据，判断文件的编码类型
        /// <param name="fileName">文件路径</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(string fileName)
        {
            var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            Encoding r = GetType(fs);
            fs.Close();

            return r;
        }

        /// 通过给定的文件流，判断文件的编码类型
        /// <param name="fs">文件流</param>
        /// <returns>文件的编码类型</returns>
        public static Encoding GetType(FileStream fs)
        {
            //var unicode = {0xFF, 0xFE, 0x41};
            //var unicodeBig = {0xFE, 0xFF, 0x00};
            //var utf8 = {0xEF, 0xBB, 0xBF}; //带BOM
            Encoding reVal = Encoding.Default;

            var r = new BinaryReader(fs, Encoding.Default);
            int i;
            int.TryParse(fs.Length.ToString(), out i);
            byte[] ss = r.ReadBytes(i);
            if (IsUtf8Bytes(ss) || ss[0] == 0xEF && ss[1] == 0xBB && ss[2] == 0xBF)
                reVal = Encoding.UTF8;
            else if (ss[0] == 0xFE && ss[1] == 0xFF && ss[2] == 0x00)
                reVal = Encoding.BigEndianUnicode;
            else if (ss[0] == 0xFF && ss[1] == 0xFE && ss[2] == 0x41)
                reVal = Encoding.Unicode;

            r.Close();

            return reVal;
        }

        /// <summary>
        /// 从csv读取数据返回table
        /// </summary>
        /// <param name="filePath">路径</param>
        /// <returns>DataTable</returns>
        public static DataTable OpenCsv(string filePath)
        {
            Encoding encoding = GetType(filePath); //Encoding.ASCII
            var dt = new DataTable();
            var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var sr = new StreamReader(fs, encoding);

            //记录每次读取的一行记录
            string strLine;
            //记录每行记录中的各字段内容
            string[] aryLine = null;
            string[] tableHead = null;
            //标示列数
            var columnCount = 0;
            //标示是否是读取的第一行
            var isFirst = true;
            //逐行读取CSV中的数据
            while ((strLine = sr.ReadLine()) != null)
                if (isFirst)
                {
                    tableHead = strLine.Split(',');
                    isFirst = false;
                    columnCount = tableHead.Length;
                    //创建列
                    for (var i = 0; i < columnCount; i++)
                    {
                        var dc = new DataColumn(tableHead[i]);
                        dt.Columns.Add(dc);
                    }
                }
                else
                {
                    aryLine = strLine.Split(',');
                    DataRow dr = dt.NewRow();
                    for (var j = 0; j < columnCount; j++)
                        dr[j] = aryLine[j];
                    dt.Rows.Add(dr);
                }

            if (aryLine != null && aryLine.Length > 0)
                dt.DefaultView.Sort = tableHead[0] + " " + "asc";

            sr.Close();
            fs.Close();

            return dt;
        }

        /// <summary>
        /// table数据写入csv
        /// </summary>
        /// <param name="dt">DataTable</param>
        /// <param name="fullPath">路径</param>
        public static void SaveCsv(DataTable dt, string fullPath)
        {
            var fi = new FileInfo(fullPath);
            var fs = new FileStream(fullPath, FileMode.Create, FileAccess.Write);
            var sw = new StreamWriter(fs, Encoding.UTF8);
            var data = "";

            if (fi.Directory != null && !fi.Directory.Exists)
                fi.Directory.Create();
            for (var i = 0; i < dt.Columns.Count; i++) //写入列名
            {
                data += dt.Columns[i].ColumnName;
                if (i < dt.Columns.Count - 1)
                    data += ",";
            }

            sw.WriteLine(data);

            for (var i = 0; i < dt.Rows.Count; i++) //写入各行数据
            {
                data = "";
                for (var j = 0; j < dt.Columns.Count; j++)
                {
                    string str = dt.Rows[i][j].ToString();
                    str = str.Replace("\"", "\"\"");                                                        //替换英文冒号 英文冒号需要换成两个冒号
                    if (str.Contains(',') || str.Contains('"') || str.Contains('\r') || str.Contains('\n')) //含逗号 冒号 换行符的需要放到引号中
                        str = $"\"{str}\"";

                    data += str;
                    if (j < dt.Columns.Count - 1)
                        data += ",";
                }

                sw.WriteLine(data);
            }

            sw.Close();
            fs.Close();
        }

        /// <summary>
        /// 直接数据写入csv
        /// </summary>
        /// <param name="data">直接数据</param>
        /// <param name="fullPath">路径</param>
        /// <returns>返回成功与否</returns>
        public static bool SaveCsv(string data, string fullPath)
        {
            var re = true;
            try
            {
                var fileStream = new FileStream(fullPath, FileMode.Append);
                var sw = new StreamWriter(fileStream, Encoding.UTF8);
                sw.WriteLine(data);
                //清空缓冲区
                sw.Flush();
                //关闭流
                sw.Close();
                fileStream.Close();
            }
            catch
            {
                re = false;
            }

            return re;
        }

        /// 判断是否是不带 BOM 的 UTF8 格式
        /// <param name="data"></param>
        /// <returns></returns>
        private static bool IsUtf8Bytes(IEnumerable<byte> data)
        {
            var charByteCounter = 1; //计算当前正分析的字符应还有的字节数
            foreach (byte t in data)
            {
                byte curByte = t; //当前分析的字节.
                if (charByteCounter == 1)
                {
                    if (curByte < 0x80)
                        continue;

                    //判断当前
                    while (((curByte <<= 1) & 0x80) != 0)
                        charByteCounter++;

                    //标记位首位若为非0 则至少以2个1开始 如:110XXXXX...........1111110X　
                    if (charByteCounter == 1 || charByteCounter > 6)
                        return false;
                }
                else
                {
                    //若是UTF-8 此时第一位必须为1
                    if ((curByte & 0xC0) != 0x80)
                        return false;

                    charByteCounter--;
                }
            }

            if (charByteCounter > 1)
                throw new Exception("非预期的byte格式");

            return true;
        }
        #endregion
    }
}
