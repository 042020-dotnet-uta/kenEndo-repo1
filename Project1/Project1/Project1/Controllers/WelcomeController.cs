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
using Newtonsoft.Json;

namespace Project1.Controllers
{
    public class WelcomeController : Controller
    {
        private readonly ILogger<WelcomeController> _logger;
        private readonly IRepoUserInfo _repoUserInfo;


        public WelcomeController(ILogger<WelcomeController> logger
            , IRepoUserInfo repoUserInfo)
        {
            _logger = logger;
            _repoUserInfo = repoUserInfo;
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Welcome");
        }
        public IActionResult Index()
        {
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
        ///  When user click register in welcome index, the action directs them to this registration post action
        ///  This action adds the new user to the database and returns exception if
        ///  there is a duplicate username available
        /// </summary>
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
                    //logging successful registration information
                    _logger.LogInformation(string.Format("Adding new user to the database: {0}", JsonConvert.SerializeObject(userinfo)));
                    //redirects user to the login page-----------------------------
                    return RedirectToAction(nameof(Index));
                }
                catch (InvalidOperationException ioe)
                {
                    _logger.LogError(ioe.Message);
                    //if there were duplicate in the database an exception is thrown and shows
                    //this message on view
                    ViewData["Error"] = "The entered username already exists, please try again.";
                    return View();
                }
                catch (Exception e)
                {
                    _logger.LogError(e.Message);
                }
            }
            else
            {
                //any issue with modelstate, a message will be thrown and user needs to repeat
                _logger.LogError("ModelState invalid");
            }
            //error message displayed on view if any exception is caught or invalid modelstate
            ViewData["Error"] = "There was an error, please try again.";
            return View();
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
                _logger.LogError(string.Format("User logging in: {0}", JsonConvert.SerializeObject(userInfo)));
                //below is used to create auth cookie to store username
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
            _logger.LogError("ModelState invalid");
            return View();
        }
    }
}