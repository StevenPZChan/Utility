#define ORACLE
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;

namespace Utility.Database
{
    /// <summary>
    /// 数据库连接字符串类
    /// </summary>
    public class DBConn
    {

        /// <summary>
        /// 从配置文件获取连接字符串
        /// </summary>
        /// <param name="sConnName">配置名</param>
        /// <returns>连接字符串</returns>
        public static string GetConnStr(string sConnName)
        {
            return ConfigurationManager.ConnectionStrings[sConnName].ConnectionString;
        }

#if ORACLE
        /// <summary>
        /// 生成Oracle连接
        /// </summary>
        /// <param name="sConn">连接字符串</param>
        /// <returns>Oracle连接</returns>
        public static Oracle.ManagedDataAccess.Client.OracleConnection GetOrcConn(string sConn)
        {
            return new Oracle.ManagedDataAccess.Client.OracleConnection(sConn);
        }

        /// <summary>
        /// 从配置文件生成Oracle连接
        /// </summary>
        /// <returns>Oracle连接</returns>
        public static Oracle.ManagedDataAccess.Client.OracleConnection GetOrcConn()
        {
            return GetOrcConn(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
        }

        /// <summary>
        /// 从配置文件生成玖坤连接
        /// </summary>
        /// <returns>Oracle连接</returns>
        public static Oracle.ManagedDataAccess.Client.OracleConnection GetJKConn()
        {
            return GetOrcConn(ConfigurationManager.ConnectionStrings["JKDBConn"].ConnectionString);
        }

        /// <summary>
        /// 从配置文件生成  连接
        /// </summary>
        /// <returns>Oracle连接</returns>
        public static Oracle.ManagedDataAccess.Client.OracleConnection GetELECConn()
        {
            return GetOrcConn(ConfigurationManager.ConnectionStrings["ELECConn"].ConnectionString);
        }
#endif

        /// <summary>
        /// 生成SQL连接
        /// </summary>
        /// <param name="sConn">连接字符串</param>
        /// <returns>SQL连接</returns>
        public static SqlConnection GetSqlConn(string sConn)
        {
            return new SqlConnection(sConn);
        }

        /// <summary>
        /// 从配置文件生成SQL连接
        /// </summary>
        /// <returns>SQL连接</returns>
        public static SqlConnection GetSqlConn()
        {
            return GetSqlConn(ConfigurationManager.ConnectionStrings["SQLConn"].ConnectionString);
        }

        /// <summary>
        /// 生成OleDb连接
        /// </summary>
        /// <param name="sConn">连接字符串</param>
        /// <returns>OleDb连接</returns>
        public static OleDbConnection GetOleConn(string sConn)
        {
            return new OleDbConnection(sConn);
        }

        /// <summary>
        /// 从配置文件生成OleDb连接
        /// </summary>
        /// <returns>OleDb连接</returns>
        public static OleDbConnection GetOleConn()
        {
            return GetOleConn(ConfigurationManager.ConnectionStrings["OleConn"].ConnectionString);
        }
    }
}