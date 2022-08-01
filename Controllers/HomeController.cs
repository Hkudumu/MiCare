using MiCare.Models;
using Micron.Keystone.DirectoryServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace MiCare.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        protected IEmailService EmailService { get; }
        public HomeController(ILogger<HomeController> logger, IEmailService emailService)
        {
            _logger = logger;
            this.EmailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            TempData["Name"] = username;
            if ((username != null && password != null) && username.ToLower() == "admin" && password.ToLower() == "admin")
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
            var draw = HttpContext.Request.Form["draw"].FirstOrDefault();
            DataSet ds = new DataSet();
            List<Employee> list=new List<Employee>();
            int recordsTotal = 0;
            string query = "Select * from dbo.ApprovalFlow";
            string connectionString = "Data Source=.;Initial Catalog=MiCare;User ID =sa; Password =Micron@123";
            // create connection and command
            using (SqlConnection cn = new SqlConnection(connectionString))
            using (SqlCommand cmd = new SqlCommand(query, cn))
            {
                try
                {
                    cn.Open();
                    SqlDataAdapter sda = new SqlDataAdapter(cmd);
                    sda.Fill(ds);

                    cn.Close();
                     list = ds.Tables[0].AsEnumerable()
                         .Select(dataRow => new Employee
                         {
                             UserId = dataRow.Field<string>("UserId"),
                             UserName = dataRow.Field<string>("UserName"),
                             UserEmail = dataRow.Field<string>("UserEmail"),
                             UserPhone = Convert.ToInt64(dataRow.Field<Int64>("UserPhone")),
                             Aadhar_Number = Convert.ToInt64(dataRow.Field<Int64>("UserIdenitity")),
                             Registation_Date = Convert.ToDateTime(dataRow.Field<DateTime>("RegistrationDate"))
                         }).ToList();
                    recordsTotal = ds.Tables[0].Rows.Count;
                    TempData["Msg"] = "Thankyou for Scheduling RTPCR Test";

                }
                catch (Exception ex)
                {

                    throw;
                }
              
                return Json(new { draw = draw, recordsFiltered = recordsTotal, recordsTotal = recordsTotal, data = list });
            }
           
        }
        [HttpPost]
        public IActionResult Home(string fullname, string emailaddress, string username, string phonenumber, string nidnumber, string txtDate)
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
                DoSendEmail(emailaddress);
            }

            return View();
        }
        public IActionResult Admin()
        {
            return View();
        }

        public async void DoSendEmail(string username)
        {
            try
            {
                var body = "Thanks for Scheduling RTPCR Test.. Agent will be assigned soon";

                var message = new MailMessage()
                {
                    To = { new MailAddress($"{username}") },
                    Subject = "RTPCR Test Appointment Date "+ DateTime.Now,
                    Body = body,
                    IsBodyHtml = false // set to true if passing an HTML body
                };

                await this.EmailService.SendEmailAsync(message);
            }
            catch (Exception ex)
            {

                throw;
            }
           
        }

    }
}
