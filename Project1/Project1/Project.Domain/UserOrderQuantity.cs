using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain
{
    /// <summary>
    /// Model for User Ordered Quantity
    /// </summary>
    public class UserOrderQuantity
    {
        public int UserOrderQuantityId { get; set; } //PRIMARY KEY
        public virtual UserOrder UserOrder { get; set; } //RELATION TO USERORDER

        public virtual StoreItem StoreItem { get; set; } //RELATION TO STOREITEM

        private int _orderQuantity; //QUANTITY OF ORDER 

        public int orderQuantity
        {
            get { return _orderQuantity; }
            set { _orderQuantity = value; }
        }
    }
}
