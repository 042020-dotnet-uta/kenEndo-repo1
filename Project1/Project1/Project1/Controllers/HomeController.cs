using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
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
            return View();
        }
        //Displays all locations
        public IActionResult Location()
        {
            return View(_repository.GetAllStoreLocations());
        }
        public IActionResult Items(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            //stores all store items from a location into the items field
            var items = _repository.GetAllStoreItems(_repository
                .GetAllStoreLocations().First(x => x.StoreLocationId == id));
            if(items == null)
            {
                return NotFound();
            }
            return View(items);
        }
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
