using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Spire.Xls;
using Spring.Model;

namespace Excel2Pdf
{
    public partial class Form1 : Form
    {
        private string dirNanme = "";
        private Thread thread;
        /// <summary>
        /// UI线程的同步上下文
        /// </summary>
        SynchronizationContext m_SyncContext = null;

        private string checkName = "";
        private string checkDate = "";
        private string check = "";

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            String[] CmdArgs = System.Environment.GetCommandLineArgs();
            if (CmdArgs.Length >= 2)
            {
                //参数0是它本身的路径
                dirNanme = CmdArgs[1].ToString();
                checkName = CmdArgs[2];
                checkDate = CmdArgs[3];
                check = CmdArgs[4].ToString();
            }
            //check = "check";
            //获取UI线程同步上下文
            m_SyncContext = SynchronizationContext.Current;

            thread = new Thread(new ThreadStart(this.ThreadProcSafePost));
            thread.Start();
        }

        /// <summary>
        /// 线程的主体方法
        /// </summary>
        private void ThreadProcSafePost()
        {
            //...执行线程任务
            int percentage = 0;
            string json = File.ReadAllText("coordinates.json");
            List<PlotModel> plotList = JsonToObj(json, typeof(List<PlotModel>)) as List<PlotModel>;

            #region 自检报告生成
            if (check == "check")
            {
                ExecCoordinateCheckExcel(plotList);
            }
            #endregion

            #region 标准模板生成
            else
            {
                for (int i = 0; i < plotList.Count; i++)
                {
                    var plotModel = plotList[i];
                    ExecCoordinateExcel(plotModel);

                    //在线程中更新UI（通过UI线程同步上下文m_SyncContext）
                    m_SyncContext.Post(UpdateUIProcess, (i + 1) * 100 / plotList.Count);
                }
                ExecCoordinateSummary(plotList);//生成汇总表

                if (MessageBox.Show(@"是否需要打开生成目录", "操作成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    string makePath = GetGeneratePath("");
                    if (Directory.Exists(makePath))
                        System.Diagnostics.Process.Start(makePath);
                    this.Close();
                }
            }
            #endregion
        }

        /// <summary>
        /// 更新UI的方法
        /// </summary>
        /// <param name="text"></param>
        private void UpdateUIProcess(object obj)
        {
            progressBar.Value = Convert.ToInt16(obj);
            lbPrec.Text = obj.ToString() + "%";
        }

        public static Object JsonToObj(String json, Type t)
        {
            try
            {
                System.Runtime.Serialization.Json.DataContractJsonSerializer serializer = new System.Runtime.Serialization.Json.DataContractJsonSerializer(t);
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
                {
                    return serializer.ReadObject(ms);
                }
            }
            catch
            {
                return null;
            }
        }

        private Dictionary<string, string> GetConfigDict(string code)
        {
            XmlDocument doc = new XmlDocument();
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
            doc.Load(configFilePath);
            var dict = new Dictionary<string, string>();

            XmlNodeList nodes = doc.SelectNodes("/Config/" + code + "/value");
            foreach (XmlNode node in nodes)
            {
                dict.Add(node.InnerText, node.Attributes["desc"].Value);
            }
            return dict;
        }
        /// <summary>
        /// 根据路径创建文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        private void CreateDir(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            else
            {
                Directory.Delete(dirPath, true);
                Directory.CreateDirectory(dirPath);
            }
        }

        private void ExecCoordinateCheckExcel(List<PlotModel> plotList)
        {
            Workbook workbook = new Workbook();
            var tempetePath = AppDomain.CurrentDomain.BaseDirectory;
            tempetePath = Path.Combine(tempetePath, "Templete", "templete4.xlsx");
            workbook.LoadFromFile(tempetePath);
            if (workbook.Worksheets.Count == 0)
                return;
          
           

            int rowIndex = 7;
            var worksheet = workbook.Worksheets[0];
            //表底信息
            worksheet.Range["A14"].Text = worksheet.Range["A14"].Text + checkName;
            worksheet.Range["H14"].Text = worksheet.Range["H14"].Text + checkDate;

            var worksheet2 = workbook.Worksheets[1];
            //表底信息
            worksheet2.Range["A10"].Text = worksheet2.Range["A10"].Text + checkName;
            worksheet2.Range["F10"].Text = worksheet2.Range["F10"].Text + checkDate;

            worksheet2.InsertRow(5,plotList.Count-1);

            //界址点中误差m
            double sumM = 0;
            int sumN = 0;
            for (int i = 0; i < plotList.Count; i++)
            {
                var model = plotList[i];
                #region 界址点数据

                //模板中只有行，如果数据超过则需要增加行
                int insertRowsCount = model.CoordinateList.Count;
                worksheet.InsertRow((i == 0) ? rowIndex : rowIndex - 1, i == 0 ? insertRowsCount - 1 : insertRowsCount);
                //为新增加行复制样式
                for (int j = 0; j < insertRowsCount; j++)
                {
                    if (j == insertRowsCount - 1 && i == 0)
                        break;
                    int currentRow = ((i == 0) ? rowIndex : rowIndex - 1) + j;
                    worksheet.SetRowHeight(currentRow, 18);
                    worksheet.Copy(worksheet.Range["A6:L6"], worksheet.Range["A" + currentRow + ":L" + currentRow], true);
                }
                sumN += model.CoordinateList.Count;
                //循环处理行数据
                for (int j = 0; j < model.CoordinateList.Count; j++)
                {
                    var coordinate = model.CoordinateList[j];

                    sumM += Convert.ToDouble(coordinate.difSquareL);

                    int currentRowIndex = rowIndex + j - 1;
                    worksheet.Range["A" + currentRowIndex].Text = (currentRowIndex - 5).ToString();
                    worksheet.Range["B" + currentRowIndex].Text = model.PlotCode;
                    worksheet.Range["C" + currentRowIndex].Text = coordinate.BoundaryPointNum;
                    worksheet.Range["D" + currentRowIndex].Text = coordinate.X;
                    worksheet.Range["E" + currentRowIndex].Text = coordinate.Y;
                    worksheet.Range["F" + currentRowIndex].Text = coordinate.cX;
                    worksheet.Range["G" + currentRowIndex].Text = coordinate.cY;
                    worksheet.Range["H" + currentRowIndex].Text = coordinate.difX;
                    worksheet.Range["I" + currentRowIndex].Text = coordinate.difY;
                    worksheet.Range["J" + currentRowIndex].Text = coordinate.difL;
                    worksheet.Range["K" + currentRowIndex].Text = coordinate.difSquareL;
                    worksheet.Range["L" + currentRowIndex].Text = "";
                }
                rowIndex += insertRowsCount;

                #endregion

                #region 面积数据
                int currentRowIndex2 = 4;

                currentRowIndex2 += i;
                if (i != 0)
                {
                    worksheet2.SetRowHeight(currentRowIndex2, 18);
                    worksheet2.Copy(worksheet2.Range["A4:H4"], worksheet2.Range["A" + currentRowIndex2 + ":H" + currentRowIndex2], true);
                }

                worksheet2.Range["A" + currentRowIndex2].Text = (i + 1).ToString();
                worksheet2.Range["B" + currentRowIndex2].Text = model.PlotCode;
                worksheet2.Range["C" + currentRowIndex2].Text = model.PlotArea;
                worksheet2.Range["D" + currentRowIndex2].Text = model.PlotCheckArea;
                worksheet2.Range["E" + currentRowIndex2].Text = model.DifArea;
                worksheet2.Range["F" + currentRowIndex2].Text = model.PercentageError + "%";

                #endregion

                //在线程中更新UI（通过UI线程同步上下文m_SyncContext）
                m_SyncContext.Post(UpdateUIProcess, (i + 1) * 100 / plotList.Count);
            }

            //计算界址点中误差m sqrt（∑[△L²]/2n）
            string m = Math.Sqrt(sumM / (2 * sumN)).ToString("f3");
            worksheet.Range["E" + (rowIndex-1)].Text = m;

            string generatePath = GetGeneratePath("");
            if (!Directory.Exists(generatePath))
                Directory.CreateDirectory(generatePath);
            var dict = GetConfigDict("JZD");
            var zjdCode = dict.Keys.First();

            generatePath = Path.Combine(generatePath, "自检报告", "自检报告");//每个生成的文件放到对应组号下面

            workbook.SaveToFile(generatePath + ".xlsx", ExcelVersion.Version2010);

        }
        /// <summary>
        /// 操作填充界址点检查记录表 与面积检查记录表
        /// </summary>
        /// <param name="model"></param>
        private void ExecCoordinateExcel(PlotModel model)
        {
            #region 界址点检查表
            Workbook workbook = new Workbook();
            var tempetePath = AppDomain.CurrentDomain.BaseDirectory;
            tempetePath = Path.Combine(tempetePath, "Templete", "templete1.xlsx");
            workbook.LoadFromFile(tempetePath);
            if (workbook.Worksheets.Count == 0)
                return;

            var worksheet = workbook.Worksheets[0];
            //抬头数据 承包方代码 承包方代表名称 地块编码 地块名称
            worksheet.Range["A2"].Text = worksheet.Range["A2"].Text + model.CbfCode;
            worksheet.Range["F2"].Text = worksheet.Range["F2"].Text + model.CbfDBName;
            worksheet.Range["A3"].Text = worksheet.Range["A3"].Text + model.PlotCode;
            worksheet.Range["F3"].Text = worksheet.Range["F3"].Text + model.PlotName;
            //表底信息
            worksheet.Range["A29"].Text = worksheet.Range["A29"].Text + checkName;
            worksheet.Range["F29"].Text = worksheet.Range["F29"].Text + checkDate;

            //先给界址点中误差m赋值
            worksheet.Range["D25"].Text = model.PlotM;
            worksheet.Range["D25"].Style.HorizontalAlignment = HorizontalAlignType.Right;
            //模板中只有19行，如果数据超过则需要增加行
            if (model.CoordinateList.Count > 18)
            {
                int insertRowsCount = model.CoordinateList.Count - 18;
                worksheet.InsertRow(24, insertRowsCount);
                //为新增加行复制样式
                for (int i = 0; i < insertRowsCount; i++)
                {
                    int currentRow = 24 + i;
                    worksheet.SetRowHeight(currentRow, 22);
                    worksheet.Copy(worksheet.Range["A23:K23"], worksheet.Range["A" + currentRow + ":K" + currentRow], true);
                }
            }

            //循环处理行数据
            for (int i = 0; i < model.CoordinateList.Count; i++)
            {
                var coordinate = model.CoordinateList[i];
                int currentRowIndex = i + 7;
                worksheet.Range["A" + currentRowIndex].Text = (i + 1).ToString();
                worksheet.Range["B" + currentRowIndex].Text = coordinate.BoundaryPointNum;
                worksheet.Range["C" + currentRowIndex].Text = coordinate.X;
                worksheet.Range["D" + currentRowIndex].Text = coordinate.Y;
                worksheet.Range["E" + currentRowIndex].Text = coordinate.cX;
                worksheet.Range["F" + currentRowIndex].Text = coordinate.cY;
                worksheet.Range["G" + currentRowIndex].Text = coordinate.difX;
                worksheet.Range["H" + currentRowIndex].Text = coordinate.difY;
                worksheet.Range["I" + currentRowIndex].Text = coordinate.difL;
                worksheet.Range["J" + currentRowIndex].Text = coordinate.difSquareL;
            }

            string generatePath = GetGeneratePath("");
            if (!Directory.Exists(generatePath))
                Directory.CreateDirectory(generatePath);
            var dict = GetConfigDict("JZD");
            var zjdCode = dict.Keys.First();
            string groupNum = model.PlotCode.Substring(12, 2);
            generatePath = Path.Combine(generatePath, groupNum, "承包方", zjdCode + model.PlotCode);//每个生成的文件放到对应组号下面

            workbook.SaveToFile(generatePath + ".xlsx", ExcelVersion.Version2010);
            PdfUtility.ConvertExcel2PDF(generatePath + ".xlsx", generatePath + ".pdf");
            File.Delete(generatePath + ".xlsx");//删除excel文件 

            #endregion

            #region 面积检查表
            Workbook workbook2 = new Workbook();
            var tempetePath2 = AppDomain.CurrentDomain.BaseDirectory;
            tempetePath2 = Path.Combine(tempetePath2, "Templete", "templete2.xlsx");
            workbook2.LoadFromFile(tempetePath2);
            if (workbook2.Worksheets.Count == 0)
                return;

            var worksheet2 = workbook2.Worksheets[0];

            //抬头数据 承包方代码 承包方代表名称 地块编码 地块名称
            worksheet2.Range["A2"].Text = worksheet2.Range["A2"].Text + model.CbfCode;
            worksheet2.Range["D2"].Text = worksheet2.Range["D2"].Text + model.CbfDBName;
            worksheet2.Range["A3"].Text = worksheet2.Range["A3"].Text + model.PlotCode;
            worksheet2.Range["D3"].Text = worksheet2.Range["D3"].Text + model.PlotName;
            //表底信息
            worksheet2.Range["A29"].Text = worksheet2.Range["A29"].Text + checkName;
            worksheet2.Range["D29"].Text = worksheet2.Range["D29"].Text + checkDate;

            worksheet2.Range["A5"].Text = "1";
            worksheet2.Range["B5"].Text = model.PlotArea;
            worksheet2.Range["C5"].Text = model.PlotCheckArea;
            worksheet2.Range["D5"].Text = model.DifArea;
            worksheet2.Range["E5"].Text = model.PercentageError + "%";

            string generatePath2 = GetGeneratePath("");

            var dict2 = GetConfigDict("AREA");
            var areaCode = dict2.Keys.First();
            groupNum = model.PlotCode.Substring(12, 2);
            generatePath2 = Path.Combine(generatePath2, groupNum, "承包方", areaCode + model.PlotCode);

            if (model.IsGenerate)
            {
                workbook2.SaveToFile(generatePath2 + ".xlsx", ExcelVersion.Version2010);
                PdfUtility.ConvertExcel2PDF(generatePath2 + ".xlsx", generatePath2 + ".pdf");
                File.Delete(generatePath2 + ".xlsx");//删除excel文件 }
            }
            else//当有错误面积表文件时，单独生成到错误文件夹里
            {
                string errorPath = GetGeneratePath("");
                errorPath = Path.Combine(errorPath, "面积精度检查表错误文件", areaCode + model.PlotCode);
                workbook2.SaveToFile(errorPath + ".xlsx", ExcelVersion.Version2010);
            }

            #endregion
        }

        /// <summary>
        /// 处理界址点与面积汇总表
        /// </summary>
        /// <param name="plotList"></param>
        private void ExecCoordinateSummary(List<PlotModel> plotList)
        {
            int totalCount = 1;
            Workbook workbook = new Workbook();
            var tempetePath = AppDomain.CurrentDomain.BaseDirectory;
            tempetePath = Path.Combine(tempetePath, "Templete", "templete3.xlsx");
            workbook.LoadFromFile(tempetePath);
            if (workbook.Worksheets.Count == 0)
                return;
            var worksheet = workbook.Worksheets[0];
            for (int i = 0; i < plotList.Count; i++)
            {
                var plotModel = plotList[i];
                for (int j = 0; j < plotModel.CoordinateList.Count; j++)
                {
                    var coordinate = plotModel.CoordinateList[j];
                    int currentRowIndex = totalCount + 5;

                    if (currentRowIndex > 20)//如果超出20条数据，则增加数据行
                    {
                        worksheet.InsertRow(19 + totalCount, 1);
                        worksheet.SetRowHeight(currentRowIndex, 22);
                        worksheet.Copy(worksheet.Range["A19:P19"], worksheet.Range["A" + currentRowIndex + ":P" + currentRowIndex], true);
                    }

                    worksheet.Range["A" + currentRowIndex].Text = totalCount.ToString();
                    worksheet.Range["B" + currentRowIndex].Text = coordinate.BoundaryPointNum;
                    worksheet.Range["C" + currentRowIndex].Text = coordinate.X;
                    worksheet.Range["D" + currentRowIndex].Text = coordinate.Y;
                    worksheet.Range["E" + currentRowIndex].Text = coordinate.X1;
                    worksheet.Range["F" + currentRowIndex].Text = coordinate.Y1;
                    worksheet.Range["G" + currentRowIndex].Text = coordinate.X2;
                    worksheet.Range["H" + currentRowIndex].Text = coordinate.Y2;
                    worksheet.Range["I" + currentRowIndex].Text = coordinate.X3;
                    worksheet.Range["J" + currentRowIndex].Text = coordinate.Y3;
                    worksheet.Range["K" + currentRowIndex].Text = coordinate.cX;
                    worksheet.Range["L" + currentRowIndex].Text = coordinate.cY;
                    worksheet.Range["M" + currentRowIndex].Text = coordinate.difX;
                    worksheet.Range["N" + currentRowIndex].Text = coordinate.difY;
                    worksheet.Range["O" + currentRowIndex].Text = coordinate.difL;
                    worksheet.Range["P" + currentRowIndex].Text = coordinate.difSquareL;

                    totalCount++;//自增
                }
            }

            string generatePath = GetGeneratePath("承包地块界址点精度检查汇总记录");
            if (!Directory.Exists(generatePath))
                Directory.CreateDirectory(generatePath);
            generatePath = Path.Combine(generatePath, "承包地块界址点精度检查汇总记录-(" + dirNanme + ").xlsx");
            workbook.SaveToFile(generatePath, ExcelVersion.Version2010);
        }

        private string GetGeneratePath(string directName = "承包方")
        {
            string currentPath = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetParent(currentPath).FullName).FullName);

            currentPath = Directory.GetCurrentDirectory();
            return Path.Combine(currentPath, dirNanme, directName);
        }

        //窗口关闭事件
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            thread.Abort();
            Application.Exit();
        }
    }

}
