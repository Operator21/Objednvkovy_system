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
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Page
    {
        int ID;
        bool update;
        Grid Control;
        public NewUser(Grid c)
        {
            InitializeComponent();
            Control = c;
            Delete.Visibility = Visibility.Collapsed;
            Pass_change.Visibility = Visibility.Collapsed;
        }
        public NewUser(Grid c, int id)
        {
            InitializeComponent();
            Control = c;
            ID = id;
            var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Customers&ID=" + ID + "&key=" + BackControl.APIKey);
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);
            string parsed = response.Content.Substring(1, response.Content.Length - 2);
            Customer cus = JsonConvert.DeserializeObject<Customer>(parsed);

            Pass_lbl.Content = "Nové heslo";
            Name.Text = cus.Name;
            Surname.Text = cus.Surname;
            Nick.Text = cus.Nick;
            Pass.Password = cus.Password;
            Pass_check.Password = cus.Password;
            Nick.IsEnabled = false;
            //Name.IsEnabled = false;
            //Surname.IsEnabled = false;
            update = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Name.Text) || string.IsNullOrWhiteSpace(Surname.Text) || string.IsNullOrWhiteSpace(Nick.Text))
            {
                MessageBox.Show("Všechny informace musí být vyplněné");
            }
            else
            {
                try
                {
                    if (update)
                    {
                        Customer c = new Customer();
                        c.Name = Name.Text;
                        c.Surname = Surname.Text;
                        c.Nick = Nick.Text;
                        c.Password = Pass.Password;
                        string url = "insert.php?Table=API_Customers&ID=" + ID;
                        var client1 = new RestClient(BackControl.URL + url);
                        var request1 = new RestRequest(Method.POST);
                        request1.AddHeader("cache-control", "no-cache");
                        request1.AddHeader("content-type", "application/json");
                        request1.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(c), ParameterType.RequestBody);
                        IRestResponse response1 = client1.Execute(request1);
                        MessageBox.Show("Zmeny byly ulozeny");
                        BackControl.frame.Navigate(new NewUser(BackControl.panel, ID));
                    }
                    else if(Pass.Password == Pass_check.Password)
                    {
                        var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/script.php?Table=API_Customers&Nick=" + Nick.Text + "&key=" + BackControl.APIKey);
                        var request = new RestRequest(Method.GET);
                        request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
                        request.AddHeader("cache-control", "no-cache");
                        IRestResponse response = client.Execute(request);
                        if(response.Content != "1")
                        {
                            Customer c = new Customer();
                            c.Name = Name.Text;
                            c.Surname = Surname.Text;
                            c.Nick = Nick.Text;
                            c.Password = Pass.Password;

                            string url = "insert.php?Table=API_Customers";
                            var client1 = new RestClient(BackControl.URL + url);
                            var request1 = new RestRequest(Method.POST);
                            request1.AddHeader("cache-control", "no-cache");
                            request1.AddHeader("content-type", "application/json");
                            request1.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(c), ParameterType.RequestBody);
                            IRestResponse response1 = client1.Execute(request1);
                            BackControl.panel.Visibility = Visibility.Collapsed;
                            BackControl.frame.Navigate(new Login(Control));
                        }
                        else
                        {
                            MessageBox.Show("Tato přezdívka je již používána");
                        }

                        
                    }
                    else
                    {
                        MessageBox.Show("Hesla nejsou stejná");
                        Pass.Password = "";
                        Pass_check.Password = "";
                    } 
                }
                catch
                {
                    MessageBox.Show("Vyskytla se neočekávaná chyba");
                }

            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Smazat uzivatele?","Smazat",MessageBoxButton.YesNo);
            if(result == MessageBoxResult.Yes)
            {
                MessageBox.Show("Uzivatel smazan");
                var client1 = new RestClient(BackControl.URL + "delete.php?Table=API_Customers&ID=" + BackControl.Logged);
                var request1 = new RestRequest(Method.GET);
                request1.AddHeader("cache-control", "no-cache");
                IRestResponse response1 = client1.Execute(request1);
                BackControl.panel.Visibility = Visibility.Collapsed;
                BackControl.frame.Navigate(new NewUser(BackControl.panel,ID));
            }           
        }

        private void Pass_change_Click(object sender, RoutedEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(Pass.Password) || string.IsNullOrWhiteSpace(Pass_check.Password))
            {
                MessageBox.Show("Nejdrive vyplnte nove heslo");
            }
            else
            {
                Customer c = new Customer();
                c.Name = Name.Text;
                c.Surname = Surname.Text;
                c.Nick = Nick.Text;
                c.Password = Pass.Password;

                string url = "insert.php?Table=API_Customers&ID=" + ID;
                var client1 = new RestClient(BackControl.URL + url);
                var request1 = new RestRequest(Method.POST);
                request1.AddHeader("cache-control", "no-cache");
                request1.AddHeader("content-type", "application/json");
                request1.AddParameter("application/json", Newtonsoft.Json.JsonConvert.SerializeObject(c), ParameterType.RequestBody);
                IRestResponse response1 = client1.Execute(request1);
                MessageBox.Show("Heslo bylo zmeneno");
                BackControl.frame.Navigate(new NewUser(BackControl.panel,ID));
            }            
        }
    }
}
