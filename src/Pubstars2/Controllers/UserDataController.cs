using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using PubstarsDtos;
using Microsoft.EntityFrameworkCore;
using Pubstars2.Data;
using Microsoft.AspNetCore.Authorization;
using System.Net;
using System.Threading.Tasks;

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
        public IActionResult GetAllUserData()
        {            
            return new JsonResult(_db.GetUserData());
        }

        [HttpGet]
        [Authorize(Roles = "client")]
        public async Task<IActionResult> GetUserData(string name)
        {
            name = WebUtility.UrlDecode(name);
            var result = await _db.GetUserData(name);
            if (result == null)
            {
                return NotFound("could not find " + name);
            }
            return new JsonResult(result);
        }
    }
}