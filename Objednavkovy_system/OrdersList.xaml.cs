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
    /// Interaction logic for OrdersList.xaml
    /// </summary>
    public partial class OrdersList : Page
    {
        List<GameOrder> id_list = new List<GameOrder>();
        List<Order> order_list = new List<Order>();
        List<Game> games_list = new List<Game>();
        List<Order> Orders_in_gui = new List<Order>();
        bool display_all = true;
        public OrdersList()
        {
            InitializeComponent();
            GetOrders();
        }
        private void GetOrders()
        {
            var client1 = new RestClient(BackControl.URL + "script.php?Table=API_Orders&Customer_ID=" + BackControl.Logged);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client1.Execute(request1);
            try
            {
                order_list = JsonConvert.DeserializeObject<List<Order>>(response1.Content);
                var client = new RestClient(BackControl.URL + "script.php?Table=API_Games_Orders");
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                id_list = JsonConvert.DeserializeObject<List<GameOrder>>(response.Content);

                foreach (Order o in order_list)
                {
                    int count = 0;
                    foreach (GameOrder go in id_list)
                    {
                        if (go.OrderID == o.ID)
                        {
                            count++;
                            Debug.WriteLine(count);
                        }
                    }
                    if (count > 0)
                    {
                        Orders_in_gui.Add(o);
                        Debug.WriteLine(o.ID + " Jde do listu");
                    }
                    else
                    {
                        Debug.WriteLine(o.ID + "- nemá žádné položky, je automaticky odstraněna");
                        string url = "delete.php?Table=API_Orders&ID=" + o.ID;
                        var cliento = new RestClient(BackControl.URL + url);
                        var requesto = new RestRequest(Method.POST);
                        requesto.AddHeader("cache-control", "no-cache");
                        requesto.AddHeader("content-type", "application/json");
                        cliento.Execute(requesto);
                    }
                }
                Order_list.ItemsSource = Orders_in_gui;
            }
            catch
            {
                MessageBox.Show("Nejsou zadne objednavky");
            }      
        }
        private void GetOrders(string u)
        {
            var client1 = new RestClient(BackControl.URL + "script.php?Table=API_Orders&Customer_ID=" + BackControl.Logged + u);
            Debug.WriteLine(BackControl.URL + "script.php?Table=API_Orders&Customer_ID=" + BackControl.Logged + u);
            var request1 = new RestRequest(Method.GET);
            request1.AddHeader("cache-control", "no-cache");
            IRestResponse response1 = client1.Execute(request1);
            try
            {
                order_list = JsonConvert.DeserializeObject<List<Order>>(response1.Content);
                var client = new RestClient(BackControl.URL + "script.php?Table=API_Games_Orders");
                var request = new RestRequest(Method.GET);
                request.AddHeader("cache-control", "no-cache");
                IRestResponse response = client.Execute(request);
                id_list = JsonConvert.DeserializeObject<List<GameOrder>>(response.Content);

                foreach (Order o in order_list)
                {
                    int count = 0;
                    foreach (GameOrder go in id_list)
                    {
                        if (go.OrderID == o.ID)
                        {
                            count++;
                            Debug.WriteLine(count);
                        }
                    }
                    if (count > 0)
                    {
                        Orders_in_gui.Add(o);
                        Debug.WriteLine(o.ID + " Jde do listu");
                    }
                    else
                    {
                        Debug.WriteLine(o.ID + "- nemá žádné položky, je automaticky odstraněna");
                        string url = "delete.php?Table=API_Orders&ID=" + o.ID;
                        var cliento = new RestClient(BackControl.URL + url);
                        var requesto = new RestRequest(Method.POST);
                        requesto.AddHeader("cache-control", "no-cache");
                        requesto.AddHeader("content-type", "application/json");
                        cliento.Execute(requesto);
                    }
                }
                Order_list.ItemsSource = Orders_in_gui;
            }
            catch
            {
                MessageBox.Show("Nejsou zadne objednavky");
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Order o = (Order)Order_list.SelectedItem;
            BackControl.frame.Navigate(new OrderView(o));
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Order o = (Order)Order_list.SelectedItem;
            if (o.Paid == 1)
            {
                MessageBox.Show("Zaplacené objednávky není možné smazat");
            }
            else
            {
                var result = MessageBox.Show("Odstranit tuto objednavku ?", "Odstranit", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {

                    Debug.WriteLine(o.ID);
                    string url = "delete.php?Table=API_Orders&ID=" + o.ID;
                    var cliento = new RestClient(BackControl.URL + url);
                    var requesto = new RestRequest(Method.POST);
                    requesto.AddHeader("cache-control", "no-cache");
                    requesto.AddHeader("content-type", "application/json");
                    cliento.Execute(requesto);
                }
            }
            
            BackControl.frame.Navigate(new OrdersList());
        }

        private void Order_btn_Click(object sender, RoutedEventArgs e)
        {
            Orders_in_gui.Clear();
            if (display_all)
            {
                GetOrders("&Paid=0");
                display_all = false;
                Order_btn.Content = "Zobrazit vše";
            } else
            {
                GetOrders();
                display_all = true;
                Order_btn.Content = "Zobrazit nezaplacené";
            }
            Order_list.Items.Refresh();
        }
    }
}
