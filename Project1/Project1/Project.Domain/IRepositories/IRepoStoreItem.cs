using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain.IRepositories
{
    public interface IRepoStoreItem
    {
        //update store item inventory quantity after a user order is placed
        void UpDateInventoryQuantity(List<UserOrderItem> orders);
        //return all store item from a location id
        IEnumerable<StoreItem> GetAllStoreItemByLocationId(int locationId);
        //return store item by a store item id
        StoreItem GetStoreItemByStoreItemId(int id);

    }
}
