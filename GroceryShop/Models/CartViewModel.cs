using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryShop.Models
{
    public class CartViewModel
    {
        public List<Cartitem> Cartitems { get; set; }
        public decimal GrandTotal { get; set; }
    }
}
