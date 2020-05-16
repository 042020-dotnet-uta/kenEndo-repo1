using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class UserOrderItemStoredList
    {
        public int itemId { get; set; }
        public int? orderId { get; set; }
        public int quantity { get; set; }
    }
}
