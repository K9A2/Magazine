using System.Data;
using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : Window
    {

        private string strCon = "";

        User user = new User();

        private string currentSql = "";

        DataTable source = new DataTable();

        public Search(string strCon, User user)
        {
            InitializeComponent();

            this.strCon = strCon;

            this.user = user;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            main main = new main(strCon, user);

            main.dg_main.IsReadOnly = false;

            string target = "'%" + txt_search.Text.ToString().Trim().ToUpper() + "%'";

            string strSql = "SELECT * FROM view_all WHERE ID like " + target + " or ISSN like " + target + " or ShortName like " + target + " or FullName like " + target + " or ChineseName like " + target;

            //string strSql = "SELECT * FROM dbo.view_all";

            AccessUtil util = new AccessUtil(strCon);

            DataTable table = util.Query(strSql);

            currentSql = strSql;

            main.dg_main.ItemsSource = table.DefaultView;

            main.dg_main.IsReadOnly = true;

            source = table;

            //往数据库中添加这次查询的详细资料

            string time = System.DateTime.Now.ToString();

            strSql = "DELETE * FROM tb_query";

            util.ExecuteWithoutReturn(strSql);

            strSql = "INSERT INTO tb_query VALUES('关键字" + txt_search.Text.ToString().Trim() + "的搜索结果','" + user.UserID + "','" + user.UserName + "','" + time + "')";
            util.ExecuteWithoutReturn(strSql);
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

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
