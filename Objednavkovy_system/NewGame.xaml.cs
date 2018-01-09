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
    /// Interaction logic for NewGame.xaml
    /// </summary>
    public partial class NewGame : Page
    {
        public NewGame()
        {
            InitializeComponent();
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
                    Game g = new Game();
                    g.Name = Name.Text;
                    float x = 0;
                    float.TryParse(Price.Text, out x);
                    g.Price = x;
                    g.URL = URL.Text;

                    string url = "insert.php?Table=API_Games";
                    var client = new RestClient(BackControl.URL + url);
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("content-type", "application/json");
                    request.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(g), ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);

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
