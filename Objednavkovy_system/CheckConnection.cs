using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Objednavkovy_system
{
    public static class CheckConnection
    {
        static bool connect = true;
        public static bool IsTrue()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~zdychst14/Game_shop/connection.php");
            var request = new RestRequest(Method.GET);
            request.AddHeader("postman-token", "831baaf3-6305-6de2-22ea-daee8334e754");
            request.AddHeader("cache-control", "no-cache");
            IRestResponse response = client.Execute(request);

            if (connect)
            {
                if (response.Content == "1")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }      
        }
    }
}
