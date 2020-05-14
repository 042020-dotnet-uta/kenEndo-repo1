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

        public IActionResult OrderHistoryByLocation(string location)
        {
            var locations = _repository.GetAllStoreLocations().Select(x=>x.Location);
            var userOrderss = _repository.GetAllOrderByLocation(location);
            OrdersByLocation order = new OrdersByLocation
            {
                storeLocations = new SelectList(locations.ToList()),
                userOrders = userOrderss.ToList()
            };
            return View(order);
        }


    }
}
