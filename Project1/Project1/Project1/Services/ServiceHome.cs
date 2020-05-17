using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Project1.Domain;
using Project1.Domain.IRepositories;
using Project1.Models;

namespace Project1.Services
{
    public class ServiceHome : IServiceHome
    {

        private readonly IRepoUserOrder _repoUserOrder;
        private readonly IRepoStoreLocation _repoStoreLocation;
        private readonly IRepoUserOrderItem _repoUserOrderItem;
        private readonly IRepoStoreItem _repoStoreItem;
        private readonly IRepoUserInfo _repoUserInfo;

        public ServiceHome(IRepoUserOrder repository, IRepoStoreLocation repoStoreLocation
            , IRepoUserOrderItem repoUserOrderItem, IRepoStoreItem repoStoreItem
            , IRepoUserInfo repoUserInfo)
        {
            _repoUserOrder = repository;
            _repoStoreLocation = repoStoreLocation;
            _repoUserOrderItem = repoUserOrderItem;
            _repoStoreItem = repoStoreItem;
            _repoUserInfo = repoUserInfo;
        }
        //store all location and orders instantiating them into an object that will display orders by location
        public OrdersByLocationModel ServOrderHistoryByLocation(string location)
        {
            //storing all location names
            var locations = _repoStoreLocation.GetAllStoreLocations().Select(x => x.Location);
            //storing all orders
            var userOrders = _repoUserOrder.GetAllOrders();
            if (!string.IsNullOrEmpty(location))
            {
                //if user selects a location, get all the order of that location
                userOrders = _repoUserOrder.GetAllOrderByLocation(location);
            }
            //this model helps the view to make a order by location filter
            OrdersByLocationModel order = new OrdersByLocationModel
            {
                //displays all the location in a drop down list
                storeLocations = new SelectList(locations.ToList()),
                //display user order in a list
                userOrders = userOrders.ToList()
            };
            return order;
        }

        //converts the list of stored object with information (item id, order id, quantity) into
        //a list of UserOrderItem instance
        public List<UserOrderItem> ServCart(List<UserOrderItemStoredList> orderList)
        {
            //creates a list of UserOrderItem so that informations can be stored
            List<UserOrderItem> actualOrderList = new List<UserOrderItem>();
            //For each to convert id and quantity values into an instance of UserOrderItem and added to the list
            foreach (UserOrderItemStoredList x in orderList)
            {
                actualOrderList.Add(_repoUserOrderItem.CreateUserOrderItem(x.itemId, x.orderId, x.quantity));
            }
            return actualOrderList;
        }

        public List<UserOrderItemStoredList> ServItemPost(int id, int? orderId, int quantity)
        {
            //creates a list to store all the items user order
            List<UserOrderItemStoredList> listOfItemsOrdered = new List<UserOrderItemStoredList>();
            //stores the item user add to cart into the list
            UserOrderItemStoredList storedList = new UserOrderItemStoredList
            {
                itemId = id,
                orderId = orderId,
                quantity = quantity
            };
            //adds the storedList to the created listOfItemsOrdered
            listOfItemsOrdered.Add(storedList);
            return listOfItemsOrdered;
        }
        //creates an instance that contain information of the item id, order id, and quantity
        public UserOrderItemStoredList ServItemPostElse(int id, int? orderId, int quantity)
        {
            UserOrderItemStoredList storedList = new UserOrderItemStoredList
            {
                itemId = id,
                orderId = orderId,
                quantity = quantity
            };
            return storedList;
        }
        //retrieves all store item from repostoreitem and insert it into storeitemmodel as a list
        public StoreItemModel ServItems(int id)
        {
            //stores all store items from a location into the items field
            var items = _repoStoreItem.GetAllStoreItemByLocationId(id);
            StoreItemModel storeItem = new StoreItemModel
            {
                storeItems = items.ToList()
            };
            return storeItem;
        }

        public void ServPurchased(List<UserOrderItemStoredList> orderList)
        {
            //creates a list of UserOrderItem so that informations can be stored
            List<UserOrderItem> actualOrderList = new List<UserOrderItem>();
            //For each to convert id and quantity values into an instance of UserOrderItem and added to the list
            foreach (UserOrderItemStoredList x in orderList)
            {
                actualOrderList.Add(_repoUserOrderItem.CreateUserOrderItem(x.itemId, x.orderId, x.quantity));
            }
            //adds the final order item and its quantity to the database
            _repoUserOrderItem.AddUserOrderItem(actualOrderList);
            //updates the database item inventory
            _repoStoreItem.UpDateInventoryQuantity(actualOrderList);
        }
        //provide double filter to search user by first name, last name or together
        public SearchUserByNameModel ServSearchUserByName(string firstName, string lastName)
        {
            //stores all user information
            var name = _repoUserInfo.GetAllUserInfo();
            SearchUserByNameModel userOrder = new SearchUserByNameModel();
            //if both first name and last name is empty display no name
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                name = name.Where(x => x.fName.Contains("0"));
                userOrder.userInfos = name.ToList();
            }
            //if something is entered into first name search any first name that contain that letter
            else if (!string.IsNullOrEmpty(firstName))
            {
                name = name.Where(x => x.fName.Contains(firstName.ToLower()));
            }
            //if something is entered into last name search any last name that contain that letter
            else if (!string.IsNullOrEmpty(lastName))
            {
                name = name.Where(x => x.lName.Contains(lastName.ToLower()));
            }
            //store the list of names that match into a list
            userOrder.userInfos = name.ToList();
            return userOrder;
        }
    }
}
