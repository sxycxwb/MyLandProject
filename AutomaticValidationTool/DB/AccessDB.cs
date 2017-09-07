using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.OleDb;

namespace AutomaticValidationTool
{
    public class AccessDB
    {
        private OleDbConnection connection = null;

        //----创建连接串并连接数据库----
        public AccessDB(string path, string password="")
        {
            string conn_str = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + path+";";
            connection = new OleDbConnection(conn_str);
            connection.Open();
        }

        //----关闭数据库连接----
        public void CloseConnection()
        {
            connection.Close();
            connection = null;
        }

        /// <summary> 
        /// 执行一个查询语句，返回一个包含查询结果的DataTable 
        /// </summary> 
        /// <param name="sql">要执行的查询语句</param> 
        /// <param name="parameters">执行SQL查询语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public DataTable ExecuteDataTable(string sql, OleDbParameter[] parameters=null)
        {
            try
            {
                using (OleDbCommand Command = new OleDbCommand(sql, connection))
                {
                    if (parameters != null)
                    {
                        Command.Parameters.AddRange(parameters);
                    }
                    OleDbDataAdapter adapter = new OleDbDataAdapter(Command);
                    DataTable dataTable = new DataTable();
                    adapter.Fill(dataTable);
                    return dataTable;
                }
            }
            catch (OleDbException ex)
            {
                System.Exception exc = ex;
                throw (exc);
            }
        }

        /// <summary> 
        /// 对Access数据库执行增删改操作，返回受影响的行数。 
        /// </summary> 
        /// <param name="sql">要执行的增删改的SQL语句</param> 
        /// <param name="parameters">执行增删改语句所需要的参数，参数必须以它们在SQL语句中的顺序为准</param> 
        /// <returns></returns> 
        public int ExecuteNonQuery(string sql, OleDbParameter[] parameters)
        {
            int affectRows = 0;

            try
            {
                using (OleDbTransaction Transaction = connection.BeginTransaction())
                {
                    using (OleDbCommand Command = new OleDbCommand(sql, connection, Transaction))
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
            catch (OleDbException ex)
            {
                affectRows = -1;
                //log.Error("ExecuteNonQuery occurs exception：" + ex.Message);
            }
            return affectRows;
        }
    }
}