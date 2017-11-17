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

namespace GenerateTools
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
                this.Hide();
                Form1 mainFrom = new Form1();
                mainFrom.ShowDialog();
            }
        }

        

        private void btnLogin_Click(object sender, EventArgs e)
        {

            if (!string.IsNullOrEmpty(txtRegCode.Text.Trim()))
            {
                //先验证注册码，如果正确则保存
                if (PublicCode.CheckRegCode(txtRegCode.Text.Trim()))
                    UpdateConfigRegCode(txtRegCode.Text.Trim());
                else
                {
                    MessageBox.Show("注册码不正确，请联系管理员索取！");
                    return;
                }
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
