﻿using Newtonsoft.Json;
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
    /// Interakční logika pro OrderView.xaml
    /// </summary>
    public partial class OrderView : Page
    {
        List<GameOrder> id_list = new List<GameOrder>();
        List<Game> games_list = new List<Game>();
        List<GameOrder_List> games_in_order = new List<GameOrder_List>();
        int ID;
        Order order;
        public OrderView(Order o)
        {
            InitializeComponent();
            ID = o.ID;
            order = o;
            //MessageBox.Show(order.Paid.ToString());
            if (order.Paid == 1)
            {
                Check.IsEnabled = false;
                Check.Content = "Zaplaceno";
                context.Visibility = Visibility.Collapsed;
            }

            GetGames();            
        }
        private void GetGames()
        {
            var client = new RestClient(BackControl.URL + "script.php?Table=API_Games_Orders&OrderID=" + ID);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            id_list = JsonConvert.DeserializeObject<List<GameOrder>>(response.Content);

            var client2 = new RestClient(BackControl.URL + "script.php?Table=API_Games");
            var request2 = new RestRequest(Method.GET);
            request2.AddHeader("cache-control", "no-cache");
            IRestResponse response2 = client2.Execute(request2);
            games_list = JsonConvert.DeserializeObject<List<Game>>(response2.Content);

            foreach(GameOrder go in id_list)
            {
                if(go.OrderID == ID)
                {
                    Debug.WriteLine("-" + go.ID);
                    foreach(Game g in games_list)
                    {
                        if(g.ID == go.GameID)
                        {
                            Debug.WriteLine("--" + g.Name);
                            GameOrder_List gol = new GameOrder_List();
                            gol.ID = go.ID;
                            gol.Name = g.Name;
                            gol.OrderID = ID;
                            gol.Price = g.Price;
                            gol.URL = g.URL;
                            games_in_order.Add(gol);
                        }    
                    }
                }                    
            }
            float price = 0;
            foreach (GameOrder_List gol in games_in_order)
            {
                price += gol.Price;
            }
            Games.ItemsSource = games_in_order;
            Check.Content += " " + price + "$";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new OrdersList());
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            GameOrder_List g = (GameOrder_List)Games.SelectedItem;
            
            var client1 = new RestClient(BackControl.URL + "delete.php?Table=API_Games_Orders&ID=" + g.ID);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client1.Execute(request1);
            BackControl.frame.Navigate(new OrderView(order));
        }

        private void Check_Click(object sender, RoutedEventArgs e)
        {
            float price = 0;
            foreach(GameOrder_List gol in games_in_order)
            {
                price += gol.Price;
            }
            string url = "insert.php?Table=API_Orders&Paid=1&ID=" + ID;
            var cliento = new RestClient(BackControl.URL + url);
            var requesto = new RestRequest(Method.POST);
            requesto.AddHeader("cache-control", "no-cache");
            requesto.AddHeader("content-type", "application/json");
            cliento.Execute(requesto);
            MessageBox.Show("Objednávka " + ID + " zaplacena: " + price + "$");
            BackControl.frame.Navigate(new OrdersList());
        }
    }
}
