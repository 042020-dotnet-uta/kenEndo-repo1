using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain.IRepositories
{
    public interface IRepoUserOrderItem
    {
        //add list of user order item to database with addRange
        void AddUserOrderItem(List<UserOrderItem> lists);
        //return all user order items by user order id
        IEnumerable<UserOrderItem> GetAllUserOrderItemByUserOrderId(int id);
        //return an instance of an user order item to the cart so that it can be stored into a list in session
        UserOrderItem CreateUserOrderItem(int itemId, int? orderId, int quantity);
    }
}
