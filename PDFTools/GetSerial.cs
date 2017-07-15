using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Management;


namespace PDFTools
{
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
                    HDid = (string) mo.Properties["Model"].Value;
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
                    if ((bool) mo["IPEnabled"] == true)
                        MoAddress = mo["MacAddress"].ToString();
                    mo.Dispose();
                }
            }
            return MoAddress.ToString();
        }


    }
}
    

