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
            BackControl.frame = Frame;
            BackControl.CartItems = Cart_Items;
            BackControl.APIKey = "1bbedf5f-e277-451f-b895-0f972ea47383";
            BackControl.panel = Controls;
            Controls.Visibility = Visibility.Collapsed;
            if (!CheckConnection.IsTrue() )
            {
                Order_btn.Visibility = Visibility.Collapsed;
                User_btn.Visibility = Visibility.Collapsed;
            }
            Frame.Navigate(new Login(Controls));
        }

        private void games_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new GameList());
        }

        private void customers_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new CustomerList());
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new ShoppingCart());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new OrdersList());
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new NewUser(Controls, BackControl.Logged));
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            BackControl.panel.Visibility = Visibility.Collapsed;
            BackControl.IsLogged = false;
            BackControl.Logged = 0;
            BackControl.IsAdmin = false;
            BackControl.GamesOrdered.Clear();
            BackControl.frame.Navigate(new Login(Controls));
        }
    }
}
