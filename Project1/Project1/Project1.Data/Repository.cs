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
        public void AddNewOrder(UserInfo userInfo, Domain.StoreLocation storeLocation, StoreItem storeItem, int orderQuantity)
        {
            var newOrder = new UserOrder
            {
                UserInfo = userInfo,
                StoreLocation = storeLocation,
                timeStamp = DateTime.Now
            };
            var orderItem = new UserOrderItem
            {
                UserOrder = newOrder,
                StoreItem = storeItem
            };
            var orderQuantities = new UserOrderQuantity
            {
                UserOrder = newOrder,
                orderQuantity = orderQuantity
            };
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

                return _context.UserOrders.Include(x=>x.UserOrderQuantity)
                .Include(x=>x.UserOrderItems).ThenInclude(x=>x.StoreItem)
                    .Where(x => x.StoreLocation.Location == storeLocation);
            

        }
        //displays all order placed by a user
        public IEnumerable<UserOrder> GetAllOrderByUser(UserInfo userInfo)
        {
            return _context.UserOrders
                .Where(x => x.UserInfo == userInfo).ToList();
        }
        //displays all item at a selected location
        public IEnumerable<StoreItem> GetAllStoreItems(Domain.StoreLocation storeLocation)
        {
            return _context.StoreItems.Include(x => x.StoreLocation)
                .Where(x => x.StoreLocation == storeLocation).ToList();
        }
        //displays all store locations
        public IEnumerable<Domain.StoreLocation> GetAllStoreLocations()
        {
            return _context.StoreLocations;
        }
    }
}
