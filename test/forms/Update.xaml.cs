using System;
using System.Data.OleDb;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Update.xaml 的交互逻辑
    /// </summary>
    public partial class Update : Window
    {

        private double ClassID = 1;

        private OleDbConnection _connection;

        private string IsTop = "";

        private double FAVG;

        private string strCon = "";

        Magazine magazine = new Magazine();

        public Update(OleDbConnection connection, Magazine magazine)
        {
            InitializeComponent();

            //this.strCon = strCon;

            _connection = connection;

            this.magazine = magazine;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            Close();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            //确认修改

            UpdateInfo();

            Close();

        }

        private void UpdateInfo()
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

            string strSql = "UPDATE view_all SET ISSN='" + TxtIssn.Text.Trim() + "',ShortName='" + TxtSname.Text.Trim() + "',FullName='" + TxtFname.Text.Trim() + "',ChineseName='" + TxtCname.Text.Trim() + "',ClassName='" + ((ContentControl)CboClassName.SelectedValue).Content + "',MultiClassName='" + TxtMclass.Text.Trim() + "',ClassID='" + ClassID + "',IsTop='" + IsTop + "',Factor2007='" + TxtF2007.Text.Trim() + "',Factor2008='" + TxtF2008.Text.Trim() + "',Factor2009='" + TxtF2009.Text.Trim() + "',FactorAvg='" + FAVG + "',Quote2007='" + TxtQ2007.Text.Trim() + "',Quote2008='" + TxtQ2008.Text.Trim() + "',Quote2009='" + TxtQ2009.Text.Trim() + "',[Note]='" + TxtNote.Text.Trim() + "' WHERE ID=" + Convert.ToInt32(magazine.ID) + "";

            AccessUtil.ExecuteWithoutReturn(strSql, _connection);

            //AccessUtil util = new AccessUtil(strCon);

            //util.ExecuteWithoutReturn(strSql);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //在加载窗体的时候加载传入的magazine实体
            TxtFname.Text = magazine.FullName;
            TxtSname.Text = magazine.ShortName;
            TxtIssn.Text = magazine.ISSN;
            TxtCname.Text = magazine.ChineseName;

            switch (magazine.ClassName)
            {
                case "地学": CboClassName.SelectedIndex = 0; break;
                case "地学天文": CboClassName.SelectedIndex = 1; break;
                case "工程技术": CboClassName.SelectedIndex = 2; break;
                case "管理科学": CboClassName.SelectedIndex = 3; break;
                case "化学": CboClassName.SelectedIndex = 4; break;
                case "环境科学": CboClassName.SelectedIndex = 5; break;
                case "农林科学": CboClassName.SelectedIndex = 6; break;
                case "社会科学": CboClassName.SelectedIndex = 7; break;
                case "生物": CboClassName.SelectedIndex = 8; break;
                case "数学": CboClassName.SelectedIndex = 9; break;
                case "物理": CboClassName.SelectedIndex = 10; break;
                case "医学": CboClassName.SelectedIndex = 11; break;
                case "综合性期刊": CboClassName.SelectedIndex = 12; break;
                default: break;
            }
            TxtMclass.Text = magazine.MultyClassName;
            switch (magazine.ClassID)
            {
                case "1": Rdo1.IsChecked = true; break;
                case "2": Rdo2.IsChecked = true; break;
                case "3": Rdo3.IsChecked = true; break;
                case "4": Rdo4.IsChecked = true; break;
                default: break;
            }
            if (magazine.isTop == "Y")
            {
                RdoYes.IsChecked = true;
            }
            else
            {
                RdoNo.IsChecked = true;
            }
            TxtF2007.Text = magazine.f2007;
            TxtF2008.Text = magazine.f2008;
            TxtF2009.Text = magazine.f2009;
            if (magazine.fAVG.Length <= 13)
            {
                TxbAvg.Text = magazine.fAVG;
            }
            else
            {
                TxbAvg.Text = magazine.fAVG.Substring(0, 13);
            }
            TxtQ2007.Text = magazine.q2007;
            TxtQ2008.Text = magazine.q2008;
            TxtQ2009.Text = magazine.q2009;
            TxtNote.Text = magazine.Note;
        }

        private void txt_f2007_LostFocus(object sender, RoutedEventArgs e)
        {
            TxbAvg.Text = Convert.ToString((double.Parse(TxtF2007.Text.Trim()) + double.Parse(TxtF2008.Text.Trim()) + double.Parse(TxtF2009.Text)) / 3);
        }
    }
}
