using System.Data;
using System.Windows;
using System.Windows.Input;
using test.entities;

namespace test.forms
{
    /// <summary>
    ///     Search.xaml 的交互逻辑
    /// </summary>
    public partial class Search : Window
    {
        private string _currentSql = "";

        private DataTable _source = new DataTable();

        private string _strCon = "";

        private User _user = new User();

        public Search(string strCon, User user)
        {
            InitializeComponent();

            _strCon = strCon;

            _user = user;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            /*
            Main main = new Main(strCon, user);

            main.DgMain.IsReadOnly = false;

            string target = "'%" + TxtSearch.Text.ToString().Trim().ToUpper() + "%'";

            string strSql = "SELECT * FROM view_all WHERE ID like " + target + " or ISSN like " + target + " or ShortName like " + target + " or FullName like " + target + " or ChineseName like " + target;

            //string strSql = "SELECT * FROM dbo.view_all";

            AccessUtil util = new AccessUtil(strCon);

            DataTable table = util.Query(strSql);

            currentSql = strSql;

            main.DgMain.ItemsSource = table.DefaultView;

            main.DgMain.IsReadOnly = true;

            source = table;

            //往数据库中添加这次查询的详细资料

            string time = System.DateTime.Now.ToString();

            strSql = "DELETE * FROM tb_query";

            util.ExecuteWithoutReturn(strSql);

            strSql = "INSERT INTO tb_query VALUES('关键字" + TxtSearch.Text.ToString().Trim() + "的搜索结果','" + user.UserID + "','" + user.UserName + "','" + time + "')";
            util.ExecuteWithoutReturn(strSql);
            */
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                DragMove();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}