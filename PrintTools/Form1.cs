using grproLib;
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

namespace PrintTools
{

    public partial class Form1 : Form
    {
        private string keyVal = "sinldo.com";
        private string ivVal = "http://www.sinldo.com";
        List<string> list = new List<string>();
        List<string> listDb = new List<string>();
        string currentCBFMB = "";
        DataTable dk_dt;
        DataTable xx_dt;
        DataTable ht_dt;
        //定义Grid++Report报表主对象
        private GridppReport Report = new GridppReport();

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


        private void btnSure_Click(object sender, EventArgs e)
        {



        }

        private string CutStr(string str)
        {
            int index = str.IndexOf("，");
            if (index > -1)
                str = str.Substring(index + 2, str.Length - index - 2);

            else
            {
                index = str.IndexOf(",");
                if (index > -1)
                    str = str.Substring(index + 2, str.Length - index - 2);
            }
            return str;
        }

        private DataSet GetNewDs(DataTable dk_dt, DataTable xx_dt, DataTable fbf_dt, DataTable ht_dt)
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

                    //机动地判断
                    string lastCode = currentCBFBM.Substring(currentCBFBM.Length - 4);
                    if (lastCode == "9001")
                        continue;

                    if (currentCBFBM == oldCBFBM)
                        continue;

                    var cbfmc = row["CBFMC"].ToString();
                    var dkRows = dk_dt.Select("CBFBM='" + currentCBFBM + "'");
                    var xxRows = xx_dt.Select("CBFBM='" + currentCBFBM + "'");
                    var dkCount = dkRows.Length;//地块数目
                    var xxCount = xxRows.Length;//家庭人员数量
                    var tHTMJRows = ht_dt.Select("CBFBM='" + currentCBFBM + "'");
                    //var totalHTMJ = GetTotalArea(dkRows, "HTMJ").ToString("0.00");
                    //var totalSCMJ = GetTotalArea(dkRows, "SCMJ", true).ToString("0.00");
                    var totalHTMJ = tHTMJRows.Length == 0 ? "0" : GetAcres(tHTMJRows[0]["YHTZMJ"].ToString()).ToString("0.00");
                    var totalSCMJ = tHTMJRows.Length == 0 ? "0" : GetAcres(tHTMJRows[0]["HTZMJ"].ToString()).ToString("0.00");
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

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrint_Click(object sender, EventArgs e)
        {


            //载入报表模板文件
            //关于动态设置报表路径与数据绑定参数请参考其它例子程序
            var tempetePath = AppDomain.CurrentDomain.BaseDirectory;
            Report.LoadFromFile(Path.Combine(tempetePath, "Template", "template1_2.grf"));

            //连接报表事件
            Report.Initialize += new _IGridppReportEvents_InitializeEventHandler(ReportGetData);

        }

        private void ReportGetData()
        {

            #region MyRegion
            //表二参数
            Report.ParameterByName("FBFMC").AsString = "中国工商";//发包方名称
            Report.ParameterByName("CBFMC").AsString = "锐浪软件技术有限公司";//承包方名称
            Report.ParameterByName("CYZJHM").AsInteger = 2008;//身份证号
            Report.ParameterByName("HTDM").AsInteger = 8;//合同代码 承包方编码+J
            Report.ParameterByName("CBRQ").AsInteger = 8;//承包期限  CBF-> CBQSRQ CBZZRQ

            //家庭成员
            for (int i = 1; i <= 12; i++)
            {
                string RELNAME = "RELNAME" + i;
                string KINSHIP = "KINSHIP" + i;
                string RELBAK = "RELBAK" + i;

            }
            //地块信息
            for (int i = 1; i <= 12; i++)
            {
                string DKDM = "DKDM" + i;
                string ZLXX = "ZLXX" + i;
                string SCMJ = "SCMJ" + i;
                string SFNT = "SFNT" + i;//是否基本农田
                string HTMJ = "HTMJ" + i;


            }

            #endregion


        }

        /// <summary>
        /// 查询信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchInfo_Click(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// 确认选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (cbDbSelect.SelectedIndex <= 0)
            {
                MessageBox.Show("请选择一个单位进行操作！");
                return;
            }
            if (string.IsNullOrEmpty(currentCBFMB))
            {
                MessageBox.Show("尚未选择数据！");
                return;
            }

            string dbConnStr = Path.Combine("DB", cbDbSelect.SelectedItem.ToString() + ".mdb");
            AccessDB accessHelper = new AccessDB(dbConnStr);
            dk_dt = accessHelper.ExecuteDataTable("select FBFBM,CBFBM,CBFMC,DKBM,DKMC,HTMJ,SCMJ,DKDZ,DKNZ,DKXZ,DKBZ from CBDKDC WHERE CBFBM ='" + currentCBFMB + "' ORDER BY DKBM");
            xx_dt = accessHelper.ExecuteDataTable("select CBFBM,CYXM,CYXB,CSRQ,CYZJHM,YHZGX from CBF_JTCY WHERE CBFBM ='" + currentCBFMB + "'");
            ht_dt = accessHelper.ExecuteDataTable("select CBFBM,HTZMJ,YHTZMJ from CBHT WHERE CBFBM ='" + currentCBFMB + "'");
        }
    }
}
