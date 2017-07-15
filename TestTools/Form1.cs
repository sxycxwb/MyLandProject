using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Spire.Doc;
using Spire.Doc.Collections;
using Spire.Pdf.General.Render.Decode.Jpeg2000.j2k.image;
//using Spire.Doc;
using UtilityCode;
using Spire.Xls;

namespace TestTools
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //PdfUtility.ConvertExcel2PDF("1.xlsx", "1.pdf");
            PdfUtility.ConvertExcel2PDF("templete2.xlsx", "templete2.xlsx.pdf");
            //Workbook workbook = new Workbook();
            //workbook.LoadFromFile("test.xlsx");
            //workbook.Worksheets[0].
            //workbook.SaveToFile("test.pdf", Spire.Xls.FileFormat.PDF);


            //PlotModel model = new PlotModel();

            //Document document = new Document();
            //document.LoadFromFile(@"1.doc");
            //TableCollection tables = document.Sections[0].Tables;
            //if (tables.Count == 0)
            //    return;
            //if (tables[0].Rows.Count == 0)
            //    return;

            //model.CoordinateList = new List<CoordinatesModel>();
            //#region 处理界址点检查记录信息
            //List<string> list = new List<string>();
            //for (int i = 10; i < tables[0].Rows.Count; i++)
            //{
            //    var row = tables[0].Rows[i];
            //    if (string.IsNullOrEmpty(row.Cells[0].Paragraphs[0].Text))
            //        break;
            //    var coordinate = new CoordinatesModel();
            //    coordinate.OrderNum = (i - 9).ToString();
            //    coordinate.SerialNumber = row.Cells[0].Paragraphs[0].Text.Trim();
            //    coordinate.BoundaryPointNum = row.Cells[1].Paragraphs[0].Text.Trim();
            //    coordinate.X = Convert.ToDouble(row.Cells[2].Paragraphs[0].Text.Trim()).ToString("f3");
            //    coordinate.Y = Convert.ToDouble(row.Cells[3].Paragraphs[0].Text.Trim()).ToString("f3");
            //    //先随机 ∆L
            //    coordinate.difL = MathCode.GetRandomNumber(0.05, 0.4, 4).ToString("f3");
            //    coordinate.difSquareL = Math.Pow(Convert.ToDouble(coordinate.difL), 2.0).ToString("f3");
            //    //再随机 X'
            //    coordinate.cX = (Convert.ToDouble(coordinate.X) + MathCode.GetRandomNumber(-Convert.ToDouble(coordinate.difL), Convert.ToDouble(coordinate.difL), 3)).ToString("f3");
            //    coordinate.difX = (Convert.ToDouble(coordinate.X) - Convert.ToDouble(coordinate.cX)).ToString("f3");

            //    //根据公式求出 ∆y ∆y = 开平方（∆L2-∆x2）  每次随机正负值
            //    Random rd = new Random();
            //    var r = rd.Next(0, 2);
            //    coordinate.difY = ((r==0?-1:1)*Math.Sqrt(Convert.ToDouble(coordinate.difSquareL) - Math.Pow(Convert.ToDouble(coordinate.difX), 2.0))).ToString("f3");

            //    coordinate.cY = (Convert.ToDouble(coordinate.Y) - Convert.ToDouble(coordinate.difY)).ToString("f3");

            //    if (!list.Contains(coordinate.BoundaryPointNum))
            //    {
            //        list.Add(coordinate.BoundaryPointNum);
            //        model.CoordinateList.Add(coordinate);
            //    }
            //}
            //#endregion

            //#region 处理面积检查记录信息
            //model.PlotArea = Convert.ToDouble(document.Sections[0].Tables[0].Rows[6].Cells[1].Paragraphs[0].Text).ToString("f2");
            //#region 计算P'
            //double[] difXArr = new double[model.CoordinateList.Count];
            //double[] difYArr = new double[model.CoordinateList.Count];
            //for (int i = 0; i < model.CoordinateList.Count; i++)
            //{
            //    difXArr[i] = Convert.ToDouble(model.CoordinateList[i].cX);
            //    difYArr[i] = Convert.ToDouble(model.CoordinateList[i].cY);
            //}
            //model.PlotCheckArea = MathCode.AoArea(model.CoordinateList.Count, difXArr, difYArr).ToString("f2");

            //#endregion
            //model.DifArea = Math.Abs(Convert.ToDouble(model.PlotArea) - Convert.ToDouble(model.PlotCheckArea)).ToString("f2");
            ////R=∆P÷P′×100％
            //model.PercentageError = (Convert.ToDouble(model.DifArea) / Convert.ToDouble(model.PlotCheckArea) * 100).ToString("f1");

            //#endregion


            ////段落
            //Spire.Doc.Collections.ParagraphCollection p = document.Sections[0].Tables[0].Rows[12].Cells[0].Paragraphs;
            //string c = p[0].Text;
            //var doc = new WordTableRead("1.doc");
            //doc.Open();
            //string b = doc.ReadWord(0, 1, 1);
            //doc.Close();

            //PdfUtility.ConvertExcel2PDF("1.xlsx", "1.pdf");
            //PdfUtility.ConvertExcel2PDF("2.xlsx", "2.pdf");

            //double[] X = new double[] {3893981.826,3893986.345,3893723.565,3893710.419,3893706.486};
            //double[] Y = new double[] {500826.561,500838.536,500941.509,500945.204,500937.140};

            //double[] X = new double[] { 3834005.425,3834006.113,3834006.260,3834127.972,3834138.864};
            //double[] Y = new double[] {470663.538,470654.878,470673.397,470658.562,470679.694};

            //double[] X = new double[] { 3834365.382,3834366.222,3834372.584,3834372.967};
            //double[] Y = new double[] {467575.606,467650.769,467576.204,467644.555};
            //double[] X = new double[] { 3848946.816, 3848945.608, 3848938.377, 3848938.403, 3848946.816 };
            //double[] Y = new double[] {488831.228,488967.958,488967.860,488831.733,488831.228};

            //double result = MathCode.AoArea(5, X, Y);
            //result= Math.Round(result, 2);




        }

        private void button2_Click(object sender, EventArgs e)
        {
            //string path = "C:\\Users\\spring\Documents\\Tencent Files\\646323970\\FileRecv\\寺前 4组界址点成果表\\寺前 4组界址点成果表";
            byte[] bytes = Encoding.Default.GetBytes(GetSerial.getMNum() + "sinldo.com");
            string regCode = Convert.ToBase64String(bytes);
            txtRegCode.Text = regCode;
        }
    }
}
