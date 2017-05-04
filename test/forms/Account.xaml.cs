using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Account.xaml 的交互逻辑
    /// </summary>
    public partial class Account : Window
    {

        User user = new User();

        private string UserID = "";

        private string UserName = "";

        private string UserPassWord = "";

        private int UserRight = 0;

        private string strCon = "";


        public Account(string strCon,User user)
        {
            InitializeComponent();

            this.user = user;

            UserID = user.UserID;
            UserName = user.UserName;
            UserPassWord = user.UserPassWord;
            UserRight = user.UserRight;

            this.strCon = strCon;
       }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            if (txt_oldName.Text.ToString().Trim() == user.UserName)
            {
                string strSql = "UPDATE view_user SET uname='" + txt_newName.Text.ToString().Trim() + "' WHERE uid='" + user.UserID + "'";
                AccessHandler handler = new AccessHandler(strCon);

                handler.ExecuteWithoutReturn(strSql);

                this.user.UserName = txt_newName.Text.ToString().Trim();
            }
            else
            {
                MessageBox.Show("您输入了错误的账户名");
            }
            

            


        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void btn_confirm_Copy1_Click(object sender, RoutedEventArgs e)
        {
            txt_newName.Text = "";
            txt_oldName.Text = "";
        }

        private void btn_confirm_Copy3_Click(object sender, RoutedEventArgs e)
        {
            pwd_oldPass.Password = "";
            pwd_newPass.Password = "";
        }

        private void btn_confirm_Copy2_Click(object sender, RoutedEventArgs e)
        {
            if (Coder.StrToMD5(pwd_oldPass.Password.ToString().Trim()) == user.UserPassWord)
            {
                string strSql = "UPDATE view_user SET upass='" + Coder.StrToMD5(pwd_newPass.Password.ToString().Trim()) + "' WHERE uid='" + user.UserID + "'";

                AccessHandler handler = new AccessHandler(strCon);

                handler.ExecuteWithoutReturn(strSql);

                this.user.UserName = txt_newName.Text.ToString().Trim();
            }
            else
            {
                MessageBox.Show("您输入了错误的密码");
            }

        }
    }
}
