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
using System.Diagnostics;

namespace SqliteDemo2
{
    public partial class Form1 : Form
    {
        DateTime temp1;
        DateTime temp2;
        string brand = string.Empty;
        string datetime1;
        string datetime2;
        int totalNum = 0;
        int OKNum = 0;
        int numOfRows = 0;
        SqliteClassLib.SQLiteHelper sqlite1 = new SqliteClassLib.SQLiteHelper();
        FileStream filestream = new FileStream(@"C:\Users\Administrator\Desktop\table2.xlsx", FileMode.Create);
        XSSFWorkbook wk = new XSSFWorkbook();
        Stopwatch st = new Stopwatch(); //计时对象

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            sqlite1.DataBaseName = "TP";
            sqlite1.StrDataSource = "C:\\Users\\Administrator\\Desktop\\testDB\\";
            sqlite1.SQLite_connect();
            

            using (FileStream fs = File.OpenRead("C:\\Users\\Administrator\\Desktop\\table1.xlsx"))
            {
                IWorkbook workbook = null;
                workbook = new XSSFWorkbook(fs);    //xlsx
                if (workbook != null)
                {
                    ISheet sheet = null;
                    //int numOfSheets = workbook.NumberOfSheets;
                    int numOfSheets = 1;
                    for (int i = 0; i < numOfSheets; i++)
                    {
                        sheet = workbook.GetSheetAt(i);
                        if (sheet != null)
                        {
                            ISheet isheet = wk.CreateSheet("Sheet" + i.ToString());
                            IRow row = null;
                            numOfRows = sheet.LastRowNum + 1;   //sheet.LastRowNum是最后一行的标号，从0开始，总行数=LastRowNum+1
                            for (int j = 0; j < numOfRows; j++)
                            {
                                string cmdTotal = string.Empty;
                                string cmdOK = string.Empty;
                                row = sheet.GetRow(j);
                                if (row != null)
                                {
                                    //string temp = GetCellValue(row.GetCell(1)).ToString();
                                    //DateTime dt = 
                                    //datetime1 = dt.ToString();
                                    //datetime2 = GetCellValue(row.GetCell(1)).ToString();

                                    //datetime1 = (DateTime)GetCellValue(row.GetCell(1));
                                    //datetime2 = (DateTime)GetCellValue(row.GetCell(2));
                                    //string.Format("{yyyy-MM-dd HH:mm:ss}", datetime1);
                                    //datetime2 = row.GetCell(2);
                                    brand = (String)GetCellValue(row.GetCell(0));
                                    temp1 = (DateTime)GetCellValue(row.GetCell(1));
                                    temp2 = (DateTime)GetCellValue(row.GetCell(2));
                                    datetime1 = temp1.ToString("yyyy-MM-dd HH:mm:ss");
                                    datetime2 = temp2.ToString("yyyy-MM-dd HH:mm:ss");

                                }

                                cmdTotal = "CAMERA1 WHERE `BRAND`= " + brand +" AND `RIQI` BETWEEN '" + datetime1 + "' AND '" + datetime2 + "';";

                                st.Start();
                                totalNum = sqlite1.SQLite_count(cmdTotal);
                                st.Stop();
                                MessageBox.Show("查询耗时：" + st.ElapsedMilliseconds.ToString() + "ms");

                                cmdOK = "CAMERA1 WHERE `BRAND`= " + brand + " AND (`RIQI` BETWEEN '" + datetime1 + "' AND '" + datetime2 + "') AND `RESULT` = 1;";
                                OKNum = sqlite1.SQLite_count(cmdOK);

                                IRow row2;
                                ICell cell;
                                row2 = isheet.CreateRow(j);
                                cell = row2.CreateCell(0);
                                cell.SetCellValue(brand);
                                cell = row2.CreateCell(1);
                                cell.SetCellValue(totalNum);    //第二列总数 
                                cell = row2.CreateCell(2);
                                cell.SetCellValue(totalNum - OKNum);  //第三列不合格数


                            }
                        }
                    }
                }
            }
            MessageBox.Show("done!");
            wk.Write(filestream);
            filestream.Close();
            wk.Close();

        }

        void insertExcel(int sheetIndex, int rowIndex)
        {
            
        }

        public object GetCellValue(ICell cell)
        {
            object value = null;
            try
            {
                if (cell.CellType != CellType.Blank)
                {
                    switch (cell.CellType)
                    {
                        case CellType.Numeric:
                            // Date comes here
                            if (DateUtil.IsCellDateFormatted(cell))
                            {
                                value = cell.DateCellValue;
                            }
                            else
                            {
                                // Numeric type
                                value = cell.NumericCellValue;
                            }
                            break;
                        case CellType.Boolean:
                            // Boolean type
                            value = cell.BooleanCellValue;
                            break;
                        case CellType.Formula:
                            value = cell.CellFormula;
                            break;
                        default:
                            // String type
                            value = cell.StringCellValue;
                            break;
                    }
                }
            }
            catch (Exception)
            {
                value = "";
            }

            return value;
        }


    }
}
