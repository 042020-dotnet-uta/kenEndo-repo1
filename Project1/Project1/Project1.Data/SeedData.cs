using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Project1.Domain;
using System.Linq;

namespace Project1.Data
{
    public class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using(var context = new Project1Context
                (serviceProvider.GetRequiredService<DbContextOptions<Project1Context>>())){
                if (context.StoreLocations.Any() && context.StoreItems.Any())
                {
                    return;
                }
                if (context.StoreItems.Any() && !context.StoreLocations.Any())
                {
                    locationHelper(context);
                }
                if (!context.StoreItems.Any() && context.StoreLocations.Any())
                {
                    storeInventoryHelper(context);
                    storeItemHelper(context);
                }
                else
                {
                    locationHelper(context);
                    storeInventoryHelper(context);
                    storeItemHelper(context);
                }

            }
        }
        static void locationHelper(Project1Context context)
        {
            context.StoreLocations.AddRange(
                new StoreLocation
                {
                    Location = "Houston"
                },
                 new StoreLocation
                 {
                     Location = "Dallas"
                 },
                new StoreLocation
                {
                    Location = "Austin"
                },
                new StoreLocation
                {
                    Location = "San Antonio"
                },
                new StoreLocation
                {
                    Location = "Midland"
                }
                );
            context.SaveChanges();
        }
        static void storeInventoryHelper(Project1Context context)
        {
            context.StoreItemInventories.AddRange(
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                },
                new StoreItemInventory
                {
                    itemInventory = 10
                }
                );
            context.SaveChanges();
        }

        static void storeItemHelper(Project1Context context)
        {
            context.StoreItems.AddRange(
                new StoreItem
                {
                    itemName = "German Shepherd",
                    itemPrice = 600,
                    StoreLocation = context.StoreLocations.First(x=>x.StoreLocationId==1),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x=>x.StoreItemInventoryId==1)
                },
                new StoreItem
                {
                    itemName = "Siberian Husky",
                    itemPrice = 700,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 1),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 2)
                },
                new StoreItem
                {
                    itemName = "Border Collie",
                    itemPrice = 850,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 1),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 3)
                },
                new StoreItem
                {
                    itemName = "Siamese Cat",
                    itemPrice = 10000,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 2),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 4)
                },
                new StoreItem
                {
                    itemName = "Persian Cat",
                    itemPrice = 1200,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 2),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 5)
                },
                new StoreItem
                {
                    itemName = "Himalayan Cat",
                    itemPrice = 1250,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 2),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 6)
                },
                new StoreItem
                {
                    itemName = "Peach Cream Gecko",
                    itemPrice = 300,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 3),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 7)
                },
                new StoreItem
                {
                    itemName = "Spotted Viper",
                    itemPrice = 750,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 3),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 8)
                },
                new StoreItem
                {
                    itemName = "Red Tarantula",
                    itemPrice = 150,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 3),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 9)
                },
                new StoreItem
                {
                    itemName = "Zebra Angel Fish",
                    itemPrice = 40,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 4),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 10)
                },
                new StoreItem
                {
                    itemName = "Elephant Nose Fish",
                    itemPrice = 30,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 4),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 11)
                },
                new StoreItem
                {
                    itemName = "Royal Purple Discus",
                    itemPrice = 120,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 4),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 12)
                },
                new StoreItem
                {
                    itemName = "Colombian Boa",
                    itemPrice = 275,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 5),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 13)
                },
                new StoreItem
                {
                    itemName = "King Snake",
                    itemPrice = 175,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 5),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 14)
                },
                new StoreItem
                {
                    itemName = "Blood Pythond",
                    itemPrice = 500,
                    StoreLocation = context.StoreLocations.First(x => x.StoreLocationId == 5),
                    StoreItemInventory = context.StoreItemInventories
                    .First(x => x.StoreItemInventoryId == 15)
                }
                );
            context.SaveChanges();
        }



    }
}
