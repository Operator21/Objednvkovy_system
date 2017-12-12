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
        }
        private void GetGames()
        {
            /*var client1 = new RestClient(BackControl.URL + "script.php?Table=API_Orders&Customer_ID=" + BackControl.Logged);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client1.Execute(request1);
            order_list = JsonConvert.DeserializeObject<List<Order>>(response1.Content);

            var client = new RestClient(BackControl.URL + "script.php?Table=API_Games_Orders");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            id_list = JsonConvert.DeserializeObject<List<GameOrder>>(response.Content);

            foreach (Order o in order_list)
            {
                Debug.WriteLine("Prověřuji: " + o.ID);
                count = 0;
                foreach(GameOrder go in id_list)
                {
                    if(go.OrderID == o.ID)
                    {
                        count++;
                        Debug.WriteLine(count);
                    }               
                }
                if(count > 0)
                {
                    Orders_in_gui.Add(o);
                    Debug.WriteLine(o.ID + " Jde do listu");
                } else
                {
                    Debug.WriteLine(o.ID + "-");
                }
            }
            Order_list.ItemsSource = Orders_in_gui;*/
            /*
            var client2 = new RestClient(BackControl.URL + "script.php?Table=API_Games");
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("cache-control", "no-cache");
            IRestResponse response2 = client2.Execute(request2);
            games_list = JsonConvert.DeserializeObject<List<Game>>(response2.Content);*/

            
            /*foreach (Order o in order_list)
            {
                Debug.WriteLine("Objednavka: " + o.ID);
                foreach(GameOrder go in id_list)
                {
                    if(go.OrderID == o.ID)
                    {
                        Debug.WriteLine("-" + go.ID);
                        foreach(Game g in games_list)
                        {
                            if(g.ID == go.GameID)
                            {
                                Debug.WriteLine("--" + g.Name);
                                games_in_order.Add(g);
                                price += g.Price;
                                Check.Content = "Zaplatit " + price;
                            }    
                        }
                    }                    
                }
            }*/
            //Games.ItemsSource = games_in_order;
        }

        private void Check_Click(object sender, RoutedEventArgs e)
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
    }
}
