using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Project1.Domain;


namespace Project1.Data
{
    public class Project1Context : DbContext
    {
        public Project1Context (DbContextOptions<Project1Context> options) : base(options) { }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<StoreItemInventory> StoreItemInventories { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<UserOrderItem> UserOrderItems { get; set; }
    }
}
