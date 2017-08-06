using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace UtilityCode
{
    public static class CombineToPDF
    {

        public static int GetDirLevel(string path)
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

        /// <summary>
        /// 处理文件夹下的图片改为pdf格式
        /// </summary>
        /// <param name="dir"></param>
        public static void dirPhoto2Pdf(DirectoryInfo dir, bool addFlag = false)
        {
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".jpg" || file.Extension == ".png")//如果是图片格式，则转换为pdf格式
                {
                    string fileName = Regex.Replace(Path.GetFileNameWithoutExtension(file.FullName), @"[^\d]*", "");

                    string photoPath = file.FullName;
                    string fdfPath = file.FullName.Replace(file.Extension, ".pdf");
                    if (addFlag)//如果既有pdf又有jpg,则为改名递增
                    {
                        int index = 0;
                        if (!string.IsNullOrEmpty(fileName))//能取到数字文件名
                            Convert.ToInt32(fileName);
                        index += 10000;
                        fdfPath = file.FullName.Replace(file.Name, index + ".pdf");
                    }
                    PdfUtility.ConvertJPG2PDF(photoPath, fdfPath);
                }
            }
        }

        /// <summary>
        /// 获取文件夹下pdf文件列表（按升序排序）
        /// </summary>
        /// <param name="dir"></param>
        /// <returns></returns>
        public static string[] getFileArr(DirectoryInfo dir)
        {
            FileInfo[] files = dir.GetFiles();
            Dictionary<int, string> dict = new Dictionary<int, string>();
            ArrayList list = new ArrayList();

            foreach (FileInfo file in files)
            {
                if (file.Extension == ".pdf")
                {
                    string fileName = Regex.Replace(Path.GetFileNameWithoutExtension(file.FullName), @"[^\d]*", "");

                    if (string.IsNullOrEmpty(fileName)) //如果文件名中没有截取到数字，则从当前集合中取最后一个值加1
                    {
                        fileName = "0";
                        if (dict.Count > 0)
                            fileName = (dict.Last().Key + 1).ToString();
                    }
                    int index = Convert.ToInt32(fileName);
                    string fileFullName = file.FullName;
                    bool flag = true;
                    while (flag)
                    {
                        if (!dict.ContainsKey(index))//如果截取遇到了相同的数字，则加1处理
                        {
                            dict.Add(index, fileFullName);
                            list.Add(index);
                            flag = false;
                        }
                        else
                            index++;
                    }

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

        public static void Sort(ArrayList list)
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

        /// <summary>
        /// 判断文件夹下是否同时包含PDF和JPG文件
        /// </summary>
        /// <param name="dirItem"></param>
        /// <returns></returns>
        public static bool CheckPdfJpg(DirectoryInfo dirItem)
        {
            bool pdf = false;
            bool jpg = false;
            FileInfo[] files = dirItem.GetFiles();
            foreach (FileInfo f in files)
            {
                if (pdf && jpg)
                    continue;
                if (f.Extension == ".pdf" && !pdf)
                    pdf = true;
                if ((f.Extension == ".jpg" || f.Extension == ".png") && !jpg)
                    jpg = true;
            }
            if (pdf && jpg)
                return true;
            else
                return false;
        }
    }
}
