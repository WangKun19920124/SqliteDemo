/**
 * https://blog.csdn.net/weixin_41732430/article/details/83753628
 * https://www.cnblogs.com/mengdongsky/p/6867038.html
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;
using System.Data.SQLite.Generic;
using System.Diagnostics;//计时头文件
using System.Threading;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Stopwatch st = new Stopwatch(); //计时对象
        private string strDataSource = "C:\\Users\\Administrator\\Desktop\\testDB\\";
        private string dataBaseName = "1";
        private string dataSource = string.Empty;   //db文件路径
        private string password = "123";    //密码
        private string connStr=string.Empty;
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
            }
        }

        public string DataSource
        {
            get
            {
                return this.dataSource;
            }
            set
            {
                this.dataSource = value;
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
            dataSource = strDataSource + dataBaseName+".db3";
            connStr = "Data Source=" + dataSource + ";Version=3;";
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
                MessageBox.Show(ex.ToString());
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
            dataSource = strDataSource + dataBaseName + ".db3";
            connStr = "Data Source=" + dataSource + ";Version=3;";
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
                MessageBox.Show(ex.ToString());
            }
            return flag;
        }

        //删除数据库
        public Boolean SQLite_deleteDataBase()
        {
            bool flag = false;
            dataSource = dataBaseName + strDataSource;
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
            }catch(Exception ex)
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
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);
            strCmd = "CREATE TABLE IF NOT EXISTS " + strCmd;
            if (dataBaseName == ""||strCmd=="")
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
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);

            try
            {
                if (sqliteConn.State != ConnectionState.Open)
                {
                    sqliteConn.Open();
                }
                SQLiteCommand cmd = new SQLiteCommand();
                cmd.Connection = sqliteConn;
                cmd.CommandText = "DROP TABLE IF EXISTS "+ tableName;
                cmd.ExecuteNonQuery();
                flag = true;
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
                flag = false;
            }
            return flag;
        }

        //显示所有表的结构
        public string SQLite_QueryAllTablesInfo()
        {
            string result = string.Empty;
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);

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
            }catch(Exception ex)
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
        public Boolean SQLite_insert(string strCmd,SQLiteParameter[] param, string transactionBegin, string transactionEnd)
        {
            bool flag = false;
            if (strCmd == "")
            {
                return false;
            }
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);
            strCmd =transactionBegin + "INSERT INTO " + strCmd + transactionEnd;
            

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
            }catch(Exception ex)
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
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);
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
            }catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);
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
                //SQLiteDataReader reader = cmd.ExecuteReader();
                //while (reader.Read())
                //{
                //    totalNum++;
                //}
            }
            catch(Exception ex)
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
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);

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
            //dataSource = strDataSource + dataBaseName + ".db3";
            //connStr = "Data Source=" + dataSource + ";Version=3;";
            //sqliteConn = new SQLiteConnection(connStr);
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

            } catch(Exception ex)
            {
                ex.ToString();
                flag = false;
            }
            return flag;
        }


        //测试
        private void button1_Click(object sender, EventArgs e)
        {
            if (SQLite_createDataBase() == true)
            {
                MessageBox.Show("create success");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (SQLite_connect() == true)
            {
                MessageBox.Show("connect success");
            }
            else
            {
                MessageBox.Show("connect failed");
            }
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string strCmd = "CAMERA1 (BRAND TEXT, RESULT TINYINT, RIQI TIMESTAMP NOT NULL);";
            //string cmd = "CAMERA2 (BRAND TEXT, RESULT TINYINT, DATETIME TIMESTAMP, MS TINYINT);";
            if (SQLite_createTable(strCmd) == true)
            {
                MessageBox.Show("table success");
            }
            else
            {
                MessageBox.Show("table failed");
            }
            //cmd = "CAMERA2 (BRAND TEXT, RESULT TINYINT, DATETIME TIMESTAMP, MS TINYINT);";
            strCmd = "CAMERA2 (BRAND TEXT, RESULT TINYINT, RIQI TIMESTAMP NOT NULL);";
            if (SQLite_createTable(strCmd) == true)
            {
                MessageBox.Show("table success");
            }
            else
            {
                MessageBox.Show("table failed");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            bool flag = SQLite_deleteTable("TP");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SQLite_QueryAllTablesInfo();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            //int ms = 0;//给tiny int赋值字符串？
            int total = 1000000;  //记录总条数
            string cmd1 = "CAMERA1 (BRAND,RESULT,RIQI) VALUES (@brand,@result,datetime('now','localtime'));";
            //string cmd2 = "CAMERA2 (BRAND,RESULT,DATATIME,MS) VALUES (@brand,@result,@datatime,@ms);";
            SQLiteParameter[] param =
            {
                new SQLiteParameter("@brand",DbType.String),   //DbType？MSDN上写的是SqliteType，但是找不到这个Enum
                new SQLiteParameter("@result", DbType.Int16),
                //new SQLiteParameter("@riqi",DbType.DateTime2),
                //new SQLiteParameter("@ms",DbType.Int16),
            };
            
                param[0].Value = "brand";
                param[1].Value = 1;
                //param[2].Value = DateTime.Now.ToString();//给DATATIME类型变量赋值
                //param[3].Value = ms;
                st.Start(); //计时开始
                SQLite_insert(cmd1, param, "BEGIN TRANSACTION;\n", "");
                for (int i = 1; i < total - 1; i++)
                {
                    param[0].Value = "brand";
                    param[1].Value = i%2;
                    //param[2].Value = DateTime.Now.ToString();//给DATATIME类型变量赋值
                    //param[3].Value = ms;
                    SQLite_insert(cmd1, param, "", "");
                    //Thread.Sleep(400);
                }
                param[0].Value = "brand";
                param[1].Value = 1;
                //param[2].Value = DateTime.Now.ToString();//给DATATIME类型变量赋值
                //param[3].Value = ms;
                SQLite_insert(cmd1, param, "", "\nEND TRANSACTION;");
                st.Stop();  //计时结束
                MessageBox.Show(st.ElapsedMilliseconds.ToString());
        }

        private void button7_Click(object sender, EventArgs e)
        {
            //SQLite_delete("TP WHERE `RESULT`=0");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string cmd = "TP WHERE `RESULT`=0;";
            DataTable dt = SQLite_search(cmd);
            string result = string.Empty;
            if (dt.Rows.Count > 0)
            {
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    for(int j = 0; j < dt.Columns.Count; j++)
                    {
                        result += dt.Rows[i][j].ToString()+"\t";
                    }
                    result += "\n";
                }
            }
            MessageBox.Show(result);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            string cmd = "TP;";
            Int32 count = SQLite_count(cmd);
            MessageBox.Show(count.ToString());
        }

        private void button10_Click(object sender, EventArgs e)
        {
            string cmd = "DATATIME_index ON CAMERA1(RIQI);";
            if (SQLite_index(cmd))
            {
                
            }
        }
    }
}

