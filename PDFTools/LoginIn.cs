using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace PDFTools
{
    public partial class LoginIn : Form
    {
        public LoginIn()
        {
            InitializeComponent();

            InitUI();
        }

        private void InitUI()
        {
            if (PublicCode.CheckRegCode())
            {
                label3.Visible = false;
                txtRegCode.Visible = false;
                label4.Visible = false;
            }
        }

        

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtRegCode.Text.Trim()))
            {
                //先验证注册码，如果正确则保存
                if (PublicCode.CheckRegCode(txtRegCode.Text.Trim()))
                    UpdateConfigRegCode(txtRegCode.Text.Trim());
            }
            else
            {
                //先验证注册码，如果正确则保存
                if (!PublicCode.CheckRegCode())
                {
                    MessageBox.Show("注册码不正确，请联系管理员索取！");
                    return;
                }
            }

            string userId = txtUser.Text.Trim();
            string pwd = txtPwd.Text.Trim();
            if (userId == "admin" && pwd == "admin778899")
            {
                PublicCode.UserType = "admin";
            }
            else
                PublicCode.UserType = "user";

            this.Hide();
            Form1 mainFrom = new Form1();
            mainFrom.ShowDialog();
            
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <returns></returns>
        public void UpdateConfigRegCode(string regCode)
        {
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(configFilePath);
                XmlNode xn = doc.SelectSingleNode("/Config/RegCode/value");

                XmlElement xe = (XmlElement)xn;
                xe.InnerText = regCode;

                doc.Save(configFilePath);
            }
            catch { }
        }
    }
}
