using System;
using System.Collections.Generic;
using System.Text;
using Project1.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Project1.Domain.IRepositories;

namespace Project1.Data.Repositories
{
    public class RepoStoreLocation : IRepoStoreLocation
    {
        private readonly Project1Context _context;
        public RepoStoreLocation(Project1Context context)
        {
            _context = context;
        }
        //return all store location
        public IEnumerable<Domain.StoreLocation> GetAllStoreLocations()
        {
            return _context.StoreLocations;
        }

        //returns store location form an order id
        public Domain.StoreLocation GetStoreLocationFromUserOrder(int? id)
        {
            return _context.UserOrders.Include(x => x.StoreLocation)
                .First(x => x.UserOrderId == id).StoreLocation;
        }

        //returns store location from an item id
        public Domain.StoreLocation GetStoreLocationFromItem(int id)
        {
            return _context.StoreItems.Include(x => x.StoreLocation)
                .First(x => x.StoreItemId == id).StoreLocation;
        }

    }
}
