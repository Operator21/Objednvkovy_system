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
    /// Interakční logika pro GamePage.xaml
    /// </summary>
    public partial class GamePage : Page
    {
        Label l;
        public GamePage(Game g)
        {
            InitializeComponent();
            Background.Source = new BitmapImage(new Uri(g.URL));
            name.Content = g.Name;
            price.Content = g.Price + "$";
            l = BackControl.CartItems;
        }

        private void buy_Click(object sender, RoutedEventArgs e)
        {
            int items = Convert.ToInt32(l.Content);
            items++;
            BackControl.CartItems.Content = items;
        }
    }
}
