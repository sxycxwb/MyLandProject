using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SQLite;

namespace AutomaticValidationTool
{
    public class SqlLiteBD
    {
        SQLiteConnection connection = null;
        SQLiteTransaction transaction = null;
        string conn_str = "";

        //----创建连接串并连接数据库----
        public SqlLiteBD(string path)
        {
            conn_str = "data source=" + path + ";Version=3;";
        }

        public bool Connect()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }

                connection = new SQLiteConnection(conn_str);
                connection.Open();
                if (connection == null)
                {
                    return false;
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        //----修改数据库密码----
        public bool ChangePassword(string newPassword)
        {
            try
            {
                connection.ChangePassword(newPassword);
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        //----关闭数据库连接----
        public bool DisConnect()
        {
            try
            {
                if (connection != null)
                {
                    connection.Close();
                    connection = null;
                }
                return true;
            }
            catch (SQLiteException ex)
            {
                return false;
            }
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string sql, SQLiteParameter[] parameters=null)
        {
            try
            {
                using (SQLiteCommand Command = new SQLiteCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        Command.Parameters.AddRange(parameters);
                    }
                    SQLiteDataAdapter adapter = new SQLiteDataAdapter(Command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (SQLiteException ex)
            {
                return null;
            }
        }

        /// <summary> 
        /// 对SQLite数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string sql, SQLiteParameter[] parameters)
        {
            int affectRows = 0;

            try
            {
                using (SQLiteTransaction Transaction = connection.BeginTransaction())
                {
                    using (SQLiteCommand Command = new SQLiteCommand(sql, connection, Transaction))
                    {
                        if (parameters != null)
                        {
                            Command.Parameters.AddRange(parameters);
                        }
                        affectRows = Command.ExecuteNonQuery();
                    }
                    Transaction.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                affectRows = -1;
            }
            return affectRows;
        }

        public int ExecuteTransNonQuery(List<string> sqlList)
        {
            int affectRows = 0;
            try
            {
                using (SQLiteTransaction Transaction = connection.BeginTransaction())
                {
                    foreach (var sql in sqlList)
                    {
                        using (SQLiteCommand Command = new SQLiteCommand(sql, connection, Transaction))
                        {
                            affectRows = Command.ExecuteNonQuery();
                        }
                    }
                    Transaction.Commit();
                }
            }
            catch (SQLiteException ex)
            {
                affectRows = -1;
            }
            return affectRows;
        }

        //收缩数据库 VACUUM 
        public bool Vacuum()
        {
            try
            {
                using (SQLiteCommand Command = new SQLiteCommand("VACUUM", connection))
                {
                    Command.ExecuteNonQuery();
                }
                return true;
            }
            catch (System.Data.SQLite.SQLiteException ex)
            {
                return false;
            }

        }

        public void BeginTransaction()
        {
            try
            {
                transaction = connection.BeginTransaction();
            }
            catch (SQLiteException ex)
            {

            }
        }

        public void CommitTransaction()
        {
            try
            {
                transaction.Commit();
            }
            catch (SQLiteException ex)
            {

            }
        }

        public void RollbackTransaction()
        {
            try
            {
                transaction.Rollback();
            }
            catch (SQLiteException ex)
            {

            }
        }

        //public void test()
        //{
        //    SqlLiteBD helper = new SqlLiteBD("D:\\mysqlite.db", "123456");     //连接到D盘下的mysqlite.db数据库,连接密码为123456
        //    //bool ch = helper.ChangePassword("654321");                            //将密码修改为：654321
        //    helper.Connect();

        //    string select_sql = "select * from student";                            //查询的SQL语句
        //    DataTable dt = helper.ExecuteDataTable(select_sql, null);               //执行查询操作,结果存放在dt中

        //    //向数据库中student表中插入了一条(name = "马兆瑞"，sex = "男"，telephone = "15550000000")的记录
        //    string insert_sql = "insert into student(name,sex,telephone) values(?,?,?)";        //插入的SQL语句(带参数)
        //    SQLiteParameter[] para = new SQLiteParameter[3];                        //构造并绑定参数
        //    string[] tag = { "name", "sex", "telephone" };
        //    string[] value = { "马兆瑞", "男", "15550000000" };
        //    for (int i = 0; i < 3; i++)
        //    {
        //        para[i] = new SQLiteParameter(tag[i], value[i]);
        //    }
        //    int ret = helper.ExecuteNonQuery(insert_sql, para);                     //执行插入操作
        //}
    }
}