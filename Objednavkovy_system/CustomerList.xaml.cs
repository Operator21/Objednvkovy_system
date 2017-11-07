using Newtonsoft.Json;
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
    /// Interakční logika pro CustomerList.xaml
    /// </summary>
    public partial class CustomerList : Page
    {
        public CustomerList()
        {
            InitializeComponent();
            GetCustomers();
        }
        private async Task GetCustomers()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Customers");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            People_list.ItemsSource = JsonConvert.DeserializeObject<List<Customer>>(response.Content);
        }
    }
}
