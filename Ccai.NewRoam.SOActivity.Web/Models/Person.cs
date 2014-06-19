using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Ccai.NewRoam.SOActivity.Web.Models
{
    public class Person
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class Employee : Person
    {
        public DateTime JoinDate { get; set; }
    }
}