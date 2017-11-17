using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Management;

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
            StringBuilder sb = new StringBuilder();
            if (!File.Exists(file))
                return "";
                var logArr = File.ReadAllLines(file);
            foreach (var log in logArr)
            {
                byte[] c = Convert.FromBase64String(log);
                sb.AppendLine(Encoding.Default.GetString(c));
            }
           
            return sb.ToString();
        }
    }

    /// <summary>
    /// GetSerial 的摘要说明
    /// </summary>
    public class GetSerial
    {
        public static char[] Charcode = new char[25]; //存储机器码字

        //获取硬盘号
        public static string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        //获得CPU的序列号
        public static string getCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuConnection = myCpu.GetInstances();
            foreach (ManagementObject myObject in myCpuConnection)
            {
                strCpu = myObject.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        /// <summary>
        /// 生成机器码
        /// </summary>
        /// <returns></returns>

        public static string getMNum()
        {
            string[] m = GetMoAddress().Split(':');
            string strNum = GetDiskVolumeSerialNumber();
            string strMNum = m[2]; //从生成的字符串中取出前24个字符做为机器码
            strMNum += m[4];
            strMNum += m[1] + strNum;
            strMNum += m[0];
            strMNum += m[5];
            return strMNum;
        }

        ///   <summary>    
        ///   获取cpu序列号        
        ///   </summary>    
        ///   <returns> string </returns>    
        public string GetCpuInfo()
        {
            string cpuInfo = " ";
            using (ManagementClass cimobject = new ManagementClass("Win32_Processor"))
            {
                ManagementObjectCollection moc = cimobject.GetInstances();

                foreach (ManagementObject mo in moc)
                {
                    cpuInfo = mo.Properties["ProcessorId"].Value.ToString();
                    mo.Dispose();
                }
            }
            return cpuInfo.ToString();
        }

        ///   <summary>    
        ///   获取硬盘ID        
        ///   </summary>    
        ///   <returns> string </returns>    
        public static string GetHDid()
        {
            string HDid = " ";
            using (ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive"))
            {
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    HDid = (string)mo.Properties["Model"].Value;
                    mo.Dispose();
                }
            }
            return HDid.ToString();
        }

        ///   <summary>    
        ///   获取网卡硬件地址    
        ///   </summary>    
        ///   <returns> string </returns>    
        public static string GetMoAddress()
        {
            string MoAddress = " ";
            using (ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration"))
            {
                ManagementObjectCollection moc2 = mc.GetInstances();
                foreach (ManagementObject mo in moc2)
                {
                    if ((bool)mo["IPEnabled"] == true)
                        MoAddress = mo["MacAddress"].ToString();
                    mo.Dispose();
                }
            }
            return MoAddress.ToString();
        }


    }
}
