using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Utility.Database
{
    /// <summary>
    /// OleDb数据库辅助类
    /// </summary>
    public class DbOleHelper
    {
        #region Methods
        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(DataTable dt, string sSql, ref string sErr) => AdapterSaveData(DbConn.GetOleConn(), dt, sSql, ref sErr);

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(OleDbConnection oConn, DataTable dt, string sSql, ref string sErr)
        {
            try
            {
                OleDbDataAdapter oda = GetAdapter(oConn, sSql);
                var _ = new OleDbCommandBuilder(oda)
                    {QuotePrefix = "[", QuoteSuffix = "]", ConflictOption = ConflictOption.OverwriteChanges, SetAllValues = false};
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
            AdapterSaveMastData(DbConn.GetOleConn(), dicDt, ref sErr);

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="dicDt">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(OleDbConnection oConn, Dictionary<string, DataTable> dicDt, ref string sErr)
        {
            bool result;
            var dt = new DataTable();
            dt.Columns.Add(new DataColumn("ColSda", typeof(OleDbDataAdapter)));
            dt.Columns.Add(new DataColumn("ColScb", typeof(OleDbCommandBuilder)));
            dt.Columns.Add(new DataColumn("ColSQL", typeof(string)));
            dt.Columns.Add(new DataColumn("ColDt", typeof(DataTable)));
            foreach (KeyValuePair<string, DataTable> dic in dicDt)
            {
                var oda = new OleDbDataAdapter(dic.Key, oConn) {MissingSchemaAction = MissingSchemaAction.AddWithKey};
                var ocb = new OleDbCommandBuilder(oda) {ConflictOption = ConflictOption.OverwriteChanges, SetAllValues = false};
                DataRow dr = dt.NewRow();
                dr["ColSda"] = oda;
                dr["ColScb"] = ocb;
                dr["ColSQL"] = dic.Key;
                dr["ColDt"] = dic.Value;
                dt.Rows.Add(dr);
            }

            if (oConn.State == ConnectionState.Closed)
                oConn.Open();
            OleDbTransaction trans = oConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var oda = dr["ColSda"] as OleDbDataAdapter;

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
        /// <param name="arrPara">OleDb数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(string sProcName, OleDbParameter[] arrPara, ref string sErr) =>
            ExecProcedre(DbConn.GetOleConn(), sProcName, arrPara, ref sErr);

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">OleDb数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(OleDbConnection oConn, string sProcName, OleDbParameter[] arrPara, ref string sErr)
        {
            try
            {
                var comm = new OleDbCommand(sProcName, oConn) {CommandType = CommandType.StoredProcedure};
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
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(string sSql, ref string sErr) => ExecSql(DbConn.GetOleConn(), sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(OleDbConnection oConn, string sSql, ref string sErr)
        {
            var cmd = new OleDbCommand(sSql, oConn);
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
        /// <param name="aPara">OleDb数据库参数</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(OleDbParameter[] aPara, string sSql, ref string sErr) => ExecSql(DbConn.GetOleConn(), aPara, sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="aPara">SQL数据库参数</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSql(OleDbConnection oConn, OleDbParameter[] aPara, string sSql, ref string sErr)
        {
            var cmd = new OleDbCommand(sSql, oConn);
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
        public static object ExecSqlScalar(string sSql, ref string sErr) => ExecSqlScalar(DbConn.GetOleConn(), sSql, ref sErr);

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSql">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSqlScalar(OleDbConnection oConn, string sSql, ref string sErr)
        {
            object result = null;
            var cmd = new OleDbCommand(sSql, oConn);
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
        /// <returns>OleDb适配器</returns>
        public static OleDbDataAdapter GetAdapter(string sSql) => GetAdapter(DbConn.GetOleConn(), sSql);

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSql">查询语句</param>
        /// <returns>OleDb适配器</returns>
        public static OleDbDataAdapter GetAdapter(OleDbConnection oConn, string sSql)
        {
            var oda = new OleDbDataAdapter(sSql, oConn) {MissingSchemaAction = MissingSchemaAction.AddWithKey, ContinueUpdateOnError = true};

            return oda;
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(string sSql, ref string sErr) => GetDataTable(DbConn.GetOleConn(), sSql, ref sErr);

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSql">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(OleDbConnection oConn, string sSql, ref string sErr)
        {
            var dt = new DataTable();
            OleDbDataAdapter oda = GetAdapter(oConn, sSql);
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

        /// <summary>
        /// 查询Excel表格中是否含有某张表
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sheetName">表名</param>
        /// <returns></returns>
        public static bool IfSheetExist(OleDbConnection oConn, string sheetName)
        {
            // 获得excel文件的各个表单(sheet)的名字
            DataTable xlsTable = oConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] {null, null, null, "TABLE"});

            var sheetExist = false;

            if (xlsTable?.Rows == null)
                return false;

            foreach (DataRow sheetRow in xlsTable.Rows)
                if (sheetRow["TABLE_NAME"].ToString() == sheetName)
                {
                    sheetExist = true;
                    break;
                }

            return sheetExist;
        }
        #endregion
    }
}
