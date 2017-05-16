using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{

    //Event handler for user login
    //public delegate void UserLoginHandler(OleDbConnection connection, User user);


    /// <summary>
    /// Login window.
    /// </summary>
    public partial class Login
    {

        //TODO: Use delegation and event to exchange messages between windows

        //Database connection.
        private OleDbConnection _connection;

        //User login event and delegate
        public delegate void UserLoginHandler(User user, OleDbConnection connection);
        public event UserLoginHandler UserLoginEvent;

        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute while login windown loaded. Designed for set lbl_woronginput as an invisible control.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            LblWronginput.Visibility = Visibility.Hidden;

            //Check if the Access database file exiets
            if (File.Exists(@"./dbo.mdb"))
            {
                return;
            }
            MessageBox.Show("The Access database file does not exists" +
                            Properties.Resources.string_program_terminated, "Fatal Error");
            Environment.Exit(-1);
        }

        /// <summary>
        /// Make this window dragable.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Mouse button event</param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        /// <summary>
        /// Mininize this window when clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Close this window and terminate this program when clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        /// Start the authentication process when clicked.
        /// </summary>
        /// <param name="sender">Event Sender</param>
        /// <param name="e">Routed event</param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            string uname = TxtUname.Text.Trim();
            string ucode = Coder.StrToMD5(PwdLogin.Password.Trim());

            string strSql = "SELECT * FROM view_user WHERE uname='" + uname + "' AND upass='" + ucode + "'";

            //Get database connection
            _connection = AccessUtil.GetConnection(Properties.Resources.string_connection_string);
            //Get query result
            DataTable table = AccessUtil.Query(strSql, _connection);

            if (table == null)
            {
                //Failed, and try again
                LblWronginput.Visibility = Visibility.Visible;
            }
            else if (table.Rows[0][1].ToString() == uname && table.Rows[0][2].ToString() == ucode)
            {
                //Success, construct user entity
                User user = new User
                {
                    UserID = table.Rows[0][0].ToString().Trim(),
                    UserName = table.Rows[0][1].ToString().Trim(),
                    UserPassWord = ucode,
                    UserRight = int.Parse(table.Rows[0][3].ToString().Trim())
                };

                //Open main window
                Main main = new Main(this);
                UserLoginEvent?.Invoke(user, _connection);
                main.Show();

                //Close this window
                Close();
            }
        }

    }
}
