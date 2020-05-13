using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Domain;
using Project1.Models;

namespace Project1.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly ILogger<WelcomeController> _logger;
        private readonly IRepository _repository;

        public WelcomeController(ILogger<WelcomeController> logger,
            IRepository repository)
        {
            _logger = logger;
            _repository = repository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Registration()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration([Bind("fName,lName,userName,password")]UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _repository.AddNewUser(userinfo);
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException)
                {
                    ViewData["Error"] = "The entered username already exists, please try again.";
                    return View();
                }
                catch (Exception)
                {
                    ViewData["Error"] = "There was an error, please try again.";
                    return View();
                }
            }
            else
            {
                ViewData["Error"] = "There was an error, please try again.";
                return View();
            }
        }
    }
}