using System;
using System.Windows;
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

        private string IsTop = "";

        private double FAVG = 0;

        private string strCon = "";

        Magazine magazine = new Magazine();

        public Update(string strCon, Magazine magazine)
        {
            InitializeComponent();

            this.strCon = strCon;

            this.magazine = magazine;
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            //确认修改

            UpdateInfo();

            this.Close();

        }

        private void UpdateInfo()
        {
            if (rdo_1.IsChecked == true)
            {
                ClassID = 1;
            }
            else if (rdo_2.IsChecked == true)
            {
                ClassID = 2;
            }
            else if (rdo_3.IsChecked == true)
            {
                ClassID = 3;
            }
            else if (rdo_4.IsChecked == true)
            {
                ClassID = 4;
            }

            if (rdo_yes.IsChecked == true)
            {
                IsTop = "Y";
            }
            else if (rdo_no.IsChecked == true)
            {
                IsTop = "N";
            }

            FAVG = (double.Parse(txt_f2007.Text.ToString().Trim()) + double.Parse(txt_f2008.Text.ToString().Trim()) + double.Parse(txt_f2009.Text.ToString())) / 3;

            string strSql = "UPDATE view_all SET ISSN='" + txt_issn.Text.ToString().Trim() + "',ShortName='" + txt_sname.Text.ToString().Trim() + "',FullName='" + txt_fname.Text.Trim() + "',ChineseName='" + txt_cname.Text.ToString().Trim() + "',ClassName='" + ((System.Windows.Controls.ContentControl)cbo_ClassName.SelectedValue).Content.ToString() + "',MultiClassName='" + txt_mclass.Text.ToString().Trim() + "',ClassID='" + ClassID + "',IsTop='" + IsTop + "',Factor2007='" + txt_f2007.Text.ToString().Trim() + "',Factor2008='" + txt_f2008.Text.ToString().Trim() + "',Factor2009='" + txt_f2009.Text.ToString().Trim() + "',FactorAvg='" + FAVG.ToString() + "',Quote2007='" + txt_q2007.Text.ToString().Trim() + "',Quote2008='" + txt_q2008.Text.ToString().Trim() + "',Quote2009='" + txt_q2009.Text.ToString().Trim() + "',[Note]='" + txt_note.Text.ToString().Trim() + "' WHERE ID=" + Convert.ToInt32(magazine.ID) + "";

            AccessHandler handler = new AccessHandler(strCon);

            handler.ExecuteWithoutReturn(strSql);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //在加载窗体的时候加载传入的magazine实体
            txt_fname.Text = magazine.FullName;
            txt_sname.Text = magazine.ShortName;
            txt_issn.Text = magazine.ISSN;
            txt_cname.Text = magazine.ChineseName;

            switch (magazine.ClassName)
            {
                case "地学": cbo_ClassName.SelectedIndex = 0; break;
                case "地学天文": cbo_ClassName.SelectedIndex = 1; break;
                case "工程技术": cbo_ClassName.SelectedIndex = 2; break;
                case "管理科学": cbo_ClassName.SelectedIndex = 3; break;
                case "化学": cbo_ClassName.SelectedIndex = 4; break;
                case "环境科学": cbo_ClassName.SelectedIndex = 5; break;
                case "农林科学": cbo_ClassName.SelectedIndex = 6; break;
                case "社会科学": cbo_ClassName.SelectedIndex = 7; break;
                case "生物": cbo_ClassName.SelectedIndex = 8; break;
                case "数学": cbo_ClassName.SelectedIndex = 9; break;
                case "物理": cbo_ClassName.SelectedIndex = 10; break;
                case "医学": cbo_ClassName.SelectedIndex = 11; break;
                case "综合性期刊": cbo_ClassName.SelectedIndex = 12; break;
                default: break;
            }
            txt_mclass.Text = magazine.MultyClassName;
            switch (magazine.ClassID)
            {
                case "1": rdo_1.IsChecked = true; break;
                case "2": rdo_2.IsChecked = true; break;
                case "3": rdo_3.IsChecked = true; break;
                case "4": rdo_4.IsChecked = true; break;
                default: break;
            }
            if (magazine.isTop == "Y")
            {
                rdo_yes.IsChecked = true;
            }
            else
            {
                rdo_no.IsChecked = true;
            }
            txt_f2007.Text = magazine.f2007;
            txt_f2008.Text = magazine.f2008;
            txt_f2009.Text = magazine.f2009;
            if (magazine.fAVG.Length <= 13)
            {
                txb_avg.Text = magazine.fAVG;
            }
            else
            {
                txb_avg.Text = magazine.fAVG.Substring(0, 13);
            }
            txt_q2007.Text = magazine.q2007;
            txt_q2008.Text = magazine.q2008;
            txt_q2009.Text = magazine.q2009;
            txt_note.Text = magazine.Note;
        }

        private void txt_f2007_LostFocus(object sender, RoutedEventArgs e)
        {
            txb_avg.Text = Convert.ToString((double.Parse(txt_f2007.Text.ToString().Trim()) + double.Parse(txt_f2008.Text.ToString().Trim()) + double.Parse(txt_f2009.Text.ToString())) / 3);
        }
    }
}
