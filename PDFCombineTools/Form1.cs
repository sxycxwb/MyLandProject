using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UtilityCode;

namespace PDFCombineTools
{
    public partial class Form1 : Form
    {
        private int processTotalCount = 0;
        private int currentpdfIndex = 0;
        private int pdfCount = 0;//pdf文件总数
        private int photoCount = 0;//图片总数
        private int level;//目录级别
        private string keyVal = "sinldo.com";
        private string ivVal = "http://www.sinldo.com";
        List<string> list = new List<string>();  

        public Form1()
        {
            InitializeComponent();
            try
            {
                string[] arr = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spire.Excel.dll"));
                foreach (var str in arr)
                {
                    list.Add(EncryptUtil.UnAesStr(str, keyVal, ivVal));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("非法访问!");
                return;
            }

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
                //PublicCode.Log("【自动合并生成PDF文件操作】操作路径【" + path + "】级别 【" + levelName + "】包含【" + pdfCount + "】个pdf文件,【" + photoCount + "】个图片文件");
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
                            if (!CheckNum(pdfN))
                            {
                                MessageBox.Show("您正在处理非授权数据，请联系管理员！");
                                return;
                            }
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
                            if (!CheckNum(pdfN))
                            {
                                MessageBox.Show("您正在处理非授权数据，请联系管理员！");
                                return;
                            }
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
                MessageBox.Show(currentFilePath + "文件夹下文件有误，请检查！\r\n错误信息：" + ex.Message);
                return;
            }
        }

        private bool CheckNum(string num)
        {
            bool returnVal = false;
            foreach (var str in list)
            {
                if (num.IndexOf(str) > -1)
                {
                    returnVal = true;
                    break;
                }
            }
            return returnVal;
        }

        private void ShowCompletePath()
        {
            if (MessageBox.Show(@"是否需要打开生成目录", "操作成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (Directory.Exists(txtCombinePath.Text.Trim()))
                    System.Diagnostics.Process.Start(txtCombinePath.Text.Trim());
            }
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

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();//程序退出
        }
    }
}
