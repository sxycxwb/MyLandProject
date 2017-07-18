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

        public Form1()
        {
            InitializeComponent();
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
            getPath(path);
            level = GetDirLevel(path);
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
            foreach (var str in dirList)
            {
                #region 发包方处理
                string fbfPath = Path.Combine(str, "发包方");
                DirectoryInfo dir = new DirectoryInfo(fbfPath);
                DirectoryInfo[] dirArr = dir.GetDirectories();
                foreach (DirectoryInfo dirItem in dirArr)
                {
                    string dirName = dirItem.Name;
                    dirPhoto2Pdf(dirItem);//处理图片转PDF
                    string[] fileArr = getFileArr(dirItem);//获取PDF文件
                    if (fileArr.Length == 0)
                    {
                        if (ckIsDelete.Checked)
                            Directory.Delete(Path.Combine(fbfPath, dirName), true);
                        continue;
                    }
                    string pdfN = dirName.Split('#')[1].Trim();
                    string newPdfName = Path.Combine(fbfPath, pdfN + ".pdf");
                    PdfUtility.MergePDF(fileArr, newPdfName);
                    if (ckIsDelete.Checked)
                        Directory.Delete(Path.Combine(fbfPath, dirName), true);
                    currentpdfIndex += fileArr.Length;

                    int progeress = currentpdfIndex * 100 / processTotalCount;
                    worker.ReportProgress(progeress);
                    if (progeress == 100)  // 如果用户取消则跳出处理数据代码 
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
                        dirPhoto2Pdf(item);//处理图片转PDF
                        string[] fileArr = getFileArr(item);
                        if (fileArr.Length == 0)
                        {
                            continue;
                        }
                        string pdfN = dirName.Split('#')[1].Trim();
                        string newPdfName = Path.Combine(cbfPath, pdfN + ".pdf");
                        PdfUtility.MergePDF(fileArr, newPdfName);
                        currentpdfIndex += fileArr.Length;

                        int progeress = currentpdfIndex * 100 / processTotalCount;
                        worker.ReportProgress(progeress);
                        if (progeress == 100)  // 如果用户取消则跳出处理数据代码 
                        {
                            e.Cancel = true;
                            ShowCompletePath();
                            break;
                        }
                    }
                    if (ckIsDelete.Checked)
                        Directory.Delete(Path.Combine(cbfPath, dirItemName), true);
                }
                #endregion
            }
        }

        /// <summary>
        /// 处理文件夹下的图片改为pdf格式
        /// </summary>
        /// <param name="dir"></param>
        private void dirPhoto2Pdf(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".jpg" || file.Extension == ".png")//如果是图片格式，则转换为pdf格式
                {
                    string photoPath = file.FullName;
                    string fdfPath = file.FullName.Replace(file.Extension, ".pdf");
                    PdfUtility.ConvertJPG2PDF(photoPath, fdfPath);
                }
            }
        }

        /// <summary>
        /// 获取文件夹下pdf文件列表（按升序排序）
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        private string[] getFileArr(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            ArrayList list = new ArrayList();

            foreach (FileInfo file in files)
            {
                if (file.Extension == ".pdf")
                {
                    string fileName = Regex.Replace(Path.GetFileNameWithoutExtension(file.FullName), @"[^\d]*", "");
                    if (string.IsNullOrEmpty(fileName))//如果为空跳出本次循环
                        continue;
                    int index = Convert.ToInt16(fileName);
                    string fileFullName = file.FullName;
                    dict.Add(index, fileFullName);
                    list.Add(index);
                }
            }
            Sort(list);//大小排序
            ArrayList fileList = new ArrayList();
            foreach (int item in list)
            {
                fileList.Add(dict[item]);
            }

            return (string[])fileList.ToArray(Type.GetType("System.String"));
        }

        public void Sort(ArrayList list)
        {
            for (int i = 1; i < list.Count; ++i)
            {
                int t = Convert.ToInt32(list[i]);
                int j = i;
                while ((j > 0) && (Convert.ToInt32(list[j - 1]) > t))
                {
                    list[j] = list[j - 1];
                    --j;
                }
                list[j] = t;
            }
        }

        private void ShowCompletePath()
        {
            if (MessageBox.Show(@"是否需要打开生成目录", "操作成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                if (Directory.Exists(txtCombinePath.Text.Trim()))
                    System.Diagnostics.Process.Start(txtCombinePath.Text.Trim());
            }
        }

        private int GetDirLevel(string path)
        {
            var dir = new DirectoryInfo(path);

            var dii = dir.GetDirectories();
            if (dii.Length == 0)
                return 1;
            foreach (DirectoryInfo d in dii)
            {
                if (d.Name == "发包方" || d.Name == "承包方")
                    return 2;
            }
            //二级
            var dirLevel2 = new DirectoryInfo(dii[0].FullName);
            var dii2 = dirLevel2.GetDirectories();
            foreach (DirectoryInfo d in dii2)
            {
                if (d.Name == "发包方" || d.Name == "承包方")
                    return 3;
            }
            return 1;
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
