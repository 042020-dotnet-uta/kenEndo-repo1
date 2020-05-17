using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Project1.Domain;



namespace Project1.Data
{
    public class Project1Context : DbContext
    {
        public Project1Context() { }
        public Project1Context (DbContextOptions<Project1Context> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\\mssqllocaldb;Database=Project1Context-1;Trusted_Connection=True;MultipleActiveResultSets=true");
            }
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<StoreLocation> StoreLocations { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<StoreItemInventory> StoreItemInventories { get; set; }
        public DbSet<UserOrder> UserOrders { get; set; }
        public DbSet<UserOrderItem> UserOrderItems { get; set; }
    }
}
