using MiCare.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
        public IActionResult LoadData()
        {
            return Json("");
        }
        [HttpPost]
        public IActionResult Home(string fullname,string emailaddress,string username,string phonenumber,string nidnumber, string txtDate)
        {
            string query = "INSERT INTO dbo.ApprovalFlow (UserId, UserName, UserEmail, UserPhone, UserIdenitity, RegistrationDate) " +
                   "VALUES (@UserId, @UserName, @UserEmail, @UserPhone, @UserIdenitity, @RegistrationDate) ";
            string connectionString = "Data Source=.;Initial Catalog=MiCare;User ID =sa; Password =Micron@123";
            // create connection and command
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                // define parameters and their values
                cmd.Parameters.Add("@UserName", SqlDbType.VarChar, 50).Value = fullname;
                cmd.Parameters.Add("@UserEmail", SqlDbType.VarChar, 50).Value = emailaddress;
                cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 50).Value = username;
                cmd.Parameters.Add("@UserPhone", SqlDbType.BigInt, 50).Value = Convert.ToInt64(phonenumber);
                cmd.Parameters.Add("@UserIdenitity", SqlDbType.BigInt).Value = Convert.ToInt64(nidnumber);
                cmd.Parameters.Add("@RegistrationDate", SqlDbType.Date, 50).Value = txtDate;
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();
                TempData["Msg"] = "Thankyou for Scheduling RTPCR Test";
            }
            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

    }
}
