using Microsoft.AspNetCore.Mvc.Rendering;
using Project1.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project1.Models
{
    public class OrdersByLocationModel
    {
        public List<UserOrder> userOrders { get; set; }
        public List<UserOrderItem> userOrderItems { get; set; }
        public SelectList storeLocations { get; set; }
        public string location { get; set; }
    }
}
