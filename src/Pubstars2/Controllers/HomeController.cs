using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;

namespace Pubstars2.Controllers
{
   
    public class HomeController : Controller
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController(SignInManager<ApplicationUser> sign)
        {
            _signInManager = sign;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}