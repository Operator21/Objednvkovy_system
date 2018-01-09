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
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : Page
    {
        bool update;
        int ID;
        public NewGame()
        {
            InitializeComponent();
        }
        public NewGame(int id)
        {
            InitializeComponent();
            update = true;
            var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Games&ID=" + id);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            string parsed = response.Content.Substring(1, response.Content.Length - 2);
            Game g = JsonConvert.DeserializeObject<Game>(parsed);
            Name.Text = g.Name;
            URL.Text = g.URL;
            Price.Text = g.Price.ToString();
            ID = id;
            MessageBox.Show("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Games&ID=" + id);
        }

        private void URL_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Uri.IsWellFormedUriString(URL.Text, UriKind.Absolute))
            {
                Img.Source = new BitmapImage(new Uri(URL.Text, UriKind.Absolute));
            }
            else
            {
                Img.Source = null;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Price.Text))
            {
                MessageBox.Show("Všechny informace musí být vyplněné");
            }
            else
            {
                try
                {
                    string url = "insert.php?Table=API_Games";
                    Game g = new Game();
                    if (update)
                    {
                        url = "insert.php?Table=API_Games&ID=" + ID;
                    }
                    g.Name = Name.Text;
                    float x = 0;
                    float.TryParse(Price.Text.ToString(), out x);
                    g.Price = x;
                    g.URL = URL.Text;

                    var client = new RestClient(BackControl.URL + url);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(g), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    MessageBox.Show(BackControl.URL + url);
                    Debug.WriteLine(response.Content);
                    BackControl.frame.Navigate(new GameList());
                }
                catch
                {
                    MessageBox.Show("Vyskytla se neočekávaná chyba");
                }
                
            }
        }
    }
}
