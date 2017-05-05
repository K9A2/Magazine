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
    /// main.xaml 的交互逻辑
    /// </summary>
    public partial class main : Window
    {

        //TODO: Fix all the errors.
        //TODO: Use delegation and event to exchange messages between windows

        private string currentSql = "";

        User user = new User();

        DataTable source = new DataTable();

        DataTable copy = new DataTable();

        private bool IsFilterShowed = false;

        private bool IsFilterEnabled = true;

        private OleDbCommand connection = null;

        private bool IsClassNameFilterEnabled = false;

        private bool IsClassIDFilterEnabled = false;

        private bool IsIsTopFilterEnabled = false;

        private bool IsFactorFilterEnabled = false;

        private bool IsQuoteFilterEnabled = false;

        public main(User user, OleDbConnection connection)
        {
            InitializeComponent();

            this.user = user;

            txb_user.Text = user.UserName;

            grid_filter.Visibility = Visibility.Hidden;

        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Top = 0;
            this.Left = 0;
            this.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Height;
        }

        private void btn_search_Click(object sender, RoutedEventArgs e)
        {
            //快速查询

            QuickQuery();

        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            //全显
            string strSql = "SELECT * FROM view_all";

            AccessUtil util = new AccessUtil(strCon);

            DataTable table = util.Query(strSql);

            dg_main.ItemsSource = table.DefaultView;

            source = table;

            AddQueryInfo("所有记录");
        }

        private void button2_Copy_Click(object sender, RoutedEventArgs e)
        {
            //添加资料
            add add = new add(strCon);
            add.ShowDialog();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            //打印报表
            if (source.Rows.Count == 0)
            {
                MessageBox.Show("无数据可供打印");
            }
            else
            {
                MagazineReportWindow report = new MagazineReportWindow(TableNameChnToEng(copy), user, txt_search.Text.Trim());
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

        private void button2_Copy3_Click(object sender, RoutedEventArgs e)
        {
            //判断dg_main中是否有资料，有资料也是否有选中行
            if (dg_main.SelectedItem == null)
            {
                MessageBox.Show("您还没有选中数据，无法删除");
            }
            else
            {
                //打开删除窗口
                Delete delete = new Delete(strCon, CreateMagazine());
                delete.ShowDialog();
            }
        }

        private void button2_Copy2_Click(object sender, RoutedEventArgs e)
        {
            //打开用户账户控制界面
            Account account = new Account(strCon, user);
            account.ShowDialog();
        }

        private void button4_Copy_Click(object sender, RoutedEventArgs e)
        {
            //刷新
            if (currentSql == "")
            {
                //用户还没有进行过查询
                MessageBox.Show("您还没有进行过查询，无法刷新");
            }
            else
            {
                //用户已经进行过查询
                dg_main.IsReadOnly = false;

                AccessUtil util = new AccessUtil(strCon);

                DataTable table = util.Query(currentSql);

                table = TableNameEngToChn(table);

                dg_main.ItemsSource = table.DefaultView;

                dg_main.IsReadOnly = true;
            }
        }

        private Magazine CreateMagazine()
        {
            //删除dg_main中选中的资料
            Magazine magazine = new Magazine();

            //填入选中的内容
            DataRowView drv = (DataRowView)dg_main.SelectedItem;
            magazine.ID = Int32.Parse(drv.Row[0].ToString());
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

        private void button2_Copy4_Click_1(object sender, RoutedEventArgs e)
        {
            //更新
            if (dg_main.SelectedItem == null)
            {
                MessageBox.Show("您还没有选中数据，无法更新");
            }
            else
            {
                //打开删除窗口
                Update update = new Update(strCon, CreateMagazine());
                update.ShowDialog();
            }
        }

        private void dg_main_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (dg_main.SelectedItem == null)
            {
                e.Handled = true;
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (dg_main.SelectedItem == null)
            {
                //没有选择，则不应该弹出详细信息窗口
                e.Handled = true;
            }
            else
            {
                //选择了就应该弹出对应窗口
                Details detail = new Details(CreateMagazine());
                detail.ShowDialog();
            }
        }

        private void button2_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //报表
            if (source.Rows.Count == 0)
            {
                MessageBox.Show("无数据可供打印");
            }
            else
            {
                MagazineReportWindow report = new MagazineReportWindow(TableNameChnToEng(copy), user, txt_search.Text.Trim());
                report.ShowDialog();
            }
        }

        public void QuickQuery()
        {
            dg_main.IsReadOnly = false;

            string target = "'%" + txt_search.Text.ToString().Trim().ToUpper() + "%'";

            string strSql = "SELECT * FROM view_all WHERE ID like " + target + " or ISSN like " + target + " or ShortName like " + target + " or FullName like " + target + " or ChineseName like " + target;

            //string strSql = "SELECT * FROM dbo.view_all";

            AccessUtil util = new AccessUtil(strCon);

            DataTable table = util.Query(strSql);

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

            table = TableNameEngToChn(table);

            dg_main.ItemsSource = table.DefaultView;

            dg_main.IsReadOnly = true;

            source = table;

            copy = source.Copy();

            //copy = table;

            //往数据库中添加这次查询的详细资料

            AddQueryInfo(txt_search.Text.ToString().Trim());

        }

        private void AddQueryInfo(string title)
        {
            string time = System.DateTime.Now.ToString();

            string strSql = "DELETE * FROM tb_query";

            AccessUtil util = new AccessUtil(strCon);

            util.ExecuteWithoutReturn(strSql);

            if (title != "所有记录")
            {
                strSql = "INSERT INTO tb_query VALUES('关键字" + title + "的搜索结果','" + user.UserID + "','" + user.UserName + "','" + time + "')";
            }
            else
            {
                strSql = "INSERT INTO tb_query VALUES('" + title + "','" + user.UserID + "','" + user.UserName + "','" + time + "')";
            }

            util.ExecuteWithoutReturn(strSql);
        }

        private void button4_Copy1_Click(object sender, RoutedEventArgs e)
        {
            //过滤器，得到数据后筛选，而非在SQL语句中添加条件

            if (IsFilterShowed == true)
            {
                //过滤器已打开，隐藏过滤器
                dg_main.Visibility = Visibility.Visible;

                grid_filter.Visibility = Visibility.Hidden;

                IsFilterShowed = false;

            }
            else
            {
                //过滤器已关闭，显示过滤器
                dg_main.Visibility = Visibility.Hidden;

                grid_filter.Visibility = Visibility.Visible;

                IsFilterShowed = true;
            }

        }

        private void Rdo4Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != rdo_4.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
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
                dg_main.ItemsSource = copy.DefaultView;
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
                dg_main.ItemsSource = copy.DefaultView;
            }
        }

        private void FactorFilter()
        {
            if (copy.Rows.Count != 0)
            {
                int column = 0;
                switch (((System.Windows.Controls.ContentControl)cbo_Factor.SelectedValue).Content.ToString())
                {
                    case "2007年影响因子": column = 9; break;
                    case "2008年影响因子": column = 10; break;
                    case "2009年影响因子": column = 11; break;
                    case "平均影响因子": column = 12; break;
                    default: column = 9; break;
                }
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (!(double.Parse(copy.Rows[i][column].ToString()) <= double.Parse(txt_2.Text.ToString().Trim()) && double.Parse(copy.Rows[i][column].ToString()) >= double.Parse(txt_1.Text.ToString().Trim())))
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
            }
        }

        private void QuoteFilter()
        {
            if (copy.Rows.Count != 0)
            {
                int column = 0;
                switch (((System.Windows.Controls.ContentControl)cbo_Quote.SelectedValue).Content.ToString())
                {
                    case "2007年引用数": column = 14; break;
                    case "2008年引用数": column = 15; break;
                    case "2009年引用数": column = 16; break;
                    default: column = 13; break;
                }
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (!(double.Parse(copy.Rows[i][column].ToString()) <= double.Parse(txt_4.Text.ToString().Trim()) && double.Parse(copy.Rows[i][column].ToString()) >= double.Parse(txt_3.Text.ToString().Trim())))
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
            }
        }

        private void FilterScanner()
        {
            //逐项扫描各选项，若选中则调用指定的时间处理程序
            copy.Clear();
            copy = source.Copy();

            if (cbo_ClassName.SelectedIndex != -1)
            {
                ClassNameFilter();
            }
            if (rdo_1.IsChecked == true)
            {
                Rdo1Filter();
            }
            if (rdo_2.IsChecked == true)
            {
                Rdo2Filter();
            }
            if (rdo_3.IsChecked == true)
            {
                Rdo3Filter();
            }
            if (rdo_4.IsChecked == true)
            {
                Rdo4Filter();
            }
            if (rdo_yes.IsChecked == true)
            {
                RdoYesFilter();
            }
            if (rdo_no.IsChecked == true)
            {
                RdoNoFIlter();
            }
            if (cbo_Factor.SelectedIndex != -1)
            {
                FactorFilter();
            }
            if (cbo_Quote.SelectedIndex != -1)
            {
                QuoteFilter();
            }
        }

        private void ClearFilter()
        {
            //清除过滤器，并恢复至默认视图
            //ClassName
            cbo_ClassName.SelectedIndex = -1;

            //ClassID
            rdo_1.IsChecked = false;
            rdo_2.IsChecked = false;
            rdo_3.IsChecked = false;
            rdo_4.IsChecked = false;

            //IsTop
            rdo_yes.IsChecked = false;
            rdo_no.IsChecked = false;

            //Factor
            cbo_Factor.SelectedIndex = -1;
            txt_1.Text = "";
            txt_2.Text = "";

            //Quote
            cbo_Quote.SelectedIndex = -1;
            txt_3.Text = "";
            txt_4.Text = "";

            //还原数据视图
            dg_main.ItemsSource = source.DefaultView;
        }

        private void TurnOffFilter()
        {
            //关闭各过滤器
            cbo_ClassName.IsEnabled = false;

            //ClassID
            rdo_1.IsEnabled = false;
            rdo_2.IsEnabled = false;
            rdo_3.IsEnabled = false;
            rdo_4.IsEnabled = false;

            //IsTop
            rdo_yes.IsEnabled = false;
            rdo_no.IsEnabled = false;

            //Factor
            cbo_Factor.IsEnabled = false;
            txt_1.IsEnabled = false;
            txt_2.IsEnabled = false;
            btn_Factor.IsEnabled = false;

            //Quote
            cbo_Quote.IsEnabled = false;
            txt_3.IsEnabled = false;
            txt_4.IsEnabled = false;
            btn_Quote.IsEnabled = false;
        }

        private void TurnOnFilter()
        {
            //打开各过滤器
            //ClassName
            cbo_ClassName.IsEnabled = true;

            //ClassID
            rdo_1.IsEnabled = true;
            rdo_2.IsEnabled = true;
            rdo_3.IsEnabled = true;
            rdo_4.IsEnabled = true;

            //IsTop
            rdo_yes.IsEnabled = true;
            rdo_no.IsEnabled = true;

            //Factor
            cbo_Factor.IsEnabled = true;
            txt_1.IsEnabled = true;
            txt_2.IsEnabled = true;
            btn_Factor.IsEnabled = true;

            //Quote
            cbo_Quote.IsEnabled = true;
            txt_3.IsEnabled = true;
            txt_4.IsEnabled = true;
            btn_Quote.IsEnabled = true;
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
            if (copy.Rows.Count != 0 && cbo_ClassName.SelectedValue != null)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][5].ToString() != ((System.Windows.Controls.ContentControl)cbo_ClassName.SelectedValue).Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
            }
        }

        private void Rdo1Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != rdo_1.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
            }
        }

        private void Rdo2Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != rdo_2.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
            }
        }

        private void Rdo3Filter()
        {
            if (copy.Rows.Count != 0)
            {
                for (int i = 0; i < copy.Rows.Count; i++)
                {
                    if (copy.Rows[i][7].ToString() != rdo_3.Content.ToString())
                    {
                        copy.Rows.RemoveAt(i);
                        i--;
                    }
                }
                dg_main.ItemsSource = copy.DefaultView;
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

    }
}
