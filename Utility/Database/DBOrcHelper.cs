using System;
using System.Collections.Generic;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace Utility.Database
{
    /// <summary>
    /// Oracle数据库辅助类
    /// </summary>
    public class DBOrcHelper
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSQLScalar(string sSQL, ref string sErr)
        {
            return ExecSQLScalar(DBConn.GetOrcConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSQLScalar(OracleConnection oConn, string sSQL, ref string sErr)
        {
            object result = null;
            OracleCommand cmd = new OracleCommand(sSQL, oConn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }
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
            ExecSQL(DBConn.GetOrcConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(OracleConnection oConn, string sSQL, ref string sErr)
        {
            OracleCommand cmd = new OracleCommand(sSQL, oConn);
            try
            {
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }
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
        /// <param name="aPara">Oracle数据库参数</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(OracleParameter[] aPara, string sSQL, ref string sErr)
        {
            ExecSQL(DBConn.GetOrcConn(), aPara, sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="aPara">Oracle数据库参数</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(OracleConnection oConn, OracleParameter[] aPara, string sSQL, ref string sErr)
        {
            OracleCommand cmd = new OracleCommand(sSQL, oConn);
            try
            {
                cmd.Parameters.AddRange(aPara);
                if (cmd.Connection.State != ConnectionState.Open) { cmd.Connection.Open(); }
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
            return GetDataTable(DBConn.GetOrcConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(OracleConnection oConn, string sSQL, ref string sErr)
        {
            DataTable dt = new DataTable();
            OracleDataAdapter oda = DBOrcHelper.GetAdapter(oConn, sSQL);
            try
            {
                oda.Fill(dt);
                oConn.Close();
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
        /// <returns>Oracle适配器</returns>
        public static OracleDataAdapter GetAdapter(string sSQL)
        {
            return GetAdapter(DBConn.GetOrcConn(), sSQL);
        }

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="sSQL">查询语句</param>
        /// <returns>Oracle适配器</returns>
        public static OracleDataAdapter GetAdapter(OracleConnection oConn, string sSQL)
        {
            OracleDataAdapter oda = new OracleDataAdapter(sSQL, oConn);
            oda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            return oda;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(string sProcName, OracleParameter[] arrPara, ref string sErr)
        {
            ExecProcedre(DBConn.GetOrcConn(), sProcName, arrPara, ref sErr);
        }

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
                OracleCommand comm = new OracleCommand(sProcName, oConn);
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
        /// 查询得到数据表
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">Oracle数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable ExecProcFillData(string sProcName, OracleParameter[] arrPara, ref string sErr)
        {
            return ExecProcFillData(DBConn.GetOrcConn(), sProcName, arrPara, ref sErr);
        }

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
            DataTable dt = new DataTable("");
            try
            {
                OracleCommand cmd = new OracleCommand(sProcName, oConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(arrPara);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                oConn.Close();
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
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
        public static DataSet ExecProcFillDataSet(string sProcName, OracleParameter[] arrPara, ref string sErr)
        {
            return ExecProcFillDataSet(DBConn.GetOrcConn(), sProcName, arrPara, ref sErr);
        }

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
            DataSet ds = new DataSet();
            try
            {
                OracleCommand cmd = new OracleCommand(sProcName, oConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(arrPara);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
                oConn.Close();
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            return ds;
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
            return AdapterSaveData(DBConn.GetOrcConn(), dt, sSQL, ref sErr);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(OracleConnection oConn, DataTable dt, string sSQL, ref string sErr)
        {
            try
            {
                OracleDataAdapter oda = DBOrcHelper.GetAdapter(oConn, sSQL);
                OracleCommandBuilder scb = new OracleCommandBuilder(oda);
                scb.ConflictOption = ConflictOption.OverwriteChanges;
                scb.SetAllValues = false;
                oda.Update(dt);
                oConn.Close();
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
            return AdapterSaveMastData(DBConn.GetOrcConn(), dicDT, ref sErr);
        }

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="oConn">Oracle连接</param>
        /// <param name="dicDT">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(OracleConnection oConn, Dictionary<string, DataTable> dicDT, ref string sErr)
        {
            bool result = false;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ColSda", typeof(OracleDataAdapter)));
            dt.Columns.Add(new DataColumn("ColScb", typeof(OracleCommandBuilder)));
            dt.Columns.Add(new DataColumn("ColSQL", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ColDt", typeof(DataTable)));
            foreach (KeyValuePair<string, DataTable> dic in dicDT)
            {
                OracleDataAdapter oda = new OracleDataAdapter(dic.Key, oConn);
                oda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                OracleCommandBuilder ocb = new OracleCommandBuilder(oda);
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
            OracleTransaction trans = oConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    OracleDataAdapter oda = dr["ColSda"] as OracleDataAdapter;
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
    }
}