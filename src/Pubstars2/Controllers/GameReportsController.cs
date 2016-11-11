using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PubstarsDtos;

namespace Pubstars2.Controllers
{
    public class GameReportsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostGameResult(RankedGameReport report)
        {
            
            return Ok();
        }
    }
}