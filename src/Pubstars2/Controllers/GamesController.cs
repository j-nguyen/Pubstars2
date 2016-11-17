using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PubstarsDtos;
using Pubstars2.Data;
using Pubstars2.Models.PubstarsStats;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using Pubstars2.Models.PubstarsViewModels;

namespace Pubstars2.Controllers
{
    public class GamesController : Controller
    {
        private ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;

        public GamesController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            List<GameSummaryViewModel> gameSummaries = new List<GameSummaryViewModel>();
            foreach(PubstarsGame game in _db.Games)
            {
                //todo statlines
                gameSummaries.Add(new GameSummaryViewModel()
                {
                    redScore = game.redScore,
                    blueScore = game.blueScore,
                    time = game.date
                });
            }
            return View(gameSummaries);
        }

        [HttpPost]
        public async Task<IActionResult> PostGameResult([FromBody]RankedGameReport report)
        {
            List<PubstarsPlayer> pubplayers = new List<PubstarsPlayer>();         

   
            foreach (RankedGameReport.PlayerStatLine p in report.PlayerStats)
            {
                ApplicationUser user = await _userManager.FindByNameAsync(p.Name);
                if(user == null)
                {
                    throw new InvalidOperationException("tried to report game with unregistered players.");
                }

                PubstarsPlayer pp = new PubstarsPlayer()
                {
                    PubstarsPlayerId = user.Id,
                    User = user,                    
                    Team = p.Team == "Red" ? HqmTeam.red : HqmTeam.blue,
                    Goals = p.Goals,
                    Assists = p.Assists
                };

                pubplayers.Add(pp);
            }

            PubstarsGame game = new PubstarsGame()
            {
                gameId = report.ServerName + report.Date,
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