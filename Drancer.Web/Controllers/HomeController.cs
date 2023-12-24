using Microsoft.AspNetCore.Mvc;

namespace Drancer.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
