using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// login.xaml 的交互逻辑
    /// </summary>
    public partial class insert : Window
    {

        //string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\magazine\test\bin\Debug\dbo.mdb";
        //string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=D:\magazine\dbo.mdb";
        string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=dbo.mdb";
        public insert()
        {
            InitializeComponent();

            lbl_wronginput.Visibility = Visibility.Hidden;
        }

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

    }
}
