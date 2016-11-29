using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using PubstarsDtos;
using Microsoft.EntityFrameworkCore;
using Pubstars2.Data;
using Microsoft.AspNetCore.Authorization;

namespace Pubstars2.Controllers
{
    public class UserdataController : Controller
    {
        IPubstarsDb _db;

        public UserdataController(IPubstarsDb db)
        {
            _db = db;
            
        }

        public IActionResult Index()
        {
            return View();
        }

        //for client
        [HttpGet]
        [Authorize(Roles ="client")]
        public IActionResult GetUserData()
        {            
            return new JsonResult(_db.GetUserData());
        }
    }
}