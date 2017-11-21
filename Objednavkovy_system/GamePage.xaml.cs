using RestSharp;
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
        Game game;
        public GamePage(Game g)
        {
            InitializeComponent();
            game = g;
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
            GameOrder go = new GameOrder();
            go.GameID = game.ID;
            go.OrderID = 7;
            string url = "https://student.sps-prosek.cz/~zdychst14/Game_shop/insert.php?Table=API_Games_Orders";
            //string url = "https://requestb.in/10kkloz1";
            var client = new RestClient(url);
            var request = new RestRequest(Method.POST);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("content-type", "application/json");
            request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(go), ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            MessageBox.Show(response.Content);
    }
    }
}
