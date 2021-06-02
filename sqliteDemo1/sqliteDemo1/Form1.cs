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
        private static object lockObj = new object();  //线程锁对象


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

            int total = 100;  //记录总条数，至少是4
            //string cmd1 = "CAMERA1 (BRAND,RESULT,RIQI) VALUES (@brand,@result,datetime('now','localtime'));";
            //string cmd2 = "CAMERA2 (BRAND,RESULT,RIQI) VALUES (@brand,@result,datetime('now','localtime'));";
            string cmd1 = "CAMERA1 (BRAND,RESULT,RIQI) VALUES (@brand,@result,STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW'));";  //时间戳精确到ms
            string cmd2 = "CAMERA2 (BRAND,RESULT,RIQI) VALUES (@brand,@result,STRFTIME('%Y-%m-%d %H:%M:%f', 'NOW'));";

            SQLiteParameter[] param =
            {
                new SQLiteParameter("@brand",DbType.String),   //DbType？MSDN上写的是SqliteType，但是找不到这个Enum
                new SQLiteParameter("@result", DbType.Int16),
            };
            param[0].Value = "brand";
            param[1].Value = 1;

            ThreadInsert.setTotal(total);   //调用静态函数给类变量赋值
            ThreadInsert mt1 = new ThreadInsert(cmd1,param);
            ThreadInsert mt2 = new ThreadInsert(cmd2, param);
            Thread thread1 = new Thread(new ThreadStart(mt1.threadInsert));
            thread1.Name = "thread1";
            Thread thread2 = new Thread(new ThreadStart(mt2.threadInsert));
            thread2.Name = "thread2";

            //插入开始
            st.Start(); //计时开始

            param[0].Value = "brand";
            param[1].Value = 1;
            sqlite1.SQLite_insert(cmd1, param, "BEGIN TRANSACTION;\n", ""); //第一条插入和最后一条插入加上事务指令
            sqlite1.SQLite_insert(cmd2, param, "", ""); //第一条插入和最后一条插入加上事务指令

            thread1.Start();
            thread2.Start();
            thread1.Join();
            thread2.Join();

            sqlite1.SQLite_insert(cmd1, param, "", "");
            sqlite1.SQLite_insert(cmd2, param, "", "\nEND TRANSACTION;");

            st.Stop();  //计时结束
            MessageBox.Show("插入耗时：" + st.ElapsedMilliseconds.ToString() + "ms");
        }

        //线程类，封装子线程函数
        public class ThreadInsert
        {
            public static int total = 0;
            //要传递给线程函数的参数
            public string cmd { set; get; }
            public SQLiteParameter[] param { set; get; }

            public ThreadInsert(string param2, SQLiteParameter[] param3)
            {
                this.cmd = param2;
                this.param = param3;
            }

            //给total赋值（静态函数给类静态变量赋值）
            public static void setTotal(int t)
            {
                ThreadInsert.total = t;
            }

            //线程执行函数
            public void threadInsert(){
                while ((ThreadInsert.total - 4) > 0)
                {
                    this.param[0].Value = "brand";
                    this.param[1].Value = ThreadInsert.total % 2;
                    //Thread.Sleep(400);
                    Monitor.Enter(Form1.lockObj); //加锁
                    sqlite1.SQLite_insert(this.cmd, this.param, "", "");
                    ThreadInsert.total--;
                    Monitor.Exit(lockObj);  //释放锁

                }
            }

        }
    }
}
