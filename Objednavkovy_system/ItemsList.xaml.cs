﻿using Newtonsoft.Json;
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
    /// Interakční logika pro ItemsList.xaml
    /// </summary>
    public partial class ItemsList : Page
    {
        public ItemsList()
        {
            InitializeComponent();
        }
        private async Task GetItems()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Games");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            Item_list.ItemsSource = JsonConvert.DeserializeObject<List<Game>>(response.Content);
        }
    }
}
