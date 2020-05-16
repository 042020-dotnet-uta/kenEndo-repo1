using Microsoft.EntityFrameworkCore;
using Project1.Domain;
using Project1.Domain.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project1.Data.Repositories
{
    public class RepoUserOrderItem: IRepoUserOrderItem
    {
        private readonly Project1Context _context;
        public RepoUserOrderItem(Project1Context context)
        {
            _context = context;
        }
        //adds list of user order item to database with addRange
        public void AddUserOrderItem(List<UserOrderItem> lists)
        {
            _context.AddRange(lists);
            _context.SaveChanges();
        }

        //return all user order item by user order id
        public IEnumerable<UserOrderItem> GetAllUserOrderItemByUserOrderId(int id)
        {
            return _context.UserOrderItems.Include(x => x.UserOrder).ThenInclude(x => x.UserInfo)
                .Include(x => x.StoreItem).Where(x => x.UserOrder.UserOrderId == id);
        }

        //return an instance of an user order item to the cart so that it can be stored into a list in session
        public UserOrderItem CreateUserOrderItem(int itemId, int? orderId, int quantity)
        {
            var storeItem = _context.StoreItems.First(x => x.StoreItemId == itemId);
            var order = _context.UserOrders.First(x => x.UserOrderId == orderId);
            UserOrderItem newOrderItem = new UserOrderItem
            {
                StoreItem = storeItem,
                UserOrder = order,
                OrderQuantity = quantity
            };
            return newOrderItem;
        }

    }
}
