using System;
using System.Collections.Generic;
using System.Data;

using Oracle.ManagedDataAccess.Client;

namespace Utility.Database
{
    /// <summary>
    /// Oracle数据库辅助类
    /// </summary>
    public class DbOrcHelper
    {
        #region Methods
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(DataTable dt, string sSql, ref string sErr) => AdapterSaveData(DbConn.GetOrcConn(), dt, sSql, ref sErr);

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(OracleConnection oConn, DataTable dt, string sSql, ref string sErr)
        {
            try
            {
                OracleDataAdapter oda = GetAdapter(oConn, sSql);
                var _ = new OracleCommandBuilder(oda) {ConflictOption = ConflictOption.OverwriteChanges, SetAllValues = false};
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
            AdapterSaveMastData(DbConn.GetOrcConn(), dicDt, ref sErr);

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="dicDt">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(OracleConnection oConn, Dictionary<string, DataTable> dicDt, ref string sErr)
        {
            bool result;
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("ColSda", typeof(OracleDataAdapter)));
            dt.Columns.Add(new DataColumn("ColScb", typeof(OracleCommandBuilder)));
            dt.Columns.Add(new DataColumn("ColSQL", typeof(string)));
            dt.Columns.Add(new DataColumn("ColDt", typeof(DataTable)));
            foreach (KeyValuePair<string, DataTable> dic in dicDt)
            {
                var oda = new OracleDataAdapter(dic.Key, oConn) {MissingSchemaAction = MissingSchemaAction.AddWithKey};
                var ocb = new OracleCommandBuilder(oda) {ConflictOption = ConflictOption.OverwriteChanges, SetAllValues = false};
                DataRow dr = dt.NewRow();
                dr["ColSda"] = oda;
                dr["ColScb"] = ocb;
                dr["ColSQL"] = dic.Key;
                dr["ColDt"] = dic.Value;
                dt.Rows.Add(dr);
            }

            if (oConn.State == ConnectionState.Closed)
                oConn.Open();
            OracleTransaction trans = oConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var oda = dr["ColSda"] as OracleDataAdapter;

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
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(string sProcName, OracleParameter[] arrPara, ref string sErr) =>
            ExecProcedre(DbConn.GetOrcConn(), sProcName, arrPara, ref sErr);

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(OracleConnection oConn, string sProcName, OracleParameter[] arrPara, ref string sErr)
        {
            try
            {
                var comm = new OracleCommand(sProcName, oConn) {CommandType = CommandType.StoredProcedure};
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
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable ExecProcFillData(string sProcName, OracleParameter[] arrPara, ref string sErr) =>
            ExecProcFillData(DbConn.GetOrcConn(), sProcName, arrPara, ref sErr);

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable ExecProcFillData(OracleConnection oConn, string sProcName, OracleParameter[] arrPara, ref string sErr)
        {
            var dt = new DataTable("");
            try
            {
                var cmd = new OracleCommand(sProcName, oConn) {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.AddRange(arrPara);
                var oda = new OracleDataAdapter(cmd);
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
        /// 查询得到数据集
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据集</returns>
        public static DataSet ExecProcFillDataSet(string sProcName, OracleParameter[] arrPara, ref string sErr) =>
            ExecProcFillDataSet(DbConn.GetOrcConn(), sProcName, arrPara, ref sErr);

        /// <summary>
        /// 查询得到数据集
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据集</returns>
        public static DataSet ExecProcFillDataSet(OracleConnection oConn, string sProcName, OracleParameter[] arrPara, ref string sErr)
        {
            var ds = new DataSet();
            try
            {
                var cmd = new OracleCommand(sProcName, oConn) {CommandType = CommandType.StoredProcedure};
                cmd.Parameters.AddRange(arrPara);
                var oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            finally
            {
                oConn.Close();
            }

            return ds;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(string sSql, ref string sErr) => ExecSql(DbConn.GetOrcConn(), sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(OracleConnection oConn, string sSql, ref string sErr)
        {
            var cmd = new OracleCommand(sSql, oConn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
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
        /// <param name="aPara">Oracle数据库参数</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(OracleParameter[] aPara, string sSql, ref string sErr) => ExecSql(DbConn.GetOrcConn(), aPara, sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="aPara">Oracle数据库参数</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(OracleConnection oConn, OracleParameter[] aPara, string sSql, ref string sErr)
        {
            var cmd = new OracleCommand(sSql, oConn);
            try
            {
                cmd.Parameters.AddRange(aPara);
                if (cmd.Connection.State != ConnectionState.Open)
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
        public static object ExecSqlScalar(string sSql, ref string sErr) => ExecSqlScalar(DbConn.GetOrcConn(), sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSqlScalar(OracleConnection oConn, string sSql, ref string sErr)
        {
            object result = null;
            var cmd = new OracleCommand(sSql, oConn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open)
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
        /// <returns>Oracle适配器</returns>
        public static OracleDataAdapter GetAdapter(string sSql) => GetAdapter(DbConn.GetOrcConn(), sSql);

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSql">查询语句</param>
        /// <returns>Oracle适配器</returns>
        public static OracleDataAdapter GetAdapter(OracleConnection oConn, string sSql)
        {
            var oda = new OracleDataAdapter(sSql, oConn) {MissingSchemaAction = MissingSchemaAction.AddWithKey};

            return oda;
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(string sSql, ref string sErr) => GetDataTable(DbConn.GetOrcConn(), sSql, ref sErr);

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(OracleConnection oConn, string sSql, ref string sErr)
        {
            var dt = new DataTable();
            OracleDataAdapter oda = GetAdapter(oConn, sSql);
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
