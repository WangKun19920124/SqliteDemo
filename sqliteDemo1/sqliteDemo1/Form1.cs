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
using SqliteClassLib;
using System.Diagnostics;//计时头文件
using System.Threading;


namespace sqliteDemo1
{
    public partial class Form1 : Form
    {
        Stopwatch st = new Stopwatch(); //计时对象
        public static SqliteClassLib.SQLiteHelper sqlite1 = new SqliteClassLib.SQLiteHelper();

        public Form1()
        {
            InitializeComponent();
        }

        //测试
        private void button_createDataBase_Click(object sender, EventArgs e)
        {
            sqlite1.DataBaseName = "TP";
            sqlite1.StrDataSource = "C:\\Users\\Administrator\\Desktop\\testDB\\";

            if (sqlite1.SQLite_createDataBase() == true)
            {
                MessageBox.Show("create success");
            }
        }

        private void button_connect_Click(object sender, EventArgs e)
        {
            sqlite1.DataBaseName = "TP";
            sqlite1.StrDataSource = "C:\\Users\\Administrator\\Desktop\\testDB\\";

            if (sqlite1.SQLite_connect() == true)
            {
                MessageBox.Show("connect success");
            }
            else
            {
                MessageBox.Show("connect failed");
            }
        }

        private void button_createTable_Click(object sender, EventArgs e)
        {
            string strCmd = "CAMERA1 (BRAND TEXT, RESULT TINYINT, RIQI TIMESTAMP NOT NULL);";
            if (sqlite1.SQLite_createTable(strCmd) == true)
            {
                MessageBox.Show("table success");
            }
            else
            {
                MessageBox.Show("table failed");
            }
            strCmd = "CAMERA2 (BRAND TEXT, RESULT TINYINT, RIQI TIMESTAMP NOT NULL);";
            if (sqlite1.SQLite_createTable(strCmd) == true)
            {
                MessageBox.Show("table success");
            }
            else
            {
                MessageBox.Show("table failed");
            }
        }

        private void button_Index_Click(object sender, EventArgs e)
        {
            string cmd = "DATATIME_index ON CAMERA1(RIQI);";
            if (sqlite1.SQLite_index(cmd))
            {

            }
        }

        private void button_insert_Click(object sender, EventArgs e)
        {
            //int ms = 0;//给tiny int赋值字符串？
            int total = 1000;  //记录总条数
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
            sqlite1.SQLite_insert(cmd1, param, "BEGIN TRANSACTION;\n", "");
            for (int i = 1; i < total - 1; i++)
            {
                param[0].Value = "brand";
                param[1].Value = i % 2;
                //param[2].Value = DateTime.Now.ToString();//给DATATIME类型变量赋值
                //param[3].Value = ms;
                sqlite1.SQLite_insert(cmd1, param, "", "");
                //Thread.Sleep(400);
            }
            param[0].Value = "brand";
            param[1].Value = 1;
            //param[2].Value = DateTime.Now.ToString();//给DATATIME类型变量赋值
            //param[3].Value = ms;
            sqlite1.SQLite_insert(cmd1, param, "", "\nEND TRANSACTION;");
            st.Stop();  //计时结束
            MessageBox.Show(st.ElapsedMilliseconds.ToString());
        }
    }
}
