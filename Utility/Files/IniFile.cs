using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Utility.Files
{
    /// <summary>
    /// ini文件操作类
    /// </summary>
    public class IniFile
    {
        #region Fields
        private readonly string path;
        #endregion

        #region Constructors
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="path">文件路径</param>
        public IniFile(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// 析构IniFile实例。
        /// </summary>
        ~IniFile()
        {
            UpdateFile();
        }
        #endregion

        #region Methods
        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <returns>返回的键值</returns>
        public string IniReadValue(string section, string key)
        {
            if (!File.Exists(this.path))
                return "";

            var temp = new StringBuilder(255);

            GetPrivateProfileString(section, key, "", temp, 255, this.path);

            return temp.ToString();
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <param name="def">读取为空时的默认值</param>
        /// <returns>返回的键值</returns>
        public string IniReadValue(string section, string key, string def)
        {
            string str = IniReadValue(section, key);

            return str == "" ? def : str;
        }

        /// <summary>
        /// 读取INI文件
        /// </summary>
        /// <param name="section">段，格式[]</param>
        /// <param name="key">键</param>
        /// <returns>返回byte类型的section组或键值组</returns>
        public byte[] IniReadValues(string section, string key)
        {
            if (!File.Exists(this.path))
                return null;

            var temp = new byte[255];

            GetPrivateProfileString(section, key, "", temp, 255, this.path);

            return temp;
        }

        /// <summary>
        /// 写INI文件
        /// </summary>
        /// <param name="section">段落</param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public void IniWriteValue(string section, string key, string value) => WritePrivateProfileString(section, key, value, this.path);

        /// <summary>
        /// 更新文件。
        /// </summary>
        /// <returns>返回更新是否成功。</returns>
        public bool UpdateFile() => WritePrivateProfileString(null, null, null, this.path);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, StringBuilder retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defVal, byte[] retVal, int size, string filePath);

        [DllImport("kernel32")]
        private static extern bool WritePrivateProfileString(string section, string key, string val, string filePath);
        #endregion
    }
}
