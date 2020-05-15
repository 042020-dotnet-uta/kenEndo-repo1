using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Domain;
using Project1.Models;
using System.Web;
using Microsoft.AspNetCore.Http;

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
            ViewData["Test"] = HttpContext.Session.GetString("UserName");
            return View();
        }
        /// <summary>
        /// Action for user registration, includes client and server input validation
        /// </summary>
        public IActionResult Registration()
        {
            //directs user to the registration view
            return View();
        }
        /// <summary>
        ///  When user registers it direct them to this registration post action
        ///  This action adds the new user to the database and returns exception if
        ///  there is a duplicate username available
        /// </summary>
        /// <param name="userinfo"></param>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Registration([Bind("fName,lName,userName,password")]UserInfo userinfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    //converts username to lowercase for name search purpose
                    userinfo.fName = userinfo.fName.ToLower();
                    userinfo.lName = userinfo.lName.ToLower();
                    //function to add new user to the database
                    _repository.AddNewUser(userinfo);
                    //redirects user to the login page-----------------------------
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException)
                {
                    //if there were duplicate in the database an exception is thrown and shows
                    //this message on view
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
                //any issue with modelstate, a message will be thrown and user needs to repeat
                ViewData["Error"] = "There was an error, please try again.";
                return View();
            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("userName,password")]UserInfo userInfo)
        {
            var x = _repository.CheckUser(userInfo);
            if (ModelState.IsValid && x!=null)
            {
                HttpContext.Session.SetString("UserName", x.userName);
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}