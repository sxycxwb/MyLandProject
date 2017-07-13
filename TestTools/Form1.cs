using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using Spire.Doc;
using UtilityCode;

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
            //Document document = new Document();
            //document.LoadFromFile(@"1.doc");
            //var section = document.Sections[0].Body;
            //var a = section.Document;

            ////段落
            //Spire.Doc.Collections.ParagraphCollection p  = document.Sections[0].Tables[0].Rows[12].Cells[0].Paragraphs;
            //string c = p[0].Text;
            //var doc = new WordTableRead("1.doc");
            //doc.Open();
            //string b = doc.ReadWord(0, 1, 1);
            //doc.Close();

            PdfUtility.ConvertExcel2PDF("1.xlsx","1.pdf");
            PdfUtility.ConvertExcel2PDF("2.xlsx", "2.pdf");
        }
    }
}
