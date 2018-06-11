using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace Utility.Database
{
    /// <summary>
    /// SQL数据库辅助类
    /// </summary>
    public class DBSqlHelper
    {
        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSQLScalar(string sSQL, ref string sErr)
        {
            return ExecSQLScalar(DBConn.GetSqlConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>查询得到的第一行第一列</returns>
        public static object ExecSQLScalar(SqlConnection oConn, string sSQL, ref string sErr)
        {
            object result = null;
            SqlCommand cmd = new SqlCommand(sSQL, oConn);
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
            ExecSQL(DBConn.GetSqlConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(SqlConnection oConn, string sSQL, ref string sErr)
        {
            SqlCommand cmd = new SqlCommand(sSQL, oConn);
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
        /// <param name="aPara">SQL数据库参数</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(SqlParameter[] aPara, string sSQL, ref string sErr)
        {
            ExecSQL(DBConn.GetSqlConn(), aPara, sSQL, ref sErr);
        }

        /// <summary>
        /// 执行SQL语句
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="aPara">SQL数据库参数</param>
        /// <param name="sSQL">SQL语句</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecSQL(SqlConnection oConn, SqlParameter[] aPara, string sSQL, ref string sErr)
        {
            SqlCommand cmd = new SqlCommand(sSQL, oConn);
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
            return GetDataTable(DBConn.GetSqlConn(), sSQL, ref sErr);
        }

        /// <summary>
        /// 查询得到数据表
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable GetDataTable(SqlConnection oConn, string sSQL, ref string sErr)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter oda = GetAdapter(oConn, sSQL);
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
        /// <returns>SQL适配器</returns>
        public static SqlDataAdapter GetAdapter(string sSQL)
        {
            return GetAdapter(DBConn.GetSqlConn(), sSQL);
        }

        /// <summary>
        /// 得到连接适配器
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="sSQL">查询语句</param>
        /// <returns>SQL适配器</returns>
        public static SqlDataAdapter GetAdapter(SqlConnection oConn, string sSQL)
        {
            SqlDataAdapter oda = new SqlDataAdapter(sSQL, oConn);
            oda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
            oda.ContinueUpdateOnError = true;
            return oda;
        }

        /// <summary>
        /// 执行操作
        /// </summary>
        /// <param name="sProcName">操作字符串</param>
        /// <param name="arrPara">SQL数据库参数</param>
        /// <param name="sErr">错误信息</param>
        public static void ExecProcedre(string sProcName, SqlParameter[] arrPara, ref string sErr)
        {
            ExecProcedre(DBConn.GetSqlConn(), sProcName, arrPara, ref sErr);
        }

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
                SqlCommand comm = new SqlCommand(sProcName, oConn);
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
        /// <param name="arrPara">SQL数据库参数</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>数据表</returns>
        public static DataTable ExecProcFillData(string sProcName, SqlParameter[] arrPara, ref string sErr)
        {
            return ExecProcFillData(DBConn.GetSqlConn(), sProcName, arrPara, ref sErr);
        }

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
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand(sProcName, oConn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(arrPara);
                SqlDataAdapter oda = new SqlDataAdapter(cmd);
                oda.Fill(dt);
            }
            catch (Exception ex)
            {
                sErr = ex.Message;
            }
            return dt;
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
            return AdapterSaveData(DBConn.GetSqlConn(), dt, sSQL, ref sErr);
        }

        /// <summary>
        /// 更新数据库
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="dt">待同步数据表</param>
        /// <param name="sSQL">查询语句</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveData(SqlConnection oConn, DataTable dt, string sSQL, ref string sErr)
        {
            try
            {
                SqlDataAdapter oda = GetAdapter(oConn, sSQL);
                SqlCommandBuilder scb = new SqlCommandBuilder(oda);
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
            return AdapterSaveMastData(DBConn.GetSqlConn(), dicDT, ref sErr);
        }

        /// <summary>
        /// 更新大量数据
        /// </summary>
        /// <param name="oConn">SQL连接</param>
        /// <param name="dicDT">数据表字典</param>
        /// <param name="sErr">错误信息</param>
        /// <returns>是否成功更新</returns>
        public static bool AdapterSaveMastData(SqlConnection oConn, Dictionary<string, DataTable> dicDT, ref string sErr)
        {
            bool result = false;
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ColSda", typeof(SqlDataAdapter)));
            dt.Columns.Add(new DataColumn("ColScb", typeof(SqlCommandBuilder)));
            dt.Columns.Add(new DataColumn("ColSQL", typeof(System.String)));
            dt.Columns.Add(new DataColumn("ColDt", typeof(DataTable)));
            foreach (KeyValuePair<string, DataTable> dic in dicDT)
            {
                SqlDataAdapter oda = new SqlDataAdapter(dic.Key, oConn);
                oda.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                SqlCommandBuilder ocb = new SqlCommandBuilder(oda);
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
            SqlTransaction trans = oConn.BeginTransaction();
            try
            {
                foreach (DataRow dr in dt.Rows)
                {
                    SqlDataAdapter oda = dr["ColSda"] as SqlDataAdapter;
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