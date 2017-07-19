using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Spire.Xls;

namespace Excel2Pdf
{
    public static class PdfUtility
    {
        public static void ConvertExcel2PDF(string excelFIle, string pdf)
        {
            Workbook workbook = new Workbook();
            workbook.LoadFromFile(excelFIle);
            workbook.SaveToFile(pdf, FileFormat.PDF);
        }
    }
}
