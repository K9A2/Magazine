using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace test.forms
{
    /// <summary>
    /// frm_insert.xaml 的交互逻辑
    /// </summary>
    public partial class frm_insert : Window
    {
        public frm_insert()
        {
            InitializeComponent();
        }

        private void button_Copy_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            Point position = e.GetPosition(frm_insert_main_grid);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                if (position.X >= 0 && position.X < frm_insert_main_grid.ActualWidth && position.Y >= 0 && position.Y < frm_insert_main_grid.ActualHeight)
                {
                    this.DragMove();
                }
            }
        }
    }
}
