using Microsoft.EntityFrameworkCore;
using Project1.Domain;
using Project1.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1.Data.Repositories
{
    public class RepoStoreItem:IRepoStoreItem
    {
        private readonly Project1Context _context;
        public RepoStoreItem(Project1Context context)
        {
            _context = context;
        }

        //update store item inventory quantity after a user order is placed
        public void UpDateInventoryQuantity(List<UserOrderItem> orders)
        {
            foreach (UserOrderItem x in orders)
            {
                var itemInventory = _context.StoreItems.Include(x => x.StoreItemInventory)
                    .First(t => t.StoreItemId == x.StoreItem.StoreItemId);
                itemInventory.StoreItemInventory.itemInventory -= x.OrderQuantity;
                _context.SaveChanges();
            }
        }
        //return all store item from a location id
        public IEnumerable<StoreItem> GetAllStoreItemByLocationId(int locationId)
        {
            return _context.StoreItems.Include(x => x.StoreLocation).Include(x => x.StoreItemInventory)
                .Where(x => x.StoreLocation.StoreLocationId == locationId);
        }
        //return store item by a store item id
        public StoreItem GetStoreItemByStoreItemId(int id)
        {
            return _context.StoreItems.Include(x => x.StoreItemInventory).First(x => x.StoreItemId == id);
        }

    }
}
