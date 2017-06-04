﻿using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    ///     Account.xaml 的交互逻辑
    /// </summary>
    public partial class Account : Window
    {
        //private string strCon = "";

        private readonly OleDbConnection _connection;

        private readonly User _user;

        private string _userId = "";

        private string _userName = "";

        private string _userPassWord = "";

        private int _userRight;

        public Account(OleDbConnection connection, User user)
        {
            InitializeComponent();

            _user = user;

            _connection = connection;

            _userId = user.UserId;
            _userName = user.UserName;
            _userPassWord = user.UserPassWord;
            _userRight = user.UserRight;

            //this.strCon = strCon;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            if (TxtOldName.Text.Trim() == _user.UserName)
            {
                var strSql = "UPDATE view_user SET uname='" + TxtNewName.Text.Trim() + "' WHERE uid='" + _user.UserId +
                             "'";

                AccessUtil.ExecuteWithoutReturn(strSql, _connection);

                //AccessUtil util = new AccessUtil(strCon);

                //util.ExecuteWithoutReturn(strSql);

                _user.UserName = TxtNewName.Text.Trim();
            }
            else
            {
                MessageBox.Show("您输入了错误的账户名");
            }
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void btn_confirm_Copy1_Click(object sender, RoutedEventArgs e)
        {
            TxtNewName.Text = "";
            TxtOldName.Text = "";
        }

        private void btn_confirm_Copy3_Click(object sender, RoutedEventArgs e)
        {
            PwdOldPass.Password = "";
            PwdNewPass.Password = "";
        }

        private void btn_confirm_Copy2_Click(object sender, RoutedEventArgs e)
        {
            if (Coder.StrToMd5(PwdOldPass.Password.Trim()) == _user.UserPassWord)
            {
                var strSql = "UPDATE view_user SET upass='" + Coder.StrToMd5(PwdNewPass.Password.Trim()) +
                             "' WHERE uid='" + _user.UserId + "'";

                AccessUtil.ExecuteWithoutReturn(strSql, _connection);

                //AccessUtil util = new AccessUtil(strCon);

                //util.ExecuteWithoutReturn(strSql);

                _user.UserName = TxtNewName.Text.Trim();
            }
            else
            {
                MessageBox.Show("您输入了错误的密码");
            }
        }
    }
}