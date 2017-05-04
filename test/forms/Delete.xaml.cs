using System.Windows;
using System.Windows.Input;
using test.common;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Delete.xaml 的交互逻辑
    /// </summary>
    public partial class Delete : Window
    {

        private string strCon = "";

        Magazine magazine = new Magazine();

        public Delete(string strCon, Magazine magazine)
        {
            InitializeComponent();

            this.strCon = strCon;

            this.magazine = magazine;

        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btn_abort_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //加载获取的magazine实体
            txt_fname.Text = magazine.FullName;
            txt_sname.Text = magazine.ShortName;
            txt_issn.Text = magazine.ISSN;
            txt_cname.Text = magazine.ChineseName;

            switch (magazine.ClassName)
            {
                case "地学":cbo_ClassName.SelectedIndex = 0;break;
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
                default:break;
            }
            txt_mclass.Text = magazine.MultyClassName;
            switch (magazine.ClassID)
            {
                case "1":rdo_1.IsChecked = true;break;
                case "2":rdo_2.IsChecked = true;break;
                case "3":rdo_3.IsChecked = true;break;
                case "4":rdo_4.IsChecked = true;break;
                default:break;
            }
            if (magazine.isTop == "Y")
            {
                rdo_yes.IsChecked = true;
            }else
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

            //设置只读
            txt_issn.IsReadOnly = true;
            txt_sname.IsReadOnly = true;
            txt_fname.IsReadOnly = true;
            txt_cname.IsReadOnly = true;
            cbo_ClassName.IsEnabled = false;
            txt_mclass.IsReadOnly = true;
            rdo_1.IsEnabled = false;
            rdo_2.IsEnabled = false;
            rdo_3.IsEnabled = false;
            rdo_4.IsEnabled = false;
            rdo_no.IsEnabled = false;
            rdo_yes.IsEnabled = false;
            txt_f2007.IsReadOnly = true;
            txt_f2008.IsReadOnly = true;
            txt_f2009.IsReadOnly = true;
            txt_q2007.IsReadOnly = true;
            txt_q2008.IsReadOnly = true;
            txt_q2009.IsReadOnly = true;
            txt_note.IsReadOnly = true;
        }

        private void btn_delete_Click(object sender, RoutedEventArgs e)
        {
            //确定删除
            AccessHandler handler = new AccessHandler(strCon);
            handler.ExecuteWithoutReturn("DELETE FROM view_all WHERE ID=" + magazine.ID);

            this.Close();   
        }
    }
}
