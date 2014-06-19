using Ccai.NewRoam.SOActivity.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Ccai.NewRoam.SOActivity.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Employee> persons = new List<Employee>() {
                new Employee {
                    FirstName = "Oetawan",
                    LastName = "Chandra"
                }
            };
            ViewBag.Message = PrintFullname(persons) +  ", modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public string PrintFullname(IEnumerable<Person> persons)
        {
            string result = "";
            foreach (Person person in persons)
            {
                result += person.FirstName + " " + person.LastName;
            }
            return result;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
