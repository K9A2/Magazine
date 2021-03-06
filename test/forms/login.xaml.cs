﻿using System;
using System.Data.OleDb;
using System.IO;
using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    ///     Login window.
    /// </summary>
    public partial class Login
    {
        //User login event and delegation
        public delegate void UserLoginHandler(User user, OleDbConnection connection);

        //Database connection.
        private OleDbConnection _connection;

        public Login()
        {
            InitializeComponent();

            //Check if the Access database file exiets
            if (!File.Exists(@"./dbo.mdb"))
            {
                MessageBox.Show(
                    "The Access database file does not exists" + Properties.Resources.string_program_terminated,
                    "Fatal Error");
                Environment.Exit(-1);
            }
        }

        public event UserLoginHandler UserLoginEvent;

        /// <summary>
        ///     Execute while login windown loaded. Designed for set lbl_woronginput as an invisible control.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            LblWronginput.Visibility = Visibility.Hidden;
        }

        /// <summary>
        ///     Make this window dragable.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Mouse button event</param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        /// <summary>
        ///     Mininize this window when clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     Close this window and terminate this program when clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        /// <summary>
        ///     Start the authentication process when clicked.
        /// </summary>
        /// <param name="sender">Event Sender</param>
        /// <param name="e">Routed event</param>
        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            UserLogin();
        }

        /// <summary>
        ///     Key down event handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void PwdLogin_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UserLogin();
        }

        /// <summary>
        ///     User login procedure
        /// </summary>
        private void UserLogin()
        {
            var uname = TxtUname.Text.Trim();
            var ucode = Coder.StrToMd5(PwdLogin.Password.Trim());

            var strSql = "SELECT * FROM view_user WHERE uname='" + uname + "' AND upass='" + ucode + "'";

            //Get database connection
            _connection = AccessUtil.GetConnection(Properties.Resources.string_connection_string);
            //Get query result
            var table = AccessUtil.Query(strSql, _connection);

            if (table == null)
            {
                //Failed, then try again and clear the password box
                LblWronginput.Visibility = Visibility.Visible;
                PwdLogin.Clear();
            }
            else if (table.Rows[0][1].ToString() == uname && table.Rows[0][2].ToString() == ucode)
            {
                //Success, construct user entity
                var user = new User
                {
                    UserId = table.Rows[0][0].ToString().Trim(),
                    UserName = table.Rows[0][1].ToString().Trim(),
                    UserPassWord = ucode,
                    UserRight = int.Parse(table.Rows[0][3].ToString().Trim())
                };

                //Open main window
                var main = new Main(this);
                _connection.Close();
                UserLoginEvent?.Invoke(user, _connection);
                main.Show();

                //Close this window
                Close();
            }
        }
    }
}