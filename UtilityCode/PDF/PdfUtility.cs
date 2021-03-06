﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace UtilityCode
{
    public static class PdfUtility
    {
        /// <summary>
        /// 图片转PDF
        /// </summary>
        /// <param name="jpgfile">原图片路径</param>
        /// <param name="pdf">生成pdf路径</param>
        public static void ConvertJPG2PDF(string jpgfile, string pdf)
        {
            var document = new Document(iTextSharp.text.PageSize.A4, 25, 25, 25, 25);
            using (var stream = new FileStream(pdf, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();
                using (var imageStream = new FileStream(jpgfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var image = Image.GetInstance(imageStream);
                    if (image.Height > iTextSharp.text.PageSize.A4.Height - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 25, iTextSharp.text.PageSize.A4.Height - 25);
                    }
                    else if (image.Width > iTextSharp.text.PageSize.A4.Width - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width - 25, iTextSharp.text.PageSize.A4.Height - 25);
                    }
                    image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                    document.Add(image);
                }

                document.Close();
            }
        }

        /// <summary>
        /// 图片转PDF
        /// </summary>
        /// <param name="jpgfile">原图片路径</param>
        /// <param name="pdf">生成pdf路径</param>
        public static void ConvertJPG2PDF2(string jpgfile, string pdf)
        {
            var document = new Document(iTextSharp.text.PageSize.A4.Rotate(), 0, 0, 0, 0);
            using (var stream = new FileStream(pdf, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                PdfWriter.GetInstance(document, stream);
                document.Open();
                using (var imageStream = new FileStream(jpgfile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    var image = Image.GetInstance(imageStream);
                    if (image.Height > iTextSharp.text.PageSize.A4.Height - 25)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width+250 , iTextSharp.text.PageSize.A4.Height + 250);
                    }
                    else if (image.Width > iTextSharp.text.PageSize.A4.Width)
                    {
                        image.ScaleToFit(iTextSharp.text.PageSize.A4.Width + 250, iTextSharp.text.PageSize.A4.Height + 250);
                    }
                    image.Alignment = iTextSharp.text.Image.ALIGN_MIDDLE;
                    document.Add(image);
                }

                document.Close();
            }
        }

        /// <summary>
        /// 合并PDF文件
        /// </summary>
        /// <param name="files">pdf文件列表</param>
        /// <param name="outputFilePath">输出路径</param>
        public static void MergePDF(string[] files, string outputFilePath)
        {
            PDFFactory pf = new PDFFactory();
            foreach (string li in files)
            {
                pf.AddDocument(li);
            }
            pf.Merge(outputFilePath);
        }

        

    }
}
