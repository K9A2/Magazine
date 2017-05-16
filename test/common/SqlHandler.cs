using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows;

namespace dbms
{
    /// <summary>
    /// 用来链接数据库，进行查询和清理无效链接
    /// </summary>
    class SqlHandler
    {
        //连接字符串
        private string strCon = "";

        /// <summary>
        /// 构造方法，可传入连接字符串，在数据库链接属性中可找到
        /// </summary>
        /// <param name="strCon">连接字符串</param>
        public SqlHandler(string strCon)
        {
            this.strCon = strCon;
        }

        /// <summary>
        /// 进行数据库连接
        /// </summary>
        /// <returns>返回数据库连接</returns>
        public SqlConnection Link()
        {
            SqlConnection conn = new SqlConnection(strCon);
            try
            {
                conn.Open();
            }
            catch (SqlException e)
            {
                MessageBox.Show(e.ToString());
            }
            return conn;
        }

        /// <summary>
        /// 根据提供的SQL语句进行查询
        /// </summary>
        /// <param name="conn">已经OPEN的数据库连接</param>
        /// <param name="cmdText">SQL语句</param>
        /// <returns>返回DataTable</returns>
        public DataTable Query(SqlConnection conn, string cmdText)
        {

            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = conn.CreateCommand();
                cmd.CommandText = cmdText;
                cmd.CommandTimeout = 3;
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                adapter.Fill(dt);
                cmd.Dispose();
            }
            catch (Exception e)
            {
                MessageBox.Show("详细信息"+e, "数据库连接失败！");
            }

            return dt;
        }

        /// <summary>
        /// 清理不需要的数据库连接
        /// </summary>
        /// <param name="conn">需要清理的连接</param>
        public void Disopse(SqlConnection conn)
        {
            conn.Close();
            conn.Dispose();
        }
    }
}
