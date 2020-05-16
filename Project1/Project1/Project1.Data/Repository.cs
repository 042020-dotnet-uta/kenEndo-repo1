using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project1.Domain;


namespace Project1.Data
{
    public class Repository : IRepository
    {
        private readonly Project1Context _context;
        public Repository(Project1Context context)
        {
            _context = context;
        }
        //adds new user order - not done yet - stuck
        public void AddNewOrder(string userName, int itemId)
        {
            var userInfo = _context.UserInfos.First(x => x.userName == userName);
            var location = _context.StoreLocations
            .First(x => x.StoreLocationId == _context.StoreItems.Include(x=>x.StoreLocation)
            .First(x => x.StoreItemId == itemId).StoreLocation.StoreLocationId);
            var newOrder = new UserOrder
            {
                UserInfo = userInfo,
                StoreLocation = location,
                timeStamp = DateTime.Now
            };
            _context.Add(newOrder);
            _context.SaveChanges();
        }

        //returns an instance of an item added to the cart so that it can be stored into a list in session
        public UserOrderItem ReturnNewOrderItem(int itemId, int? orderId, int quantity)
        {
            var storeItem = _context.StoreItems.First(x => x.StoreItemId == itemId);
            var order = _context.UserOrders.First(x => x.UserOrderId == orderId);
            UserOrderItem newOrderItem = new UserOrderItem
            {
                StoreItem = storeItem,
                UserOrder = order,
                OrderQuantity = quantity
            };
            return newOrderItem;
        }

        //Adds new user to the database
        public void AddNewUser(UserInfo userInfo)
        {            
            if(_context.UserInfos.Any(x => x.userName == userInfo.userName))
            {
                throw new InvalidOperationException("Username already exists");
            }
            _context.Add(userInfo);
            _context.SaveChanges();
        }
        //checks if user exists in the database for user login
        public UserInfo CheckUser(UserInfo userInfo)
        {
            var obj = _context.UserInfos.Where(x => x.userName
            .Equals(userInfo.userName) && x.password.Equals(userInfo.password)).FirstOrDefault();
            return obj;
        }
        //displays all order placed from a location
        public IEnumerable<UserOrder> GetAllOrderByLocation(string storeLocation)
        {

                return _context.UserOrders
                .Include(x=>x.UserOrderItems).ThenInclude(x=>x.StoreItem)
                    .Where(x => x.StoreLocation.Location == storeLocation);
            

        }
        //displays all order placed by a user
        public IEnumerable<UserOrder> GetAllOrderByUser(int id)
        {
            return _context.UserOrders
                .Include(x => x.UserOrderItems).ThenInclude(x => x.StoreItem)
                .Where(x => x.UserInfo.UserInfoId == id);
        }

        public IEnumerable<UserOrder> GetAllOrders()
        {
            return _context.UserOrders;
        }

        public IEnumerable<StoreItem> GetAllStoreItem()
        {
            return _context.StoreItems.Include(x=>x.StoreLocation);
        }

        //displays all item at a selected location
        public IEnumerable<StoreItem> GetAllStoreItemByLocation(int locationId)
        {
            return _context.StoreItems.Include(x => x.StoreLocation).Include(x=>x.StoreItemInventory)
                .Where(x => x.StoreLocation.StoreLocationId == locationId);
        }
        //displays all store locations
        public IEnumerable<Domain.StoreLocation> GetAllStoreLocations()
        {
            return _context.StoreLocations;
        }

        public IEnumerable<UserInfo> GetAllUserInfo()
        {
            return _context.UserInfos;
        }

        public IEnumerable<UserOrder> GetOrderByOrderId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserOrderItem> GetOrderItemById(int id)
        {
            return _context.UserOrderItems.Include(x => x.UserOrder).ThenInclude(x=>x.UserInfo)
                .Include(x=>x.StoreItem).Where(x => x.UserOrder.UserOrderId == id);
        }

        public IEnumerable<UserInfo> GetUserInfoByFirstName(string fName)
        {
            IEnumerable<UserInfo> user;
            try
            {
                user = _context.UserInfos.Where(x => x.fName.Contains(fName));
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<UserInfo> GetUserInfoByLastName(string lName)
        {
            IEnumerable<UserInfo> user;
            try
            {
                user = _context.UserInfos.Where(x => x.lName.Contains(lName));
                return user;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public IEnumerable<UserOrderItem> GetUserOrderItems(UserOrder userOrder)
        {
            throw new NotImplementedException();
        }

        public int GetOrderLocationFromOrder(int? id)
        {
            return _context.UserOrders.Include(x => x.StoreLocation)
                .First(x => x.UserOrderId == id).StoreLocation.StoreLocationId;
        }

        public void AddOrderItemToDb(List<UserOrderItem> lists)
        {
            _context.AddRange(lists);
            _context.SaveChanges();
        }

        public void UpDateInventoryQuantity(List<UserOrderItem> orders)
        {
            foreach(UserOrderItem x in orders)
            {
                var itemInventory = _context.StoreItems.Include(x => x.StoreItemInventory)
                    .First(t => t.StoreItemId == x.StoreItem.StoreItemId);
                itemInventory.StoreItemInventory.itemInventory-=x.OrderQuantity;
                _context.SaveChanges();
            }
        }
    }
}
