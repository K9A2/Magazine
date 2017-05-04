using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Login window.
    /// </summary>
    public partial class Login : Window
    {

        //TODO: Reconstruct it.

        //Database connection.
        private OleDbConnection connection;

        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Make this window dragable.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Mouse button event.</param>
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            string uname = txt_uname.Text.ToString().Trim();
            string ucode = Coder.StrToMD5(pwd_login.Password.ToString().Trim());
            //不能添加dbo，vs会自动从当前目录查找dbo.mdb
            string strSql = "SELECT * FROM view_user WHERE uname='" + uname + "' AND upass='" + ucode + "'";

            //Console.WriteLine(Coder.StrToMD5("user"));

            AccessHandler handler = new AccessHandler(strCon);

            DataTable table = handler.Query(strSql);

            if (table.Rows.Count == 0)
            {
                //登录失败
                lbl_wronginput.Visibility = Visibility.Visible;
            }
            else if (table.Rows[0][1].ToString() == uname && table.Rows[0][2].ToString() == ucode)
            {
                //登录成功

                //构建用户实体
                User user = new User();
                user.UserID = table.Rows[0][0].ToString().Trim();
                user.UserName = table.Rows[0][1].ToString().Trim();
                user.UserPassWord = ucode;
                user.UserRight = Int32.Parse(table.Rows[0][3].ToString().Trim());

                //打开主窗体并传入用户实体
                main main = new main(strCon, user, handler.GetConnection());
                main.Show();

                //登录窗体退出
                this.Close();
            }

        }

        /// <summary>
        /// Execute while login windown loaded. Designed for set lbl_woronginput as an invisible control.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Routed event</param>
        private void Login_Loaded(object sender, RoutedEventArgs e)
        {
            lbl_wronginput.Visibility = Visibility.Hidden;
        }
    }
}
