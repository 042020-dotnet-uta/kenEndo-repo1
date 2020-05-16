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
        IEnumerable<StoreItem> GetAllStoreItemByLocation(int locationId);
        IEnumerable<StoreItem> GetAllStoreItem();
        //get all order history by user
        IEnumerable<UserOrder> GetAllOrderByUser(int id);
        //get all order history by location
        IEnumerable<UserOrder> GetAllOrderByLocation(string storeLocation);
        IEnumerable<UserOrder> GetAllOrders();
        IEnumerable<UserOrder> GetOrderByOrderId(int id);
        IEnumerable<UserOrderItem> GetOrderItemById(int id);
        IEnumerable<UserOrderItem> GetUserOrderItems(UserOrder userOrder);

        int GetOrderLocationFromOrder(int? id);
        IEnumerable<UserInfo> GetUserInfoByFirstName(string fName);
        IEnumerable<UserInfo> GetUserInfoByLastName(string lName);
        IEnumerable<UserInfo> GetAllUserInfo();
        //add new user to the database
        void AddNewUser(UserInfo userInfo);
        //add new order to the database
        void AddNewOrder(string userName, int itemId);
        void AddOrderItemToDb(List<UserOrderItem> lists);
        void UpDateInventoryQuantity(List<UserOrderItem> orders);
        UserOrderItem ReturnNewOrderItem(int itemId, int? orderId, int quantity);
        UserInfo CheckUser(UserInfo userInfo);
    }
}
