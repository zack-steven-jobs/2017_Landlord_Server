using System;
using System.Data.OleDb;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System.Data;
using System.Web.Configuration;

public class MySqlDb
{
    private Dictionary<string, int> m_mapDbServer = new Dictionary<string, int>();
    private MySqlDbServer[] m_server;

    public MySqlDb()
    {
        int count = Convert.ToInt32(WebConfigurationManager.AppSettings["mySqlServerCount"]);
        m_server = new MySqlDbServer[count];

        for (int i = 0; i < count; i++)
        {
            string str = string.Format("mysql_{0}", i + 1);
            m_server[i] = new MySqlDbServer(WebConfigurationManager.AppSettings[str]);
            m_mapDbServer.Add(m_server[i].serverIp, i);
        }
    }

    public MySqlDbServer getServer(int index)
    {
        return m_server[index];
    }

    // 查询
    public MySqlDataReader startQuery(string sql, int serverIndex, int dbName)
    {
        try
        {
            return m_server[serverIndex].startQuery(sql, dbName);
        }
        catch (System.Exception ex)
        {
        }
        return null;
    }

    // 查询一条记录
    public Dictionary<string, object> queryOne(string sql, int serverIndex, int dbName)
    {
        return m_server[serverIndex].queryOne(sql, dbName);
    }

    // 查询一组记录
    public List<Dictionary<string, object>> queryList(string sql, int serverIndex, int dbName)
    {
        return m_server[serverIndex].queryList(sql, dbName);
    }

    // 修改，删除，添加
    public int executeOp(string sql, int serverIndex, int dbName)
    {
        return m_server[serverIndex].executeOp(sql, dbName);
    }

    public int executeOpTran(string sql, int serverIndex, int dbName)
    {
        return m_server[serverIndex].executeOpTran(sql, dbName);
    }

    public void end(int serverIndex)
    {
        m_server[serverIndex].end();
    }

    public bool keyIntExists(string tableName, string field, object val, int serverIndex, int dbName)
    {
        return m_server[serverIndex].keyIntExists(tableName, field, val, dbName);
    }

    public bool keyStrExists(string tableName, string field, object val, int serverIndex, int dbName)
    {
        return m_server[serverIndex].keyStrExists(tableName, field, val, dbName);
    }

    public bool keyExists(string tableName, string sqlCondition, int serverIndex, int dbName)
    {
        return m_server[serverIndex].keyExists(tableName, sqlCondition, dbName);
    }

    // 返回记录个数
    public long getRecordCount(string tableName, string sqlCondition, int serverIndex, int dbName)
    {
        return m_server[serverIndex].getRecordCount(tableName, sqlCondition, dbName);
    }

    // 返回记录个数
    public long getRecordCountNoWhere(string tableName, string sqlCondition, int serverIndex, int dbName)
    {
        return m_server[serverIndex].getRecordCountNoWhere(tableName, sqlCondition, dbName);
    }

    public long getRecordCount(string sql, int serverIndex, int dbName)
    {
        return m_server[serverIndex].getRecordCount(sql, dbName);
    }
}

public struct MySqlDbName
{
    public const int DB_XIANXIA = 0;

    public const int DB_WORLD = 1;
}

// 一个Db服务器
public class MySqlDbServer
{
    private static string[] s_dbName = { "xianxia", "world" };

    private static string CONNECT_STR = WebConfigurationManager.AppSettings["connectStr"];

    private string m_serverIp = "";

    private MySqlConnection m_conn = null;

    public MySqlDbServer(string serverIp)
    {
        m_serverIp = serverIp;
    }

    public string serverIp
    {
        get { return m_serverIp; }
    }

    public string getConnStr(int dbName)
    {
        return string.Format(CONNECT_STR, m_serverIp, s_dbName[dbName]);
    }

    // 查询
    public MySqlDataReader startQuery(string sql, int dbName)
    {
        try
        {
            end();

            string connStr = string.Format(CONNECT_STR, m_serverIp, s_dbName[dbName]);
            m_conn = new MySqlConnection(connStr);
            m_conn.Open();

            // SQL命令执行器  
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.CommandText = sql;
            sqlCmd.CommandType = CommandType.Text;

            // 设置SQL命令执行器的连接  
            sqlCmd.Connection = m_conn;

            // SQL数据读取器  
            MySqlDataReader dataReader = sqlCmd.ExecuteReader();

            return dataReader;
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
            end();
        }
        return null;
    }

    // 查询一条记录
    public Dictionary<string, object> queryOne(string sql, int dbName)
    {
        Dictionary<string, object> retData = null;
        // SQL数据读取器  
        MySqlDataReader dataReader = startQuery(sql, dbName);
        if (dataReader != null)
        {
            if (dataReader.Read())
            {
                retData = new Dictionary<string, object>();
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    string key = dataReader.GetName(i);
                    object val = dataReader.GetValue(i);
                    retData.Add(key, val);
                }
            }
        }
        end();
        return retData;
    }

    // 查询一组记录
    public List<Dictionary<string, object>> queryList(string sql, int dbName)
    {
        List<Dictionary<string, object>> retList = new List<Dictionary<string, object>>();
        // SQL数据读取器  
        MySqlDataReader dataReader = startQuery(sql, dbName);
        if (dataReader != null)
        {
            while (dataReader.Read())
            {
                Dictionary<string, object> data = new Dictionary<string, object>();
                retList.Add(data);

                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    string key = dataReader.GetName(i);
                    object val = dataReader.GetValue(i);
                    data.Add(key, val);
                }
            }
        }
        end();
        return retList;
    }

    // 修改，删除，添加
    public int executeOp(string sql, int dbName)
    {
        int cnt = -1;
        try
        {
            string connStr = string.Format(CONNECT_STR, m_serverIp, s_dbName[dbName]);
            m_conn = new MySqlConnection(connStr);
            m_conn.Open();

            // SQL命令执行器  
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.CommandText = sql;
            sqlCmd.CommandType = CommandType.Text;

            // 设置SQL命令执行器的连接  
            sqlCmd.Connection = m_conn;

            cnt = sqlCmd.ExecuteNonQuery();
            end();
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
            end();
        }
        return cnt;
    }

    public int executeOpTran(string sql, int dbName)
    {
        int cnt = -1;
        try
        {
            string connStr = string.Format(CONNECT_STR, m_serverIp, s_dbName[dbName]);
            m_conn = new MySqlConnection(connStr);
            m_conn.Open();

            // SQL命令执行器  
            MySqlCommand sqlCmd = new MySqlCommand();
            sqlCmd.CommandText = sql;
            sqlCmd.CommandType = CommandType.Text;
            // 设置SQL命令执行器的连接  
            sqlCmd.Connection = m_conn;

            MySqlTransaction tran = m_conn.BeginTransaction(IsolationLevel.ReadCommitted);
            sqlCmd.Transaction = tran;

            cnt = sqlCmd.ExecuteNonQuery();
            tran.Commit();
            end();
        }
        catch (System.Exception ex)
        {
            LOGW.Info(ex.ToString());
            end();
        }
        return cnt;
    }

    public void end()
    {
        if (m_conn != null)
        {
            m_conn.Close();
            m_conn = null;
        }
    }

    public bool keyIntExists(string tableName, string field, object val, int dbName)
    {
        int v = Convert.ToInt32(val);
        string sql = string.Format(SqlStrCMD.SQL_COUNT_INT, tableName, field, v);
        MySqlDataReader r = startQuery(sql, dbName);
        if (r != null)
        {
            if (r.Read())
            {
                int cnt = Convert.ToInt32(r["cnt"]);
                end();
                return cnt > 0;
            }
        }
        return false;
    }

    public bool keyStrExists(string tableName, string field, object val, int dbName)
    {
        string v = Convert.ToString(val);
        string sql = string.Format(SqlStrCMD.SQL_COUNT_CHAR, tableName, field, v);
        MySqlDataReader r = startQuery(sql, dbName);
        if (r != null)
        {
            if (r.Read())
            {
                int cnt = Convert.ToInt32(r["cnt"]);
                end();
                return cnt > 0;
            }
        }
        return false;
    }

    public bool keyExists(string tableName, string sqlCondition, int dbName)
    {
        string sql = string.Format(SqlStrCMD.SQL_COUNT_WHERE, tableName, sqlCondition);
        MySqlDataReader r = startQuery(sql, dbName);
        if (r != null)
        {
            if (r.Read())
            {
                int cnt = Convert.ToInt32(r["cnt"]);
                end();
                return cnt > 0;
            }
        }
        return false;
    }

    // 返回记录个数
    public long getRecordCount(string tableName, string sqlCondition, int dbName)
    {
        int cnt = 0;
        string sql = string.Format(SqlStrCMD.SQL_COUNT_WHERE, tableName, sqlCondition);
        MySqlDataReader r = startQuery(sql, dbName);
        if (r != null)
        {
            if (r.Read())
            {
                cnt = Convert.ToInt32(r["cnt"]);
                end();
            }
        }
        return cnt;
    }

    // where关键字由外部直接传入，不用内部组装
    public long getRecordCountNoWhere(string tableName, string sqlCondition, int dbName)
    {
        int cnt = 0;
        string sql = string.Format(SqlStrCMD.SQL_COUNT_NO_WHERE, tableName, sqlCondition);
        MySqlDataReader r = startQuery(sql, dbName);
        if (r != null)
        {
            if (r.Read())
            {
                cnt = Convert.ToInt32(r["cnt"]);
                end();
            }
        }
        return cnt;
    }

    // 外部传入sql语句
    public long getRecordCount(string sql, int dbName)
    {
        int cnt = 0;
        MySqlDataReader r = startQuery(sql, dbName);
        if (r != null)
        {
            if (r.Read())
            {
                cnt = Convert.ToInt32(r["cnt"]);
                end();
            }
        }
        return cnt;
    }
}




