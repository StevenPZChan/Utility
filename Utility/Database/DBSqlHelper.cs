using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Utility.Database
{
    /// <summary>
    /// SQL数据库辅助类
    /// </summary>
    public class DbSqlHelper
    {
        #region Methods
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(DataTable dt, string sSql, ref string sErr) => AdapterSaveData(DbConn.GetSqlConn(), dt, sSql, ref sErr);

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(SqlConnection oConn, DataTable dt, string sSql, ref string sErr)
        {
            try
            {
                SqlDataAdapter oda = GetAdapter(oConn, sSql);
                var _ = new SqlCommandBuilder(oda) {ConflictOption = ConflictOption.OverwriteChanges, SetAllValues = false};
                oda.Update(dt);
                return true;
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
                return false;
            }
            finally
            {
                oConn.Close();
            }
        }

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="dicDt">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(Dictionary<string, DataTable> dicDt, ref string sErr) =>
            AdapterSaveMastData(DbConn.GetSqlConn(), dicDt, ref sErr);

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="dicDt">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(SqlConnection oConn, Dictionary<string, DataTable> dicDt, ref string sErr)
        {
            bool result;
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("ColSda", typeof(SqlDataAdapter)));
            dt.Columns.Add(new DataColumn("ColScb", typeof(SqlCommandBuilder)));
            dt.Columns.Add(new DataColumn("ColSQL", typeof(string)));
            dt.Columns.Add(new DataColumn("ColDt", typeof(DataTable)));
            foreach (KeyValuePair<string, DataTable> dic in dicDt)
            {
                var oda = new SqlDataAdapter(dic.Key, oConn) {MissingSchemaAction = MissingSchemaAction.AddWithKey};
                var ocb = new SqlCommandBuilder(oda) {ConflictOption = ConflictOption.OverwriteChanges, SetAllValues = false};
                DataRow dr = dt.NewRow();
                dr["ColSda"] = oda;
                dr["ColScb"] = ocb;
                dr["ColSQL"] = dic.Key;
                dr["ColDt"] = dic.Value;
                dt.Rows.Add(dr);
            }

            if (oConn.State == ConnectionState.Closed)
                oConn.Open();
            SqlTransaction trans = oConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var oda = dr["ColSda"] as SqlDataAdapter;

                    if (oda == null)
                        continue;

                    oda.SelectCommand.Transaction = trans;
                    var upData = dr["ColDt"] as DataTable;
                    if (upData != null)
                        oda.Update(upData);
                }

                trans.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                sErr = ex.Message;
                result = false;
            }
            finally
            {
                oConn.Close();
            }

            return result;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">SQL数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(string sProcName, SqlParameter[] arrPara, ref string sErr) =>
            ExecProcedre(DbConn.GetSqlConn(), sProcName, arrPara, ref sErr);

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">SQL数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(SqlConnection oConn, string sProcName, SqlParameter[] arrPara, ref string sErr)
        {
            try
            {
                var comm = new SqlCommand(sProcName, oConn) {CommandType = CommandType.StoredProcedure};
                if (arrPara != null)
                    comm.Parameters.AddRange(arrPara);
                if (oConn.State != ConnectionState.Open)
                    oConn.Open();
                comm.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                oConn.Close();
            }
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">SQL数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable ExecProcFillData(string sProcName, SqlParameter[] arrPara, ref string sErr) =>
            ExecProcFillData(DbConn.GetSqlConn(), sProcName, arrPara, ref sErr);

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">SQL数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable ExecProcFillData(SqlConnection oConn, string sProcName, SqlParameter[] arrPara, ref string sErr)
        {
            var dt = new DataTable();
            try
            {
                var cmd = new SqlCommand(sProcName, oConn) {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.AddRange(arrPara);
                var oda = new SqlDataAdapter(cmd);
                oda.Fill(dt);
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                oConn.Close();
            }

            return dt;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(string sSql, ref string sErr) => ExecSql(DbConn.GetSqlConn(), sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(SqlConnection oConn, string sSql, ref string sErr)
        {
            var cmd = new SqlCommand(sSql, oConn);
            try
            {
                if (oConn.State != ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="aPara">SQL数据库参数</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(SqlParameter[] aPara, string sSql, ref string sErr) => ExecSql(DbConn.GetSqlConn(), aPara, sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="aPara">SQL数据库参数</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(SqlConnection oConn, SqlParameter[] aPara, string sSql, ref string sErr)
        {
            var cmd = new SqlCommand(sSql, oConn);
            try
            {
                cmd.Parameters.AddRange(aPara);
                if (oConn.State != ConnectionState.Open)
                    cmd.Connection.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSqlScalar(string sSql, ref string sErr) => ExecSqlScalar(DbConn.GetSqlConn(), sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSqlScalar(SqlConnection oConn, string sSql, ref string sErr)
        {
            object result = null;
            var cmd = new SqlCommand(sSql, oConn);
            try
            {
                if (oConn.State != ConnectionState.Open)
                    cmd.Connection.Open();
                result = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                cmd.Connection.Close();
            }

            return result;
        }

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="sSql">查询语句</param>
        /// <returns>SQL适配器</returns>
        public static SqlDataAdapter GetAdapter(string sSql) => GetAdapter(DbConn.GetSqlConn(), sSql);

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSql">查询语句</param>
        /// <returns>SQL适配器</returns>
        public static SqlDataAdapter GetAdapter(SqlConnection oConn, string sSql)
        {
            var oda = new SqlDataAdapter(sSql, oConn) {MissingSchemaAction = MissingSchemaAction.AddWithKey, ContinueUpdateOnError = true};

            return oda;
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(string sSql, ref string sErr) => GetDataTable(DbConn.GetSqlConn(), sSql, ref sErr);

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(SqlConnection oConn, string sSql, ref string sErr)
        {
            var dt = new DataTable();
            SqlDataAdapter oda = GetAdapter(oConn, sSql);
            try
            {
                oda.Fill(dt);
                foreach (DataColumn col in dt.Columns)
                    col.ReadOnly = false;
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                oConn.Close();
            }

            return dt;
        }
        #endregion
    }
}
