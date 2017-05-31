using System;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using test.common;
using test.entities;
using MessageBox = System.Windows.MessageBox;

namespace test.forms
{
    /// <summary>
    /// Main window for this program.
    /// </summary>
    public partial class Main : Window
    {

        //TODO: Fix all the errors.
        //TODO: Use delegation and event to exchange messages between windows

        private string currentSql = "";

        private User user = new User();

        private DataTable source = new DataTable();

        private DataTable copy = new DataTable();

        private OleDbConnection _connection;

        private bool _isWidnwoMaximized = false;

        private Rect normal;

        private bool IsFilterShowed;

        private bool IsFilterEnabled = true;

        private bool IsClassNameFilterEnabled = false;

        private bool IsClassIDFilterEnabled = false;

        private bool IsIsTopFilterEnabled = false;

        private bool IsFactorFilterEnabled = false;

        private bool IsQuoteFilterEnabled = false;

        public Main(Login window_login)
        {
            InitializeComponent();

            window_login.UserLoginEvent += Init;

            /*
            this.user = user;

            txb_user.Text = user.UserName;

            grid_filter.Visibility = Visibility.Hidden;

            this.connection = connection;
            */

        }

        /// <summary>
        /// Delegation method for initialize the database connection and user entity.
        /// </summary>
        /// <param name="loginedUser">Logined user</param>
        /// <param name="connection">Database connection</param>
        private void Init(User loginedUser, OleDbConnection connection)
        {
            _connection = connection;
            user = loginedUser;
            TxbUser.Text = user.UserName;
            GridFilter.Visibility = Visibility.Hidden;

            //Access control base on user right
            if (loginedUser.UserRight != 1)
            {
                //This user is not the administrator, disable some senstive features
                BtnAdd.IsEnabled = false;
                BtnDelete.IsEnabled = false;
            }
        }

        /// <summary>
        /// Click this button to maximize this window
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }
            */
            if (_isWidnwoMaximized == false)
            {
                //This window had not been maximized yet
                normal = new Rect(this.Left, this.Top, this.Width, this.Height); //保存下当前位置与大小
                this.Left = 0; //设置位置
                this.Top = 0;
                Rect rc = SystemParameters.WorkArea; //获取工作区大小
                this.Width = rc.Width;
                this.Height = rc.Height;
                _isWidnwoMaximized = true;
            }
            else
            {
                this.Left = normal.Left;
                this.Top = normal.Top;
                this.Width = normal.Width;
                this.Height = normal.Height;
                _isWidnwoMaximized = false;
            }
        }

        /// <summary>
        /// Click this button to start a quick and fuzzy query process.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            QuickQuery();
        }

        /// <summary>
        /// Show all records in the database.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            //Show all records
            string strSql = "SELECT * FROM view_all";

            //AccessUtil util = new AccessUtil(strCon);
            DataTable queryResult;

            if (_connection != null)
            {
                queryResult = AccessUtil.Query(strSql, _connection);
            }
            else
            {
                MessageBox.Show("Null database connection, please restart this program and try again.");
                return;
            }

            DgMain.ItemsSource = queryResult.DefaultView;

            source = queryResult;

            AddQueryInfo("所有记录");
        }

        /// <summary>
        /// Click this button to open Add window
        /// </summary>
        /// <param name="sender">Evnet sender</param>
        /// <param name="e">Routed event</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            add add = new add(_connection);
            add.ShowDialog();
        }

        /// <summary>
        /// Print report for query result
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //打印报表
            if (source.Rows.Count == 0)
            {
                MessageBox.Show("无数据可供打印");
            }
            else
            {
                MagazineReportWindow report = new MagazineReportWindow(TableNameChnToEng(copy), user, TxtSearch.Text.Trim());
                report.ShowDialog();
            }

            /*
            PrintDialog dialog = new PrintDialog();
            dialog.PrintTicket.PageOrientation = dg.PageOrientation.Portrait;
            Nullable<bool> result = dialog.ShowDialog();
            if (result == true)
            {
                dialog.PrintVisual(this.lbPrintList, "Print ListBox");
            }
            */
        }

        /// <summary>
        /// Open the Delete window
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DgMain.Items.Count != 0 && DgMain.SelectedItem != null)
            {
                Delete delete = new Delete(_connection, CreateMagazine());
                delete.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a record before delete it.");
            }
        }

        /// <summary>
        /// Open user control panel
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnUserControl_Click(object sender, RoutedEventArgs e)
        {
            Account account = new Account(_connection, user);
            account.ShowDialog();
        }

        /// <summary>
        /// Refresh DgMain base the stored query command
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        { 
            if (currentSql == "")
            {
                MessageBox.Show("您还没有进行过查询，无法刷新");
            }
            else
            {
                //用户已经进行过查询
                DgMain.IsReadOnly = false;

                DataTable queryResult = AccessUtil.Query(currentSql, _connection);

                //AccessUtil util = new AccessUtil(strCon);

                //DataTable table = util.Query(currentSql);

                queryResult = TableNameEngToChn(queryResult);

                DgMain.ItemsSource = queryResult.DefaultView;

                DgMain.IsReadOnly = true;
            }
        }

        /// <summary>
        /// Cteate a magazine entity from selected item in DgMain for further use
        /// </summary>
        /// <returns>Constructed magazine entity</returns>
        private Magazine CreateMagazine()
        {

            //Initialization
            Magazine magazine = new Magazine();

            //Construction
            DataRowView drv = (DataRowView)DgMain.SelectedItem;
            magazine.ID = int.Parse(drv.Row[0].ToString());
            magazine.ISSN = drv.Row[1].ToString();
            magazine.ShortName = drv.Row[2].ToString();
            magazine.FullName = drv.Row[3].ToString();
            magazine.ChineseName = drv.Row[4].ToString();
            magazine.ClassName = drv.Row[5].ToString();
            magazine.MultyClassName = drv.Row[6].ToString();
            magazine.ClassID = drv.Row[7].ToString();
            magazine.isTop = drv.Row[8].ToString();
            magazine.f2007 = drv.Row[9].ToString();
            magazine.f2008 = drv.Row[10].ToString();
            magazine.f2009 = drv.Row[11].ToString();
            magazine.fAVG = drv.Row[12].ToString();
            magazine.q2007 = drv.Row[13].ToString();
            magazine.q2008 = drv.Row[14].ToString();
            magazine.q2009 = drv.Row[15].ToString();
            magazine.Note = drv.Row[16].ToString();

            return magazine;
        }

        /// <summary>
        /// Update selected item
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (DgMain.SelectedItem == null)
            {
                MessageBox.Show("Select an item before update it.");
            }
            else
            {
                Update update = new Update(_connection, CreateMagazine());
                update.ShowDialog();
            }
        }

        /// <summary>
        /// Open context menu for DgMain
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed evetn</param>
        private void DgMain_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DgMain.SelectedItem == null)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Open detail information window for a selected item in DgMain
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (DgMain.SelectedItem == null)
            {
                e.Handled = true;
            }
            else
            {
                Details detail = new Details(CreateMagazine());
                detail.ShowDialog();
            }
        }

        /// <summary>
        /// Quick and fuzzy query for target key word. No return values.
        /// </summary>
        private void QuickQuery()
        {
            DgMain.IsReadOnly = false;

            string target = "'%" + TxtSearch.Text.Trim().ToUpper() + "%'";

            string strSql = "SELECT * FROM view_all WHERE ID like " + target + " or ISSN like " + target + " or ShortName like " + target + " or FullName like " + target + " or ChineseName like " + target;

            //string strSql = "SELECT * FROM dbo.view_all";

            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
            
            var queryResult = AccessUtil.Query(strSql, _connection);

            if (queryResult == null)
            {
                MessageBox.Show("在此关键字下没有搜索结果");
                return;
            }

            //AccessUtil util = new AccessUtil(strCon);

            //DataTable table = util.Query(strSql);

            /*
            table.Columns[2].ColumnName = "刊名简称";
            table.Columns[3].ColumnName = "刊名全称";
            table.Columns[4].ColumnName = "中文名称";
            table.Columns[5].ColumnName = "大类名称";
            table.Columns[6].ColumnName = "复分";
            table.Columns[7].ColumnName = "大类分区";
            table.Columns[8].ColumnName = "是否为Top期刊";
            table.Columns[9].ColumnName = "2007年影响因子";
            table.Columns[10].ColumnName = "2008年影响因子";
            table.Columns[11].ColumnName = "2009年影响因子";
            table.Columns[12].ColumnName = "平均影响因子";
            table.Columns[13].ColumnName = "2007年引用数";
            table.Columns[14].ColumnName = "2008年引用数";
            table.Columns[15].ColumnName = "2009年引用数";
            table.Columns[16].ColumnName = "备注";
            */


            currentSql = strSql;

            queryResult = TableNameEngToChn(queryResult);

            DgMain.ItemsSource = queryResult.DefaultView;

            DgMain.IsReadOnly = true;

            source = queryResult;

            copy = source.Copy();

            //copy = table;

            //往数据库中添加这次查询的详细资料

            AddQueryInfo(TxtSearch.Text.Trim());

        }

        /// <summary>
        /// Add query info to database
        /// </summary>
        /// <param name="title"></param>
        private void AddQueryInfo(string title)
        {
            string time = DateTime.Now.ToString(CultureInfo.InvariantCulture);

            //Clear the database for storing query info
            string strSql = "DELETE * FROM tb_query";
            AccessUtil.ExecuteWithoutReturn(strSql, _connection);

            //AccessUtil util = new AccessUtil(strCon);

            //util.ExecuteWithoutReturn(strSql);

            if (title != "所有记录")
            {
                strSql = "INSERT INTO tb_query VALUES('关键字" + title + "的搜索结果','" + user.UserID + "','" + user.UserName + "','" + time + "')";
            }
            else
            {
                strSql = "INSERT INTO tb_query VALUES('" + title + "','" + user.UserID + "','" + user.UserName + "','" + time + "')";
            }

            AccessUtil.ExecuteWithoutReturn(strSql, _connection);
        }

        private void button4_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //过滤器，得到数据后筛选，而非在SQL语句中添加条件

            if (IsFilterShowed)
            {
                //过滤器已打开，隐藏过滤器
                DgMain.Visibility = Visibility.Visible;

                GridFilter.Visibility = Visibility.Hidden;

                IsFilterShowed = false;

            }
            else
            {
                //过滤器已关闭，显示过滤器
                DgMain.Visibility = Visibility.Hidden;

                GridFilter.Visibility = Visibility.Visible;

                IsFilterShowed = true;
            }

        }

        private void Rdo4Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != Rdo4.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void RdoYesFilter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][8].ToString() != "Y")
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void RdoNoFIlter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][8].ToString() != "N")
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void FactorFilter()
        {
            if (copy.Rows.Count != 0)
            {
                int column;
                switch (((ContentControl)CboFactor.SelectedValue).Content.ToString())
                {
                    case "2007年影响因子": column = 9; break;
                    case "2008年影响因子": column = 10; break;
                    case "2009年影响因子": column = 11; break;
                    case "平均影响因子": column = 12; break;
                    default: column = 9; break;
                }
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (!(double.Parse(copy.Rows[i][column].ToString()) <= double.Parse(Txt2.Text.Trim()) && double.Parse(copy.Rows[i][column].ToString()) >= double.Parse(Txt1.Text.Trim())))
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void QuoteFilter()
        {
            if (copy.Rows.Count != 0)
            {
                int column;
                switch (((ContentControl)CboQuote.SelectedValue).Content.ToString())
                {
                    case "2007年引用数": column = 14; break;
                    case "2008年引用数": column = 15; break;
                    case "2009年引用数": column = 16; break;
                    default: column = 13; break;
                }
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (!(double.Parse(copy.Rows[i][column].ToString()) <= double.Parse(Txt4.Text.Trim()) && double.Parse(copy.Rows[i][column].ToString()) >= double.Parse(Txt3.Text.Trim())))
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void FilterScanner()
        {
            //逐项扫描各选项，若选中则调用指定的时间处理程序
            copy.Clear();
            copy = source.Copy();

            if (CboClassName.SelectedIndex != -1)
            {
                ClassNameFilter();
            }
            if (Rdo1.IsChecked == true)
            {
                Rdo1Filter();
            }
            if (Rdo2.IsChecked == true)
            {
                Rdo2Filter();
            }
            if (Rdo3.IsChecked == true)
            {
                Rdo3Filter();
            }
            if (Rdo4.IsChecked == true)
            {
                Rdo4Filter();
            }
            if (RdoYes.IsChecked == true)
            {
                RdoYesFilter();
            }
            if (RdoNo.IsChecked == true)
            {
                RdoNoFIlter();
            }
            if (CboFactor.SelectedIndex != -1)
            {
                FactorFilter();
            }
            if (CboQuote.SelectedIndex != -1)
            {
                QuoteFilter();
            }
        }

        private void ClearFilter()
        {
            //清除过滤器，并恢复至默认视图
            //ClassName
            CboClassName.SelectedIndex = -1;

            //ClassID
            Rdo1.IsChecked = false;
            Rdo2.IsChecked = false;
            Rdo3.IsChecked = false;
            Rdo4.IsChecked = false;

            //IsTop
            RdoYes.IsChecked = false;
            RdoNo.IsChecked = false;

            //Factor
            CboFactor.SelectedIndex = -1;
            Txt1.Text = "";
            Txt2.Text = "";

            //Quote
            CboQuote.SelectedIndex = -1;
            Txt3.Text = "";
            Txt4.Text = "";

            //还原数据视图
            DgMain.ItemsSource = source.DefaultView;
        }

        private void TurnOffFilter()
        {
            //关闭各过滤器
            CboClassName.IsEnabled = false;

            //ClassID
            Rdo1.IsEnabled = false;
            Rdo2.IsEnabled = false;
            Rdo3.IsEnabled = false;
            Rdo4.IsEnabled = false;

            //IsTop
            RdoYes.IsEnabled = false;
            RdoNo.IsEnabled = false;

            //Factor
            CboFactor.IsEnabled = false;
            Txt1.IsEnabled = false;
            Txt2.IsEnabled = false;
            BtnFactor.IsEnabled = false;

            //Quote
            CboQuote.IsEnabled = false;
            Txt3.IsEnabled = false;
            Txt4.IsEnabled = false;
            BtnQuote.IsEnabled = false;
        }

        private void TurnOnFilter()
        {
            //打开各过滤器
            //ClassName
            CboClassName.IsEnabled = true;

            //ClassID
            Rdo1.IsEnabled = true;
            Rdo2.IsEnabled = true;
            Rdo3.IsEnabled = true;
            Rdo4.IsEnabled = true;

            //IsTop
            RdoYes.IsEnabled = true;
            RdoNo.IsEnabled = true;

            //Factor
            CboFactor.IsEnabled = true;
            Txt1.IsEnabled = true;
            Txt2.IsEnabled = true;
            BtnFactor.IsEnabled = true;

            //Quote
            CboQuote.IsEnabled = true;
            Txt3.IsEnabled = true;
            Txt4.IsEnabled = true;
            BtnQuote.IsEnabled = true;
        }

        private void btn_OnOff_Click(object sender, RoutedEventArgs e)
        {
            //开关过滤器
            if (IsFilterEnabled == false)
            {
                //过滤器被关闭，打开之
                IsFilterEnabled = true;
                TurnOnFilter();
            }
            else
            {
                //过滤器打开，关闭之
                IsFilterEnabled = false;
                ClearFilter();
                TurnOffFilter();
            }
        }

        private void ClassNameFilter()
        {
            //大类名称过滤器     
            if (copy.Rows.Count != 0 && CboClassName.SelectedValue != null)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][5].ToString() != ((ContentControl)CboClassName.SelectedValue).Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void Rdo1Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != Rdo1.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void Rdo2Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != Rdo2.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void Rdo3Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != Rdo3.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                DgMain.ItemsSource = copy.DefaultView;
            }
        }

        private void cbo_ClassName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterScanner();
        }

        private void rdo_1_Checked(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void rdo_2_Checked(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void rdo_3_Checked(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void rdo_4_Checked(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void rdo_yes_Checked(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void rdo_no_Checked(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void btn_Factor_Click(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private void btn_Quote_Click(object sender, RoutedEventArgs e)
        {
            FilterScanner();
        }

        private DataTable TableNameEngToChn(DataTable table)
        {
            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "ISSN";
            table.Columns[2].ColumnName = "刊名简称";
            table.Columns[3].ColumnName = "刊名全称";
            table.Columns[4].ColumnName = "中文名称";
            table.Columns[5].ColumnName = "大类名称";
            table.Columns[6].ColumnName = "复分";
            table.Columns[7].ColumnName = "大类分区";
            table.Columns[8].ColumnName = "是否为Top期刊";
            table.Columns[9].ColumnName = "2007年影响因子";
            table.Columns[10].ColumnName = "2008年影响因子";
            table.Columns[11].ColumnName = "2009年影响因子";
            table.Columns[12].ColumnName = "平均影响因子";
            table.Columns[13].ColumnName = "2007年引用数";
            table.Columns[14].ColumnName = "2008年引用数";
            table.Columns[15].ColumnName = "2009年引用数";
            table.Columns[16].ColumnName = "备注";

            return table;
        }

        private DataTable TableNameChnToEng(DataTable table)
        {
            table.Columns[0].ColumnName = "ID";
            table.Columns[1].ColumnName = "ISSN";
            table.Columns[2].ColumnName = "ShortName";
            table.Columns[3].ColumnName = "FullName";
            table.Columns[4].ColumnName = "ChineseName";
            table.Columns[5].ColumnName = "ClassName";
            table.Columns[6].ColumnName = "MultiClassName";
            table.Columns[7].ColumnName = "ClassID";
            table.Columns[8].ColumnName = "IsTop";
            table.Columns[9].ColumnName = "Factor2007";
            table.Columns[10].ColumnName = "Factor2008";
            table.Columns[11].ColumnName = "Factor2009";
            table.Columns[12].ColumnName = "FactorAvg";
            table.Columns[13].ColumnName = "Quote2007";
            table.Columns[14].ColumnName = "Quote2008";
            table.Columns[15].ColumnName = "Quote2009";
            table.Columns[16].ColumnName = "Note";

            return table;
        }

        /// <summary>
        /// Close this program while clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            _connection.Dispose();
            Environment.Exit(0);
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
        /// Minimize this program when clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        /// Key down handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void TxtSearch_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QuickQuery();
            }
        }
    }
}
