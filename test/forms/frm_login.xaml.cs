using System;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Input;

namespace test
{
    /// <summary>
    /// frm_login.xaml 的交互逻辑
    /// </summary>
    public partial class frm_insert : Window
    {

        //数据库连接字符串
        string strCon = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=E:\Projects\CS\test\test\bin\Debug\dbo.mdb";

        public frm_insert()
        {
            InitializeComponent();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            base.OnMouseLeftButtonDown(e);

            Point position = e.GetPosition(frm_login_main_grid);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (position.X >= 0 && position.X < frm_login_main_grid.ActualWidth && position.Y >= 0 && position.Y < frm_login_main_grid.ActualHeight)
                {
                    this.DragMove();
                }
            }

        }

        private void button1_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void frm_login_btn_close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void txt_uname_GotFocus(object sender, RoutedEventArgs e)
        {
            txt_uname.Text = "";
        }

        private void txt_uname_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txt_uname.Text == "")
            {
                txt_uname.Text = "请输入账号";
            }
        }

        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            //先判定输入是否符合条件，符合条件则进行查询，最后清理提示信息和数据库连接
            if (txt_uname.Text == "")
            {
                //是否输入账号
                lbl_input.Visibility = Visibility.Visible;
            }
            else if (pass_upass.Password.ToString() == "")
            {
                //是否输入密码
                lbl_input_password.Visibility = Visibility.Visible;
            }
            else
            {
                OleDbConnection conn = new OleDbConnection(strCon);
                string cmd = "SELECT  uid, uname, upass, uright FROM view_user WHERE(uname = '" + txt_uname.Text.ToString().Trim() + "') AND(upass = '" + pass_upass.Password.ToString().Trim() + "')";
                OleDbCommand myCommand = new OleDbCommand(cmd, conn);
                conn.Open();
                OleDbDataReader reader = myCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                /*
                //可以开始查询判断操作
                dbms.SqlHandler handler = new dbms.SqlHandler(strCon);
                SqlConnection conn = handler.Link();
                string cmd = "select * from dbo.view_user where dbo.view_user.uname=" + txt_uname.Text.ToString().Trim() + " and dbo.view_user.upass=" + pass_upass.Password.ToString().Trim();
                DataTable dt = handler.Query(conn, cmd);
                */
                if (dt.Rows.Count == 0)
                {   
                    //账号或者密码输入错误，给出提示
                    lbl_input_wrong.Visibility = Visibility.Visible;
                }
                else
                {
                    //成功登陆，传递信息并跳转到主窗体
                    //frm_main frm_main = new frm_main();
                    //frm_main.Show();
                    this.Close();
                }
                conn.Close();
                lbl_input.Visibility = Visibility.Hidden;
                lbl_input_password.Visibility = Visibility.Hidden;
            }
        }
    }
}
