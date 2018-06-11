using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace Utility.Database
{
    /// <summary>
    /// OleDb数据库辅助类
    /// </summary>
    public class DBOleHelper
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSQLScalar(string sSQL, ref string sErr)
        {
            return ExecSQLScalar(DBConn.GetOleConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSQLScalar(OleDbConnection oConn, string sSQL, ref string sErr)
        {
            object result = null;
            OleDbCommand cmd = new OleDbCommand(sSQL, oConn);
            try
            {
                if (oConn.State != ConnectionState.Open) { cmd.Connection.Open(); }
                result = cmd.ExecuteScalar();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                sErr = ex.Message;
            }
            return result;
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(string sSQL, ref string sErr)
        {
            ExecSQL(DBConn.GetOleConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(OleDbConnection oConn, string sSQL, ref string sErr)
        {
            OleDbCommand cmd = new OleDbCommand(sSQL, oConn);
            try
            {
                if (oConn.State != ConnectionState.Open) { cmd.Connection.Open(); }
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                sErr = ex.Message;
            }
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="aPara">OleDb数据库参数</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(OleDbParameter[] aPara, string sSQL, ref string sErr)
        {
            ExecSQL(DBConn.GetOleConn(), aPara, sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="aPara">SQL数据库参数</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(OleDbConnection oConn, OleDbParameter[] aPara, string sSQL, ref string sErr)
        {
            OleDbCommand cmd = new OleDbCommand(sSQL, oConn);
            try
            {
                cmd.Parameters.AddRange(aPara);
                if (oConn.State != ConnectionState.Open) { cmd.Connection.Open(); }
                cmd.ExecuteNonQuery();
                cmd.Connection.Close();
            }
            catch (Exception ex)
            {
                cmd.Connection.Close();
                sErr = ex.Message;
            }
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(string sSQL, ref string sErr)
        {
            return GetDataTable(DBConn.GetOleConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(OleDbConnection oConn, string sSQL, ref string sErr)
        {
            DataTable dt = new DataTable();
            OleDbDataAdapter oda = GetAdapter(oConn, sSQL);
            try
            {
                oda.Fill(dt);
                foreach (DataColumn col in dt.Columns)
                {
                    col.ReadOnly = false;
                }
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            return dt;
        }


        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="sSQL">查询语句</param>
        /// <returns>OleDb适配器</returns>
        public static OleDbDataAdapter GetAdapter(string sSQL)
        {
            return GetAdapter(DBConn.GetOleConn(), sSQL);
        }

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="sSQL">查询语句</param>
        /// <returns>OleDb适配器</returns>
        public static OleDbDataAdapter GetAdapter(OleDbConnection oConn, string sSQL)
        {
            OleDbDataAdapter oda = new OleDbDataAdapter(sSQL, oConn);
            oda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oda.ContinueUpdateOnError = true;
            return oda;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">OleDb数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(string sProcName, OleDbParameter[] arrPara, ref string sErr)
        {
            ExecProcedre(DBConn.GetOleConn(), sProcName, arrPara, ref sErr);
        }

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
                OleDbCommand comm = new OleDbCommand(sProcName, oConn);
                comm.CommandType = CommandType.StoredProcedure;
                if (arrPara != null)
                {
                    comm.Parameters.AddRange(arrPara);
                }
                if (oConn.State != ConnectionState.Open)
                { oConn.Open(); }
                comm.ExecuteNonQuery();
                oConn.Close();
            }
            catch (Exception ex)
            {
                oConn.Close();
                sErr = ex.Message;
            }
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(DataTable dt, string sSQL, ref string sErr)
        {
            return AdapterSaveData(DBConn.GetOleConn(), dt, sSQL, ref sErr);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(OleDbConnection oConn, DataTable dt, string sSQL, ref string sErr)
        {
            try
            {
                OleDbDataAdapter oda = GetAdapter(oConn, sSQL);
                OleDbCommandBuilder scb = new OleDbCommandBuilder(oda);
                scb.QuotePrefix = "[";
                scb.QuoteSuffix = "]";
                scb.ConflictOption = ConflictOption.OverwriteChanges;
                scb.SetAllValues = false;
                oda.Update(dt);
                return true;
            }
            catch (Exception e)
            {
                sErr = e.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="dicDT">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(Dictionary<string, DataTable> dicDT, ref string sErr)
        {
            return AdapterSaveMastData(DBConn.GetOleConn(), dicDT, ref sErr);
        }

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="oConn">OleDb连接</param>
        /// <param name="dicDT">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(OleDbConnection oConn, Dictionary<string, DataTable> dicDT, ref string sErr)
        {
            bool result = false;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ColSda", typeof(OleDbDataAdapter)));
            dt.Columns.Add(new DataColumn("ColScb", typeof(OleDbCommandBuilder)));
            dt.Columns.Add(new DataColumn("ColSQL", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ColDt", typeof(DataTable)));
            foreach (KeyValuePair<string, DataTable> dic in dicDT)
            {
                OleDbDataAdapter oda = new OleDbDataAdapter(dic.Key, oConn);
                oda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                OleDbCommandBuilder ocb = new OleDbCommandBuilder(oda);
                ocb.ConflictOption = ConflictOption.OverwriteChanges;
                ocb.SetAllValues = false;
                DataRow dr = dt.NewRow();
                dr["ColSda"] = oda;
                dr["ColScb"] = ocb;
                dr["ColSQL"] = dic.Key;
                dr["ColDt"] = dic.Value;
                dt.Rows.Add(dr);
            }
            if (oConn.State == ConnectionState.Closed)
            {
                oConn.Open();
            }
            OleDbTransaction trans = oConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OleDbDataAdapter oda = dr["ColSda"] as OleDbDataAdapter;
                    oda.SelectCommand.Transaction = trans;
                    DataTable upData = dr["ColDt"] as DataTable;
                    oda.Update(upData);
                }
                trans.Commit();
                result = true;
            }
            catch (Exception ex)
            {
                trans.Rollback();
                oConn.Close();
                sErr = ex.Message;
                result = false;
            }
            return result;
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
            DataTable xlsTable = oConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables,
                new object[] { null, null, null, "TABLE" });

            bool sheetExist = false;
            foreach (DataRow sheetRow in xlsTable.Rows)
            {
                if (sheetRow["TABLE_NAME"].ToString() == sheetName)
                {
                    sheetExist = true;

                    break;
                }
            }

            return sheetExist;
        }
    }
}