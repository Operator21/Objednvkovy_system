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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Objednavkovy_system
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            BackControl.frame = frame;
            BackControl.CartItems = Cart_Items;
            frame.Navigate(new Login());
        }

        private void games_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new GameList());
        }

        private void customers_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new CustomerList());
        }
    }
}
