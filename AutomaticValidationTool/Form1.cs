using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using UtilityCode;

namespace AutomaticValidationTool
{
    public partial class Form1 : Form
    {
        private string keyVal = "sinldo.com";
        private string ivVal = "http://www.sinldo.com";
        List<string> list = new List<string>();

        public Form1()
        {
            InitializeComponent();
            try
            {
                string[] arr = File.ReadAllLines(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Spire.Pdf.dll"));
                foreach (var str in arr)
                {
                    list.Add(EncryptUtil.UnAesStr(str, keyVal, ivVal));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("非法访问，所有功能已失效！");
                return;
            }
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            #region 1.获取access数据库连接字符串并测试连接情况
            string accessDBConStr = "";
            string path = txtCombinePath.Text;
            string db_dirpath = GetConfigDict("ACCESS_DB");
            DirectoryInfo TheFolder = new DirectoryInfo(Path.Combine(path, db_dirpath));
            //遍历文件夹下文件
            var fileArr = TheFolder.GetFiles();
            foreach (FileInfo file in fileArr)
            {
                if (file.Extension == ".mdb")
                {
                    accessDBConStr = Path.Combine(path, db_dirpath, file.Name);
                    break;
                }
            }
            AccessDB accessHelper = new AccessDB(accessDBConStr);
            try
            {
                var testDt = accessHelper.ExecuteDataTable("select count(1) from CBF");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查【" + db_dirpath + "】文件夹下是否存在.mdb文件");
                return;
            }
            #endregion

            #region 2.获取sqllite数据库连接字符串并测试连接情况
            string sqlliteDBConStr = "";
            db_dirpath = GetConfigDict("SQLLITE_DB");
            TheFolder = new DirectoryInfo(Path.Combine(path, db_dirpath));
            //遍历文件夹下文件
            fileArr = TheFolder.GetFiles();
            foreach (FileInfo file in fileArr)
            {
                if (file.Extension == ".sqlite")
                {
                    sqlliteDBConStr = Path.Combine(path, db_dirpath, file.Name);
                    break;
                }
            }
            SqlLiteBD sqlliteHelper = new SqlLiteBD(sqlliteDBConStr);
            sqlliteHelper.Connect();
            try
            {
                var testDt = sqlliteHelper.ExecuteDataTable("SELECT COUNT(1) FROM INSPECTFIELD");
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查【" + db_dirpath + "】文件夹下是否存在.sqlite文件\r\n提示：通过质检后，打开“正确性检查”页签即可生成该.sqlite文件！");
                return;
            }

            #endregion

            #region 3.获取dbf数据并测试连接情况
            //string dbfDBConStr = "";
            //string dbName = "";
            //var fileItem = GetDbfFileInfo("DK");

            //dbfDBConStr = fileItem.DirectoryName;
            //dbName = Path.GetFileNameWithoutExtension(fileItem.Name);

            //DbfDB dbfHelper = new DbfDB(dbfDBConStr);
            //try
            //{
            //    var testDt = dbfHelper.ExecuteDataTable(dbName);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show("请检查【" + db_dirpath + "】文件夹下是否存在.dbf,或者没有安装《VFPOLEDBSetup.msi》软件");
            //    return;
            //}

            #endregion

            #region 4.六个表正确性自动检测

            #region 获取检查人信息和检查日期
            string checkUser = txtCheckName.Text.Trim();
            string checkDate = Convert.ToDateTime(txtCheckDate.Text).ToString("yyyy/M/d");

            #endregion

            #region 定义sqllite检测两个表字段
            //InspectTable 字段
            string IT_ID = "", IT_TableType = "", IT_TableName = "", IT_FieldName = "", IT_FieldValue = "", IT_InspectResult = "", IT_InspectRemark = "", IT_Inspector = "", IT_InspectTime = "", IT_VerifyUser = "", IT_VerifyTime = "";

            //InspectField 字段
            string IF_ID = "", IF_TableID = "", IF_InspectRule = "", IF_Name = "", IF_Key = "", IF_Value = "", IF_InspectCategoryID = "", IF_InspectCategoryName = "", IF_InspectResult = "", IF_InspectRemark = "";
            #endregion

            #region  表数据查询
            //发包方数据
            var fbfDt = accessHelper.ExecuteDataTable("select * from FBF");
            //承包方数据
            var cbfDt = accessHelper.ExecuteDataTable("select * from CBF");
            //承包方地块数据
            var cbfdcDt = accessHelper.ExecuteDataTable("select * from CBDKDC");
            //承包方联系人数据
            var cbfLinkDt = accessHelper.ExecuteDataTable("select * from CBF_JTCY");
            //承包经营权信息数据
            var cbjyqDt = accessHelper.ExecuteDataTable("select * from CBHT");

            //承包方与地块关系数据
            var cbfdkDt = accessHelper.ExecuteDataTable("select * from CBDKXX");
            //地块信息数据
            //var dkDt = dbfHelper.ExecuteDataTable(dbName);


            #endregion

            #region 授权监测
            if (fbfDt != null && fbfDt.Rows.Count > 0)
            {
                for (int i = 0; i < fbfDt.Rows.Count; i++)
                {
                    DataRow row = fbfDt.Rows[i];
                    string fbfbm = row["FBFBM"].ToString().Trim();
                    if (!CheckNum(fbfbm))
                    {
                        MessageBox.Show("您正在处理非授权数据，请联系管理员！");
                        return;
                    }
                }
            }
            #endregion

            #region ① 发包方调查表
            if (fbfDt != null && fbfDt.Rows.Count > 0)
            {
                var insertSqlList = new List<string>();
                for (int i = 0; i < fbfDt.Rows.Count; i++)
                {
                    DataRow row = fbfDt.Rows[i];

                    string insertSql = "";

                    #region InspectTable 组装
                    IT_ID = Guid.NewGuid().ToString();
                    IT_TableType = "1";
                    IT_TableName = "FBF";
                    IT_FieldName = "FBFBM";
                    IT_FieldValue = row["FBFBM"].ToString().Trim();
                    IT_InspectResult = "2";
                    IT_Inspector = checkUser;
                    IT_InspectTime = checkDate;
                    IT_VerifyUser = checkUser;
                    IT_VerifyTime = checkDate;
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue, IT_InspectResult, "null", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 5条数据组装
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "FBFMC", "", row["FBFMC"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "FBFBM", "", row["FBFBM"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "FBFFZRXM", "", row["FBFFZRXM"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "FBFDCY", "", row["FBFDCY"].ToString().Trim(), "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "SHR", "", row["SHR"].ToString().Trim(), "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);
                    #endregion

                }

                var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);
            }

            #endregion

            #region ② 承包方调查表
            if (cbfDt != null && cbfDt.Rows.Count > 0)
            {
                var insertSqlList = new List<string>();
                for (int i = 0; i < cbfDt.Rows.Count; i++)
                {
                    DataRow row = cbfDt.Rows[i];
                    string insertSql = "";

                    #region InspectTable 组装
                    IT_ID = Guid.NewGuid().ToString();
                    IT_TableName = "CBF";
                    IT_FieldName = "CBFBM";
                    IT_FieldValue = row["CBFBM"].ToString().Trim();
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue, IT_InspectResult, "null", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 5条数据组装
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFBM", "", row["CBFBM"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFMC", "", row["CBFMC"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFMCQZ", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFDCY", "", row["CBFDCY"].ToString().Trim(), "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "SHR", "", row["SHR"].ToString().Trim(), "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);
                    #endregion
                }
                var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);
            }
            #endregion

            #region ③ 发包方调查表
            if (cbfdcDt != null && cbfdcDt.Rows.Count > 0)
            {
                var insertSqlList = new List<string>();
                for (int i = 0; i < cbfdcDt.Rows.Count; i++)
                {

                    DataRow row = cbfdcDt.Rows[i];

                    string insertSql = "";

                    #region InspectTable 组装
                    IT_ID = Guid.NewGuid().ToString();
                    IT_TableName = "CBDKDC";
                    IT_FieldName = "DKBM";
                    IT_FieldValue = row["DKBM"].ToString().Trim();
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue, IT_InspectResult, "null", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 5条数据组装
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFMCQZ", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "DCY", "", row["DCY"].ToString().Trim(), "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "SHR", "", row["SHR"].ToString().Trim(), "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "JZDZBCLJD", "", "", "1", "准确性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "DKSCMJJD", "", "", "1", "准确性", "2", "");
                    insertSqlList.Add(insertSql);
                    #endregion


                }
                var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);

            }
            #endregion

            #region ④ 承包经营权调查结果核实表
            if (cbjyqDt != null && cbjyqDt.Rows.Count > 0)
            {
                var insertSqlList = new List<string>();
                for (int i = 0; i < cbjyqDt.Rows.Count; i++)
                {
                    DataRow row = cbjyqDt.Rows[i];

                    string insertSql = "";
                    string cbfbm = row["CBFBM"].ToString().Trim();
                    #region InspectTable 组装
                    IT_ID = Guid.NewGuid().ToString();
                    IT_TableName = "CBJYQDCHS";
                    IT_FieldName = "CBFBM";
                    IT_FieldValue = cbfbm;
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue, IT_InspectResult, "null", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 字段1
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBQSRQ", "", string.IsNullOrEmpty(row["CBQXQ"].ToString().Trim()) ? "" : row["CBQXQ"].ToString().Trim() , "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFS", "CBFS", row["CBFS"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBZZRQ", "", string.IsNullOrEmpty(row["CBQXZ"].ToString().Trim()) ? "" : row["CBQXZ"].ToString().Trim(), "3", "真实性", "2", "");
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 联系人数据组装

                    DataRow[] linkRows = cbfLinkDt.Select("CBFBM='" + cbfbm + "'");
                    if (linkRows.Length > 0)
                    {
                        foreach (DataRow item in linkRows)
                        {
                            string key = string.Format("CYXM${0}@{1}@{2}", item["CBFBM"].ToString().Trim(), item["CYXM"].ToString().Trim(), item["CYXB"].ToString().Trim());
                            insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", item["CYXM"].ToString().Trim(), "3", "真实性", "2", "");
                            insertSqlList.Add(insertSql);
                        }
                    }

                    #endregion

                    #region InspectField 地块数据组装
                    var dkRows = cbfdkDt.Select("CBFBM = '" + cbfbm + "'");
                    if (dkRows.Length > 0)
                    {
                        foreach (var dkItem in dkRows)
                        {
                            string dkCode = dkItem["DKBM"].ToString();
                            DataRow[] rows = cbfdcDt.Select("DKBM = '" + dkCode + "'");
                            if (rows.Length > 0)
                            {
                                string key = string.Format("CB_DKXZ${0}@{1}", cbfbm, dkCode);
                                insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", rows[0]["DKXZ"].ToString().Trim(), "3", "真实性", "2", "");
                                insertSqlList.Add(insertSql);

                                key = string.Format("CB_DKDZ${0}@{1}", cbfbm, dkCode);
                                insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", rows[0]["DKDZ"].ToString().Trim(), "3", "真实性", "2", "");
                                insertSqlList.Add(insertSql);

                                key = string.Format("CB_DKNZ${0}@{1}", cbfbm, dkCode);
                                insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", rows[0]["DKNZ"].ToString().Trim(), "3", "真实性", "2", "");
                                insertSqlList.Add(insertSql);

                                key = string.Format("CB_DKBZ${0}@{1}", cbfbm, dkCode);
                                insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", rows[0]["DKBZ"].ToString().Trim(), "3", "真实性", "2", "");
                                insertSqlList.Add(insertSql);

                                key = string.Format("CB_DKBM${0}@{1}", cbfbm, dkCode);
                                insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", dkCode, "3", "真实性", "2", "");
                                insertSqlList.Add(insertSql);

                                key = string.Format("CB_QDFS${0}@{1}", cbfbm, dkCode);
                                insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", key, "", rows[0]["CBJYQQDFS"].ToString().Trim(), "3", "真实性", "2", "");
                                insertSqlList.Add(insertSql);
                            }

                        }
                    }


                    #endregion

                    #region 承包方信息数据组装

                    var cbfRow = cbfDt.Select("CBFBM='" + cbfbm + "'");
                    if (cbfRow.Length > 0)
                    {
                        insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFDCY", "", cbfRow[0]["CBFDCY"].ToString().Trim(), "2", "法律有效性", "2", "");
                        insertSqlList.Add(insertSql);

                        insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "HSSHRQ", "", string.IsNullOrEmpty(cbfRow[0]["HSSHRQ"].ToString().Trim()) ? "" : cbfRow[0]["HSSHRQ"].ToString().Trim(), "2", "法律有效性", "2", "");
                        insertSqlList.Add(insertSql);

                        insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "HSSHR", "", cbfRow[0]["HSSHR"].ToString().Trim(), "2", "法律有效性", "2", "");
                        insertSqlList.Add(insertSql);

                        insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "CBFMCQZ", "", "", "2", "法律有效性", "2", "");
                        insertSqlList.Add(insertSql);
                    }

                    #endregion

                }
                var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);

            }
            #endregion

            #region ⑤ 承包经营权调查信息公示表
            if (fbfDt != null && fbfDt.Rows.Count > 0)
            {
                var insertSqlList = new List<string>();
                for (int i = 0; i < fbfDt.Rows.Count; i++)
                {
                    DataRow row = fbfDt.Rows[i];

                    string insertSql = "";

                    #region InspectTable 组装
                    IT_ID = Guid.NewGuid().ToString();
                    IT_TableName = "CBJYQDCGS";
                    IT_FieldName = "FBFBM";
                    IT_FieldValue = row["FBFBM"].ToString().Trim();
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue, IT_InspectResult, "null", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 3条数据组装
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "GSGG", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "GSBG", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "GSJG", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);
                    #endregion

                }

                var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);
            }
            #endregion

            #region ⑥ 承包经营权公示结果归户表
            if (cbfDt != null && cbfDt.Rows.Count > 0)
            {
                var insertSqlList = new List<string>();
                for (int i = 0; i < cbfDt.Rows.Count; i++)
                {
                    DataRow row = cbfDt.Rows[i];
                    string insertSql = "";

                    #region InspectTable 组装
                    IT_ID = Guid.NewGuid().ToString();
                    IT_TableName = "CBJYQGH";
                    IT_FieldName = "CBFBM";
                    IT_FieldValue = row["CBFBM"].ToString().Trim();
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue, IT_InspectResult, "null", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 5条数据组装
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "YHTMJ", "", "", "1", "准确性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "SCMJ", "", "", "1", "准确性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "WSHTQQMJ", "", "", "1", "准确性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "GSJGGHBQZ", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);

                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "WSHTQZ", "", "", "2", "法律有效性", "2", "");
                    insertSqlList.Add(insertSql);
                    #endregion
                }
                var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);
                #endregion

                #endregion

                MessageBox.Show("正确性检查已自动完成!\r\n请在《数据库成果质量检查软件》中关闭“正确性检查”页签后重新打开！");
            }

        }

        /// <summary>
        /// 获得InspectField 插入语句
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="TableID"></param>
        /// <param name="InspectRule"></param>
        /// <param name="Name"></param>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        /// <param name="InspectCategoryID"></param>
        /// <param name="InspectCategoryName"></param>
        /// <param name="InspectResult"></param>
        /// <param name="InspectRemark"></param>
        /// <returns></returns>
        private string GetInspectFieldInsertSql(string ID, string TableID, string InspectRule, string Name, string Key, string Value, string InspectCategoryID, string InspectCategoryName, string InspectResult, string InspectRemark)
        {
            string insertSql = string.Format("insert into InspectField(ID,TableID,InspectRule,Name,Key,Value,InspectCategoryID,InspectCategoryName,InspectResult,InspectRemark) values('{0}','{1}',{2},'{3}','{4}',{5},{6},'{7}',{8},'{9}')", ID, TableID, InspectRule, Name, Key, string.IsNullOrEmpty(Value) ? "null" : "'" + Value + "'", InspectCategoryID, InspectCategoryName, InspectResult, InspectRemark);
            return insertSql;
        }

        /// <summary>
        /// 获得InspectTable 插入语句
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="TableType"></param>
        /// <param name="TableName"></param>
        /// <param name="FieldName"></param>
        /// <param name="FieldValue"></param>
        /// <param name="InspectResult"></param>
        /// <param name="InspectRemark"></param>
        /// <param name="Inspector"></param>
        /// <param name="InspectTime"></param>
        /// <param name="VerifyUser"></param>
        /// <param name="VerifyTime"></param>
        /// <returns></returns>
        private string GetInspectTableInsertSql(string ID, string TableType, string TableName, string FieldName, string FieldValue, string InspectResult, string InspectRemark, string Inspector, string InspectTime, string VerifyUser, string VerifyTime)
        {
            string insertSql = string.Format("insert into InspectTable(ID,TableType,TableName,FieldName,FieldValue,InspectResult,InspectRemark,Inspector,InspectTime,VerifyUser,VerifyTime) values('{0}',{1},'{2}','{3}','{4}',{5},{6},'{7}','{8}','{9}','{10}')", ID, TableType, TableName, FieldName, FieldValue, InspectResult, InspectRemark, Inspector, InspectTime, VerifyUser, VerifyTime);
            return insertSql;
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

        #region 获取dbf连接字符串文件信息
        private FileInfo GetDbfFileInfo(string startCode)
        {
            string path = txtCombinePath.Text;
            string db_dirpath = GetConfigDict("DBF_DB");
            var TheFolder = new DirectoryInfo(Path.Combine(path, db_dirpath));
            //遍历文件夹下文件
            var fileArr = TheFolder.GetFiles();
            foreach (FileInfo file in fileArr)
            {
                if (file.Extension == ".dbf")
                {
                    if (file.Name.IndexOf(startCode) > -1)//地块信息
                    {
                        return file;
                        break;
                    }
                }
            }
            return null;
        }

        #endregion

        /// <summary>
        /// 设置路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSetPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请设置待正确性检测的文件夹(包含权属数据,汇总表格等文件夹)";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string foldPath = dialog.SelectedPath;
                txtCombinePath.Text = foldPath;
            }
        }

        private string GetConfigDict(string code)
        {
            XmlDocument doc = new XmlDocument();
            string configFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config.xml");
            doc.Load(configFilePath);

            XmlNodeList nodes = doc.SelectNodes("/Config/" + code + "/value");
            return nodes[0].InnerText;
        }
    }
}
