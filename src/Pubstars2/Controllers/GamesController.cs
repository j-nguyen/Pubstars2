using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PubstarsDtos;
using Pubstars2.Data;
using Pubstars2.Models.Pubstars;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using Pubstars2.Models.PubstarsViewModels;
using Moserware.Skills;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
            foreach(Game game in _db.Games)
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
        public IActionResult PostGameResult([FromBody]RankedGameReport report)
        {
            Game game = CreatePubstarsGame(report);
            //todo: update player stats/rating? or calculate from aggregate data          
            _db.Games.Add(game);
            _db.SaveChanges();            
            
            return Ok();
        }       

        private Game CreatePubstarsGame(RankedGameReport report)
        {
            List<PlayerGameStats> pubplayers = new List<PlayerGameStats>();

            foreach (RankedGameReport.PlayerStatLine p in report.PlayerStats)
            {
                ApplicationUser user = _db.Users.Include(x => x.PlayerStats).FirstOrDefault(x => x.UserName == p.Name);
                if (user == null)
                {
                    throw new InvalidOperationException("tried to report game with unregistered players.");
                }

                PlayerGameStats pp = new PlayerGameStats()
                {
                    Player = user.PlayerStats,
                    Team = p.Team == "Red" ? HqmTeam.red : HqmTeam.blue,
                    Goals = p.Goals,
                    Assists = p.Assists,
                    RatingMean = user.PlayerStats.RatingMean,
                    RatingUncertainty = user.PlayerStats.RatingUncertainty,
                };                        

                pubplayers.Add(pp);
            }

            return new Game()
            {
                gameId = new Guid(),
                playerStats = pubplayers,
                redScore = report.RedScore,
                blueScore = report.BlueScore,
                date = report.Date
            };            
        }

        
    }
}