using System;
using System.Data.OleDb;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    ///     Update.xaml 的交互逻辑
    /// </summary>
    public partial class Update
    {
        private readonly OleDbConnection _connection;
        private readonly Magazine _magazine;

        private double _classId = 1;
        private double _favg;
        private string _isTop = "";

        public Update(OleDbConnection connection, Magazine magazine)
        {
            InitializeComponent();

            _connection = connection;
            _magazine = magazine;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateInfo();
            Close();
        }

        /// <summary>
        ///     Update this record
        /// </summary>
        private void UpdateInfo()
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

            var strSql = "UPDATE view_all SET ISSN='" + TxtIssn.Text.Trim() + "',ShortName='" + TxtSname.Text.Trim() +
                         "',FullName='" + TxtFname.Text.Trim() + "',ChineseName='" + TxtCname.Text.Trim() +
                         "',ClassName='" + ((ContentControl) CboClassName.SelectedValue).Content +
                         "',MultiClassName='" + TxtMclass.Text.Trim() + "',ClassID='" + _classId + "',IsTop='" +
                         _isTop + "',Factor2007='" + TxtF2007.Text.Trim() + "',Factor2008='" + TxtF2008.Text.Trim() +
                         "',Factor2009='" + TxtF2009.Text.Trim() + "',FactorAvg='" + _favg + "',Quote2007='" +
                         TxtQ2007.Text.Trim() + "',Quote2008='" + TxtQ2008.Text.Trim() + "',Quote2009='" +
                         TxtQ2009.Text.Trim() + "',[Note]='" + TxtNote.Text.Trim() + "' WHERE ID=" +
                         Convert.ToInt32(_magazine.Id) + "";
            AccessUtil.ExecuteWithoutReturn(strSql, _connection);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //Load magazine entity
            TxtFname.Text = _magazine.FullName;
            TxtSname.Text = _magazine.ShortName;
            TxtIssn.Text = _magazine.Issn;
            TxtCname.Text = _magazine.ChineseName;

            switch (_magazine.ClassName)
            {
                case "地学":
                    CboClassName.SelectedIndex = 0;
                    break;
                case "地学天文":
                    CboClassName.SelectedIndex = 1;
                    break;
                case "工程技术":
                    CboClassName.SelectedIndex = 2;
                    break;
                case "管理科学":
                    CboClassName.SelectedIndex = 3;
                    break;
                case "化学":
                    CboClassName.SelectedIndex = 4;
                    break;
                case "环境科学":
                    CboClassName.SelectedIndex = 5;
                    break;
                case "农林科学":
                    CboClassName.SelectedIndex = 6;
                    break;
                case "社会科学":
                    CboClassName.SelectedIndex = 7;
                    break;
                case "生物":
                    CboClassName.SelectedIndex = 8;
                    break;
                case "数学":
                    CboClassName.SelectedIndex = 9;
                    break;
                case "物理":
                    CboClassName.SelectedIndex = 10;
                    break;
                case "医学":
                    CboClassName.SelectedIndex = 11;
                    break;
                case "综合性期刊":
                    CboClassName.SelectedIndex = 12;
                    break;
            }
            TxtMclass.Text = _magazine.MultyClassName;
            switch (_magazine.ClassId)
            {
                case "1":
                    Rdo1.IsChecked = true;
                    break;
                case "2":
                    Rdo2.IsChecked = true;
                    break;
                case "3":
                    Rdo3.IsChecked = true;
                    break;
                case "4":
                    Rdo4.IsChecked = true;
                    break;
            }
            if (_magazine.IsTop == "Y")
                RdoYes.IsChecked = true;
            else
                RdoNo.IsChecked = true;
            TxtF2007.Text = _magazine.F2007;
            TxtF2008.Text = _magazine.F2008;
            TxtF2009.Text = _magazine.F2009;
            if (_magazine.FAvg.Length <= 13)
                TxbAvg.Text = _magazine.FAvg;
            else
                TxbAvg.Text = _magazine.FAvg.Substring(0, 13);
            TxtQ2007.Text = _magazine.Q2007;
            TxtQ2008.Text = _magazine.Q2008;
            TxtQ2009.Text = _magazine.Q2009;
            TxtNote.Text = _magazine.Note;
        }

        private void txt_f2007_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text =
                Convert.ToString(
                    (double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) +
                     double.Parse(TxtF2009.Text)) / 3, CultureInfo.CurrentCulture);
        }
    }
}