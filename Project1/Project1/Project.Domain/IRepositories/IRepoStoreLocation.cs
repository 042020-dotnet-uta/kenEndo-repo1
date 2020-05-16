using System;
using System.Collections.Generic;
using System.Text;

namespace Project1.Domain.IRepositories
{
    public interface IRepoStoreLocation
    {
        //returns all store location
        IEnumerable<StoreLocation> GetAllStoreLocations();
        //returns store location form an order id
        StoreLocation GetStoreLocationFromUserOrder(int? id);
        //returns store location from an item id
        StoreLocation GetStoreLocationFromItem(int id);

    }
}
