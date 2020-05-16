using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Project1.Domain;
using Project1.Domain.IRepositories;

namespace Project1.Data.Repositories
{
    public class RepoUserOrder : IRepoUserOrder
    {
        private readonly Project1Context _context;
        public RepoUserOrder(Project1Context context)
        {
            _context = context;
        }

        //add new user order to database
        public void AddUserOrder(string userName, int itemId)
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

        //return all user orders by a store location
        public IEnumerable<UserOrder> GetAllOrderByLocation(string storeLocation)
        {

                return _context.UserOrders
                .Include(x=>x.UserOrderItems).ThenInclude(x=>x.StoreItem)
                    .Where(x => x.StoreLocation.Location == storeLocation);
        }

        //return all user orders by a user id
        public IEnumerable<UserOrder> GetAllOrderByUserId(int id)
        {
            return _context.UserOrders
                .Include(x => x.UserOrderItems).ThenInclude(x => x.StoreItem)
                .Where(x => x.UserInfo.UserInfoId == id);
        }

        //return all user orders
        public IEnumerable<UserOrder> GetAllOrders()
        {
            return _context.UserOrders;
        }
    }
}
