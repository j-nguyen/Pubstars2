using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using Pubstars2.Models.PubstarsViewModels;

namespace Pubstars2.Controllers
{
    public class HomeController : Controller
    {      

        public HomeController(SignInManager<ApplicationUser> sign)
        {
          
        }

        public IActionResult Index()
        {
            return View();
        }
       
    }
}