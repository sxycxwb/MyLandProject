using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace PDFTools
{
    public static class PublicCode
    {
        //日志完整路径，包括文件名
        private static string file = "Spring.dll";

        /// <summary>
        /// 用户类型
        /// </summary>
        public static string UserType { get; set; }

        /// <summary>
        /// 检查注册码有效性
        /// </summary>
        /// <param name="inputRegCode"></param>
        /// <returns></returns>
        public static bool CheckRegCode(string inputRegCode = "")
        {
            bool result = false;
            string regCode = "";

            if (string.IsNullOrEmpty(inputRegCode))
            {
                //读取配置文件注册码
                XmlDocument doc = new XmlDocument();
                string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
                doc.Load(configFilePath);

                XmlNodeList nodes = doc.SelectNodes("/Config/RegCode/value");
                if (nodes.Count > 0)
                    regCode = nodes[0].InnerText;
            }
            else
            {
                regCode = inputRegCode;
            }

            byte[] bytes = Encoding.Default.GetBytes(GetSerial.getMNum() + "sinldo.com");
            string realRegCode = Convert.ToBase64String(bytes);

            if (realRegCode == regCode)
                result = true;
            return result;
        }


        public static void Log(string logTxt)
        {
            if (!File.Exists(file))
            {
                FileStream filestream = null;
                try
                {
                    filestream = System.IO.File.Create(file);
                    filestream.Dispose();
                    filestream.Close();
                }
                catch (System.Exception ex)
                {
                    throw new System.Exception(ex + "创建日志文件失败");
                }
            }

            //true 如果日志文件存在则继续追加日志 
            System.IO.StreamWriter sw = null;
            try
            {
                sw = new System.IO.StreamWriter(file, true, System.Text.Encoding.UTF8);
                string logInfo = "【" + System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "】" + "" + logTxt +"";

                byte[] bytes = Encoding.Default.GetBytes(logInfo);
                logInfo = Convert.ToBase64String(bytes);
                sw.WriteLine(logInfo);
            }
            catch (System.Exception ex)
            {
                throw new System.Exception(ex + "写入日志失败，检查！");
            }
            finally
            {
                sw.Flush();
                sw.Dispose();
                sw.Close();
            }
        }

        public static string GetLog()
        {
            string log = File.ReadAllText(file);
            byte[] c = Convert.FromBase64String(log);
            log = Encoding.Default.GetString(c);
            return log;
        }
    }

   
}
