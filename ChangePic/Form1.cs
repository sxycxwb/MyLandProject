using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using UtilityCode;

namespace ChangePic
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
            CheckForIllegalCrossThreadCalls = false;
        }

        /// <summary>
        /// 选择操作路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置图片的根目录";
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
            if (
                MessageBox.Show(@"已选择操作路径【" + path + "】\r\n包含【" + photoCount + "】个图片文件\r\n是否批量处理图片文件？", "提示信息-操作前请做好备份", MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Question) == DialogResult.OK)
            {
                processTotalCount = pdfCount + photoCount;
                if (photoCount == 0)
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

            DirectoryInfo dir = new DirectoryInfo(path);
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".jpg" || file.Extension == ".png")//如果是图片格式，则转换为pdf格式
                {
                    string fileName = Regex.Replace(Path.GetFileNameWithoutExtension(file.FullName), @"[^\d]*", "");

                    string photoPath = file.FullName;


                    using (MemoryStream mem = new MemoryStream())
                    {
                        Image img = new Bitmap(photoPath);

                        using (Bitmap bmp = new Bitmap(img))
                        {
                            img.Dispose();
                            Graphics g = Graphics.FromImage(bmp);
                            if (comboBox1.SelectedIndex == 0)
                            {
                                Rectangle[] rec1 = { new Rectangle(290, 2179, 2000, 46) };
                                g.FillRectangles(new SolidBrush(Color.White), rec1);

                                string txt = "制图者：" + txtZTZ.Text.Trim() + " 制图日期：" + Convert.ToDateTime(dtCreate.Text).ToString("yyyy年M月d日") + " " + " " + " 审核者：" + txtSHZ.Text.Trim() + " 审核日期：" + Convert.ToDateTime(dtCheck.Text).ToString("yyyy年M月d日") + " " + " " + "  编制单位：" + txtDW.Text.Trim();
                                Font font = new Font("宋体", 26, FontStyle.Bold);//设置字体，大小，粗细
                                SolidBrush sbrush = new SolidBrush(Color.Black);//设置颜色
                                g.DrawString(txt, font, sbrush, new PointF(305, 2184));
                            }
                            else if (comboBox1.SelectedIndex == 1)//村一
                            {
                                Rectangle[] rec1 = { new Rectangle(600, 4355, 3600, 90) };
                                g.FillRectangles(new SolidBrush(Color.White), rec1);

                                string txt = "制图者：" + txtZTZ.Text.Trim() + " 制图日期：" + Convert.ToDateTime(dtCreate.Text).ToString("yyyy年M月d日") + " " + " " + " 审核者：" + txtSHZ.Text.Trim() + " 审核日期：" + Convert.ToDateTime(dtCheck.Text).ToString("yyyy年M月d日") + " " + " " + "  编制单位：" + txtDW.Text.Trim();
                                Font font = new Font("宋体", 56, FontStyle.Bold);//设置字体，大小，粗细
                                SolidBrush sbrush = new SolidBrush(Color.Black);//设置颜色
                                g.DrawString(txt, font, sbrush, new PointF(620, 4365));
                            }
                            else if (comboBox1.SelectedIndex == 2)//村二
                            {
                                Rectangle[] rec1 = { new Rectangle(204, 1450, 1500, 36) };
                                g.FillRectangles(new SolidBrush(Color.White), rec1);

                                string txt = "制图者：" + txtZTZ.Text.Trim() + " 制图日期：" + Convert.ToDateTime(dtCreate.Text).ToString("yyyy年M月d日") + " " + " " + " 审核者：" + txtSHZ.Text.Trim() + " 审核日期：" + Convert.ToDateTime(dtCheck.Text).ToString("yyyy年M月d日") + " " + " " + "  编制单位：" + txtDW.Text.Trim();
                                Font font = new Font("宋体", 20, FontStyle.Bold);//设置字体，大小，粗细
                                SolidBrush sbrush = new SolidBrush(Color.Black);//设置颜色
                                g.DrawString(txt, font, sbrush, new PointF(204, 1455));
                            }

                            //线性质量设置 ( AssumeLinear ) 高速度呈现。
                            g.CompositingQuality = CompositingQuality.HighQuality;
                            //指定高速度呈现


                            //gc.CompositingQuality = CompositingQuality.HighQuality;
                            g.SmoothingMode = SmoothingMode.HighQuality;
                            g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                            bmp.Save(photoPath, System.Drawing.Imaging.ImageFormat.Jpeg);
                            bmp.Dispose();

                        }

                    }

                    //string pdfPath = file.FullName.Replace(file.Extension, ".pdf");

                    //PdfUtility.ConvertJPG2PDF2(photoPath, pdfPath);
                }

                currentpdfIndex += 1;

                int progeress = currentpdfIndex * 100 / processTotalCount;
                worker.ReportProgress(progeress);
                if (progeress == 100) // 如果用户取消则跳出处理数据代码 
                {
                    e.Cancel = true;
                    ShowCompletePath();
                    break;
                }
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
                //if (f.Extension == ".pdf")
                //    pdfCount += 1;
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
