using System;
using System.Data;
using System.Data.OleDb;
using System.Windows.Forms;

/// <summary>
/// Responsible for the query transactions in local Access DB file.
/// </summary>
namespace test.common
{

    class AccessHandler
    {

        //todo: Reconstruct it

        //Connection descriptor
        private string strCon { get; set; }

        //Database connection
        private OleDbConnection connection { get; set; }


        /// <summary>
        /// Get the local database connection for the use in this class, or
        /// for other class
        /// </summary>
        /// <returns>Connection</returns>
        public OleDbConnection GetConnection()
        {
            if (connection == null)
            {
                try
                {
                    connection = new OleDbConnection(strCon);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString() + "Program terminated.");
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Dispose();
                    }
                    Environment.Exit(1);
                }
            }
            
            return connection;
        }

        /// <summary>
        /// Execute the database query command and return the results in
        /// a DataTable.
        /// </summary>
        /// <param name="strSql">SQL command to be excuted.</param>
        /// <returns>Resulats in a DataTable.</returns>
        public DataTable Query(string strSql)
        {
            DataTable result = new DataTable();

            /**
             * If the program unable to get the required database connection, it simply report
             * this error and terminate.
             */
            if (connection == null && strCon == null)
            {
                //Failed to get the connection.
                MessageBox.Show("Unabale to get the database connestion. Program termanated.");
                if (connection.State == ConnectionState.Open)
                {
                    connection.Dispose();
                }
                Environment.Exit(1);
            }
            else
            {
                //Successfully get the connection.
                try
                {
                    //OleDbConnection connection = new OleDbConnection(strCon);

                    //Execute the command.
                    OleDbCommand command = new OleDbCommand(strSql, connection);
                    if(connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    OleDbDataReader reader;
                    reader = command.ExecuteReader();
                    result.Load(reader);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Dispose();
                    }
                    Environment.Exit(1);
                }              
            }
            return result;
        }

        /// <summary>
        /// Execute the SQL command without return value, such as DELETE.
        /// Rrturns the number of rows affected.
        /// </summary>
        /// <param name="strSql">SQL command to be executed.</param>
        public int ExecuteWithoutReturn(string strSql)
        {
            int affected = 0;
            if (connection == null)
            {
                connection = GetConnection();
            }
            else
            {
                try
                {
                    //OleDbConnection connection = new OleDbConnection(strCon);
                    if (connection.State != ConnectionState.Open)
                    {
                        connection.Open(); 
                    }
                    OleDbCommand command = new OleDbCommand(strSql, connection);
                    connection.Open();
                    return command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.ToString());
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Dispose();
                    }
                    Environment.Exit(1);
                }
            }
            return affected; 
        }

    }

}
