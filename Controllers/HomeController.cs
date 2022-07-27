using MiCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MiCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
       
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username,string password)
        {
                TempData["Name"] = username;
                if ((username != null && password != null)&&username.ToLower() == "admin" && password.ToLower() == "admin")
                {

                    return RedirectToAction("Admin");

                }
                else
                {
                    return RedirectToAction("Home");
                }
            
           
        }

        public IActionResult Home()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Home(string fullname,string emailaddress,string username,string phonenumber,string nidnumber, string txtDate)
        {
            TempData["Msg"] = "Thankyou for Scheduling RTPCR Test";
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

    }
}
