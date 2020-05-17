using Project1.Domain;
using Project1.Models;
using System;
using System.Collections.Generic;
using System.Text;


namespace Project1.Services
{
    public interface IServiceHome
    {
        StoreItemModel ServItems(int id);
        List<UserOrderItemStoredList> ServItemPost(int id, int? orderId, int quantity);
        UserOrderItemStoredList ServItemPostElse(int id, int? orderId, int quantity);
        List<UserOrderItem> ServCart(List<UserOrderItemStoredList> orderList);
        void ServPurchased(List<UserOrderItemStoredList> orderList);
        OrdersByLocationModel ServOrderHistoryByLocation(string location);
        SearchUserByNameModel ServSearchUserByName(string firstName, string lastName);
    }
}
