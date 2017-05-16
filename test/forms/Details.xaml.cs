using System.Windows;
using System.Windows.Input;
using test.entities;

namespace test.forms
{
    /// <summary>
    /// Details.xaml 的交互逻辑
    /// </summary>
    public partial class Details : Window
    {

        Magazine magazine = new Magazine();

        public Details(Magazine magazine)
        {
            InitializeComponent();

            this.magazine = magazine;
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
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

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
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

            //设置只读
            TxtIssn.IsReadOnly = true;
            TxtSname.IsReadOnly = true;
            TxtFname.IsReadOnly = true;
            TxtCname.IsReadOnly = true;
            CboClassName.IsEnabled = false;
            TxtMclass.IsReadOnly = true;
            Rdo1.IsEnabled = false;
            Rdo2.IsEnabled = false;
            Rdo3.IsEnabled = false;
            Rdo4.IsEnabled = false;
            RdoNo.IsEnabled = false;
            RdoYes.IsEnabled = false;
            TxtF2007.IsReadOnly = true;
            TxtF2008.IsReadOnly = true;
            TxtF2009.IsReadOnly = true;
            TxtQ2007.IsReadOnly = true;
            TxtQ2008.IsReadOnly = true;
            TxtQ2009.IsReadOnly = true;
            TxtNote.IsReadOnly = true;
        }
    }
}
