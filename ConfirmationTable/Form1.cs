using Spire.Xls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UtilityCode;

namespace ConfirmationTable
{

    public partial class Form1 : Form
    {
        private string keyVal = "sinldo.com";
        private string ivVal = "http://www.sinldo.com";
        List<string> list = new List<string>();
        List<string> listDb = new List<string>();

        private int everyTableRows = 28;

        public Form1()
        {
            InitializeComponent();
            try
            {
                string[] arr = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spire.Excel.dll"));
                foreach (var str in arr)
                {
                    list.Add(EncryptUtil.UnAesStr(str, keyVal, ivVal));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("非法访问，所有功能已失效！");
                this.Close();
            }

            BindCombox();
        }

        private void BindCombox()
        {
            cbDbSelect.Items.Clear();
            DirectoryInfo di = new DirectoryInfo("DB");
            FileInfo[] fiArr = di.GetFiles("*.mdb");
            listDb.Add("");
            foreach (var fi in fiArr)
            {
                listDb.Add(Path.GetFileNameWithoutExtension(fi.Name));
            }
            cbDbSelect.DataSource = listDb;
        }

        private void cbDbSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDWMC.Text = cbDbSelect.SelectedItem.ToString();
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            if (cbDbSelect.SelectedIndex <= 0)
            {
                MessageBox.Show("请选择一组数据进行操作！");
                return;
            }
            if (!(MessageBox.Show(@"本次确认要生成《" + txtDWMC.Text.Trim() + "》单位数据吗？", "确认生成信息", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK))
                return;
            string dbConnStr = Path.Combine("DB", cbDbSelect.SelectedItem.ToString() + ".mdb");
            AccessDB accessHelper = new AccessDB(dbConnStr);
            var fbf_dt = accessHelper.ExecuteDataTable("select FBFBM,FBFMC from FBF ");
            var dk_dt = accessHelper.ExecuteDataTable("select FBFBM,CBFBM,CBFMC,DKBM,DKMC,HTMJ,SCMJ,DKDZ,DKNZ,DKXZ,DKBZ from CBDKDC ORDER BY FBFBM,CBFBM");
            var xx_dt = accessHelper.ExecuteDataTable("select CBFBM,CYXM,CYXB,CSRQ,CYZJHM,YHZGX from CBF_JTCY ");
            var ds = GetNewDs(dk_dt, xx_dt, fbf_dt);

            Workbook workbook = new Workbook();
            var tempetePath = AppDomain.CurrentDomain.BaseDirectory;
            tempetePath = Path.Combine(tempetePath, "Template", "template1.xls");
            workbook.LoadFromFile(tempetePath);
            if (workbook.Worksheets.Count == 0)
                return;
            if (ds == null)
                return;

            var firstSheet = workbook.Worksheets[0];
            //填充WorkSheet
            for (int i = 0; i < fbf_dt.Rows.Count; i++)
            {
                string fbfmc = fbf_dt.Rows[i]["FBFMC"].ToString();
                if (i != 0)
                {
                    workbook.Worksheets.AddCopy(firstSheet);
                }
                workbook.Worksheets[i].Name = fbfmc;
            }

            for (int i = 0; i < ds.Tables.Count; i++)
            {
                GenDt(workbook, ds.Tables[i], i);
            }

            string generatePath = Path.Combine("Print", txtDWMC.Text.Trim() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss"));
            workbook.SaveToFile(generatePath + ".xlsx", ExcelVersion.Version2010);

            if (MessageBox.Show(@"是否需要打开生成目录", "操作成功", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                var makePath = AppDomain.CurrentDomain.BaseDirectory;
                makePath = Path.Combine(makePath, "Print");
                if (Directory.Exists(makePath))
                    System.Diagnostics.Process.Start(makePath);
            }
        }

        private void GenDt(Workbook workbook, DataTable dt, int sheetIndex)
        {
            int totalSheetCount = workbook.Worksheets.Count;

            var nowDate = Convert.ToDateTime(txtSelectDate.Text).ToString("yyyy年MM月dd日");
            var query = from t in dt.AsEnumerable()
                        group t by new { CBFBM = t.Field<string>("CBFBM") } into m
                        select new
                        {
                            CBFBM = m.Key.CBFBM,
                            COUNT = m.Count()
                        };
            var list = query.Where(t => t.COUNT <= 20).ToList();
            var bigList = query.Where(t => t.COUNT > 20).ToList();

            Worksheet worksheet = workbook.Worksheets[sheetIndex];

            //1.计算表格数目 复制表格
            int tableCount = list.Count;
            foreach (var item in query.ToList())
            {
                if (item.COUNT > 20)
                    tableCount += 1;
            }
            worksheet.InsertRow(tableCount * everyTableRows);

            #region 复制表格
            for (int j = 0; j < tableCount - 1; j++)
            {
                int startRowIndex = (j + 1) * everyTableRows + 1;

                #region 设置行高
                worksheet.SetRowHeight(startRowIndex, 22.5);
                worksheet.SetRowHeight(startRowIndex + 1, 20);
                worksheet.SetRowHeight(startRowIndex + 2, 18);
                worksheet.SetRowHeight(startRowIndex + 3, 15);
                worksheet.SetRowHeight(startRowIndex + 4, 15);
                for (int i = startRowIndex + 5; i <= startRowIndex + 26; i++)
                {
                    worksheet.SetRowHeight(i, 20);
                }
                #endregion

                #region 合并格
                worksheet.Range["A" + startRowIndex + ":S" + startRowIndex].Merge();
                worksheet.Range["A" + (startRowIndex + 1) + ":G" + (startRowIndex + 1)].Merge();//单位
                worksheet.Range["H" + (startRowIndex + 1) + ":J" + (startRowIndex + 1)].Merge();//日期
                worksheet.Range["K" + (startRowIndex + 1) + ":S" + (startRowIndex + 1)].Merge();//户号
                worksheet.Range["A" + (startRowIndex + 2) + ":A" + (startRowIndex + 4)].Merge();//承包方  
                worksheet.Range["B" + (startRowIndex + 3) + ":B" + (startRowIndex + 4)].Merge();
                worksheet.Range["C" + (startRowIndex + 3) + ":C" + (startRowIndex + 4)].Merge();
                worksheet.Range["D" + (startRowIndex + 3) + ":D" + (startRowIndex + 4)].Merge();
                worksheet.Range["E" + (startRowIndex + 3) + ":E" + (startRowIndex + 4)].Merge();
                worksheet.Range["F" + (startRowIndex + 3) + ":F" + (startRowIndex + 4)].Merge();
                worksheet.Range["G" + (startRowIndex + 3) + ":G" + (startRowIndex + 4)].Merge();
                worksheet.Range["H" + (startRowIndex + 3) + ":H" + (startRowIndex + 4)].Merge();
                worksheet.Range["I" + (startRowIndex + 3) + ":I" + (startRowIndex + 4)].Merge();
                worksheet.Range["J" + (startRowIndex + 3) + ":J" + (startRowIndex + 4)].Merge();
                worksheet.Range["K" + (startRowIndex + 3) + ":K" + (startRowIndex + 4)].Merge();
                worksheet.Range["L" + (startRowIndex + 3) + ":L" + (startRowIndex + 4)].Merge();
                worksheet.Range["M" + (startRowIndex + 3) + ":M" + (startRowIndex + 4)].Merge();
                worksheet.Range["N" + (startRowIndex + 3) + ":N" + (startRowIndex + 4)].Merge();
                worksheet.Range["O" + (startRowIndex + 3) + ":O" + (startRowIndex + 4)].Merge();
                worksheet.Range["P" + (startRowIndex + 3) + ":S" + (startRowIndex + 3)].Merge();
                worksheet.Range["A" + (startRowIndex + 26) + ":B" + (startRowIndex + 26)].Merge();
                worksheet.Range["C" + (startRowIndex + 26) + ":G" + (startRowIndex + 26)].Merge();
                worksheet.Range["H" + (startRowIndex + 26) + ":J" + (startRowIndex + 26)].Merge();
                worksheet.Range["K" + (startRowIndex + 26) + ":S" + (startRowIndex + 26)].Merge();
                worksheet.Range["B" + (startRowIndex + 2) + ":G" + (startRowIndex + 2)].Merge();//家庭成员情况
                worksheet.Range["H" + (startRowIndex + 2) + ":S" + (startRowIndex + 2)].Merge();//承包地确权登记情况 
                #endregion

                worksheet.Copy(worksheet.Range["A1:S" + everyTableRows], worksheet.Range["A" + startRowIndex + ":S" + (startRowIndex + everyTableRows - 1)], true);
            }
            #endregion

            #region 表格赋值

            //小于20行数据处理
            int d = 0;
            for (d = 0; d < list.Count; d++)
            {
                var model = list[d];
                DataRow[] rows = dt.Select("CBFBM='" + model.CBFBM + "'");
                if (rows.Length == 0)
                    continue;

                int startRowIndex = d * everyTableRows + 1;

                worksheet.Range["A" + (startRowIndex + 1)].Text = "单位：" + dt.TableName; //单位
                worksheet.Range["H" + (startRowIndex + 1)].Text = "日期：" + nowDate;//日期
                worksheet.Range["K" + (startRowIndex + 1)].Text = "户号：" + rows[0]["HH"].ToString();//户号
                var dkCount = Convert.ToInt32(rows[0]["DKNUM"].ToString());//地块数目
                var xxCount = Convert.ToInt32(rows[0]["MEMBERS"].ToString());//家庭人员数量

                //承包方合并赋值
                worksheet.Range["A" + (startRowIndex + 5) + ":A" + (startRowIndex + 5 + (xxCount > 0 ? xxCount - 1 : 0))].Merge();
                worksheet.Range["A" + (startRowIndex + 5)].Text = rows[0]["CBFMC"].ToString();//承包方名称

                //家庭成员数
                worksheet.Range["B" + (startRowIndex + 5) + ":B" + (startRowIndex + 5 + (xxCount > 0 ? xxCount - 1 : 0))].Merge();
                worksheet.Range["B" + (startRowIndex + 5)].Text = xxCount.ToString();//家庭人员数量

                //合同编号
                worksheet.Range["H" + (startRowIndex + 5) + ":H" + (startRowIndex + 5 + (dkCount > 0 ? dkCount - 1 : 0))].Merge();
                worksheet.Range["H" + (startRowIndex + 5)].Text = rows[0]["HTBH"].ToString();

                //权证编号
                worksheet.Range["I" + (startRowIndex + 5) + ":I" + (startRowIndex + 5 + (dkCount > 0 ? dkCount - 1 : 0))].Merge();
                worksheet.Range["I" + (startRowIndex + 5)].Text = rows[0]["QZBH"].ToString();

                //合同总面积
                worksheet.Range["L" + (startRowIndex + 5) + ":L" + (startRowIndex + 5 + (dkCount > 0 ? dkCount - 1 : 0))].Merge();
                worksheet.Range["L" + (startRowIndex + 5)].Text = rows[0]["HTMJ_T"].ToString();//合同总面积
                worksheet.Range["M" + (startRowIndex + 25)].Text = rows[0]["HTMJ_T"].ToString();//合同总面积

                //实测总面积
                worksheet.Range["N" + (startRowIndex + 5) + ":N" + (startRowIndex + 5 + (dkCount > 0 ? dkCount - 1 : 0))].Merge();
                worksheet.Range["N" + (startRowIndex + 5)].Text = rows[0]["SCMJ_T"].ToString();//实测总面积
                worksheet.Range["O" + (startRowIndex + 25)].Text = rows[0]["SCMJ_T"].ToString();//实测总面积

                int rowsIndex = 0;
                for (int i = startRowIndex + 5; i < startRowIndex + 25; i++)
                {
                    if (rowsIndex < rows.Length)
                    {
                        worksheet.Range["C" + i].Text = rows[rowsIndex]["NAME"].ToString();
                        worksheet.Range["D" + i].Text = rows[rowsIndex]["SEX"].ToString();
                        worksheet.Range["E" + i].Text = rows[rowsIndex]["AGE"].ToString();
                        worksheet.Range["F" + i].Text = rows[rowsIndex]["IDNO"].ToString();
                        worksheet.Range["G" + i].Text = rows[rowsIndex]["YHZGX"].ToString();
                        worksheet.Range["J" + i].Text = rows[rowsIndex]["DKBM"].ToString();
                        worksheet.Range["K" + i].Text = rows[rowsIndex]["DKMC"].ToString();
                        worksheet.Range["M" + i].Text = rows[rowsIndex]["HTMJ"].ToString();
                        worksheet.Range["O" + i].Text = rows[rowsIndex]["SCMJ"].ToString();
                        worksheet.Range["P" + i].Text = rows[rowsIndex]["DKDZ"].ToString();
                        worksheet.Range["Q" + i].Text = rows[rowsIndex]["DKNZ"].ToString();
                        worksheet.Range["R" + i].Text = rows[rowsIndex]["DKXZ"].ToString();
                        worksheet.Range["S" + i].Text = rows[rowsIndex]["DKBZ"].ToString();
                    }
                    rowsIndex++;
                }

            }


            //大于20行数据
            for (int z = 0; z < bigList.Count; z++)
            {
                var model = bigList[z];
                DataRow[] rows = dt.Select("CBFBM='" + model.CBFBM + "'");
                if (rows.Length == 0)
                    continue;

                int startRowIndex = d * everyTableRows + 1;

                worksheet.Range["A" + (startRowIndex + 1)].Text = "单位：" + txtDWMC.Text.Trim(); //单位
                worksheet.Range["H" + (startRowIndex + 1)].Text = "日期：" + nowDate;//日期
                worksheet.Range["K" + (startRowIndex + 1)].Text = "户号：" + rows[0]["HH"].ToString();//户号
                var dkCount = Convert.ToInt32(rows[0]["DKNUM"].ToString());//地块数目
                var xxCount = Convert.ToInt32(rows[0]["MEMBERS"].ToString());//家庭人员数量

                //承包方合并赋值
                worksheet.Range["A" + (startRowIndex + 5) + ":A" + (startRowIndex + 5 + (xxCount > 0 ? xxCount - 1 : 0))].Merge();
                worksheet.Range["A" + (startRowIndex + 5)].Text = rows[0]["CBFMC"].ToString();//承包方名称

                //家庭成员数
                worksheet.Range["B" + (startRowIndex + 5) + ":B" + (startRowIndex + 5 + (xxCount > 0 ? xxCount - 1 : 0))].Merge();
                worksheet.Range["B" + (startRowIndex + 5)].Text = xxCount.ToString();//家庭人员数量

                //合同编号
                worksheet.Range["H" + (startRowIndex + 5) + ":H" + (startRowIndex + 5 + 19)].Merge();
                worksheet.Range["H" + (startRowIndex + 5)].Text = rows[0]["HTBH"].ToString();

                //权证编号
                worksheet.Range["I" + (startRowIndex + 5) + ":I" + (startRowIndex + 5 + 19)].Merge();
                worksheet.Range["I" + (startRowIndex + 5)].Text = rows[0]["QZBH"].ToString();

                //合同总面积
                worksheet.Range["L" + (startRowIndex + 5) + ":L" + (startRowIndex + 5 + 19)].Merge();
                worksheet.Range["L" + (startRowIndex + 5)].Text = rows[0]["HTMJ_T"].ToString();//合同总面积
                worksheet.Range["M" + (startRowIndex + 25)].Text = rows[0]["HTMJ_T"].ToString();//合同总面积

                //实测总面积
                worksheet.Range["N" + (startRowIndex + 5) + ":N" + (startRowIndex + 5 + 19)].Merge();
                worksheet.Range["N" + (startRowIndex + 5)].Text = rows[0]["SCMJ_T"].ToString();//实测总面积
                worksheet.Range["O" + (startRowIndex + 25)].Text = rows[0]["SCMJ_T"].ToString();//实测总面积

                int rowsIndex = 0;
                for (int i = startRowIndex + 5; i < startRowIndex + 25; i++)
                {
                    if (rowsIndex < rows.Length)
                    {
                        worksheet.Range["C" + i].Text = rows[rowsIndex]["NAME"].ToString();
                        worksheet.Range["D" + i].Text = rows[rowsIndex]["SEX"].ToString();
                        worksheet.Range["E" + i].Text = rows[rowsIndex]["AGE"].ToString();
                        worksheet.Range["F" + i].Text = rows[rowsIndex]["IDNO"].ToString();
                        worksheet.Range["G" + i].Text = rows[rowsIndex]["YHZGX"].ToString();
                        worksheet.Range["J" + i].Text = rows[rowsIndex]["DKBM"].ToString();
                        worksheet.Range["K" + i].Text = rows[rowsIndex]["DKMC"].ToString();
                        worksheet.Range["M" + i].Text = rows[rowsIndex]["HTMJ"].ToString();
                        worksheet.Range["O" + i].Text = rows[rowsIndex]["SCMJ"].ToString();
                        worksheet.Range["P" + i].Text = rows[rowsIndex]["DKDZ"].ToString();
                        worksheet.Range["Q" + i].Text = rows[rowsIndex]["DKNZ"].ToString();
                        worksheet.Range["R" + i].Text = rows[rowsIndex]["DKXZ"].ToString();
                        worksheet.Range["S" + i].Text = rows[rowsIndex]["DKBZ"].ToString();
                    }
                    rowsIndex++;
                }

                d++;

                startRowIndex = d * everyTableRows + 1;

                worksheet.Range["A" + (startRowIndex + 1)].Text = "单位：" + txtDWMC.Text.Trim(); //单位
                worksheet.Range["H" + (startRowIndex + 1)].Text = "日期：" + nowDate;//日期
                worksheet.Range["K" + (startRowIndex + 1)].Text = "户号：" + rows[0]["HH"].ToString();//户号

                //承包方合并赋值
                worksheet.Range["A" + (startRowIndex + 5) + ":A" + (startRowIndex + 5 + (xxCount > 0 ? xxCount - 1 : 0))].Merge();
                worksheet.Range["A" + (startRowIndex + 5)].Text = rows[0]["CBFMC"].ToString();//承包方名称

                //家庭成员数
                worksheet.Range["B" + (startRowIndex + 5) + ":B" + (startRowIndex + 5 + (xxCount > 0 ? xxCount - 1 : 0))].Merge();
                worksheet.Range["B" + (startRowIndex + 5)].Text = xxCount.ToString();//家庭人员数量

                //合同编号
                worksheet.Range["H" + (startRowIndex + 5) + ":H" + (startRowIndex + 5 + dkCount - 21)].Merge();
                worksheet.Range["H" + (startRowIndex + 5)].Text = rows[0]["HTBH"].ToString();

                //权证编号
                worksheet.Range["I" + (startRowIndex + 5) + ":I" + (startRowIndex + 5 + dkCount - 21)].Merge();
                worksheet.Range["I" + (startRowIndex + 5)].Text = rows[0]["QZBH"].ToString();

                //合同总面积
                worksheet.Range["L" + (startRowIndex + 5) + ":L" + (startRowIndex + 5 + dkCount - 21)].Merge();
                worksheet.Range["L" + (startRowIndex + 5)].Text = rows[0]["HTMJ_T"].ToString();//合同总面积
                worksheet.Range["M" + (startRowIndex + 25)].Text = rows[0]["HTMJ_T"].ToString();//合同总面积

                //实测总面积
                worksheet.Range["N" + (startRowIndex + 5) + ":N" + (startRowIndex + 5 + dkCount - 21)].Merge();
                worksheet.Range["N" + (startRowIndex + 5)].Text = rows[0]["SCMJ_T"].ToString();//实测总面积
                worksheet.Range["O" + (startRowIndex + 25)].Text = rows[0]["SCMJ_T"].ToString();//实测总面积

                rowsIndex = 20;
                for (int i = startRowIndex + 5; i < startRowIndex + 25; i++)
                {
                    if (rowsIndex < rows.Length)
                    {
                        worksheet.Range["C" + i].Text = rows[rowsIndex]["NAME"].ToString();
                        worksheet.Range["D" + i].Text = rows[rowsIndex]["SEX"].ToString();
                        worksheet.Range["E" + i].Text = rows[rowsIndex]["AGE"].ToString();
                        worksheet.Range["F" + i].Text = rows[rowsIndex]["IDNO"].ToString();
                        worksheet.Range["G" + i].Text = rows[rowsIndex]["YHZGX"].ToString();
                        worksheet.Range["J" + i].Text = rows[rowsIndex]["DKBM"].ToString();
                        worksheet.Range["K" + i].Text = rows[rowsIndex]["DKMC"].ToString();
                        worksheet.Range["M" + i].Text = rows[rowsIndex]["HTMJ"].ToString();
                        worksheet.Range["O" + i].Text = rows[rowsIndex]["SCMJ"].ToString();
                        worksheet.Range["P" + i].Text = rows[rowsIndex]["DKDZ"].ToString();
                        worksheet.Range["Q" + i].Text = rows[rowsIndex]["DKNZ"].ToString();
                        worksheet.Range["R" + i].Text = rows[rowsIndex]["DKXZ"].ToString();
                        worksheet.Range["S" + i].Text = rows[rowsIndex]["DKBZ"].ToString();
                    }
                    rowsIndex++;
                }
                d++;
            }

            #endregion
        }

        private DataSet GetNewDs(DataTable dk_dt, DataTable xx_dt, DataTable fbf_dt)
        {
            DataSet newDs = new DataSet();

            for (int z = 0; z < fbf_dt.Rows.Count; z++)
            {
                #region 构造新的DataTable
                DataRow itemRow = fbf_dt.Rows[z];
                DataTable newDt = GetNewDataTable();
                string fbfbm = itemRow["FBFBM"].ToString();
                string fbfmc = itemRow["FBFMC"].ToString();
                var rows = dk_dt.Select("FBFBM = '" + fbfbm + "'", "CBFBM");

                string currentCBFBM = string.Empty, oldCBFBM = string.Empty;
                for (int i = 0; i < rows.Length; i++)
                {
                    DataRow row = rows[i];
                    currentCBFBM = row["CBFBM"].ToString();

                    if (!CheckNum(currentCBFBM))
                    {
                        MessageBox.Show("您正在处理非授权数据，请联系管理员！");
                        return null;
                    }

                    if (currentCBFBM == oldCBFBM)
                        continue;

                    var cbfmc = row["CBFMC"].ToString();
                    var dkRows = dk_dt.Select("CBFBM='" + currentCBFBM + "'");
                    var xxRows = xx_dt.Select("CBFBM='" + currentCBFBM + "'");
                    var dkCount = dkRows.Length;//地块数目
                    var xxCount = xxRows.Length;//家庭人员数量
                    var totalHTMJ = GetTotalArea(dkRows, "HTMJ").ToString("0.00");
                    var totalSCMJ = GetTotalArea(dkRows, "SCMJ", true).ToString("0.00");

                    int newRowsCount = dkCount > xxCount ? dkCount : xxCount;
                    for (int j = 0; j < newRowsCount; j++)
                    {
                        var newRow = newDt.NewRow();
                        //新行赋值
                        newRow["FBFBM"] = fbfbm;
                        newRow["HH"] = currentCBFBM.Substring(currentCBFBM.Length - 4, 4);
                        newRow["CBFBM"] = currentCBFBM;
                        newRow["CBFMC"] = cbfmc;
                        newRow["MEMBERS"] = xxCount;
                        newRow["DKNUM"] = dkCount;
                        newRow["HTBH"] = currentCBFBM + "J";
                        newRow["QZBH"] = currentCBFBM + "J";
                        newRow["HTMJ_T"] = totalHTMJ;
                        newRow["SCMJ_T"] = totalSCMJ;

                        if (j <= xxCount - 1)
                        {
                            newRow["NAME"] = xxRows[j]["CYXM"].ToString();
                            newRow["SEX"] = GetSex(xxRows[j]["CYXB"].ToString());
                            newRow["AGE"] = GetAgeByBirthdate(Convert.ToDateTime(xxRows[j]["CSRQ"].ToString()));
                            newRow["IDNO"] = xxRows[j]["CYZJHM"].ToString();
                            newRow["YHZGX"] = GetRelationship(xxRows[j]["YHZGX"].ToString());
                        }
                        if (j <= dkCount - 1)
                        {
                            newRow["DKBM"] = dkRows[j]["DKBM"].ToString();
                            newRow["DKMC"] = dkRows[j]["DKMC"].ToString();
                            newRow["HTMJ"] = dkRows[j]["HTMJ"].ToString();
                            newRow["SCMJ"] = GetAcres(dkRows[j]["SCMJ"].ToString()).ToString("0.00");
                            newRow["DKDZ"] = dkRows[j]["DKDZ"].ToString();
                            newRow["DKNZ"] = dkRows[j]["DKNZ"].ToString();
                            newRow["DKXZ"] = dkRows[j]["DKXZ"].ToString();
                            newRow["DKBZ"] = dkRows[j]["DKBZ"].ToString();
                        }
                        newDt.Rows.Add(newRow);
                    }

                    oldCBFBM = row["CBFBM"].ToString();

                }
                #endregion
                newDs.Tables.Add(newDt);
                newDs.Tables[z].TableName = fbfmc;
            }

            return newDs;
        }

        private DataTable GetNewDataTable()
        {
            DataTable dt = new DataTable();
            string[] arr = { "FBFBM", "HH", "CBFBM", "CBFMC", "MEMBERS", "NAME", "SEX", "AGE", "IDNO", "YHZGX", "HTBH", "QZBH", "DKBM", "DKNUM", "DKMC", "HTMJ_T", "HTMJ", "SCMJ_T", "SCMJ", "DKDZ", "DKNZ", "DKXZ", "DKBZ" };
            foreach (var item in arr)
            {
                dt.Columns.Add(item);
            }
            return dt;
        }

        private bool CheckNum(string num)
        {
            bool returnVal = false;
            foreach (var str in list)
            {
                if (num.IndexOf(str) > -1)
                {
                    returnVal = true;
                    break;
                }
            }
            return returnVal;
        }

        /// <summary>
        /// 获取的年龄
        /// </summary>
        /// <param name="birthdate"></param>
        /// <returns></returns>
        private int GetAgeByBirthdate(DateTime birthdate)
        {
            DateTime now = DateTime.Now;
            int age = now.Year - birthdate.Year;
            if (now.Month < birthdate.Month || (now.Month == birthdate.Month && now.Day < birthdate.Day))
            {
                age--;
            }
            return age < 0 ? 0 : age;
        }

        private double GetTotalArea(DataRow[] rows, string columnName, bool trans = false)
        {
            double totalArea = 0;
            foreach (var row in rows)
            {
                if (trans)
                    totalArea += GetAcres(row[columnName].ToString());
                else
                    totalArea += Convert.ToDouble(row[columnName].ToString());
            }
            return totalArea;
        }

        /// <summary>
        /// 根据平方米获取亩
        /// </summary>
        /// <param name="sqm"></param>
        /// <returns></returns>
        private double GetAcres(string sqm)
        {
            return Convert.ToDouble(sqm) * 0.0015;
        }

        /// <summary>
        /// 获取性别
        /// </summary>
        /// <param name="cyxb"></param>
        /// <returns></returns>
        private string GetSex(string cyxb)
        {
            var returnVal = "";
            switch (cyxb)
            {
                case "1":
                    returnVal = "男";
                    break;
                case "2":
                    returnVal = "女";
                    break;
            }
            return returnVal;
        }

        #region 获得与户主关系
        /// <summary>
        /// 获得与户主关系
        /// </summary>
        /// <param name="yhzgx"></param>
        /// <returns></returns>
        private string GetRelationship(string yhzgx)
        {
            var returnVal = "";
            switch (yhzgx)
            {
                case "01":
                    returnVal = "户主";
                    break;
                case "10":
                    returnVal = "配偶";
                    break;
                case "11":
                    returnVal = "夫";
                    break;
                case "12":
                    returnVal = "妻";
                    break;

                case "20":
                    returnVal = "子";
                    break;
                case "21":
                    returnVal = "独生子";
                    break;
                case "22":
                    returnVal = "长子";
                    break;
                case "23":
                    returnVal = "次子";
                    break;
                case "24":
                    returnVal = "三子";
                    break;
                case "25":
                    returnVal = "四子";
                    break;
                case "26":
                    returnVal = "五子";
                    break;
                case "27":
                    returnVal = "养子或继子";
                    break;
                case "28":
                    returnVal = "女婿";
                    break;
                case "29":
                    returnVal = "其他儿子";
                    break;

                case "30":
                    returnVal = "女";
                    break;
                case "31":
                    returnVal = "独生女";
                    break;
                case "32":
                    returnVal = "长女";
                    break;
                case "33":
                    returnVal = "次女";
                    break;
                case "34":
                    returnVal = "三女";
                    break;
                case "35":
                    returnVal = "四女";
                    break;
                case "36":
                    returnVal = "五女";
                    break;
                case "37":
                    returnVal = "养女或继女";
                    break;
                case "38":
                    returnVal = "儿媳";
                    break;
                case "39":
                    returnVal = "其他女儿";
                    break;

                case "40":
                    returnVal = "孙子、孙女或外孙子、外孙女";
                    break;
                case "41":
                    returnVal = "孙子";
                    break;
                case "42":
                    returnVal = "孙女";
                    break;
                case "43":
                    returnVal = "外孙子";
                    break;
                case "44":
                    returnVal = "外孙女";
                    break;
                case "45":
                    returnVal = "孙媳妇或外孙媳妇";
                    break;
                case "46":
                    returnVal = "孙女婿或外孙女婿";
                    break;
                case "47":
                    returnVal = "曾孙子或外曾孙子";
                    break;
                case "48":
                    returnVal = "曾孙女或外曾孙女";
                    break;
                case "49":
                    returnVal = "其他孙子、孙女或外孙子、外孙女";
                    break;

                case "50":
                    returnVal = "父母";
                    break;
                case "51":
                    returnVal = "父亲";
                    break;
                case "52":
                    returnVal = "母亲";
                    break;
                case "53":
                    returnVal = "公公";
                    break;
                case "54":
                    returnVal = "婆婆";
                    break;
                case "55":
                    returnVal = "岳父";
                    break;
                case "56":
                    returnVal = "岳母";
                    break;
                case "57":
                    returnVal = "继父或养父";
                    break;
                case "58":
                    returnVal = "继母或养母";
                    break;
                case "59":
                    returnVal = "其他父母关系";
                    break;

                case "60":
                    returnVal = "祖父母或外祖父母";
                    break;
                case "61":
                    returnVal = "祖父";
                    break;
                case "62":
                    returnVal = "祖母";
                    break;
                case "63":
                    returnVal = "外祖父";
                    break;
                case "64":
                    returnVal = "外祖母";
                    break;
                case "65":
                    returnVal = "配偶的父祖母或外祖父母";
                    break;
                case "66":
                    returnVal = "曾祖父";
                    break;
                case "67":
                    returnVal = "曾祖母";
                    break;
                case "68":
                    returnVal = "配偶的曾祖父母或外曾祖父母";
                    break;
                case "69":
                    returnVal = "其它祖父母或外祖父母关系";
                    break;

                case "70":
                    returnVal = "兄弟姐妹";
                    break;
                case "71":
                    returnVal = "兄";
                    break;
                case "72":
                    returnVal = "嫂";
                    break;
                case "73":
                    returnVal = "弟";
                    break;
                case "74":
                    returnVal = "弟媳";
                    break;
                case "75":
                    returnVal = "姐姐";
                    break;
                case "76":
                    returnVal = "姐夫";
                    break;
                case "77":
                    returnVal = "妹妹";
                    break;
                case "78":
                    returnVal = "妹夫";
                    break;
                case "79":
                    returnVal = "其它兄弟姐妹";
                    break;
                case "80":
                    returnVal = "其他";
                    break;
                case "81":
                    returnVal = "伯父";
                    break;
                case "82":
                    returnVal = "伯母";
                    break;
                case "83":
                    returnVal = "叔父";
                    break;
                case "84":
                    returnVal = "婶母";
                    break;
                case "85":
                    returnVal = "舅父";
                    break;
                case "86":
                    returnVal = "舅母";
                    break;
                case "87":
                    returnVal = "姨父";
                    break;
                case "88":
                    returnVal = "姨母";
                    break;
                case "89":
                    returnVal = "姑父";
                    break;
                case "90":
                    returnVal = "姑母";
                    break;
                case "91":
                    returnVal = "堂兄弟、堂姐妹";
                    break;
                case "92":
                    returnVal = "表兄弟、表姐妹";
                    break;
                case "93":
                    returnVal = "侄子";
                    break;
                case "94":
                    returnVal = "侄女";
                    break;
                case "95":
                    returnVal = "外甥";
                    break;
                case "96":
                    returnVal = "外甥女";
                    break;
                case "97":
                    returnVal = "其它亲属";
                    break;
                case "99":
                    returnVal = "非亲属";
                    break;
            }
            return returnVal;
        }
        #endregion

    }
}
