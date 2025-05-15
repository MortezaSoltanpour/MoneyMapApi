using Microsoft.AspNetCore.Mvc;

namespace MoneyMap.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}