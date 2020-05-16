using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;


namespace Project1.Domain.IRepositories
{
    /// <summary>
    /// business logic repository for user order
    /// </summary>
    public interface IRepoUserOrder
    {
        //add new user order to database by user name and Item id
        void AddUserOrder(string userName, int itemId);
        //return all user orders
        IEnumerable<UserOrder> GetAllOrders();
        //return all user orders by a user id
        IEnumerable<UserOrder> GetAllOrderByUserId(int id);
        //return all user orders by a store location
        IEnumerable<UserOrder> GetAllOrderByLocation(string storeLocation);
    }
}
