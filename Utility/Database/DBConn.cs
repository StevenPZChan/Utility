#define ORACLE

using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
#if ORACLE
using Oracle.ManagedDataAccess.Client;

#endif

namespace Utility.Database
{
    /// <summary>
    /// 数据库连接字符串类
    /// </summary>
    public static class DbConn
    {
        #region Methods
        /// <summary>
        /// 从配置文件获取连接字符串
        /// </summary>
        /// <param name="sConnName">配置名</param>
        /// <returns>连接字符串</returns>
        public static string GetConnStr(string sConnName) => ConfigurationManager.ConnectionStrings[sConnName].ConnectionString;

#if ORACLE
        /// <summary>
        /// 从配置文件生成 连接
        /// </summary>
        /// <returns>Oracle连接</returns>
        public static OracleConnection GetElecConn() => GetOrcConn(ConfigurationManager.ConnectionStrings["ELECConn"].ConnectionString);
#endif
#if ORACLE
        /// <summary>
        /// 从配置文件生成玖坤连接
        /// </summary>
        /// <returns>Oracle连接</returns>
        public static OracleConnection GetJkConn() => GetOrcConn(ConfigurationManager.ConnectionStrings["JKDBConn"].ConnectionString);
#endif

        /// <summary>
        /// 生成OleDb连接
        /// </summary>
        /// <param name="sConn">连接字符串</param>
        /// <returns>OleDb连接</returns>
        public static OleDbConnection GetOleConn(string sConn) => new OleDbConnection(sConn);

        /// <summary>
        /// 从配置文件生成OleDb连接
        /// </summary>
        /// <returns>OleDb连接</returns>
        public static OleDbConnection GetOleConn() => GetOleConn(ConfigurationManager.ConnectionStrings["OleConn"].ConnectionString);
#if ORACLE
        /// <summary>
        /// 生成Oracle连接
        /// </summary>
        /// <param name="sConn">连接字符串</param>
        /// <returns>Oracle连接</returns>
        public static OracleConnection GetOrcConn(string sConn) => new OracleConnection(sConn);
#endif
#if ORACLE
        /// <summary>
        /// 从配置文件生成Oracle连接
        /// </summary>
        /// <returns>Oracle连接</returns>
        public static OracleConnection GetOrcConn() => GetOrcConn(ConfigurationManager.ConnectionStrings["DBConn"].ConnectionString);
#endif

        /// <summary>
        /// 生成SQL连接
        /// </summary>
        /// <param name="sConn">连接字符串</param>
        /// <returns>SQL连接</returns>
        public static SqlConnection GetSqlConn(string sConn) => new SqlConnection(sConn);

        /// <summary>
        /// 从配置文件生成SQL连接
        /// </summary>
        /// <returns>SQL连接</returns>
        public static SqlConnection GetSqlConn() => GetSqlConn(ConfigurationManager.ConnectionStrings["SQLConn"].ConnectionString);
        #endregion
    }
}
