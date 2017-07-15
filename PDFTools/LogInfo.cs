using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PDFTools
{
    public partial class LogInfo : Form
    {
        public LogInfo()
        {
            InitializeComponent();
            string log = PublicCode.GetLog();
            txtLog.Text = log;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(txtLog.Text);
            MessageBox.Show("已复制到粘贴板");
        }
    }
}
