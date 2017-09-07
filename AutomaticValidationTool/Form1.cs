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

namespace AutomaticValidationTool
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
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
                if (file.Extension == ".mdb") { 
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
                MessageBox.Show("请检查【"+ db_dirpath+"】文件夹下是否存在.mdb文件");
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
                MessageBox.Show("请检查【" + db_dirpath + "】文件夹下是否存在.sqlite文件");
                return;
            }

            #endregion

            #region 3.六个表正确性自动检测

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

            #region ① 发包方调查表
            var fbfDt = accessHelper.ExecuteDataTable("select * from FBF");
            if (fbfDt != null && fbfDt.Rows.Count > 0)
            {
                
                for (int i = 0; i < fbfDt.Rows.Count; i++)
                {
                    var insertSqlList = new List<string>();
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
                    insertSql = GetInspectTableInsertSql(IT_ID, IT_TableType, IT_TableName, IT_FieldName, IT_FieldValue,IT_InspectResult, "", IT_Inspector, IT_InspectTime, IT_VerifyUser, IT_VerifyTime);
                    insertSqlList.Add(insertSql);
                    #endregion

                    #region InspectField 5条数据组装
                    insertSql = GetInspectFieldInsertSql(Guid.NewGuid().ToString(), IT_ID, "1", "FBFMC","", row["FBFMC"].ToString().Trim(),"3","真实性","2","");
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

                    var result = sqlliteHelper.ExecuteTransNonQuery(insertSqlList);


                }
                
            }

            #endregion

            #region ② 承包方调查表

            #endregion

            #region ③ 发包方调查表

            #endregion

            #endregion

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
        private string GetInspectFieldInsertSql(string ID, string TableID, string InspectRule, string Name,string Key, string Value, string InspectCategoryID, string InspectCategoryName, string InspectResult, string InspectRemark)
        {
            string insertSql = string.Format("insert into InspectField(ID,TableID,InspectRule,Name,Key,Value,InspectCategoryID,InspectCategoryName,InspectResult,InspectRemark) values('{0}','{1}',{2},'{3}','{4}','{5}',{6},'{7}',{8},'{9}')", ID, TableID, InspectRule, Name, Key, Value, InspectCategoryID, InspectCategoryName, InspectResult, InspectRemark);
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
        private string GetInspectTableInsertSql(string ID,string TableType, string TableName, string FieldName, string FieldValue, string InspectResult, string InspectRemark, string Inspector, string InspectTime, string VerifyUser, string VerifyTime)
        {
            string insertSql = string.Format("insert into InspectTable(ID,TableType,TableName,FieldName,FieldValue,InspectResult,InspectRemark,Inspector,InspectTime,VerifyUser,VerifyTime) values('{0}',{1},'{2}','{3}','{4}',{5},'{6}','{7}','{8}','{9}','{10}')", ID, TableType, TableName, FieldName, FieldValue, InspectResult, InspectRemark, Inspector, InspectTime, VerifyUser, VerifyTime);
            return insertSql;
        }


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
