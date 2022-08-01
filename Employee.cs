using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MiCare
{
    public class Employee
    {
        public int Id { get; set; }
        public string UserId { get; set; }

        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public long UserPhone { get; set; }

        public long Aadhar_Number { get; set; }

        public DateTime Registation_Date { get; set; }
    }
}
