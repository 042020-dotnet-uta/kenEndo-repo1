using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using Project1.Domain;
using Project1.Domain.IRepositories;
using Project1.Models;


namespace Project1.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepoUserOrder _repoUserOrder;
        private readonly IRepoStoreLocation _repoStoreLocation;
        private readonly IRepoUserOrderItem _repoUserOrderItem;
        private readonly IRepoStoreItem _repoStoreItem;
        private readonly IRepoUserInfo _repoUserInfo;

        public HomeController(ILogger<HomeController> logger
            ,IRepoUserOrder repository, IRepoStoreLocation repoStoreLocation
            ,IRepoUserOrderItem repoUserOrderItem, IRepoStoreItem repoStoreItem
            ,IRepoUserInfo repoUserInfo)
        {
            _logger = logger;
            _repoUserOrder = repository;
            _repoStoreLocation = repoStoreLocation;
            _repoUserOrderItem = repoUserOrderItem;
            _repoStoreItem = repoStoreItem;
            _repoUserInfo = repoUserInfo;
        }

        public IActionResult Index()
        {
            ViewData["hello"] = User.FindFirstValue(ClaimTypes.Name);
            return View();
        }
        //Displays all locations
        public IActionResult Location()
        {
            return View(_repoStoreLocation.GetAllStoreLocations());
        }
        public IActionResult Items(int id)
        {
            //stores all store items from a location into the items field
            var items = _repoStoreItem.GetAllStoreItemByLocationId(id);
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
            //checks to make sure user selected order quantity is below the database inventory
            if(quantity > _repoStoreItem.GetStoreItemByStoreItemId(id).StoreItemInventory.itemInventory)
            {
                var orderId = HttpContext.Session.GetInt32("currentOrder");
                ViewData["error"] = "Selected amount is more than the store inventory, please try again";
                //need to make it so that this can direct user to view with error message displayed
                return RedirectToAction("Items", new { id = _repoStoreLocation.GetStoreLocationFromItem(id).StoreLocationId });
            }
            bool x = false;            
            if (HttpContext.Session.GetInt32("currentOrder")==null)
            {
                //adds user order to the database
                var userName = User.FindFirstValue(ClaimTypes.Name);
                _repoUserOrder.AddUserOrder(userName, id);
                ViewData["error"] = "";
                //stores the order id that was just added to the database to session
                HttpContext.Session.SetInt32("currentOrder", _repoUserOrder.GetAllOrders().Last().UserOrderId);
                x = true;
            }
            //var x = HttpContext.Session.GetComplexData<List<UserOrderItem>>("listOfItems");
            if (x)
            {
                ViewData["error"] = "";
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
                return RedirectToAction("Items", new { id=_repoStoreLocation.GetStoreLocationFromUserOrder(orderId).StoreLocationId});
            }
            else
            {
                ViewData["error"] = "";
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
                return RedirectToAction("Items", new { id = _repoStoreLocation.GetStoreLocationFromUserOrder(orderId).StoreLocationId });
            }
        }
        public IActionResult Cart()
        {
            List<UserOrderItem> actualOrderList = new List<UserOrderItem>();
            var orderList = HttpContext.Session.GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
            foreach(UserOrderItemStoredList x in orderList)
            {
                actualOrderList.Add(_repoUserOrderItem.CreateUserOrderItem(x.itemId, x.orderId, x.quantity));
            }
            return View(actualOrderList);
        }
        public IActionResult Purchased()
        {
            List<UserOrderItem> actualOrderList = new List<UserOrderItem>();
            var orderList = HttpContext.Session.GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
            foreach (UserOrderItemStoredList x in orderList)
            {
                actualOrderList.Add(_repoUserOrderItem.CreateUserOrderItem(x.itemId, x.orderId, x.quantity));
            }
            _repoUserOrderItem.AddUserOrderItem(actualOrderList);
            _repoStoreItem.UpDateInventoryQuantity(actualOrderList);
            HttpContext.Session.Remove("listOfItems");
            HttpContext.Session.Remove("currentOrder");
            return View();
        }

        //view to list all orders by location with filter
        public IActionResult OrderHistoryByLocation(string location)
        {
            var locations = _repoStoreLocation.GetAllStoreLocations().Select(x=>x.Location);
            var userOrderss = _repoUserOrder.GetAllOrders();
            if (!string.IsNullOrEmpty(location))
            {
                userOrderss = _repoUserOrder.GetAllOrderByLocation(location);
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
            IEnumerable<UserOrderItem> userOrder = _repoUserOrderItem.GetAllUserOrderItemByUserOrderId(id);
            return View(userOrder);
        }
        
        //searches user by first name, last name or together.
        //view displays a page with no user displayed, once a parameter is entered
        //and filter clicked, the correct filtered names appears
        public IActionResult SearchUserByName(string firstName, string lastName)
        {
            var name = _repoUserInfo.GetAllUserInfo();
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
            var userOrders = _repoUserOrder.GetAllOrderByUserId(id);
            return View(userOrders);
        }
      
    }
}
