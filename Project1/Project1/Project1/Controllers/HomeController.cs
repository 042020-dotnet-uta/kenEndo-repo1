using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Domain.IRepositories;
using Project1.Models;
using Project1.Services;


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
        private readonly IServiceHome _serviceHome;

        public HomeController(ILogger<HomeController> logger
            ,IRepoUserOrder repository, IRepoStoreLocation repoStoreLocation
            ,IRepoUserOrderItem repoUserOrderItem, IRepoStoreItem repoStoreItem
            ,IServiceHome serviceHome)
        {
            _logger = logger;
            _repoUserOrder = repository;
            _repoStoreLocation = repoStoreLocation;
            _repoUserOrderItem = repoUserOrderItem;
            _repoStoreItem = repoStoreItem;
            _serviceHome = serviceHome;
        }
        public IActionResult Index()
        {
            return View();
        }
        //Displays all locations using repoStoreLocation
        public IActionResult Location()
        {
            return View(_repoStoreLocation.GetAllStoreLocations());
        }
        //display all the item within a selected location
        public IActionResult Items(int id)
        {
            HttpContext.Session.SetInt32("selectedLocationId", id);
            if (ModelState.IsValid)
            {
                _logger.LogError(string.Format("Location Id selected to view items: {0}",id));
                //retrieve an instance of StoreItemModel from serviceHome to display list of items in view.
                var storeItem = _serviceHome.ServItems(id);
                return View(storeItem);
            }
            _logger.LogError("ModelState invalid");
            return RedirectToAction("Location");
        }
        //when user add an item to the cart the post is directed to this action. This action checks if 
        //it is the first item or additional item to the order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Items(int id, int quantity)
        {
            if (ModelState.IsValid)
            {
                _logger.LogDebug(string.Format("User added item id: {0} and quantity: {1} to order", id, quantity));
                //checks to make sure user selected order quantity is below the database inventory
                if (quantity > _repoStoreItem.GetStoreItemByStoreItemId(id).StoreItemInventory.itemInventory)
                {
                    ViewData["error"] = "Selected amount is more than the store inventory, please try again";
                    //need to make it so that this can direct user to view with error message displayed
                    return RedirectToAction("Items", new { id = _repoStoreLocation.GetStoreLocationFromItem(id).StoreLocationId });
                }
                bool x = false;
                if (HttpContext.Session.GetInt32("currentOrder") == null)
                {
                    //adds user order to the database
                    var userName = User.FindFirstValue(ClaimTypes.Name);
                    _repoUserOrder.AddUserOrder(userName, id);
                    //stores the order id that was just added to the database to session
                    HttpContext.Session.SetInt32("currentOrder", _repoUserOrder.GetAllOrders().Last().UserOrderId);
                    x = true;
                }
                var orderId = HttpContext.Session.GetInt32("currentOrder");
                if (x)
                {
                    //creates an instance that contain information of the item id, order id, and quantity
                    var listOfItemsOrdered = _serviceHome.ServItemPost(id, orderId, quantity);
                    //listOfItemsOrdered is saved into session as cookie until user decides to purchase
                    HttpContext.Session.SetComplexData("listOfItems", listOfItemsOrdered);
                }
                else
                {
                    //creates a list that includes the instance of UserOrderItemStoredList(contains information of item ordered)
                    //this list are made so that the new ordered item adds onto the session that holds the items ordered.
                    List<UserOrderItemStoredList> listOfItemsOrdered = HttpContext.Session
                        .GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
                    //creates an instance that contain information of the item id, order id, and quantity
                    var storedList = _serviceHome.ServItemPostElse(id, orderId, quantity);
                    //add additionally ordered item into the ordered item list
                    listOfItemsOrdered.Add(storedList);
                    //stored into session as a cookie until user is ready to purchase
                    HttpContext.Session.SetComplexData("listOfItems", listOfItemsOrdered);
                }
                //erases the error message
                ViewData["error"] = "";
                //direct user back to the store item view
                return RedirectToAction("Items", new { id = _repoStoreLocation.GetStoreLocationFromUserOrder(orderId).StoreLocationId });
            }
            return RedirectToAction("Items",new { id = HttpContext.Session.GetInt32("selectedLocationId") });
        }
        //displays user selected item into the order view
        public IActionResult Cart()
        {
            //saves the list of UserOrderItemStoredList into orderList
            var orderList = HttpContext.Session.GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
            if (orderList != null)
            {
                //returns the list of UserOrderItem so that it can be displayed on view
                var actualOrderList = _serviceHome.ServCart(orderList);
                return View(actualOrderList);
            }
            else
            {
                _logger.LogError("empty cart");
                return RedirectToAction("Items",new { id = HttpContext.Session.GetInt32("selectedLocationId") });
            }
        }
        //action is triggered when user press the purchase button. Adds the final ordered item to the database
        //and updates the item inventory of the database. Erases stored cookie for user order functionality
        public IActionResult Purchased()
        {
            //saves the list of UserOrderItemStoredList into orderList
            var orderList = HttpContext.Session.GetComplexData<List<UserOrderItemStoredList>>("listOfItems");
            //Stores the order item into the database
            _serviceHome.ServPurchased(orderList);
            //deletes the stored cookies for item order
            HttpContext.Session.Remove("listOfItems");
            HttpContext.Session.Remove("currentOrder");
            HttpContext.Session.Remove("selectedLocationId");
            return View();
        }
        //Action to list all orders by location with filter
        public IActionResult OrderHistoryByLocation(string location)
        {
            if (ModelState.IsValid)
            {
                _logger.LogDebug(string.Format("Selected the location: {0}, to view orders there", location));
                var order = _serviceHome.ServOrderHistoryByLocation(location);
                return View(order);
            }
            _logger.LogError("ModelState invalid");
            return View();
        }
        //displays the details of an order for both 'OrderHistoryByLocation' and 
        //'OrderHistoryByUser'. displays item, quantity, time, and username.
        public IActionResult OrderDetails(int id)
        {
            if (ModelState.IsValid)
            {
                _logger.LogDebug(string.Format("Selected Order Id: {0}, for more detail", id));
                var userOrder = _repoUserOrderItem.GetAllUserOrderItemByUserOrderId(id);
                return View(userOrder);
            }
            _logger.LogError("ModelState invalid");
            return RedirectToAction("Index");
        }      
        //searches user by first name, last name or together.
        //view displays a page with no user displayed, once a parameter is entered
        //and filter clicked, the correct filtered names appears
        public IActionResult SearchUserByName(string firstName, string lastName)
        {
            if (ModelState.IsValid)
            {
                _logger.LogDebug(string.Format("Searching user order by first name: {0}, last name: {1}", firstName, lastName));
                var userOrder = _serviceHome.ServSearchUserByName(firstName, lastName);
                return View(userOrder);
            }
            _logger.LogError("ModelState invalid");
            return View();
        }
        //once user is selected from 'SearchUserByName' they are directed here
        //to show all their orders
        public IActionResult OrderHistoryByUser(int id)
        {
            if (ModelState.IsValid)
            {
                _logger.LogDebug(string.Format("Selected User Id: {0} to view his/her order", id));
                var userOrders = _repoUserOrder.GetAllOrderByUserId(id);
                return View(userOrders);
            }
            _logger.LogError("modelState invalid");
            return RedirectToAction("SearchUserByName");
        }    
    }
}
