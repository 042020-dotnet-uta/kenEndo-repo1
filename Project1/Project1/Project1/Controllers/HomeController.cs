using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Project1.Data;
using Project1.Domain;
using Project1.Models;


namespace Project1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;

        public HomeController(ILogger<HomeController> logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public IActionResult Index()
        {
            ViewData["hello"] = HttpContext.Session.GetString("UserName");
            return View();
        }
        //Displays all locations
        public IActionResult Location()
        {
            return View(_repository.GetAllStoreLocations());
        }
        public IActionResult Items(int id)
        {
            //stores all store items from a location into the items field
            var items = _repository.GetAllStoreItemByLocation(id);
            StoreItemModel storeItem = new StoreItemModel
            {
                storeItems = items.ToList()
            };
            return View(storeItem);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Items(int id, int quantity)
        {
            bool x = false;            
            if (HttpContext.Session.GetInt32("currentOrder")==null)
            {
                //adds user order to the database
                var userName = HttpContext.Session.GetString("UserName");
                _repository.AddNewOrder(userName, id);
                //stores the order id that was just added to the database to session
                HttpContext.Session.SetInt32("currentOrder", _repository.GetAllOrders().Last().UserOrderId);
                x = true;
            }
            //var x = HttpContext.Session.GetComplexData<List<UserOrderItem>>("listOfItems");
            if (x)
            {
                var orderId = HttpContext.Session.GetInt32("currentOrder");
                //creates a list to store all the items user order
                List<UserOrderItemStoredList> listOfItemsOrdered = new List<UserOrderItemStoredList>();
                //stores the item user add to cart into the list
                UserOrderItemStoredList storedList = new UserOrderItemStoredList
                {
                    itemId = id,
                    orderId = orderId,
                    quantity = quantity
                };
                listOfItemsOrdered.Add(storedList);
                HttpContext.Session.SetComplexData("listOfItems", listOfItemsOrdered);
                return RedirectToAction("Items", new { id=_repository.GetOrderLocationFromOrder(orderId)});
            }
            else
            {
                var orderId = HttpContext.Session.GetInt32("currentOrder");
                List<UserOrderItemStoredList> listOfItemsOrdered = HttpContext.Session
                    .GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
                UserOrderItemStoredList storedList = new UserOrderItemStoredList
                {
                    itemId = id,
                    orderId = orderId,
                    quantity = quantity
                };
                listOfItemsOrdered.Add(storedList);
                HttpContext.Session.SetComplexData("listOfItems", listOfItemsOrdered);
                return RedirectToAction("Items", new { id = _repository.GetOrderLocationFromOrder(orderId) });
            }
        }
        public IActionResult Cart()
        {
            List<UserOrderItem> actualOrderList = new List<UserOrderItem>();
            var orderList = HttpContext.Session.GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
            foreach(UserOrderItemStoredList x in orderList)
            {
                actualOrderList.Add(_repository.ReturnNewOrderItem(x.itemId, x.orderId, x.quantity));
            }
            return View(actualOrderList);
        }
        public IActionResult Purchased()
        {
            List<UserOrderItem> actualOrderList = new List<UserOrderItem>();
            var orderList = HttpContext.Session.GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
            foreach (UserOrderItemStoredList x in orderList)
            {
                actualOrderList.Add(_repository.ReturnNewOrderItem(x.itemId, x.orderId, x.quantity));
            }
            _repository.AddOrderItemToDb(actualOrderList);
            _repository.UpDateInventoryQuantity(actualOrderList);
            HttpContext.Session.Remove("listOfItems");
            HttpContext.Session.Remove("currentOrder");
            return View();
        }





        //delete at the end or change 
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        //view to list all orders by location with filter
        public IActionResult OrderHistoryByLocation(string location)
        {
            var locations = _repository.GetAllStoreLocations().Select(x=>x.Location);
            var userOrderss = _repository.GetAllOrders();
            if (!string.IsNullOrEmpty(location))
            {
                userOrderss = _repository.GetAllOrderByLocation(location);
            }
            OrdersByLocationModel order = new OrdersByLocationModel
            {
                storeLocations = new SelectList(locations.ToList()),
                userOrders = userOrderss.ToList()
            };
            return View(order);
        }
        //displays the details of an order for both 'OrderHistoryByLocation' and 
        //'OrderHistoryByUser'. displays item, quantity, time, and username.
        public IActionResult OrderDetails(int id)
        {
            IEnumerable<UserOrderItem> userOrder = _repository.GetOrderItemById(id);
            return View(userOrder);
        }
        
        //searches user by first name, last name or together.
        //view displays a page with no user displayed, once a parameter is entered
        //and filter clicked, the correct filtered names appears
        public IActionResult SearchUserByName(string firstName, string lastName)
        {
            var name = _repository.GetAllUserInfo();
            SearchUserByNameModel userOrder = new SearchUserByNameModel();
            if (string.IsNullOrEmpty(firstName) && string.IsNullOrEmpty(lastName))
            {
                name = name.Where(x => x.fName.Contains("0"));
                userOrder.userInfos = name.ToList();
                return View(userOrder);
            }
            if (!string.IsNullOrEmpty(firstName))
            {
                name = name.Where(x=>x.fName.Contains(firstName.ToLower()));
            }
            if (!string.IsNullOrEmpty(lastName))
            {
                name = name.Where(x => x.lName.Contains(lastName.ToLower()));
            }
            userOrder.userInfos = name.ToList();
            return View(userOrder);
        }
        //once user is selected from 'SearchUserByName' they are directed here
        //to show all their orders
        public IActionResult OrderHistoryByUser(int id)
        {
            var userOrders = _repository.GetAllOrderByUser(id);
            return View(userOrders);
        }
      
    }
}
