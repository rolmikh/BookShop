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

namespace Book_Shop_WPF
{
    /// <summary>
    /// Логика взаимодействия для AdminWindow.xaml
    /// </summary>
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void btBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Close();
        }

        private void btUser_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Visibility = Visibility.Visible;
        }

        private void btStatusRole_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Visibility = Visibility.Hidden;
        }

        private void btSupply_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Visibility = Visibility.Hidden;
        }

        private void btCategory_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Visibility = Visibility.Hidden;
        }

        private void btOrder_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Visibility = Visibility.Hidden;
        }

        private void btProduct_Click(object sender, RoutedEventArgs e)
        {
            gridUser.Visibility = Visibility.Hidden;
        }
    }
}
