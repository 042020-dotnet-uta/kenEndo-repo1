using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Threading.Tasks;

namespace Project1.Domain
{
    /// <summary>
    /// DI interface for db access
    /// </summary>
    public interface IRepository
    {
        //get all store location
        IEnumerable<StoreLocation> GetAllStoreLocations();
        //get all store item by location
        IEnumerable<StoreItem> GetAllStoreItems(StoreLocation storeLocation);
        //get all order history by user
        IEnumerable<UserOrder> GetAllOrderByUser(int id);
        //get all order history by location
        IEnumerable<UserOrder> GetAllOrderByLocation(string storeLocation);
        IEnumerable<UserOrder> GetAllOrders();
        IEnumerable<UserOrder> GetOrderByOrderId(int id);
        IEnumerable<UserOrderItem> GetOrderItemById(int id);
        IEnumerable<UserOrderItem> GetUserOrderItems(UserOrder userOrder);

        IEnumerable<UserInfo> GetUserInfoByFirstName(string fName);
        IEnumerable<UserInfo> GetUserInfoByLastName(string lName);
        IEnumerable<UserInfo> GetAllUserInfo();
        //add new user to the database
        void AddNewUser(UserInfo userInfo);
        //add new order to the database
        void AddNewOrder(UserInfo userInfo, StoreLocation storeLocation,
            StoreItem storeItem, int orderQuantity);
        UserInfo CheckUser(UserInfo userInfo);
    }
}
