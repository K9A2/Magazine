using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;
using test.Properties;

namespace test.common
{

    /// <summary>
    /// A static class responsible for Access operations.
    /// </summary>
    public static class AccessUtil
    {

        /// <summary>
        /// Get the database connection according to the given connection string and return it as result. If it is 
        /// unable to get to the database connection, it returns null.
        /// </summary>
        /// <param name="strCon">Connection string</param>
        /// <returns>Valid database connection</returns>
        public static OleDbConnection GetConnection(string strCon)
        {
            OleDbConnection connection;

            try
            {
                connection = new OleDbConnection(strCon);
            }
            catch (Exception e)
            {
                MessageBox.Show(Resources.string_no_database_connection + e + Resources.string_try_again_later);
                return null;
            }

            return connection;
        }

        /// <summary>
        /// Execute the database query command and return the results in a DataTable. If it is unable to finish 
        /// successfully, it return null.
        /// </summary>
        /// <param name="strSql">SQL command to be excuted.</param>
        /// <param name="connection">Open or non-open database connection.</param>
        /// <returns>Resulats in a DataTable.</returns>
        public static DataTable Query(string strSql, OleDbConnection connection)
        {
            DataTable result = new DataTable();

            /**
             * If the program unable to get the required database connection, it simply reports this error and
             * returns null.
             */
            if (connection == null)
            {
                //Failed to get the connection.
                MessageBox.Show(Resources.string_no_database_connection + Resources.string_try_again_later);
                return null;
            }

            if (connection.State != ConnectionState.Open)
            {
                try
                {
                    connection.Open();
                }
                catch (Exception e)
                {
                    MessageBox.Show(Resources.string_unable_to_open_connection + e +
                                    Resources.string_try_again_later);
                    return null;
                }
            }

            //Successfully get the connection. Execute the command.
            OleDbCommand command = new OleDbCommand(strSql, connection);
            try
            {
                OleDbDataReader reader = command.ExecuteReader();
                if (!reader.HasRows)
                {
                    return null;
                }
                result.Load(reader);
            }
            catch (Exception e)
            {
                MessageBox.Show(Resources.string_unable_to_read_result + e + Resources.string_try_again_later);
                return null;
            }

            return result;
        }

        /// <summary>
        /// Execute the SQL command without return value, such as DELETE. If it is unable to finish successfully, 
        /// it returns -1.
        /// Rrturns the number of rows affected.
        /// </summary>
        /// <param name="strSql">SQL command to be executed.</param>
        /// <param name="connection">Open or non-open database connection.</param>
        public static int ExecuteWithoutReturn(string strSql, OleDbConnection connection)
        {
            //-1 means error. If this "strSQL" is successfully executed, it returns the number of rows affected.
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();
                }
                OleDbCommand command = new OleDbCommand(strSql, connection);
                connection.Open();
                //Returns the number of affected rows.
                return command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                MessageBox.Show(Resources.string_unable_to_execute_command + e + Resources.string_try_again_later);
                connection.Dispose();
                return -1;
            }
        }
    }

}
