using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using NPOI;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;

namespace SqliteDemo2
{
    public partial class Form1 : Form
    {
        string brand = string.Empty;
        string datetime1 = string.Empty;
        string datetime2 = string.Empty;
        SqliteClassLib.SQLiteHelper sqlite1 = new SqliteClassLib.SQLiteHelper();
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int totalNum = 0;
            int OKNum = 0;
            sqlite1.DataBaseName = "TP";
            sqlite1.StrDataSource = "C:\\Users\\25224\\Desktop\\testDB\\";
            sqlite1.SQLite_connect();
            using (FileStream fs = File.OpenRead("1.xls"))
            {
                IWorkbook workbook = null;
                workbook = new XSSFWorkbook(fs);    //xlsx
                ISheet sheet = workbook.GetSheetAt(0);
                IRow row;


               
                for(int i = 0; i < sheet.LastRowNum; i++)
                {
                    string cmdTotal = string.Empty;
                    string cmdOK = string.Empty;
                    row = sheet.GetRow(i);
                    if (row != null)
                    {
                        brand = row.GetCell(0).ToString();
                        datetime1 = row.GetCell(1).ToString();
                        datetime2 = row.GetCell(2).ToString();
                    }
                    cmdTotal = "TP WHERE `RIQI` BETWEEN '" + datetime1 + "' AND '" + datetime2 + "';";
                    totalNum = sqlite1.SQLite_count(cmdTotal);
                    cmdOK = "TP WHERE (`RIQI` BETWEEN '" + datetime1 + "' AND '" + datetime2 + "') AND `RESULT` = 1";
                    OKNum = sqlite1.SQLite_count(cmdOK);

                }
            }
        }
    }
}
