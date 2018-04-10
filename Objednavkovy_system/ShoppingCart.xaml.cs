using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Interakční logika pro ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Page
    {
        List<GameOrder> id_list = new List<GameOrder>();
        List<Order> order_list = new List<Order>();
        List<Game> games_list = new List<Game>();
        List<Order> Orders_in_gui = new List<Order>();
        float price = 0;
        int count;
        public ShoppingCart()
        {
            InitializeComponent();
            Games.ItemsSource = BackControl.GamesOrdered;
            if(BackControl.GamesOrdered.Count < 1)
            {
                alert.Visibility = Visibility.Visible;
                Check.IsEnabled = false;
            } else
            {
                alert.Visibility = Visibility.Collapsed;
            }
            if(App.Database.GetItems().Result.Count() < 1)
            {
                Offline.IsEnabled = false;
            }
            if (!CheckConnection.IsTrue())
            {
                Offline.IsEnabled = false;
            }
            if (!BackControl.IsLogged)
            {
                Check.Content = "Uložit";
            }
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            if (CheckConnection.IsTrue() && BackControl.IsLogged)
            {
                Order o = new Order();
                o.CustomerID = BackControl.Logged;

                string urlo = "insert.php?Table=API_Orders";
                var cliento = new RestClient(BackControl.URL + urlo);
                var requesto = new RestRequest(Method.POST);
                requesto.AddHeader("cache-control", "no-cache");
                requesto.AddHeader("content-type", "application/json");
                requesto.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(o), ParameterType.RequestBody);
                IRestResponse responseo = cliento.Execute(requesto);
                BackControl.Order = Convert.ToInt32(responseo.Content);

                foreach (Game g in BackControl.GamesOrdered)
                {
                    OrderAdd(g);
                }
                MessageBox.Show(BackControl.GamesOrdered.Count() + " produktů bylo objednáno");
                BackControl.GamesOrdered.Clear();
                BackControl.frame.Navigate(new ShoppingCart());
                alert.Visibility = Visibility.Visible;
            } else
            {
                List<Cart_Item> items = new List<Cart_Item>();
                foreach (Game g in BackControl.GamesOrdered)
                {
                    Cart_Item i = new Cart_Item();
                    i.Game_ID = g.ID;
                    items.Add(i);
                }
                App.Database.SaveCart(items);
                MessageBox.Show(BackControl.GamesOrdered.Count() + " produktů bylo uloženo do offline košíku");
                BackControl.GamesOrdered.Clear();
                BackControl.frame.Navigate(new ShoppingCart());
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            BackControl.GamesOrdered.Remove((Game)Games.SelectedItem);
            Dispatcher.Invoke(Refresh);
        }
        private void Refresh()
        {
            BackControl.frame.Navigate(new ShoppingCart());
        }
        private void OrderAdd(Game game)
        {
            GameOrder go = new GameOrder();
             
            go.OrderID = BackControl.Order;
            go.GameID = game.ID;

            string url = "insert.php?Table=API_Games_Orders";
            var client = new RestClient(BackControl.URL + url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(go), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
        }

        private void Offline_Click(object sender, RoutedEventArgs e)
        {
            alert.Visibility = Visibility.Collapsed;
            int loop = 0;
            foreach(Cart_Item c in App.Database.GetItems().Result)
            {
                loop++;
                Debug.WriteLine(c.ID + "/" + c.Game_ID);
                try
                {
                    var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Games&ID=" + c.Game_ID);
                    var request = new RestRequest(Method.GET);
                    request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
                    request.AddHeader("cache-control", "no-cache");
                    IRestResponse response = client.Execute(request);
                    string parsed = response.Content.Substring(1, response.Content.Length - 2);
                    Game g = JsonConvert.DeserializeObject<Game>(parsed);
                    games_list.Add(g);
                    App.Database.Delete(c);
                }
                catch
                {
                    if (loop == 1)
                    {  
                        MessageBox.Show("Některé hry z offline košíku již nejsou dostupné v obchodě");
                    }
                    Debug.WriteLine(c.Game_ID + " was not found in database");
                    App.Database.Delete(c);
                } 
            }
            BackControl.GamesOrdered = games_list;
            BackControl.Navigate(new ShoppingCart());
        }
    }
}
