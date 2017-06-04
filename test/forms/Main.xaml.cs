using System;
using System.Data;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    ///     Main window for this program.
    /// </summary>
    public partial class Main
    {
        private OleDbConnection _connection;

        //For modification
        private DataTable _copy = new DataTable();

        //Current SQL query command
        private string _currentSql = "";

        private bool _isFilterShowed;
        private bool _isWidnwoMaximized;
        private Rect _originalLocation;

        //Original query result
        private DataTable _source = new DataTable();

        //Logined user
        private User _user = new User();

        public Main(Login windowLogin)
        {
            InitializeComponent();
            windowLogin.UserLoginEvent += Init;
        }

        private bool IsFilterEnabled { get; set; } = true;

        /// <summary>
        ///     Delegation method for initialize the database connection and user entity.
        /// </summary>
        /// <param name="loginedUser">Logined user</param>
        /// <param name="connection">Database connection</param>
        private void Init(User loginedUser, OleDbConnection connection)
        {
            _connection = connection;
            _user = loginedUser;
            TxbUser.Text = _user.UserName;
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
        ///     Click this button to maximize or resotre this window
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            if (_isWidnwoMaximized == false)
            {
                //This window had not been maximized yet
                _originalLocation = new Rect(Left, Top, Width, Height);
                Left = 0;
                Top = 0;
                var rc = SystemParameters.WorkArea;
                Width = rc.Width;
                Height = rc.Height;
                _isWidnwoMaximized = true;
            }
            else
            {
                Left = _originalLocation.Left;
                Top = _originalLocation.Top;
                Width = _originalLocation.Width;
                Height = _originalLocation.Height;
                _isWidnwoMaximized = false;
            }
        }

        /// <summary>
        ///     Click this button to start a quick and fuzzy query process.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            QuickQuery();
        }

        /// <summary>
        ///     Show all records in the database.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            //Show all records
            var strSql = "SELECT * FROM view_all";
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

            //Bind the query result to datagrid
            DgMain.ItemsSource = queryResult.DefaultView;
            _source = queryResult;
        }

        /// <summary>
        ///     Click this button to open Add window
        /// </summary>
        /// <param name="sender">Evnet sender</param>
        /// <param name="e">Routed event</param>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var add = new Add(_connection);
            add.ShowDialog();
        }

        /// <summary>
        ///     Print report for query result
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            //打印报表
            if (_source.Rows.Count == 0)
            {
                MessageBox.Show("无数据可供打印");
            }
            else
            {
                var report = new MagazineReportWindow(TableNameChnToEng(_copy), _user, TxtSearch.Text.Trim());
                report.ShowDialog();
            }
        }

        /// <summary>
        ///     Open the Delete window
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DgMain.Items.Count != 0 && DgMain.SelectedItem != null)
            {
                var delete = new Delete(_connection, CreateMagazine());
                delete.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select a record before delete it.");
            }
        }

        /// <summary>
        ///     Open user control panel
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnUserControl_Click(object sender, RoutedEventArgs e)
        {
            var account = new Account(_connection, _user);
            account.ShowDialog();
        }

        /// <summary>
        ///     Refresh DgMain base the stored query command
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnRefresh_Click(object sender, RoutedEventArgs e)
        {
            if (_currentSql == "")
            {
                MessageBox.Show("您还没有进行过查询，无法刷新");
            }
            else
            {
                //User had queried before
                DgMain.IsReadOnly = false;
                var queryResult = AccessUtil.Query(_currentSql, _connection);
                queryResult = TableNameEngToChn(queryResult);
                DgMain.ItemsSource = queryResult.DefaultView;
                DgMain.IsReadOnly = true;
            }
        }

        /// <summary>
        ///     Cteate a magazine entity from selected item in DgMain for further use
        /// </summary>
        /// <returns>Constructed magazine entity</returns>
        private Magazine CreateMagazine()
        {
            var magazine = new Magazine();

            //Construction
            var drv = (DataRowView) DgMain.SelectedItem;
            magazine.Id = int.Parse(drv.Row[0].ToString());
            magazine.Issn = drv.Row[1].ToString();
            magazine.ShortName = drv.Row[2].ToString();
            magazine.FullName = drv.Row[3].ToString();
            magazine.ChineseName = drv.Row[4].ToString();
            magazine.ClassName = drv.Row[5].ToString();
            magazine.MultyClassName = drv.Row[6].ToString();
            magazine.ClassId = drv.Row[7].ToString();
            magazine.IsTop = drv.Row[8].ToString();
            magazine.F2007 = drv.Row[9].ToString();
            magazine.F2008 = drv.Row[10].ToString();
            magazine.F2009 = drv.Row[11].ToString();
            magazine.FAvg = drv.Row[12].ToString();
            magazine.Q2007 = drv.Row[13].ToString();
            magazine.Q2008 = drv.Row[14].ToString();
            magazine.Q2009 = drv.Row[15].ToString();
            magazine.Note = drv.Row[16].ToString();

            return magazine;
        }

        /// <summary>
        ///     Update selected item
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
                var update = new Update(_connection, CreateMagazine());
                update.ShowDialog();
            }
        }

        /// <summary>
        ///     Open context menu for DgMain
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed evetn</param>
        private void DgMain_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (DgMain.SelectedItem == null)
                e.Handled = true;
        }

        /// <summary>
        ///     Open detail information window for a selected item in DgMain
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
                var detail = new Details(CreateMagazine());
                detail.ShowDialog();
            }
        }

        /// <summary>
        ///     Quick and fuzzy query for target key word. No return values.
        /// </summary>
        private void QuickQuery()
        {
            DgMain.IsReadOnly = false;
            var target = "'%" + TxtSearch.Text.Trim().ToUpper() + "%'";
            var strSql = "SELECT * FROM view_all WHERE ID like " + target + " or ISSN like " + target +
                         " or ShortName like " + target + " or FullName like " + target + " or ChineseName like " +
                         target;

            if (_connection.State == ConnectionState.Closed)
                _connection.Open();

            var queryResult = AccessUtil.Query(strSql, _connection);

            if (queryResult == null)
            {
                MessageBox.Show("在此关键字下没有搜索结果");
                return;
            }

            _currentSql = strSql;
            queryResult = TableNameEngToChn(queryResult);
            DgMain.ItemsSource = queryResult.DefaultView;
            DgMain.IsReadOnly = true;
            _source = queryResult;
            _copy = _source.Copy();
        }

        private void button4_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //Display filter
            if (_isFilterShowed)
            {
                DgMain.Visibility = Visibility.Visible;
                GridFilter.Visibility = Visibility.Hidden;
                _isFilterShowed = false;
            }
            else
            {
                DgMain.Visibility = Visibility.Hidden;
                GridFilter.Visibility = Visibility.Visible;
                _isFilterShowed = true;
            }
        }

        private void Rdo4Filter()
        {
            if (_copy.Rows.Count == 0) return;
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (_copy.Rows[i][7].ToString() == Rdo4.Content.ToString()) continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void RdoYesFilter()
        {
            if (_copy.Rows.Count == 0) return;
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (_copy.Rows[i][8].ToString() == "Y") continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void RdoNoFIlter()
        {
            if (_copy.Rows.Count == 0) return;
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (_copy.Rows[i][8].ToString() == "N") continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void FactorFilter()
        {
            if (_copy.Rows.Count == 0) return;
            int column;
            switch (((ContentControl) CboFactor.SelectedValue).Content.ToString())
            {
                case "2007年影响因子":
                    column = 9;
                    break;
                case "2008年影响因子":
                    column = 10;
                    break;
                case "2009年影响因子":
                    column = 11;
                    break;
                case "平均影响因子":
                    column = 12;
                    break;
                default:
                    column = 9;
                    break;
            }
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (double.Parse(_copy.Rows[i][column].ToString()) <= double.Parse(Txt2.Text.Trim()) &&
                    double.Parse(_copy.Rows[i][column].ToString()) >= double.Parse(Txt1.Text.Trim())) continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void QuoteFilter()
        {
            if (_copy.Rows.Count == 0) return;
            int column;
            switch (((ContentControl) CboQuote.SelectedValue).Content.ToString())
            {
                case "2007年引用数":
                    column = 14;
                    break;
                case "2008年引用数":
                    column = 15;
                    break;
                case "2009年引用数":
                    column = 16;
                    break;
                default:
                    column = 13;
                    break;
            }
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (double.Parse(_copy.Rows[i][column].ToString()) <= double.Parse(Txt4.Text.Trim()) &&
                    double.Parse(_copy.Rows[i][column].ToString()) >= double.Parse(Txt3.Text.Trim())) continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void FilterScanner()
        {
            //逐项扫描各选项，若选中则调用指定的事件处理程序
            _copy.Clear();
            _copy = _source.Copy();

            if (CboClassName.SelectedIndex != -1)
                ClassNameFilter();
            if (Rdo1.IsChecked == true)
                Rdo1Filter();
            if (Rdo2.IsChecked == true)
                Rdo2Filter();
            if (Rdo3.IsChecked == true)
                Rdo3Filter();
            if (Rdo4.IsChecked == true)
                Rdo4Filter();
            if (RdoYes.IsChecked == true)
                RdoYesFilter();
            if (RdoNo.IsChecked == true)
                RdoNoFIlter();
            if (CboFactor.SelectedIndex != -1)
                FactorFilter();
            if (CboQuote.SelectedIndex != -1)
                QuoteFilter();
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
            DgMain.ItemsSource = _source.DefaultView;
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
            if (_copy.Rows.Count == 0 || CboClassName.SelectedValue == null) return;
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (_copy.Rows[i][5].ToString() ==
                    ((ContentControl) CboClassName.SelectedValue).Content.ToString()) continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void Rdo1Filter()
        {
            if (_copy.Rows.Count == 0) return;
            for (var i = 0; i < _copy.Rows.Count; i++)
            {
                if (_copy.Rows[i][7].ToString() == Rdo1.Content.ToString()) continue;
                _copy.Rows.RemoveAt(i);
                i--;
            }
            DgMain.ItemsSource = _copy.DefaultView;
        }

        private void Rdo2Filter()
        {
            if (_copy.Rows.Count != 0)
            {
                for (var i = 0; i < _copy.Rows.Count; i++)
                    if (_copy.Rows[i][7].ToString() != Rdo2.Content.ToString())
                    {
                        _copy.Rows.RemoveAt(i);
                        i--;
                    }
                DgMain.ItemsSource = _copy.DefaultView;
            }
        }

        private void Rdo3Filter()
        {
            if (_copy.Rows.Count != 0)
            {
                for (var i = 0; i < _copy.Rows.Count; i++)
                    if (_copy.Rows[i][7].ToString() != Rdo3.Content.ToString())
                    {
                        _copy.Rows.RemoveAt(i);
                        i--;
                    }
                DgMain.ItemsSource = _copy.DefaultView;
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

        public static DataTable TableNameChnToEng(DataTable table)
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
        ///     Close this program while clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            _connection.Dispose();
            Environment.Exit(0);
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
        ///     Minimize this program when clicked.
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        /// <summary>
        ///     Key down handler
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                QuickQuery();
        }
    }
}