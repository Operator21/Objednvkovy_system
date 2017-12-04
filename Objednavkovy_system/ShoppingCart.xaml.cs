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
        List<Game> games_in_order = new List<Game>();
        float price = 0;
        public ShoppingCart()
        {
            InitializeComponent();
            GetGames();
        }
        private void GetGames()
        {
            var client1 = new RestClient(BackControl.URL + "script.php?Table=API_Orders&Customer_ID=" + BackControl.Logged);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client1.Execute(request1);
            order_list = JsonConvert.DeserializeObject<List<Order>>(response1.Content);

            var client = new RestClient(BackControl.URL + "script.php?Table=API_Games_Orders");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            id_list = JsonConvert.DeserializeObject<List<GameOrder>>(response.Content);

            var client2 = new RestClient(BackControl.URL + "script.php?Table=API_Games");
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("cache-control", "no-cache");
            IRestResponse response2 = client2.Execute(request2);
            games_list = JsonConvert.DeserializeObject<List<Game>>(response2.Content);

            foreach (Order o in order_list)
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
            }
            Games.ItemsSource = games_in_order;
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
