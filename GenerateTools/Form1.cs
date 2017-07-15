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
using Spire.Doc;
using Spire.Doc.Collections;
using Newtonsoft.Json;
using Spring.Model;

namespace GenerateTools
{
    public partial class Form1 : Form
    {
        List<PlotModel> plotList = new List<PlotModel>();
        private int totalCount = 0;
        private int currentIndex = 0;

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
            thread = new Thread(new ThreadStart(this.ThreadProcSafePost));
            thread.Start();
        }

        /// <summary>
        /// 线程的主体方法
        /// </summary>
        private void ThreadProcSafePost()
        {
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
                string cbfCode = fileName.Split('_')[0]; //承包方代码
                string plotCode = fileName.Split('_')[1]; //地块代码
                plotModel.CbfCode = cbfCode;
                plotModel.PlotCode = plotCode;
                SetPlotModel(plotModel, f.FullName);
                plotList.Add(plotModel);
                m_SyncContext.Post(UpdateUIProcess, currentIndex * 100 / totalCount);
            }
            //将结果集持久化保存json
            string json = JsonConvert.SerializeObject(plotList);
            string rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "excel2pdf");
            if (!Directory.Exists(rootPath))
                Directory.CreateDirectory(rootPath);

            File.WriteAllText("excel2pdf/coordinates.json", json);

            if (
                MessageBox.Show("采集数据完成，工采集【" + plotList.Count + "】条数据\r\n是否进行生成PDF操作？", "采集数据完成",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) ==
                DialogResult.OK)
            {
                System.Diagnostics.Process p = new System.Diagnostics.Process();
                p.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "excel2pdf"); //要启动程序路径

                p.StartInfo.FileName = "Excel2Pdf"; //需要启动的程序名   
                //获得文件夹名称
                var dirName = txtWorkPath.Text.Trim().Substring(txtWorkPath.Text.Trim().LastIndexOf(@"\") + 1);
                p.StartInfo.Arguments = dirName.Trim().Replace(" ", ""); //传递的参数       
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
        /// 为实体赋值
        /// </summary>
        /// <param name="model"></param>
        private void SetPlotModel(PlotModel model, string filePath,bool recursive=false)
        {
            Document document = new Document();
            document.LoadFromFile(filePath);
            TableCollection tables = document.Sections[0].Tables;
            if (tables.Count == 0)
                return;
            if (tables[0].Rows.Count == 0)
                return;
            if (tables.Count == 0)
                return;
            if (tables[0].Rows.Count == 0)
                return;

            model.CoordinateList = new List<CoordinatesModel>();

            if (model.PlotCode == "1408301022090400002")
            {
                string a = "1";
            }

            #region 处理界址点检查记录信息
            List<string> list = new List<string>();
            for (int i = 10; i < tables[0].Rows.Count; i++)
            {
                var row = tables[0].Rows[i];
                if (string.IsNullOrEmpty(row.Cells[0].Paragraphs[0].Text))
                    break;
                var coordinate = new CoordinatesModel();
                coordinate.OrderNum = (i - 9).ToString();
                coordinate.SerialNumber = row.Cells[0].Paragraphs[0].Text.Trim();
                coordinate.BoundaryPointNum = row.Cells[1].Paragraphs[0].Text.Trim();

                //对界址点编号中已存在的坐标点 不去重新计算赋值 直接添加入列表
                if (!list.Contains(coordinate.BoundaryPointNum)&&plotDic.ContainsKey(coordinate.BoundaryPointNum) && !recursive)//非递归模式
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
                coordinate.difL = MathCode.GetRandomNumber(0.05, 0.35, 4).ToString("f3");
                coordinate.difSquareL = Math.Pow(Convert.ToDouble(coordinate.difL), 2.0).ToString("f3");
                //再随机 X'
                coordinate.cX = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-Convert.ToDouble(coordinate.difL) / 4, Convert.ToDouble(coordinate.difL) / 4, 3)).ToString("f3");
                coordinate.difX = (Convert.ToDouble(coordinate.X) - Convert.ToDouble(coordinate.cX)).ToString("f3");

                //根据公式求出 ∆y ∆y = 开平方（∆L2-∆x2）  每次随机正负值
                Random rd = new Random();
                var r = rd.Next(0, 2);
                coordinate.difY = ((r == 0 ? -1 : 1) * Math.Sqrt(Convert.ToDouble(coordinate.difSquareL) - Math.Pow(Convert.ToDouble(coordinate.difX), 2.0))).ToString("f3");

                coordinate.cY = (Convert.ToDouble(coordinate.Y) - Convert.ToDouble(coordinate.difY)).ToString("f3");

                if (!plotDic.ContainsKey(coordinate.BoundaryPointNum))//界址点编号字典添加坐标实体
                    plotDic.Add(coordinate.BoundaryPointNum, coordinate);

                if (!list.Contains(coordinate.BoundaryPointNum))//不把最后一个回归原点的坐标统计入内
                {
                    list.Add(coordinate.BoundaryPointNum);
                    model.CoordinateList.Add(coordinate);
                }


            }
            #endregion

            #region 处理面积检查记录信息
            model.PlotArea = Convert.ToDouble(tables[0].Rows[6].Cells[1].Paragraphs[0].Text).ToString("f2");
            #region 计算P'
            double[] difXArr = new double[model.CoordinateList.Count];
            double[] difYArr = new double[model.CoordinateList.Count];
            for (int i = 0; i < model.CoordinateList.Count; i++)
            {
                difXArr[i] = Convert.ToDouble(model.CoordinateList[i].cX);
                difYArr[i] = Convert.ToDouble(model.CoordinateList[i].cY);
            }
            model.PlotCheckArea = MathCode.AoArea(model.CoordinateList.Count, difXArr, difYArr).ToString("f2");

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
                difLSum += Convert.ToDouble(coordinate.difL);
            }
            model.PlotM = (difLSum / (2 * model.CoordinateList.Count)).ToString("f8");

            #endregion

            //最后判断如果 误差>5则重新计算
            if (Convert.ToDouble(model.PercentageError) >= 5)
                SetPlotModel(model, filePath,true);
        }
    }
}
