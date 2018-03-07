using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interakční logika pro Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        string pswd;
        Grid Control;
        List<Game> game_list = new List<Game>();
        public Login(Grid c)
        {
            InitializeComponent();
            foreach(Game ga in game_list)
            {
                Debug.WriteLine(ga.ID + " " + ga.Name);
            }
            BackControl.DisplayCount = 0;
            Control = c;
            if (!CheckConnection.IsTrue())
            {
                login.IsEnabled = false;
                alogin.IsEnabled = false;
                Register.IsEnabled = false;
                email.IsEnabled = false;
                password.IsEnabled = false;
            }
        }
        private async Task GetItems()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Customers&Nick=" + email.Text + "&Password=" + password.Password + "&key=" + BackControl.APIKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            Debug.WriteLine(response.Content);

            try
            {
                string parsed = response.Content.Substring(1, response.Content.Length - 2);
                Customer c = JsonConvert.DeserializeObject<Customer>(parsed);
                MessageBox.Show("Přihlášen jako " + c.Nick);
                BackControl.Logged = c.ID;
                Control.Visibility = Visibility.Visible;
                if(c.Level > 0)
                {
                    BackControl.IsAdmin = true;
                }      
                BackControl.frame.Navigate(new GameList());
            }
            catch
            {
                MessageBox.Show("Uživatel " + email.Text + " nebyl s timto heslem nenalezen.");
                password.Password = "";
            }
        }

        private void login_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(email.Text) || String.IsNullOrWhiteSpace(password.Password))
            {
                MessageBox.Show("Nejdříve vyplňte formulář");
            }
            else
            {
                GetItems();
            }          
        }

        private void alogin_Click(object sender, RoutedEventArgs e)
        {
            email.Text = "galscze";
            password.Password = "123456";
        }

        private void alogin_Click_1(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new NewUser(Control));
        }

        private void offline_Click(object sender, RoutedEventArgs e)
        {
            BackControl.frame.Navigate(new GameList());
            Control.Visibility = Visibility.Visible;
        }
    }
}
