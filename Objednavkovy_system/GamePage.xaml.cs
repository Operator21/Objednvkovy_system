﻿using RestSharp;
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
        int ID;
        public GamePage(Game g)
        {
            InitializeComponent();
            ID = g.ID;
            game = g;
            try
            {
                Background.Source = new BitmapImage(new Uri(g.URL));
            }
            catch
            {
                MessageBox.Show("Obrázek nelze zobrazit");
            }
            
            name.Content = g.Name;
            price.Content = g.Price + "$";
            l = BackControl.CartItems;
            if (!BackControl.IsAdmin)
            {
                Edit.Visibility = Visibility.Collapsed;
            }
        }

        private void buy_Click(object sender, RoutedEventArgs e)
        {
            //OrderAdd();  
            BackControl.GamesOrdered.Add(game);
        }
        private void OrderAdd()
        {
            int items = Convert.ToInt32(l.Content);
            items++;
            BackControl.CartItems.Content = items;
            GameOrder go = new GameOrder();

            if (BackControl.Order < 1)
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
                int id = Convert.ToInt32(responseo.Content);
                BackControl.Order = id;
            }
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new GameList());
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new NewGame(ID));
        }
    }
}
