using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using UtilityCode;

namespace PDFTools
{
    public partial class Form1 : Form
    {

        private int processTotalCount = 0;
        private int currentpdfIndex = 0;
        private int pdfCount = 0;//pdf文件总数
        private int photoCount = 0;//图片总数
        private int level;//目录级别
        private Dictionary<string, List<string>> plotDict = new Dictionary<string, List<string>>();
        private string keyVal = "sinldo.com";
        private string ivVal = "http://www.sinldo.com";


        public Form1()
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(PublicCode.UserType) && !PublicCode.CheckRegCode())
            {
                MessageBox.Show("非法访问!");
                return;
            }

            if (PublicCode.UserType == "admin")//管理员允许查看操作日志
                btnQueryHistory.Visible = true;

        }

        #region Event
        private void btnSetWordPath_Click(object sender, EventArgs e)
        {
            string num = GetSerial.getMNum();

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置文件夹生成路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtWorkPath.Text = foldPath;
            }
        }

        private void btnSelectPlotCode_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count <= 1)
            {
                MessageBox.Show("尚未添加组信息！");
                return;
            }

            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择界址点成果表操作目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtPlotPath.Text = foldPath;

                GetPlotCodeDict(foldPath);
                ShowErrorMsg();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;
            string path = txtCombinePath.Text.Trim();
            var dirList = new List<string>();
            #region 处理层级结构
            if (level == 2)
            {
                var dirInfo = new DirectoryInfo(path);
                dirList.Add(dirInfo.FullName);
            }
            if (level == 3)
            {
                var dirInfo = new DirectoryInfo(path);
                var dirii = dirInfo.GetDirectories();
                foreach (DirectoryInfo ii in dirii)
                {
                    dirList.Add(ii.FullName);
                }
            }
            #endregion

            string currentFilePath = "";

            try
            {

                foreach (var str in dirList)
                {
                    #region 发包方处理

                    string fbfPath = Path.Combine(str, "发包方");
                    DirectoryInfo dir = new DirectoryInfo(fbfPath);
                    DirectoryInfo[] dirArr = dir.GetDirectories();
                    foreach (DirectoryInfo dirItem in dirArr)
                    {
                        try
                        {
                            string dirName = dirItem.Name;
                            currentFilePath = dirItem.FullName;
                            var addFlag = CombineToPDF.CheckPdfJpg(dirItem);
                            CombineToPDF.dirPhoto2Pdf(dirItem, addFlag); //处理图片转PDF
                            string[] fileArr = CombineToPDF.getFileArr(dirItem); //获取PDF文件

                            if (fileArr.Length == 0)
                                continue;

                            string pdfN = dirName.Split('#')[1].Trim();
                            string newPdfName = Path.Combine(fbfPath, pdfN + ".pdf");
                            PdfUtility.MergePDF(fileArr, newPdfName);

                            int fileCount = dir.GetFiles().Length; //获取文件夹下是否有文件，如果没有任何文件则不进行删除
                            if (ckIsDelete.Checked && fileCount > 0) //文件夹下如果没有任何文件则不进行删除
                                Directory.Delete(Path.Combine(fbfPath, dirName), true);
                            currentpdfIndex += fileArr.Length;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("【" + dirItem.FullName + "】路径下文件命名格式有误，请检查！");
                            throw ex;
                        }

                        int progeress = currentpdfIndex * 100 / processTotalCount;
                        worker.ReportProgress(progeress);
                        if (progeress == 100) // 如果用户取消则跳出处理数据代码 
                        {
                            e.Cancel = true;
                            ShowCompletePath();
                            break;
                        }
                    }

                    #endregion

                    #region 承包方处理

                    string cbfPath = Path.Combine(str, "承包方");
                    DirectoryInfo cbfDir = new DirectoryInfo(cbfPath);
                    DirectoryInfo[] cbfDirArr = cbfDir.GetDirectories();
                    foreach (DirectoryInfo dirItem in cbfDirArr)
                    {
                        string dirItemName = dirItem.Name;
                        DirectoryInfo[] cbfDirArr2 = dirItem.GetDirectories();
                        foreach (DirectoryInfo item in cbfDirArr2)
                        {
                            string dirName = item.Name;
                            currentFilePath = item.FullName;
                            var addFlag = CombineToPDF.CheckPdfJpg(dirItem);
                            CombineToPDF.dirPhoto2Pdf(item, addFlag); //处理图片转PDF
                            string[] fileArr = CombineToPDF.getFileArr(item);
                            if (fileArr.Length == 0)
                                continue;

                            string pdfN = dirName.Split('#')[1].Trim();
                            //pdfN = EncryptUtil.UnAesStr(pdfN, keyVal, ivVal);
                            string newPdfName = Path.Combine(cbfPath, pdfN + ".pdf");
                            PdfUtility.MergePDF(fileArr, newPdfName);

                            int fileCount2 = item.GetFiles().Length; //获取文件夹下是否有文件，如果没有任何文件则不进行删除
                            if (ckIsDelete.Checked && fileCount2 > 0) //文件夹下如果没有任何文件则不进行删除
                                Directory.Delete(item.FullName, true);
                            currentpdfIndex += fileArr.Length;

                            int progeress = currentpdfIndex * 100 / processTotalCount;
                            worker.ReportProgress(progeress);
                            if (progeress == 100) // 如果用户取消则跳出处理数据代码 
                            {
                                e.Cancel = true;
                                ShowCompletePath();
                                break;
                            }
                        }
                        int fileCount = dirItem.GetDirectories().Length; //获取文件夹下是否有文件夹，如果没有任何文件夹则进行删除
                        if (ckIsDelete.Checked && fileCount == 0) //文件夹下如果没有任何文件则不进行删除
                            Directory.Delete(Path.Combine(cbfPath, dirItemName), true);
                    }

                    #endregion
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(currentFilePath +"文件夹下文件有误，请检查！\r\n错误信息："+ ex.Message);
                return;
            }
        }

        private void btnMakeDir_Click(object sender, EventArgs e)
        {
            string makePath = txtWorkPath.Text.Trim();//生成路径
            string countryName = txtCountryName.Text.Trim();
            makePath = Path.Combine(makePath, countryName);

            string fbfCode = txtFBFCode.Text.Trim();

            int generCount = 0;
            if (ddlAddMoudel.SelectedIndex == 0)
            {
                generCount = txtGroupInfo.GetLineFromCharIndex(txtGroupInfo.TextLength);
            }
            else
                generCount = dataGridView1.Rows.Count - 1;

            //确认填写信息
            if (MessageBox.Show(@"村名称      【" + countryName + "】\r\n发包方代码【" + fbfCode + "】\r\n本次将生成【" + generCount + "】个组信息", "确认生成信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (!Directory.Exists(makePath))
                {
                    Directory.CreateDirectory(makePath);
                }

                string logGroupInfo = string.Empty;
                for (int i = 0; i < generCount; i++)
                {
                    string fbfName = "", cbfBigCode = "", groupNum = "";
                    if (ddlAddMoudel.SelectedIndex == 0)
                    {
                        string[] arr = txtGroupInfo.Text.Split('\n');
                        var row = arr[i].TrimEnd('\r');
                        var rows = row.Split('\t');
                        fbfName = rows[0];
                        groupNum = rows[1];
                        cbfBigCode = rows[2].PadLeft(4, '0');
                    }
                    else
                    {
                        var row = dataGridView1.Rows[i];
                        fbfName = row.Cells[0].Value.ToString();
                        groupNum = row.Cells[1].Value.ToString();
                        cbfBigCode = row.Cells[2].Value.ToString();
                    }
                    fbfCode = txtFBFCode.Text.Trim() + groupNum;
                    logGroupInfo += string.Format("【({0}) 组名称：{1},组号：{2},承包方最大编码：{3}】", i + 1, fbfName, groupNum, cbfBigCode);
                    CreateDir(makePath, groupNum, fbfCode, cbfBigCode);
                }

                #region 操作完毕 给予提示

                PublicCode.Log("【自动生成文件夹操作】 村名称【" + countryName + "】发包方代码【" + fbfCode + "】" + (dataGridView1.Rows.Count - 1) + "个组信息: { " + logGroupInfo + " }");
                if (MessageBox.Show(@"是否需要打开生成目录", "操作成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
                {
                    if (Directory.Exists(makePath))
                        System.Diagnostics.Process.Start(makePath);
                }

                #endregion
            }
        }

        /// <summary>
        /// 添加组
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddGroup_Click(object sender, EventArgs e)
        {
            string groupName = txtGroupName.Text.Trim();
            string groupNum = txtGroupNum.Text.Trim();
            string maxNum = txtCBFBigCode.Text.Trim();

            dataGridView1.Rows.Add(groupName, groupNum, maxNum, "删除");
        }

        /// <summary>
        /// 选择操作路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置发包方名称的根目录";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtCombinePath.Text = foldPath;
            }
        }

        /// <summary>
        /// 合并操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCombine_Click(object sender, EventArgs e)
        {
            string path = txtCombinePath.Text.Trim();
            pdfCount = 0;
            photoCount = 0;
            currentpdfIndex = 0;
            getPath(path);
            level = CombineToPDF.GetDirLevel(path);
            string levelName = level == 2 ? "组级别" : "村级别";
            if (
                MessageBox.Show(@"已选择【" + levelName + "】目录\r\n操作路径【" + path + "】\r\n包含【" + pdfCount + "】个pdf文件，【" + photoCount + "】个图片文件\r\n是否合并处理为pdf？", "提示信息-操作前请做好备份", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
            {
                PublicCode.Log("【自动合并生成PDF文件操作】操作路径【" + path + "】级别 【" + levelName + "】包含【" + pdfCount + "】个pdf文件,【" + photoCount + "】个图片文件");
                processTotalCount = pdfCount + photoCount;
                if (pdfCount == 0 && photoCount == 0)
                {
                    MessageBox.Show("未包含任何文件，当前操作终止！");
                    return;
                }

                this.backgroundWorker1.RunWorkerAsync(); // 运行 backgroundWorker 组件

                ProcessForm form = new ProcessForm(this.backgroundWorker1);// 显示进度条窗体
                form.ShowDialog(this);
                form.Close();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            string action = dataGridView1.Columns[e.ColumnIndex].Name;//操作类型

            switch (action)
            {
                case "delete":
                    if (MessageBox.Show("确定删除吗?", "提示信息", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        //获取相应列的数据ID,删除此数据记录      
                        dataGridView1.Rows.RemoveAt(e.RowIndex);
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Unilty

        private void ShowCompletePath()
        {
            if (MessageBox.Show(@"是否需要打开生成目录", "操作成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (Directory.Exists(txtCombinePath.Text.Trim()))
                    System.Diagnostics.Process.Start(txtCombinePath.Text.Trim());
            }
        }

        /// <summary>
        /// 根据编码获取配置文件信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
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

       
        public void getPath(string path)
        {
            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                if (f.Extension == ".pdf")
                    pdfCount += 1;
                if (f.Extension == ".jpg" || f.Extension == ".png")
                    photoCount += 1;
            }
            //获取子文件夹内的文件列表，递归遍历
            foreach (DirectoryInfo d in dii)
            {
                getPath(d.FullName);
            }
        }


        /// <summary>
        /// 生成文件夹
        /// </summary>
        /// <param name="makePath"></param>
        /// <param name="fbfName"></param>
        /// <param name="fbfCode"></param>
        /// <param name="cbfBigCode"></param>
        private void CreateDir(string makePath, string fbfName, string fbfCode, string cbfBigCode)
        {
            //1.创建根目录
            string rootPath = Path.Combine(makePath, fbfName);

            CreateDir(rootPath);


            //2.创建发包方文件夹及子文件夹
            string fbfPath = Path.Combine(rootPath, "发包方");
            CreateDir(fbfPath);

            var fbfDict = GetConfigDict("FBF");
            foreach (var item in fbfDict)
            {
                string dir = Path.Combine(fbfPath, item.Value + " # " + item.Key + fbfCode);
                CreateDir(dir);
            }

            #region 3.创建承包方文件夹及子文件夹
            string cbfPath = Path.Combine(rootPath, "承包方");
            CreateDir(cbfPath);

            var cbfDict = GetConfigDict("CBF");
            var cbfPlotDict = GetConfigDict("CBFDKB");//承包方地块信息

            //先根据承包方最大简码生成多个文件夹
            int bigNum = Convert.ToInt16(cbfBigCode);
            for (int i = 1; i <= bigNum; i++)
            {
                string cbfSimpleCode = i.ToString().PadLeft(4, '0');//承包方简码
                string singlecbfPath = Path.Combine(cbfPath, cbfSimpleCode);
                CreateDir(singlecbfPath);

                string fbfFullCode = fbfCode + cbfSimpleCode;
                foreach (var item in cbfDict)
                {
                    string dir = Path.Combine(singlecbfPath, item.Value + " # " + item.Key + fbfFullCode);
                    CreateDir(dir);
                }

                //处理地块编码信息
                foreach (var item in cbfPlotDict)
                {
                    var plotKey = item.Key;
                    var plotValue = item.Value;
                    if (plotDict.ContainsKey(fbfFullCode))
                    {
                        var plotList = plotDict[fbfFullCode]; //通过承包方代码查找地块编码
                        foreach (var plotCode in plotList)
                        {
                            string dir = Path.Combine(singlecbfPath, plotValue + " # " + plotKey + plotCode);
                            CreateDir(dir);
                        }
                    }
                }

            }
            #endregion

        }

        /// <summary>
        /// 根据路径创建文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        private void CreateDir(string dirPath)
        {
            //#region 处理文件夹加密信息

            //if (dirPath.IndexOf("#")!=-1)
            //{
            //    string code = dirPath.Split('#')[1];
            //    var encrypCode = EncryptUtil.AesStr(code, keyVal, ivVal);

            //    dirPath = dirPath.Substring(0, dirPath.IndexOf("#")) + "# "+ encrypCode;
            //}

            //#endregion

            if (!Directory.Exists(dirPath))
                Directory.CreateDirectory(dirPath);
            else
            {
                Directory.Delete(dirPath, true);
                Directory.CreateDirectory(dirPath);
            }
        }

        /// <summary>
        /// 从界址点成果表文件名获取 地块编码与承包方代码的关系
        /// </summary>
        private void GetPlotCodeDict(string dictPath)
        {
            DirectoryInfo dir = new DirectoryInfo(dictPath);
            FileInfo[] fil = dir.GetFiles();
            DirectoryInfo[] dii = dir.GetDirectories();
            foreach (FileInfo f in fil)
            {
                string fileName = Path.GetFileNameWithoutExtension(f.FullName);
                string cbfCode = "";
                string plotCode = "";
                if (comboBox1.SelectedIndex == 0)
                {
                    cbfCode = fileName.Split('_')[0];
                    plotCode = fileName.Split('_')[2];
                }
                else if (comboBox1.SelectedIndex == 1)
                {
                    cbfCode = fileName.Split('_')[0];
                    string[] arr = fileName.Split('_');
                    plotCode = Regex.Replace(arr[arr.Length - 1], @"[^0-9]+", "");
                }
                if (!plotDict.ContainsKey(cbfCode))
                {
                    var list = new List<string>();
                    list.Add(plotCode);
                    plotDict.Add(cbfCode, list);
                }
                else
                {
                    var list = plotDict[cbfCode];
                    if (!list.Contains(plotCode))
                        list.Add(plotCode);
                    plotDict[cbfCode] = list;
                }
                //获取子文件夹内的文件列表，递归遍历
                foreach (DirectoryInfo d in dii)
                {
                    GetPlotCodeDict(d.FullName);
                }
            }

        }

        /// <summary>
        /// 显示报错信息
        /// </summary>
        private void ShowErrorMsg()
        {
            var sb = new StringBuilder();

            for (int i = 0; i < dataGridView1.Rows.Count - 1; i++)
            {
                string fbfCode = txtFBFCode.Text.Trim();
                var row = dataGridView1.Rows[i];
                string fbfGroupName = row.Cells[0].Value.ToString();
                string cbfBigCode = row.Cells[2].Value.ToString();
                fbfCode += row.Cells[1].Value.ToString();

                int bigNum = Convert.ToInt16(cbfBigCode);
                for (int j = 1; j <= bigNum; j++)
                {
                    string cbfSimpleCode = j.ToString().PadLeft(4, '0');//承包方简码
                    string fbfSingleCode = fbfCode + cbfSimpleCode;
                    if (!plotDict.ContainsKey(fbfSingleCode))//判断地块编码字典中是否存在该承包方户信息，不存在，则添加错误信息
                    {
                        sb.AppendFormat("村信息：【{0}】,组信息：【{1}】,户信息【{2}】不存在界址点成果表，请检查数据！\r\n", txtCountryName.Text.Trim(),
                            fbfGroupName, cbfSimpleCode);
                    }
                }
            }
            txtErrorMsg.Text = "";
            txtErrorMsg.Text = sb.ToString();
        }


        #endregion

        /// <summary>
        /// 软件使用情况查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQueryHistory_Click(object sender, EventArgs e)
        {
            LogInfo logForm = new LogInfo();
            logForm.ShowDialog();
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();//程序退出
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
                lbTxtStyle.Text = "文件名格式：<承包方编码>_<地块编码>_界址点成果表";
            if (comboBox1.SelectedIndex == 1)
                lbTxtStyle.Text = "文件名格式：<承包方编码>_<姓名>_界址点成果表<地块编码>";
        }

        /// <summary>
        /// 开启界址点面积精度评价表生成工具
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnOpenGenerateTools_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.WorkingDirectory = Path.Combine(Application.StartupPath, "generatetools"); //要启动程序路径

            p.StartInfo.FileName = "GenerateTools"; //需要启动的程序名   
                                                    //获得文件夹名称
            p.StartInfo.Arguments = "sinldo.com"; //传递的参数       
            p.Start(); //启动  
        }


    }
}
