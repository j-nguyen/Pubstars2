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
            foreach(Game game in _db.Games.Include(x=> x.playerStats).ThenInclude(stats => stats.Player))
            {                
                gameSummaries.Add(new GameSummaryViewModel(game));
            }            
            return View(gameSummaries.OrderByDescending(x=> x.time));
        }

        [HttpPost]
        public IActionResult PostGameResult([FromBody]RankedGameReport report)
        {
            ProcessGameReport(report);                    
            return Ok();
        }       

        public string SimulateGames(int games)
        {
            for(int i = 0; i < games; i++)
            {              
                ProcessGameReport (RankedGameReport.RandomGame(_db.Users.Select(x => x.UserName).ToList()));               
            }
            _db.SaveChanges();
            return "sim done.";
        }

        private void ProcessGameReport(RankedGameReport report)
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

            Game game = new Game()
            {
                gameId = new Guid(),
                playerStats = pubplayers,
                redScore = report.RedScore,
                blueScore = report.BlueScore,
                date = report.Date
            };
            _db.Games.Add(game);

            //apply new ratings
            foreach(KeyValuePair<PlayerStats, Rating> kvp in game.GetNewRatings())
            {
                kvp.Key.RatingMean = kvp.Value.Mean;
                kvp.Key.RatingUncertainty = kvp.Value.StandardDeviation;
            }
            _db.SaveChanges();
        }

        
    }
}