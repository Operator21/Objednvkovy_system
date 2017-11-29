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
        List<Game> game_list = new List<Game>();
        List<Game> games_in_list = new List<Game>();
        public ShoppingCart()
        {
            InitializeComponent();
            GetGames();
        }
        private void GetGames()
        {
            var client = new RestClient(BackControl.URL + "script.php?Table=API_Games_Orders");
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            id_list = JsonConvert.DeserializeObject<List<GameOrder>>(response.Content);
            foreach (GameOrder g in id_list)
            {
                Debug.WriteLine("ID: " + g.ID.ToString());
                Debug.WriteLine("GameID: " + g.GameID);
                Debug.WriteLine("OrderID: " + g.OrderID);
            }
            var client1 = new RestClient(BackControl.URL + "script.php?Table=API_Games");
            var request1 = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client.Execute(request);
            game_list = JsonConvert.DeserializeObject<List<Game>>(response.Content);
            
            foreach (GameOrder go in id_list)
            {
                Debug.WriteLine("..." + go.GameID);
                foreach (Game g in game_list)
                {
                    Debug.WriteLine("_" + g.ID);
                    if(go.GameID == g.ID)
                    {
                        games_in_list.Add(g);
                    }
                }
            }
            Games.ItemsSource = games_in_list;
        }
    }
}
