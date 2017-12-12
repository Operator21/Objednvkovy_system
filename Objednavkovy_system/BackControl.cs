using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Objednavkovy_system
{
    public static class BackControl
    {
        public static Frame frame { get; set; }
        public static StackPanel panel { get; set; }
        public static Label CartItems { get; set; }
        public static string APIKey { get; set; }
        public static int Logged { get; set; }
        public static int Order { get; set; }
        public static string URL = "https://student.sps-prosek.cz/~zdychst14/Game_shop/";
        public static List<Game> GamesOrdered = new List<Game>();
    }
}
