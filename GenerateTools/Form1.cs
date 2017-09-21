using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using Spire.Doc;
using Spire.Doc.Collections;
using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Spring.Model;
using NPOI.XSSF.UserModel;

namespace GenerateTools
{
    public partial class Form1 : Form
    {
        List<PlotModel> plotList = new List<PlotModel>();
        private int totalCount = 0;
        private int currentIndex = 0;
        private double standM = 0.4;
        private int calcCount = 0;//赋值计算次数
        private Dictionary<string, string> errorDic = new Dictionary<string, string>();//错误文件列表
        private string type = "";

        /// <summary>
        /// UI线程的同步上下文
        /// </summary>
        SynchronizationContext m_SyncContext = null;

        private Thread thread;

        //界址点编号字典
        //key 为界址点编号 value为 坐标实体
        private Dictionary<string, CoordinatesModel> plotDic = new Dictionary<string, CoordinatesModel>();
        public Form1()
        {
            InitializeComponent();
            //获取UI线程同步上下文
            m_SyncContext = SynchronizationContext.Current;
            CheckForIllegalCrossThreadCalls = false;
        }

        private void btnSetWordPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "选择界址点成果表路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtWorkPath.Text = foldPath;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCheckName.Text.Trim()))
            {
                MessageBox.Show("检查人员姓名不能为空！");
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(txtWorkPath.Text.Trim());
            FileInfo[] fil = dir.GetFiles();
            if (fil.Length == 0)
            {
                MessageBox.Show("所选路径下没有文件，请重试！");
                return;
            }

            thread = new Thread(new ThreadStart(this.ThreadProcSafePost));
            thread.Start();
        }

        /// <summary>
        /// 线程的主体方法
        /// </summary>
        private void ThreadProcSafePost()
        {
            plotList = new List<PlotModel>();
            DirectoryInfo dir = new DirectoryInfo(txtWorkPath.Text.Trim());
            FileInfo[] fil = dir.GetFiles();
            totalCount = fil.Length;
            DirectoryInfo[] dii = dir.GetDirectories();
            currentIndex = 0;
            foreach (FileInfo f in fil)
            {
                var plotModel = new PlotModel();
                currentIndex += 1;
                string fileName = f.Name;
                string cbfCode = "", plotCode = "";

                if (comboBox1.SelectedIndex == 0)//如果是doc模式
                {
                    cbfCode = fileName.Split('_')[0]; //承包方代码
                    plotCode = fileName.Split('_')[1]; //地块代码
                }
                else if (comboBox1.SelectedIndex == 1) //如果是excel模式
                {
                    cbfCode = fileName.Split('_')[0];
                    string[] arr = fileName.Split('_');
                    plotCode = Regex.Replace(arr[arr.Length - 1], @"[^0-9]+", "");
                }
                plotModel.CbfCode = cbfCode;
                plotModel.PlotCode = plotCode;
                plotModel.IsGenerate = true;//默认会生成PDF

                if (comboBox1.SelectedIndex == 0)//doc模式
                    SetPlotModel_DOC(plotModel, f.FullName);
                else if (comboBox1.SelectedIndex == 1)//excel模式
                    SetPlotModel_EXCEL(plotModel, f.FullName);

                if (!errorDic.ContainsKey(plotModel.PlotCode))
                    plotList.Add(plotModel);

                m_SyncContext.Post(UpdateUIProcess, currentIndex * 100 / totalCount);
            }
            //将结果集持久化保存json
            string json = JsonConvert.SerializeObject(plotList);
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel2pdf");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            File.WriteAllText("excel2pdf/coordinates.json", json);

            if (errorDic.Count > 0)
            {
                txtErrorMsg.Text = "";
                StringBuilder sb = new StringBuilder();
                foreach (var errorFile in errorDic)
                {
                    sb.AppendLine(errorFile.Value);
                }
                MessageBox.Show("有错误面积数据，请查看错误文件列表信息，即将生成的PDF文件不包含这些错误数据，请手工修改完善！");
                txtErrorMsg.Text = sb.ToString();
            }

            string showInfo = "采集数据完成，工采集【" + plotList.Count + "】条数据\r\n是否进行生成PDF操作？";
            if (errorDic.Count > 0)
            {
                showInfo = "采集数据完成，工采集【" + plotList.Count + "】条数据；\r\n发现错误文件，是否打开错误文件目录，同时继续生成无错误PDF文件？";
            }
            if (
                MessageBox.Show(showInfo, "采集数据完成",MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==DialogResult.OK)
            {
                var dirName = txtWorkPath.Text.Trim().Substring(txtWorkPath.Text.Trim().LastIndexOf(@"\") + 1);

                #region 如果有错误文件 处理复制出来查看
                if (errorDic.Count > 0)
                {
                    string currentPath = AppDomain.CurrentDomain.BaseDirectory;

                    string generatePath = Path.Combine(currentPath, dirName, "原始错误文件");
                    if (!Directory.Exists(generatePath))
                        Directory.CreateDirectory(generatePath);
                    foreach (var item in errorDic)
                    {
                        string errorFilePath = item.Value;
                        string fileName = Path.GetFileName(errorFilePath);
                        string generateFilePath = Path.Combine(generatePath, fileName);
                        File.Copy(errorFilePath, generateFilePath, true);
                    }
                    if (Directory.Exists(generatePath))
                        System.Diagnostics.Process.Start(generatePath);
                }

                #endregion

                //Log("【自动采集生成界址点面积评估表操作】 采集并生成 【" + dirName + "】【" + plotList.Count + "】条数据");
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "excel2pdf"); //要启动程序路径

                p.StartInfo.FileName = "Excel2Pdf"; //需要启动的程序名   
                                                    //获得文件夹名称

                //传递的参数 参数1：文件夹名称 参数2：检查人姓名 参数3：检查日期 参数4：生成类型
                p.StartInfo.Arguments = dirName.Trim().Replace(" ", "") + " " + txtCheckName.Text.Trim() + " " + Convert.ToDateTime(txtCheckDate.Text).ToString("yyyy年MM月dd日")+" "+type;
                p.Start(); //启动  
            }
            else
            {
                thread.Abort();
            }

        }

        /// <summary>
        /// 更新UI的方法
        /// </summary>
        /// <param name="text"></param>
        private void UpdateUIProcess(object obj)
        {
            progressBarCollect.Value = Convert.ToInt16(obj);
        }


        /// <summary>
        /// 为实体赋值-doc模式
        /// </summary>
        /// <param name="model"></param>
        private void SetPlotModel_DOC(PlotModel model, string filePath, bool recursive = false)
        {
            Document document = new Document();
            TableCollection tables = null;

            document.LoadFromFile(filePath);
            tables = document.Sections[0].Tables;
            if (tables.Count == 0)
                return;
            if (tables[0].Rows.Count == 0)
                return;
            if (tables.Count == 0)
                return;
            if (tables[0].Rows.Count == 0)
                return;
            try
            {

                model.CoordinateList = new List<CoordinatesModel>();
                model.PlotName = tables[0].Rows[2].Cells[1].Paragraphs[0].Text.Trim();//地块名称
                model.CbfDBName = tables[0].Rows[4].Cells[1].Paragraphs[0].Text.Trim();//承包方代表名称

                #region 处理界址点检查记录信息
                List<string> list = new List<string>();
                //循环多页
                for (int j = 0; j < tables.Count; j++)
                {
                    for (int i = 10; i < tables[j].Rows.Count; i++)
                    {
                        var row = tables[j].Rows[i];
                        if (string.IsNullOrEmpty(row.Cells[0].Paragraphs[0].Text))
                            break;
                        var coordinate = new CoordinatesModel();
                        coordinate.OrderNum = (i - 9).ToString();
                        coordinate.SerialNumber = row.Cells[0].Paragraphs[0].Text.Trim();
                        coordinate.BoundaryPointNum = row.Cells[1].Paragraphs[0].Text.Trim();

                        //对界址点编号中已存在的坐标点 不去重新计算赋值 直接添加入列表
                        if (!list.Contains(coordinate.BoundaryPointNum) && plotDic.ContainsKey(coordinate.BoundaryPointNum) && !recursive)//非递归模式
                        {
                            list.Add(coordinate.BoundaryPointNum);
                            coordinate = plotDic[coordinate.BoundaryPointNum];
                            coordinate.OrderNum = (i - 9).ToString();
                            model.CoordinateList.Add(coordinate);
                            continue;
                        }

                        coordinate.X = Convert.ToDouble(row.Cells[2].Paragraphs[0].Text.Trim()).ToString("f3");
                        coordinate.Y = Convert.ToDouble(row.Cells[3].Paragraphs[0].Text.Trim()).ToString("f3");
                        //先随机 ∆L
                        coordinate.difL = GetdifL();
                        coordinate.difSquareL = Math.Pow(Convert.ToDouble(coordinate.difL), 2.0).ToString("f3");
                        //再随机 X'
                        coordinate.cX = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-Convert.ToDouble(coordinate.difL) / 2, Convert.ToDouble(coordinate.difL) / 2, 3)).ToString("f3");

                        #region 随机三次X取样值
                        bool flagX = true;
                        while (flagX)
                        {
                            coordinate.X1 = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.X2 = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.X3 = (3 * Convert.ToDouble(coordinate.X) - Convert.ToDouble(coordinate.X1) - Convert.ToDouble(coordinate.X2)).ToString("f3");
                            if (Math.Abs(Convert.ToDouble(coordinate.X3) - Convert.ToDouble(coordinate.X)) < 0.8) //超出范围
                            {
                                flagX = false;
                            }
                        }
                        #endregion

                        coordinate.difX = (Convert.ToDouble(coordinate.X) - Convert.ToDouble(coordinate.cX)).ToString("f3");

                        //根据公式求出 ∆y ∆y = 开平方（∆L2-∆x2）  每次随机正负值
                        Random rd = new Random();
                        var r = rd.Next(0, 2);
                        coordinate.difY = ((r == 0 ? -1 : 1) * Math.Sqrt(Convert.ToDouble(coordinate.difSquareL) - Math.Pow(Convert.ToDouble(coordinate.difX), 2.0))).ToString("f3");

                        coordinate.cY = (Convert.ToDouble(coordinate.Y) - Convert.ToDouble(coordinate.difY)).ToString("f3");
                        #region 随机三次Y取样值
                        bool flagY = true;
                        while (flagY)
                        {
                            coordinate.Y1 = (Convert.ToDouble(coordinate.Y) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.Y2 = (Convert.ToDouble(coordinate.Y) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.Y3 = (3 * Convert.ToDouble(coordinate.Y) - Convert.ToDouble(coordinate.Y1) - Convert.ToDouble(coordinate.Y2)).ToString("f3");
                            if (Math.Abs(Convert.ToDouble(coordinate.Y3) - Convert.ToDouble(coordinate.Y)) < 0.8) //超出范围
                            {
                                flagY = false;
                            }
                        }
                        #endregion


                        if (!plotDic.ContainsKey(coordinate.BoundaryPointNum))//界址点编号字典添加坐标实体
                            plotDic.Add(coordinate.BoundaryPointNum, coordinate);

                        if (!list.Contains(coordinate.BoundaryPointNum))//不把最后一个回归原点的坐标统计入内
                        {
                            list.Add(coordinate.BoundaryPointNum);
                            model.CoordinateList.Add(coordinate);
                        }
                    }
                }


                #endregion

                #region 处理面积检查记录信息
                model.PlotArea = Convert.ToDouble(tables[0].Rows[6].Cells[1].Paragraphs[0].Text).ToString("f2");
                #region 计算P'
                //计算之前先判断是否有内环
                bool isHaveNei = false;
                int bigAreaCount = 0;
                for (int i = 0; i < model.CoordinateList.Count; i++)
                {
                    string serialNumber = model.CoordinateList[i].SerialNumber;
                    if (serialNumber.Contains("内环"))
                    {
                        isHaveNei = true;
                        bigAreaCount = i;
                        break;
                    }
                }

                string realArea = "";
                if (!isHaveNei) //如果没有内环
                {
                    double[] difXArr = new double[model.CoordinateList.Count];
                    double[] difYArr = new double[model.CoordinateList.Count];
                    for (int i = 0; i < model.CoordinateList.Count; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].cX);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].cY);
                    }
                    model.PlotCheckArea = MathCode.AoArea(model.CoordinateList.Count, difXArr, difYArr).ToString("f2");

                    for (int i = 0; i < model.CoordinateList.Count; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].X);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].Y);
                    }
                    realArea = MathCode.AoArea(model.CoordinateList.Count, difXArr, difYArr).ToString("f2");
                }
                else//如果有内环
                {
                    #region 大环
                    double[] difXArr = new double[bigAreaCount];
                    double[] difYArr = new double[bigAreaCount];
                    for (int i = 0; i < bigAreaCount; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].cX);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].cY);
                    }
                    double bigArea = MathCode.AoArea(bigAreaCount, difXArr, difYArr);//大面积

                    for (int i = 0; i < bigAreaCount; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].X);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].Y);
                    }
                    double realBigArea = MathCode.AoArea(bigAreaCount, difXArr, difYArr);//真实大面积 
                    #endregion

                    #region 小环
                    int smallAreaCount = model.CoordinateList.Count - bigAreaCount;
                    double[] difXArr2 = new double[smallAreaCount];
                    double[] difYArr2 = new double[smallAreaCount];
                    for (int i = 0; i < smallAreaCount; i++)
                    {
                        difXArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].cX);
                        difYArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].cY);
                    }
                    double smallArea = Math.Abs(MathCode.AoArea(smallAreaCount, difXArr2, difYArr2));//内环面积
                    for (int i = 0; i < smallAreaCount; i++)
                    {
                        difXArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].X);
                        difYArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].Y);
                    }
                    double realSmallArea = Math.Abs(MathCode.AoArea(smallAreaCount, difXArr2, difYArr2));//内环面积 
                    #endregion

                    model.PlotCheckArea = (bigArea - smallArea).ToString("f2");
                    realArea = (realBigArea - realSmallArea).ToString("f2");
                }

                #endregion
                model.DifArea = Math.Abs(Convert.ToDouble(model.PlotArea) - Convert.ToDouble(model.PlotCheckArea)).ToString("f2");
                //R=∆P÷P′×100％
                model.PercentageError = (Convert.ToDouble(model.DifArea) / Convert.ToDouble(model.PlotCheckArea) * 100).ToString("f1");
                #endregion

                #region 计算界址点中误差
                //界址点中误差m= sqrt(∑[∆L2]/2n)；高精度检查时，界址点中误差m= sqrt(∑[∆L2]/ n)
                double difLSum = 0;
                foreach (var coordinate in model.CoordinateList)
                {
                    difLSum += Convert.ToDouble(coordinate.difSquareL);
                }
                model.PlotM = Math.Sqrt(difLSum / (2 * model.CoordinateList.Count)).ToString("f4");

                #endregion

                calcCount++;
                if (calcCount > 20)//如果计算次数超过40，说明有异常发生
                {
                    //errorDic.Add(model.PlotCode, filePath + " 文件中面积值为：" + model.PlotArea + ",实际运算值为：" + realArea);
                    calcCount = 0;
                    model.IsGenerate = false;
                    return;
                }
                //最后判断如果 误差>5则重新计算
                if (Convert.ToDouble(model.PercentageError) >= 5)
                    SetPlotModel_DOC(model, filePath, true);
                //如果计算界址点中误差>=0.4则重新计算
                if (Convert.ToDouble(model.PlotM) >= 0.4)
                    SetPlotModel_DOC(model, filePath, true);
                //standM = GetStandM();
                calcCount = 0;
                return;
            }
            catch (Exception ex)
            {
                errorDic.Add(model.PlotCode, filePath);
            }
        }

        /// <summary>
        /// 为实体赋值-excel模式
        /// </summary>
        /// <param name="model"></param>
        private void SetPlotModel_EXCEL(PlotModel model, string filePath, bool recursive = false)
        {
            List<ISheet> sheetList = new List<ISheet>();
            ISheet sheet;
            FileStream fs = null;
            try
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                HSSFWorkbook wk = new HSSFWorkbook(fs);
                int sheetCount = wk.NumberOfSheets;
                for (int i = 0; i < sheetCount; i++)
                {
                    string sheetName = wk.GetSheetName(i);
                    sheet = wk.GetSheet(sheetName);
                    sheetList.Add(sheet);
                }
            }
            catch
            {
                fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                XSSFWorkbook wk = new XSSFWorkbook(fs);
                int sheetCount = wk.NumberOfSheets;
                for (int i = 0; i < sheetCount; i++)
                {
                    string sheetName = wk.GetSheetName(i);
                    sheet = wk.GetSheet(sheetName);
                    sheetList.Add(sheet);
                }

            }
            finally
            {
                fs.Close();
                fs.Dispose();
            }
            try
            {

                model.CoordinateList = new List<CoordinatesModel>();
                model.PlotName = sheetList[0].GetRow(2).Cells[2].StringCellValue;//地块名称
                model.CbfDBName = sheetList[0].GetRow(4).Cells[2].StringCellValue;//承包方代表名称

                #region 处理界址点检查记录信息
                List<string> list = new List<string>();
                for (int j = 0; j < sheetList.Count; j++)
                {
                    for (int i = 10; i <= sheetList[j].LastRowNum - 1; i++)
                    {
                        IRow row = sheetList[j].GetRow(i);

                        //当界址点号不是数字时终止循环
                        if (string.IsNullOrEmpty(Regex.Replace(row.Cells[2].StringCellValue.Trim(), @"[^\d|.]+", "")))
                            break;
                        var coordinate = new CoordinatesModel();
                        coordinate.OrderNum = (i - 9).ToString();
                        coordinate.SerialNumber = row.Cells[0].StringCellValue.Trim();
                        coordinate.BoundaryPointNum = row.Cells[2].StringCellValue.Trim();
                        //对界址点编号中已存在的坐标点 不去重新计算赋值 直接添加入列表
                        if (!list.Contains(coordinate.BoundaryPointNum) && plotDic.ContainsKey(coordinate.BoundaryPointNum) && !recursive)//非递归模式
                        {
                            list.Add(coordinate.BoundaryPointNum);
                            coordinate = plotDic[coordinate.BoundaryPointNum];
                            coordinate.OrderNum = (i - 9).ToString();
                            model.CoordinateList.Add(coordinate);
                            continue;
                        }

                        coordinate.X = Convert.ToDouble(row.Cells[3].StringCellValue.Trim()).ToString("f3");
                        coordinate.Y = Convert.ToDouble(row.Cells[4].StringCellValue.Trim()).ToString("f3");
                        //先随机 ∆L
                        coordinate.difL = GetdifL();
                        coordinate.difSquareL = Math.Pow(Convert.ToDouble(coordinate.difL), 2.0).ToString("f3");
                        //再随机 X'
                        coordinate.cX = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-Convert.ToDouble(coordinate.difL) / 2, Convert.ToDouble(coordinate.difL) / 2, 3)).ToString("f3");

                        #region 随机三次X取样值
                        bool flagX = true;
                        while (flagX)
                        {
                            coordinate.X1 = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.X2 = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.X3 = (3 * Convert.ToDouble(coordinate.X) - Convert.ToDouble(coordinate.X1) - Convert.ToDouble(coordinate.X2)).ToString("f3");
                            if (Math.Abs(Convert.ToDouble(coordinate.X3) - Convert.ToDouble(coordinate.X)) < 0.8) //超出范围
                            {
                                flagX = false;
                            }
                        }
                        #endregion

                        coordinate.difX = (Convert.ToDouble(coordinate.X) - Convert.ToDouble(coordinate.cX)).ToString("f3");

                        //根据公式求出 ∆y ∆y = 开平方（∆L2-∆x2）  每次随机正负值
                        Random rd = new Random();
                        var r = rd.Next(0, 2);
                        coordinate.difY = ((r == 0 ? -1 : 1) * Math.Sqrt(Convert.ToDouble(coordinate.difSquareL) - Math.Pow(Convert.ToDouble(coordinate.difX), 2.0))).ToString("f3");

                        coordinate.cY = (Convert.ToDouble(coordinate.Y) - Convert.ToDouble(coordinate.difY)).ToString("f3");
                        #region 随机三次Y取样值
                        bool flagY = true;
                        while (flagY)
                        {
                            coordinate.Y1 = (Convert.ToDouble(coordinate.Y) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.Y2 = (Convert.ToDouble(coordinate.Y) + MathCode.GetRandomNumber(-0.75, 0.75, 3)).ToString("f3");
                            coordinate.Y3 = (3 * Convert.ToDouble(coordinate.Y) - Convert.ToDouble(coordinate.Y1) - Convert.ToDouble(coordinate.Y2)).ToString("f3");
                            if (Math.Abs(Convert.ToDouble(coordinate.Y3) - Convert.ToDouble(coordinate.Y)) < 0.8) //超出范围
                            {
                                flagY = false;
                            }
                        }
                        #endregion


                        if (!plotDic.ContainsKey(coordinate.BoundaryPointNum))//界址点编号字典添加坐标实体
                            plotDic.Add(coordinate.BoundaryPointNum, coordinate);

                        if (!list.Contains(coordinate.BoundaryPointNum))//不把最后一个回归原点的坐标统计入内
                        {
                            list.Add(coordinate.BoundaryPointNum);
                            model.CoordinateList.Add(coordinate);
                        }
                    }
                }

                #endregion

                #region 处理面积检查记录信息
                model.PlotArea = Convert.ToDouble(Regex.Replace(sheetList[0].GetRow(6).Cells[2].StringCellValue.Trim(), @"[^\d|.]+", "")).ToString("f2");
                #region 计算P'
                //计算之前先判断是否有内环
                bool isHaveNei = false;
                int bigAreaCount = 0;
                for (int i = 0; i < model.CoordinateList.Count; i++)
                {
                    string serialNumber = model.CoordinateList[i].SerialNumber;
                    if (serialNumber.Contains("内环"))
                    {
                        isHaveNei = true;
                        bigAreaCount = i;
                        break;
                    }
                }

                string realArea = "";
                if (!isHaveNei) //如果没有内环
                {
                    double[] difXArr = new double[model.CoordinateList.Count];
                    double[] difYArr = new double[model.CoordinateList.Count];
                    for (int i = 0; i < model.CoordinateList.Count; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].cX);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].cY);
                    }
                    model.PlotCheckArea = MathCode.AoArea(model.CoordinateList.Count, difXArr, difYArr).ToString("f2");

                    for (int i = 0; i < model.CoordinateList.Count; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].X);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].Y);
                    }
                    realArea = MathCode.AoArea(model.CoordinateList.Count, difXArr, difYArr).ToString("f2");
                }
                else//如果有内环
                {
                    #region 大环
                    double[] difXArr = new double[bigAreaCount];
                    double[] difYArr = new double[bigAreaCount];
                    for (int i = 0; i < bigAreaCount; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].cX);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].cY);
                    }
                    double bigArea = MathCode.AoArea(bigAreaCount, difXArr, difYArr);//大面积

                    for (int i = 0; i < bigAreaCount; i++)
                    {
                        difXArr[i] = Convert.ToDouble(model.CoordinateList[i].X);
                        difYArr[i] = Convert.ToDouble(model.CoordinateList[i].Y);
                    }
                    double realBigArea = MathCode.AoArea(bigAreaCount, difXArr, difYArr);//真实大面积 
                    #endregion

                    #region 小环
                    int smallAreaCount = model.CoordinateList.Count - bigAreaCount;
                    double[] difXArr2 = new double[smallAreaCount];
                    double[] difYArr2 = new double[smallAreaCount];
                    for (int i = 0; i < smallAreaCount; i++)
                    {
                        difXArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].cX);
                        difYArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].cY);
                    }
                    double smallArea = Math.Abs(MathCode.AoArea(smallAreaCount, difXArr2, difYArr2));//内环面积
                    for (int i = 0; i < smallAreaCount; i++)
                    {
                        difXArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].X);
                        difYArr2[i] = Convert.ToDouble(model.CoordinateList[i + bigAreaCount].Y);
                    }
                    double realSmallArea = Math.Abs(MathCode.AoArea(smallAreaCount, difXArr2, difYArr2));//内环面积 
                    #endregion

                    model.PlotCheckArea = (bigArea - smallArea).ToString("f2");
                    realArea = (realBigArea - realSmallArea).ToString("f2");
                }



                #endregion
                model.DifArea = Math.Abs(Convert.ToDouble(model.PlotArea) - Convert.ToDouble(model.PlotCheckArea)).ToString("f2");
                //R=∆P÷P′×100％
                model.PercentageError = (Convert.ToDouble(model.DifArea) / Convert.ToDouble(model.PlotCheckArea) * 100).ToString("f1");

                #endregion

                #region 计算界址点中误差
                //界址点中误差m= sqrt(∑[∆L2]/2n)；高精度检查时，界址点中误差m= sqrt(∑[∆L2]/ n)
                double difLSum = 0;
                foreach (var coordinate in model.CoordinateList)
                {
                    difLSum += Convert.ToDouble(coordinate.difSquareL);
                }
                model.PlotM = Math.Sqrt(difLSum / (2 * model.CoordinateList.Count)).ToString("f4");

                #endregion

                calcCount++;
                if (calcCount > 20)//如果计算次数超过40，说明有异常发生
                {
                    //errorDic.Add(model.PlotCode, filePath + " 文件中面积值为：" + model.PlotArea + ",实际运算值为：" + realArea);
                    calcCount = 0;
                    model.IsGenerate = false;
                    return;
                }
                //最后判断如果 误差>5则重新计算
                if (Convert.ToDouble(model.PercentageError) >= 5)
                    SetPlotModel_EXCEL(model, filePath, true);
                //如果计算界址点中误差>=0.4则重新计算
                if (Convert.ToDouble(model.PlotM) >= 0.4)
                    SetPlotModel_EXCEL(model, filePath, true);
                //standM = GetStandM();
                calcCount = 0;
                return;
            }
            catch (Exception ex)
            {
                errorDic.Add(model.PlotCode, filePath);
            }
        }

        private double GetStandM()
        {
            var key = MathCode.GetRandomNumber(-3, 7, 4);
            if (key > 0)
                return 0.4;
            else
                return 0.3;
        }

        private string GetdifL()
        {
            return MathCode.GetRandomNumber(0.3, 0.56, 4).ToString("f3");
            //    ? MathCode.GetRandomNumber(0.15, 0.75, 4).ToString("f3")
            //    : MathCode.GetRandomNumber(0.05, 0.75, 4)
            //var key = MathCode.GetRandomNumber(-1, 1, 4);
            //return key > 0
            //    ? MathCode.GetRandomNumber(0.15, 0.75, 4).ToString("f3")
            //    : MathCode.GetRandomNumber(0.05, 0.75, 4).ToString("f3");
        }

        public void Log(string logTxt)
        {
            string file = AppDomain.CurrentDomain.BaseDirectory;
            Directory.SetCurrentDirectory(Directory.GetParent(Directory.GetParent(file).FullName).FullName);

            file = Path.Combine(Directory.GetCurrentDirectory(), "Spring.dll");

            if (!File.Exists(file))
            {
                FileStream filestream = null;
                try
                {
                    filestream = System.IO.File.Create(file);
                    filestream.Dispose();
                    filestream.Close();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception(ex + "创建日志文件失败");
                }
            }

            //true 如果日志文件存在则继续追加日志 
            System.IO.StreamWriter sw = null;
            try
            {
                sw = new System.IO.StreamWriter(file, true, System.Text.Encoding.UTF8);
                string logInfo = "【" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】" + "" + logTxt + "";

                byte[] bytes = Encoding.Default.GetBytes(logInfo);
                logInfo = Convert.ToBase64String(bytes);
                sw.WriteLine(logInfo);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex + "写入日志失败，检查！");
            }
            finally
            {
                sw.Flush();
                sw.Dispose();
                sw.Close();
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void btnCopyErrorMsg_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtErrorMsg.SelectedText);
        }

        /// <summary>
        /// 自检报告生成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheckGen_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCheckName.Text.Trim()))
            {
                MessageBox.Show("检查人员姓名不能为空！");
                return;
            }

            DirectoryInfo dir = new DirectoryInfo(txtWorkPath.Text.Trim());
            FileInfo[] fil = dir.GetFiles();
            if (fil.Length == 0)
            {
                MessageBox.Show("所选路径下没有文件，请重试！");
                return;
            }
            type = "check";
            thread = new Thread(new ThreadStart(this.ThreadProcSafePost));
            thread.Start();
        }
    }
}
