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
    /// Interakční logika pro GameList.xaml
    /// </summary>
    public partial class GameList : Page
    {
        List<Game> games = new List<Game>();
        Game selected = new Game();
        public GameList()
        {
            InitializeComponent();
            if (!BackControl.IsAdmin)
            {
                Add.Visibility = Visibility.Collapsed;
            }
            GetGames();
            if (CheckConnection.IsTrue())
            {
                if (BackControl.DisplayCount < 1)
                {
                    foreach (Game g in games)
                    {
                        Save(g);
                    }
                    MessageBox.Show("Katalog uložen do offline");
                }
                BackControl.DisplayCount++;
            }           
        }
        private void GetGames()
        {
            if (CheckConnection.IsTrue())
            {
                var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Games");
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                games = JsonConvert.DeserializeObject<List<Game>>(response.Content);
                foreach (Game g in games)
                {
                    Debug.WriteLine(g.ID.ToString());
                    Debug.WriteLine(g.Name);
                    Debug.WriteLine(g.Price);
                    Debug.WriteLine(g.URL);
                }
            }
            else
            {
                games = App.Database.GetGames().Result;
                MessageBox.Show("Databáze je v režimu offline");
            }
            
            Games.ItemsSource = games;
        }

        private void Games_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selected = ((Game)Games.SelectedItem);
            BackControl.frame.Navigate(new GamePage(selected));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new NewGame());
        }
        private async Task Save(Game g)
        {
            await App.Database.SaveGame(g);
        }
    }
}
