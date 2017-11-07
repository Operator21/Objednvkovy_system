using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objednavkovy_system
{
    class Order
    {
        public int ID { get; set; }
        public int Customer_ID { get; set; }
        public DateTime Date { get; set; }
        public float Price { get; set; }
    }
}
