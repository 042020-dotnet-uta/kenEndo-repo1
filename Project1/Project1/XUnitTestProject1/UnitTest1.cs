using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update;
using Moq;
using Project1.Controllers;
using Project1.Data;
using Project1.Data.Repositories;
using Project1.Domain;
using Project1.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;


namespace XUnitTestProject1
{
    public class UnitTest1
    {
        /// <summary>
        /// Adds location to database.
        /// tests if it is stored correctly and retrievable
        /// </summary>
        [Fact]
        public void CheckAddsLocationToDbTestPersist()
        {
            //Test1
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test1")
                .Options;
            //Act
            using (var db = new Project1Context(options))
            {
                // adds location to the table
                db.StoreLocations.Add(new StoreLocation { Location = "Houston" });
                db.StoreLocations.Add(new StoreLocation { Location = "Dallas" });
                db.SaveChanges();
            }
            //Assert
            using (var db = new Project1Context(options))
            {
                //counts the total locations in the location table
                Assert.Equal(2, db.StoreLocations.Count());
                //select the first location in table
                Assert.Equal("Houston", db.StoreLocations.First(x => x.StoreLocationId == 1).Location);
            }
        }
        /// <summary>
        /// Adds user to the database.
        /// Test if the user is stored properly and retrievable
        /// </summary>
        [Fact]
        public void CheckAddsUserToDbTestPersist()
        {
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test2")
                .Options;
            UserInfo newUser = new UserInfo()
            {
                fName = "David",
                lName = "Leblanc",
                userName = "Davidl",
                password = "abc123"
            };
            UserInfo newUser1 = new UserInfo()
            {
                fName = "James",
                lName = "Jones",
                userName = "jamesj",
                password = "abc123"
            };
            //Act
            using (var db1 = new Project1Context(options))
            {
                //adds user to the table
                db1.Add(newUser);
                db1.Add(newUser1);
                db1.SaveChanges();
            }
            //Assert
            using (var db1 = new Project1Context(options))
            {
                //check if first username is correct
                Assert.Equal("David", db1.UserInfos.First().fName);
                //cecks the total count of users in user table
                Assert.Equal(2, db1.UserInfos.Count());
            }
        }
        /// <summary>
        /// test if the relational aspect(fk/pk) of database is working
        /// </summary>
        [Fact]
        public void CheckingRelationBetweenLocationAndItem()
        {
            //Test1
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test3")
                .Options;
            StoreLocation storeLocation = new StoreLocation()
            {
                Location = "Houston"
            };
            StoreItem storeItem = new StoreItem()
            {
                itemName = "Chicken",
                itemPrice = 5
            };
            StoreItemInventory storeItem1 = new StoreItemInventory()
            {
                itemInventory = 10
            };
            //Act
            using (var dbww = new Project1Context(options))
            {
                List<StoreItem> list = new List<StoreItem>();
                list.Add(storeItem);
                storeItem.StoreLocation = storeLocation;
                storeItem1.StoreItem = list;
                dbww.Add(storeLocation);
                dbww.Add(storeItem);
                dbww.Add(storeItem1);
                dbww.SaveChanges();
            }
            //Assert
            using (var dbww = new Project1Context(options))
            {
                int input1 = 1;
                //retrieves item name from item #
                Assert.Equal(10, dbww.StoreItems.Include(x => x.StoreItemInventory)
                    .First(x => x.StoreItemId == 1).StoreItemInventory.itemInventory);
                //retrieve location from location #
                Assert.Equal("Houston", dbww.StoreItems.Include(x => x.StoreLocation)
                    .First(x => x.StoreItemId == input1).StoreLocation.Location);
            }
        }
        /// <summary>
        /// checks to make sure user does not register a new account with an already
        /// existing username.
        /// </summary>
        [Fact]
        public void CheckRejectionOfDuplicateUsernameDuringRegistration()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test4")
                .Options;
            UserInfo userInfo = new UserInfo()
            {
                fName = "David",
                lName = "Leblanc",
                userName = "Davidl",
                password = "abc123"
            };
            UserInfo userInfo1 = new UserInfo()
            {
                fName = "Jack",
                lName = "Daniels",
                userName = "Davidl",
                password = "abc123"
            };
            string error = "";
            //Act
            using (var db1 = new Project1Context(options))
            {
                userInfo.fName = userInfo.fName.ToLower();
                userInfo.lName = userInfo.lName.ToLower();
                db1.Add(userInfo);
                db1.SaveChanges();
                if (db1.UserInfos.Any(x => x.userName == userInfo1.userName))
                {
                    //throws an error if there is a matching username in the database
                    error = "There was an error";
                }
                else
                {
                    db1.Add(userInfo1);
                    db1.SaveChanges();
                }
            }
            //Assert
            using (var db1 = new Project1Context(options))
            {
                //correctly blocks user from adding a new user info into the database
                Assert.Equal("There was an error", error);
            }

        }
        /// <summary>
        /// Checks if user login is working funcionally. if user name and password entered does not match 
        /// a row it should throw an exception, if it does match, allow the user to login
        /// </summary>
        [Fact]
        public void CheckUserInfoOnLogin()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test5")
                .Options;
            UserInfo userInfo = new UserInfo()
            {
                fName = "David",
                lName = "Leblanc",
                userName = "Davidl",
                password = "abc123"
            };
            UserInfo userInfo1 = new UserInfo()
            {
                fName = "Jack",
                lName = "Daniels",
                userName = "JackD",
                password = "abc123"
            };
            UserInfo correct;
            UserInfo wrong;
            //Act
            using (var db1 = new Project1Context(options))
            {
                userInfo.fName = userInfo.fName.ToLower();
                userInfo.lName = userInfo.lName.ToLower();
                db1.Add(userInfo);
                db1.SaveChanges();
                //checking the correct credential that already exist in the database
                correct = db1.UserInfos.Where(x => x.userName
                    .Equals(userInfo.userName) && x.password.Equals(userInfo.password)).FirstOrDefault();
                //checking the incorrect credential that finds no match in database, should return null
                wrong = db1.UserInfos.Where(x => x.userName
                    .Equals(userInfo1.userName) && x.password.Equals(userInfo1.password)).FirstOrDefault();
            }
            //Assert
            using (var db1 = new Project1Context(options))
            {
                //correctly allow user to login by finding matching username and password in the database
                Assert.Equal(userInfo, correct);
                //correctly returns null by not finding a match in the database
                Assert.Null(wrong);
            }
        }
        /// <summary>
        /// retrieve the store location from the user order placed
        /// </summary>
        [Fact]
        public void TestGetStoreLocationFromUserOrder()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test6")
                .Options;
            UserInfo userInfo = new UserInfo()
            {
                fName = "David",
                lName = "Leblanc",
                userName = "Davidl",
                password = "abc123"
            };
            StoreLocation storeLocation = new StoreLocation()
            {
                Location = "Houston"
            };
            StoreItem storeItem = new StoreItem()
            {
                itemName = "Chicken",
                itemPrice = 5
            };
            UserOrder userOrder = new UserOrder()
            {
                StoreLocation = storeLocation,
                UserInfo = userInfo,
                timeStamp = DateTime.Now
            };
            string checkLocation;
            int userOrderId = 1;
            //Act
            using (var db3 = new Project1Context(options))
            {
                storeItem.StoreLocation = storeLocation;
                db3.AddRange(storeLocation,storeItem,userOrder);
                db3.SaveChanges();
                //retrieving the locational information from user order
                checkLocation = db3.UserOrders.Include(x => x.StoreLocation)
                .First(x => x.UserOrderId == userOrderId).StoreLocation.Location;
            }
            //Assert
            using (var db3 = new Project1Context(options))
            {
                //if relationship stands, the checkLocation should return Houston
                Assert.Equal("Houston", checkLocation);
            }
        }
        /// <summary>
        /// retrieve store location from an item id
        /// </summary>
        [Fact]
        public void TestGetStoreLocationFromItem()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test7")
                .Options;
            StoreLocation storeLocation = new StoreLocation()
            {
                Location = "Houston"
            };
            StoreItem storeItem = new StoreItem()
            {
                itemName = "Chicken",
                itemPrice = 5
            };
            string checkLocation;
            int userOrderId = 1;
            //Act
            using (var db3 = new Project1Context(options))
            {
                storeItem.StoreLocation = storeLocation;
                db3.AddRange(storeLocation, storeItem);
                db3.SaveChanges();
                //retrieving the locational information from item
                checkLocation = db3.StoreItems.Include(x => x.StoreLocation)
                .First(x => x.StoreItemId == userOrderId).StoreLocation.Location;
            }
            //Assert
            using (var db3 = new Project1Context(options))
            {
                //if relationship stands, the checkLocation should return Houston
                Assert.Equal("Houston", checkLocation);
            }
        }
        /// <summary>
        /// test retrieving all store item by specific location. When provided a store
        /// location Id, the query should return a list of itmes
        /// </summary>
        [Fact]
        public void TestGetAllStoreItemByLocationId()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test8")
                .Options;
            StoreLocation storeLocation = new StoreLocation()
            {
                Location = "Houston"
            };
            List<StoreItem> storeItem = new List<StoreItem>()
            {
                new StoreItem
                {
                    itemName = "Chicken",
                    itemPrice = 5
                },
                new StoreItem
                {
                    itemName = "Pig",
                    itemPrice = 10
                }
            };
            int items;
            int locationId = 1;
            //Act
            using (var db3 = new Project1Context(options))
            {
                storeItem[0].StoreLocation = storeLocation;
                storeItem[1].StoreLocation = storeLocation;
                db3.AddRange(storeLocation, storeItem[0],storeItem[1]);
                db3.SaveChanges();
                //access the db for item at specific location and gets the total number of them.
                items = db3.StoreItems.Include(x => x.StoreLocation)
                    .Where(x => x.StoreLocation.StoreLocationId == locationId).Count();
            }
            //Assert
            using (var db3 = new Project1Context(options))
            {
                //it should return the number of items available in the location
                Assert.Equal(2, items);
            }
        }
        /// <summary>
        /// Test to see if user ordered quantity will update the item inventory in the store
        /// </summary>
        [Fact]
        public void TestUpdateInventoryQuantity()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test9")
                .Options;
            UserInfo userInfo = new UserInfo()
            {
                fName = "David",
                lName = "Leblanc",
                userName = "Davidl",
                password = "abc123"
            };
            StoreLocation storeLocation = new StoreLocation()
            {
                Location = "Houston"
            };
            StoreItem storeItem = new StoreItem()
            {
                itemName = "Chicken",
                itemPrice = 5
            };
            StoreItemInventory storeItemInventory = new StoreItemInventory()
            {
                itemInventory = 10
            };
            UserOrder userOrder = new UserOrder()
            {
                StoreLocation = storeLocation,
                UserInfo = userInfo,
                timeStamp = DateTime.Now
            };
            UserOrderItem userOrderItem = new UserOrderItem()
            {
                StoreItem = storeItem,
                UserOrder = userOrder,
                OrderQuantity = 2
            };
            List<UserOrderItem> listItems = new List<UserOrderItem>()
            {
                userOrderItem
            };
            storeItem.StoreItemInventory = storeItemInventory;
            int updatedInventory;
            //Act
            using (var db3 = new Project1Context(options))
            {
                storeItem.StoreLocation = storeLocation;
                db3.AddRange(storeLocation, storeItem, userOrder, userOrderItem, storeItemInventory);
                db3.SaveChanges();
                //goes throught the list of item user selected for an order.
                //access the store inventory and subtract user order quantity from it.
                foreach (UserOrderItem x in listItems)
                {
                    var itemInventory = db3.StoreItems.Include(a => a.StoreItemInventory)
                        .First(t => t.StoreItemId == 1);
                    itemInventory.StoreItemInventory.itemInventory -= x.OrderQuantity;
                }
                db3.SaveChanges();
                //retrieves the updated store item inventory for chicken
                updatedInventory = db3.StoreItems.Include(x => x.StoreItemInventory).First(x => x.StoreItemId == 1)
                    .StoreItemInventory.itemInventory;
            }
            //Assert
            using (var db3 = new Project1Context(options))
            {
                //if function works properly the updated inventory should be 8 as it was 10 before and user ordered 2
                Assert.Equal(8, updatedInventory);
            }
        }
        /// <summary>
        /// adding user order to the database with parameter of string username and int itemid
        /// </summary>
        [Fact]
        public void TestAddUserOrderToDb()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<Project1Context>()
                .UseInMemoryDatabase(databaseName: "Test10")
                .Options;
            UserInfo userInfo = new UserInfo()
            {
                fName = "David",
                lName = "Leblanc",
                userName = "Davidl",
                password = "abc123"
            };
            StoreLocation storeLocation = new StoreLocation()
            {
                Location = "Houston"
            };
            StoreItem storeItem = new StoreItem()
            {
                itemName = "Chicken",
                itemPrice = 5
            };
            storeItem.StoreLocation = storeLocation;
            int userOrderId;
            //Act
            using (var db3 = new Project1Context(options))
            {
                db3.AddRange(userInfo, storeLocation,storeItem);
                db3.SaveChanges();
                //retrieves user information from username
                var aUserInfo = db3.UserInfos.First(x => x.userName == userInfo.userName);
                //retrieves store location from store item id
                var location = db3.StoreLocations
                .First(x => x.StoreLocationId == db3.StoreItems.Include(x => x.StoreLocation)
                .First(x => x.StoreItemId == 1).StoreLocation.StoreLocationId);
                var newOrder = new UserOrder
                {
                    UserInfo = aUserInfo,
                    StoreLocation = location,
                    timeStamp = DateTime.Now
                };
                db3.Add(newOrder);
                db3.SaveChanges();
                userOrderId = db3.UserOrders.First().UserOrderId;
            }
            //Assert
            using (var db3 = new Project1Context(options))
            {
                //checks if an order exists within the database
                Assert.Equal(1, userOrderId);
            }
        }


    }
}

