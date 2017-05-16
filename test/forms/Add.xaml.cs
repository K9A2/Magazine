using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using test.common;

namespace test.forms
{
    /// <summary>
    /// add.xaml 的交互逻辑
    /// </summary>
    public partial class add : Window
    {

        //非空

        private double ClassID = 1;

        private string IsTop = "";

        private double FAVG;

        private string strCon = "";

        private OleDbConnection _connection;

        public add(OleDbConnection connection)
        {
            InitializeComponent();

            //this.strCon = strCon;
            _connection = connection;

            CboClassName.SelectedIndex = 0;

            RdoYes.IsChecked = true;

            Rdo1.IsChecked = true;
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
            WindowState = WindowState.Minimized;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            //添加并退出
            Add();

            //退出
            Close();
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            //添加数据
            AddData();
            //添加并恢复默认值
            SetToDefault();
        }

        private void Add()
        {
            //添加数据
            AddData();
        }

        private void SetToDefault()
        {
            TxtFname.Text = "";
            TxtSname.Text = "";
            TxtIssn.Text = "";
            TxtCname.Text = "";
            CboClassName.SelectedIndex = 1;
            Rdo1.IsChecked = true;
            TxtMclass.Text = "";
            RdoYes.IsChecked = true;
            TxtNote.Text = "";
            TxtF2007.Text = "0";
            TxtF2008.Text = "0";
            TxtF2009.Text = "0";
            TxbAvg.Text = "0";
            TxtQ2007.Text = "0";
            TxtQ2008.Text = "0";
            TxtQ2009.Text = "0";
        }

        private void AddData()
        {
            if (Rdo1.IsChecked == true)
            {
                ClassID = 1;
            }
            else if (Rdo2.IsChecked == true)
            {
                ClassID = 2;
            }
            else if (Rdo3.IsChecked == true)
            {
                ClassID = 3;
            }
            else if (Rdo4.IsChecked == true)
            {
                ClassID = 4;
            }

            if (RdoYes.IsChecked == true)
            {
                IsTop = "Y";
            }
            else if (RdoNo.IsChecked == true)
            {
                IsTop = "N";
            }

            FAVG = (double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) + double.Parse(TxtF2009.Text)) / 3;

            string strSql = "INSERT INTO view_all(ISSN,ShortName,FullName,ChineseName,ClassName,MultiClassName,ClassID,IsTop,Factor2007,Factor2008,Factor2009,FactorAvg,Quote2007,Quote2008,Quote2009,[Note]) VALUES ('";
            strSql += TxtIssn.Text.Trim() + "','";
            strSql += TxtSname.Text.Trim() + "','";
            strSql += TxtFname.Text.Trim() + "','";
            strSql += TxtCname.Text.Trim() + "','";
            strSql += ((ContentControl)CboClassName.SelectedValue).Content + "','";
            strSql += TxtMclass.Text.Trim() + "','";
            strSql += ClassID + "','";
            strSql += IsTop + "','";
            strSql += TxtF2007.Text.Trim() + "','";
            strSql += TxtF2008.Text.Trim() + "','";
            strSql += TxtF2009.Text.Trim() + "','";
            strSql += FAVG + "','";
            strSql += TxtQ2007.Text.Trim() + "','";
            strSql += TxtQ2008.Text.Trim() + "','";
            strSql += TxtQ2009.Text.Trim() + "','";
            strSql += TxtNote.Text.Trim() + "')";

            //AccessUtil util = new AccessUtil(strCon);

            AccessUtil.ExecuteWithoutReturn(strSql, _connection);
        }

        private void txt_f2007_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text = Convert.ToString((double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) + double.Parse(TxtF2009.Text)) / 3);
        }

        private void txt_f2008_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text = Convert.ToString((double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) + double.Parse(TxtF2009.Text)) / 3);
        }

        private void txt_f2009_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text = Convert.ToString((double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) + double.Parse(TxtF2009.Text)) / 3);
        }
    }
}
