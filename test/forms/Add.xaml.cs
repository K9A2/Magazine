using System;
using System.Data.OleDb;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using test.common;

namespace test.forms
{
    /// <summary>
    ///     add.xaml 的交互逻辑
    /// </summary>
    public partial class Add
    {
        private readonly OleDbConnection _connection;

        private double _classId = 1;

        //Average factor
        private double _favg;

        private string _isTop = "";

        public Add(OleDbConnection connection)
        {
            InitializeComponent();

            _connection = connection;
            CboClassName.SelectedIndex = 0;
            RdoYes.IsChecked = true;
            Rdo1.IsChecked = true;
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        /// <summary>
        ///     Add data and exit
        /// </summary>
        /// <param name="sender">Event sender</param>
        /// <param name="e">Routed event</param>
        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            AddData();
            Close();
        }

        /// <summary>
        ///     Add data but not exit
        /// </summary>
        /// <param name="sender">>Event sender</param>
        /// <param name="e">Routed event</param>
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            AddData();
            SetToDefault();
        }

        /// <summary>
        ///     Set all dispalyed options to default value
        /// </summary>
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

        /// <summary>
        ///     Add new record to database
        /// </summary>
        private void AddData()
        {
            if (Rdo1.IsChecked == true)
                _classId = 1;
            else if (Rdo2.IsChecked == true)
                _classId = 2;
            else if (Rdo3.IsChecked == true)
                _classId = 3;
            else if (Rdo4.IsChecked == true)
                _classId = 4;

            if (RdoYes.IsChecked == true)
                _isTop = "Y";
            else if (RdoNo.IsChecked == true)
                _isTop = "N";

            _favg = (double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) +
                     double.Parse(TxtF2009.Text)) / 3;

            var strSql =
                "INSERT INTO view_all(ISSN,ShortName,FullName,ChineseName,ClassName,MultiClassName,ClassID,IsTop,Factor2007,Factor2008,Factor2009,FactorAvg,Quote2007,Quote2008,Quote2009,[Note]) VALUES ('";
            strSql += TxtIssn.Text.Trim() + "','";
            strSql += TxtSname.Text.Trim() + "','";
            strSql += TxtFname.Text.Trim() + "','";
            strSql += TxtCname.Text.Trim() + "','";
            strSql += ((ContentControl) CboClassName.SelectedValue).Content + "','";
            strSql += TxtMclass.Text.Trim() + "','";
            strSql += _classId + "','";
            strSql += _isTop + "','";
            strSql += TxtF2007.Text.Trim() + "','";
            strSql += TxtF2008.Text.Trim() + "','";
            strSql += TxtF2009.Text.Trim() + "','";
            strSql += _favg + "','";
            strSql += TxtQ2007.Text.Trim() + "','";
            strSql += TxtQ2008.Text.Trim() + "','";
            strSql += TxtQ2009.Text.Trim() + "','";
            strSql += TxtNote.Text.Trim() + "')";

            AccessUtil.ExecuteWithoutReturn(strSql, _connection);
        }

        /*
         * Below are methods to calculate and set the average of Factors
         */
        private void txt_f2007_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text =
                Convert.ToString(
                    (double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) +
                     double.Parse(TxtF2009.Text)) / 3, CultureInfo.CurrentCulture);
        }

        private void txt_f2008_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text =
                Convert.ToString(
                    (double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) +
                     double.Parse(TxtF2009.Text)) / 3, CultureInfo.CurrentCulture);
        }

        private void txt_f2009_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text =
                Convert.ToString(
                    (double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) +
                     double.Parse(TxtF2009.Text)) / 3, CultureInfo.CurrentCulture);
        }
    }
}