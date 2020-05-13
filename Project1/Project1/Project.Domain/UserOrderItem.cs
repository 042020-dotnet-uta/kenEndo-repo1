using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain
{
    /// <summary>
    /// Model for the item name of what user ordered
    /// </summary>
    public class UserOrderItem
    {
        public int UserOrderItemId { get; set; } //Primary key
        public virtual StoreItem StoreItem { get; set; } //RELATION TO STOREITEM
        public virtual UserOrder UserOrder { get; set; } //RELATION TO USERORDER
    }
}
