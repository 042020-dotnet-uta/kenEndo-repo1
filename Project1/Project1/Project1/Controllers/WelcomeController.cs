using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Project1.Domain;
using Microsoft.AspNetCore.Http;
using Project1.Domain.IRepositories;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Project1.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly ILogger<WelcomeController> _logger;
        private readonly IRepoUserOrder _repository;
        private readonly IRepoUserInfo _repoUserInfo;


        public WelcomeController(ILogger<WelcomeController> logger
            ,IRepoUserOrder repository, IRepoUserInfo repoUserInfo)
        {
            _logger = logger;
            _repository = repository;
            _repoUserInfo = repoUserInfo;
        }
        public IActionResult Index()
        {
            HttpContext.SignOutAsync();
            HttpContext.Session.Remove("UserName");
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
                    //function to add new user to the database
                    _repoUserInfo.AddUserInfo(userinfo);
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
            if (ModelState.IsValid && _repoUserInfo.CheckUserInfoToDb(userInfo) != null)
            {
                var claims = new List<Claim>
                {
                new Claim(ClaimTypes.Name, userInfo.userName)
                };
                var identity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(CookieAuthenticationDefaults
                    .AuthenticationScheme, principal, props).Wait();
                return RedirectToAction("Index","Home");
            }
            return View();
        }
    }
}