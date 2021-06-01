using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Data;

namespace SqliteClassLib
{
    public class SQLiteHelper
    {
        private string strDataSource = "C:\\Users\\Administrator\\Desktop\\testDB\\";
        private string dataBaseName = "1";
        private string dataSource = string.Empty;   //db文件路径
        private string password = "123";    //密码
        private string connStr = string.Empty;
        private SQLiteConnection sqliteConn;//数据库连接对象

        //属性
        public string StrDataSource
        {
            get
            {
                return this.strDataSource;
            }
            set
            {
                this.strDataSource = value;
                this.dataSource = this.strDataSource + this.dataBaseName + ".db3";
                this.connStr = "Data Source=" + this.dataSource + ";Version=3;";
            }
        }

        public string DataBaseName
        {
            get
            {
                return this.dataBaseName;
            }
            set
            {
                this.dataBaseName = value;
                this.dataSource = this.strDataSource + this.dataBaseName + ".db3";
                this.connStr = "Data Source=" + this.dataSource + ";Version=3;";
            }
        }

        public string PassWord
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = value;
            }
        }

        //--------------------------库操作-------------------------------

        //----------------------------------------------------
        //连接数据库
        //----------------------------------------------------
        public Boolean SQLite_connect()
        {
            bool flag = false;
            
            if (dataBaseName == "")
            {
                return false;
            }

            try
            {
                sqliteConn = new SQLiteConnection(connStr);
                sqliteConn.Open();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }

        //---------------------------------------------------------------
        //创建数据库
        //---------------------------------------------------------------
        public Boolean SQLite_createDataBase()
        {
            bool flag = false;
            if (dataBaseName == "")
            {
                return false;
            }

            try
            {
                if (System.IO.File.Exists(dataSource) == false)
                {
                    sqliteConn = new SQLiteConnection(connStr);
                    sqliteConn.Open();
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return flag;
        }

        //删除数据库
        public Boolean SQLite_deleteDataBase()
        {
            bool flag = false;
            if (dataBaseName == "")
            {
                return false;
            }

            try
            {
                if (System.IO.File.Exists(dataSource))
                {
                    System.IO.File.Delete(dataSource);
                    flag = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }


        //-------------------------表操作-------------------------------------------------------------------

        //------------------------------------------------
        //新建表
        //参数：建表指令,strCmd="table_name (ID TEXT PRIMARY KEY,NAME TEXT,AGE INT)"
        //------------------------------------------------
        public Boolean SQLite_createTable(string strCmd)
        {
            bool flag = false;
            strCmd = "CREATE TABLE IF NOT EXISTS " + strCmd;
            if (dataBaseName == "" || strCmd == "")
            {
                return false;
            }
            //没有库文件时不能建表
            if (System.IO.File.Exists(dataSource) == false)
            {
                return false;
            }

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand sqliteCmd = new SQLiteCommand(strCmd, sqliteConn);
                sqliteCmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }


        //--------------------------------------------------
        //删除表
        //--------------------------------------------------
        public Boolean SQLite_deleteTable(string tableName)
        {
            bool flag = false;

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = "DROP TABLE IF EXISTS " + tableName;
                cmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }

        //显示所有表的结构
        public string SQLite_QueryAllTablesInfo()
        {
            string result = string.Empty;

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = "SELECT name FROM sqlite_master WHERE TYPE='table' ";
                SQLiteDataReader reader = cmd.ExecuteReader();
                List<string> tablesName = new List<string>();
                while (reader.Read())
                {
                    tablesName.Add(reader.GetString(0));    //reader.GetString(0)是什么
                }
                reader.Close();
                result = "cid\t" + "name\t" + "type\t" + "notnull\t" + "dflt_value\t" + "pk\n";
                foreach (var ele in tablesName)  //var类似auto
                {
                    cmd.CommandText = $"PRAGMA TABLE_INFO({ele})";
                    reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {

                        result += $"{reader[0]}\t {reader[1]}\t {reader[2]}\t {reader[3]}\t {reader[4]}\t {reader[5]}\n";
                    }
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return result;
        }

        //-------------------------------------记录操作--------------------------------------------

        //---------------------------------
        //插入记录
        //strCmd="`table_name` (ID, NAME, AGE, ADDRESS, SALARY) VALUES (@ID, @NAME, @AGE, @ADDRESS, @SALARY)"
        //SQLiteParameter p1 = new SQLiteParameter("@ID", DbType.Int32);
        //---------------------------------
        public Boolean SQLite_insert(string strCmd, SQLiteParameter[] param, string transactionBegin, string transactionEnd)
        {
            bool flag = false;
            if (strCmd == "")
            {
                return false;
            }
            strCmd = transactionBegin + "INSERT INTO " + strCmd + transactionEnd;


            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = strCmd;
                foreach (var ele in param)
                {
                    cmd.Parameters.Add(ele);
                }
                cmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }

        //---------------------------------
        //删除记录
        //strCmd=`table_name` WHERE `col`=val
        //---------------------------------
        public Boolean SQLite_delete(string strCmd)
        {
            bool flag = false;
            if (strCmd == "")
            {
                return false;
            }
            strCmd = "DELETE FROM " + strCmd;


            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = strCmd;
                cmd.ExecuteNonQuery();
                flag = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }

        //---------------------------------
        //修改记录
        //---------------------------------





        //---------------------------------
        //查询记录
        //strCmd=table_name WHERE
        //---------------------------------
        public DataTable SQLite_search(string strCmd)
        {

            if (strCmd == "")
            {
                return null;
            }
            DataTable dt = new DataTable();
            strCmd = "SELECT * FROM " + strCmd;

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(strCmd, sqliteConn);
                SQLiteCommandBuilder builder = new SQLiteCommandBuilder(adapter);
                adapter.Fill(dt);
            }
            catch (Exception ex)
            {
                ex.ToString();
                return null;
            }
            return dt;
        }

        //----------------------------------------------------------
        //查询计数
        //strCmd=table_name WHERE 
        //----------------------------------------------------------
        public Int32 SQLite_count(string strCmd)
        {
            Int32 count = 0;
            if (strCmd == "")
            {
                return -1;
            }

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                strCmd = "SELECT COUNT(*) FROM " + strCmd;
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = strCmd;
                SQLiteDataReader reader = cmd.ExecuteReader();
                reader.Read();
                count = reader.GetInt32(0);
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
            return count;
        }

        //----------------------------------------------------------
        //创建索引
        //strCmd=table_name WHERE 
        //----------------------------------------------------------
        public Boolean SQLite_index(string strCmd)
        {
            bool flag = false;
            if (strCmd == "")
            {
                return false;
            }
            strCmd = "CREATE INDEX " + strCmd;

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = strCmd;
                cmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }

    }
}
