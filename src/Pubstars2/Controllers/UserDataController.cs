using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using PubstarsDtos;

namespace Pubstars2.Controllers
{
    public class UserdataController : Controller
    {
        private UserManager<ApplicationUser> _userManager;

        public UserdataController(UserManager<ApplicationUser> usermanager)
        {
            _userManager = usermanager;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetUserData()
        {
            List<UserData> list = new List<UserData>();
            foreach (ApplicationUser user in _userManager.Users)
            {
                list.Add(new UserData()
                {
                    Name = user.UserName,
                    Password = user.psPassword,
                    Rating = user.rating
                });                    
            }
            return Json(list);
        }
    }
}