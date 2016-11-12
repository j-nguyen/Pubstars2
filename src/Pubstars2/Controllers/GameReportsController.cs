using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PubstarsDtos;
using Pubstars2.Data;
using Pubstars2.Models.PubstarsGame;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;

namespace Pubstars2.Controllers
{
    public class GameReportsController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;

        public GameReportsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostGameResult([FromBody]RankedGameReport report)
        {
            List<PubstarsPlayer> pubplayers = new List<PubstarsPlayer>();         

   
            foreach (RankedGameReport.PlayerStatLine p in report.PlayerStats)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(p.Name);
                PubstarsPlayer pp = new PubstarsPlayer()
                {
                    User = user == null ? null : user,                    
                    Team = p.Team == "Red" ? HqmTeam.red : HqmTeam.blue,
                    Goals = p.Goals,
                    Assists = p.Assists
                };

                pubplayers.Add(pp);
            }

            PubstarsGame game = new PubstarsGame()
            {                
                players = pubplayers,
                redScore = report.RedScore,
                blueScore = report.BlueScore,
                date = report.Date
            };

            _db.Games.Add(game);
            _db.SaveChanges();            
            
            return Ok();
        }
    }
}