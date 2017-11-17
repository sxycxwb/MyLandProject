using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace GenerateTools
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //String[] CmdArgs = System.Environment.GetCommandLineArgs();
            //if (CmdArgs.Length == 1 || CmdArgs[1] != "sinldo.com")
            //{
            //    MessageBox.Show("非法访问，请联系管理员！");
            //    return;
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginIn());
        }
    }
}
