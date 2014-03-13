using Microsoft.AspNet.Mvc;

namespace MvcSample
{
    [Area("Travel")]
    public class Flight : Controller
    {
        public IActionResult Fly()
        {
            return View();
        }
    }
}
