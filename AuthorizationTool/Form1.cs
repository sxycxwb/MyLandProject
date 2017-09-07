using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilityCode;

namespace AuthorizationTool
{
    public partial class Form1 : Form
    {
        private string keyVal = "sinldo.com";
        private string ivVal = "http://www.sinldo.com";

        public Form1()
        {
            InitializeComponent();
        }

        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置待授权工具的路径";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtCombinePath.Text = foldPath;
            }
        }

        private void btnStartAuth_Click(object sender, EventArgs e)
        {
            string[] arr = txtGroupInfo.Text.Split('\n');
            List<string> list = new List<string>();
            foreach (var str in arr)
            {
                if (!string.IsNullOrEmpty(str.Trim()))
                {
                    list.Add(str.Replace("\r",""));
                }
            }
            if (list.Count == 0)
            {
                MessageBox.Show("请填写发包方编码！");
                return;
            }
            //确认填写信息
            if (
                MessageBox.Show(@"本次将授权" + list.Count + "组发包方信息", "确认生成信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var str in list)
                {
                    sb.AppendLine(EncryptUtil.AesStr(str, keyVal, ivVal));
                }
                string filePath = Path.Combine(txtCombinePath.Text.Trim(), "Spire.Pdf.dll");
                if (File.Exists(filePath))
                    File.Delete(filePath);

                File.WriteAllText(filePath,sb.ToString());
                MessageBox.Show("授权成功！");
            }
        }
    }
}
