using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Project1.Domain
{
    /// <summary>
    /// Model for every order user makes
    /// </summary>
    public class UserOrder
    {
        [DisplayName("Order Id")]
        public int UserOrderId { get; set; } //PRIMARY KEY
        public virtual UserInfo UserInfo { get; set; } //RELATION TO USERINFO
        public virtual StoreLocation StoreLocation { get; set; } //RELATION TO LOCATION

        public virtual ICollection<UserOrderItem> UserOrderItems { get; set; } //RELATION TO STOREITEM

        private DateTime _timeStamp; //time stamp of order
        [DisplayName("Order Time")]
        public DateTime timeStamp
        {
            get { return _timeStamp; }
            set { _timeStamp = value; }
        }
    }
}

