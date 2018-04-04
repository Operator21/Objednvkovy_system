using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objednavkovy_system
{
    public class Cart_Item
    {
        [PrimaryKey,AutoIncrement]
        public int ID { get; set; }
        public int Game_ID { get; set; }
    }
}
