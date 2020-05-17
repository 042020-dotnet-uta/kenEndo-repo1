using Microsoft.EntityFrameworkCore;
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
            //Act
            using (var db1 = new Project1Context(options))
            {
                //adds user to the table
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
            //Act
            using (var dbww = new Project1Context(options))
            {
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

        [Fact]
        public void HomeController_Index_NoLeagues()
        {
            //Arrange
            
        }



    }
}

