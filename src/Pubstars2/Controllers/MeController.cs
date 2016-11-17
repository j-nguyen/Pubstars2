using Microsoft.AspNetCore.Mvc;

namespace Pubstars2.Controllers
{
    public class MeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}