using Microsoft.AspNet.Mvc;
using MvcSample.Web.Models;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MvcSample.Web.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View(new User
            {
                Name = "My name",
                Address = "My address",
                Alive = true,
                Age = 13,
                GPA = 13.37M,
                Password = "Secure string",
                Dependent = new User()
                {
                    Name = "Dependents name",
                    Address = "Dependents address",
                    Alive = false,
                },
                Profession = "Software Engineer",
                About = "I like playing Football"
            });
        }
    }
}
