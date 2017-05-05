using System;
using System.Windows;
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

        private double FAVG = 0;

        private string strCon = "";

        public add(string strCon)
        {
            InitializeComponent();

            this.strCon = strCon;

            cbo_ClassName.SelectedIndex = 0;

            rdo_yes.IsChecked = true;

            rdo_1.IsChecked = true;
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

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {

        }

        private void button3_Click_1(object sender, RoutedEventArgs e)
        {
            //添加并退出
            Add();

            //退出
            this.Close();
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
            txt_fname.Text = "";
            txt_sname.Text = "";
            txt_issn.Text = "";
            txt_cname.Text = "";
            cbo_ClassName.SelectedIndex = 1;
            rdo_1.IsChecked = true;
            txt_mclass.Text = "";
            rdo_yes.IsChecked = true;
            txt_note.Text = "";
            txt_f2007.Text = "0";
            txt_f2008.Text = "0";
            txt_f2009.Text = "0";
            txb_avg.Text = "0";
            txt_q2007.Text = "0";
            txt_q2008.Text = "0";
            txt_q2009.Text = "0";
        }

        private void AddData()
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

            string strSql = "INSERT INTO view_all(ISSN,ShortName,FullName,ChineseName,ClassName,MultiClassName,ClassID,IsTop,Factor2007,Factor2008,Factor2009,FactorAvg,Quote2007,Quote2008,Quote2009,[Note]) VALUES ('";
            strSql += txt_issn.Text.ToString().Trim() + "','";
            strSql += txt_sname.Text.ToString().Trim() + "','";
            strSql += txt_fname.Text.ToString().Trim() + "','";
            strSql += txt_cname.Text.ToString().Trim() + "','";
            strSql += ((System.Windows.Controls.ContentControl)cbo_ClassName.SelectedValue).Content.ToString() + "','";
            strSql += txt_mclass.Text.ToString().Trim() + "','";
            strSql += ClassID + "','";
            strSql += IsTop + "','";
            strSql += txt_f2007.Text.ToString().Trim() + "','";
            strSql += txt_f2008.Text.ToString().Trim() + "','";
            strSql += txt_f2009.Text.ToString().Trim() + "','";
            strSql += FAVG.ToString() + "','";
            strSql += txt_q2007.Text.ToString().Trim() + "','";
            strSql += txt_q2008.Text.ToString().Trim() + "','";
            strSql += txt_q2009.Text.ToString().Trim() + "','";
            strSql += txt_note.Text.ToString().Trim() + "')";

            AccessUtil util = new AccessUtil(strCon);

            util.ExecuteWithoutReturn(strSql);
        }

        private void txt_f2007_LostFocus(object sender, RoutedEventArgs e)
        {
            txb_avg.Text = Convert.ToString((double.Parse(txt_f2007.Text.ToString().Trim()) + double.Parse(txt_f2008.Text.ToString().Trim()) + double.Parse(txt_f2009.Text.ToString())) / 3);
        }

        private void txt_f2008_LostFocus(object sender, RoutedEventArgs e)
        {
            txb_avg.Text = Convert.ToString((double.Parse(txt_f2007.Text.ToString().Trim()) + double.Parse(txt_f2008.Text.ToString().Trim()) + double.Parse(txt_f2009.Text.ToString())) / 3);
        }

        private void txt_f2009_LostFocus(object sender, RoutedEventArgs e)
        {
            txb_avg.Text = Convert.ToString((double.Parse(txt_f2007.Text.ToString().Trim()) + double.Parse(txt_f2008.Text.ToString().Trim()) + double.Parse(txt_f2009.Text.ToString())) / 3);
        }
    }
}
