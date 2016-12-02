using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PubstarsDtos;
using Pubstars2.Data;
using PubstarsModel;
using Microsoft.AspNetCore.Identity;
using Pubstars2.Models;
using Pubstars2.Models.PubstarsViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Sakura.AspNetCore;
using Pubstars2.Services;
using Microsoft.AspNetCore.Authorization;

namespace Pubstars2.Controllers
{
    public class GamesController : Controller
    {
        private IPubstarsDb _db;
        private ILeaderboardService _leaderboards;
        private UserManager<ApplicationUser> _userManager;

        public GamesController(IPubstarsDb db, UserManager<ApplicationUser> userManager, ILeaderboardService ls)
        {
            _db = db;
            _userManager = userManager;
            _leaderboards = ls;
        }

        public IActionResult Index(int page)
        {
            List<GameSummaryViewModel> gameSummaries = new List<GameSummaryViewModel>();
            foreach(Game game in _db.Games())
            {                
                gameSummaries.Add(new GameSummaryViewModel(game));
            }
            var viewData = gameSummaries.OrderByDescending(x => x.time).ToDynamicPagedList(10, page == 0 ? 1 : page);
            return View(viewData);
        }

        [HttpPost]
        [Authorize(Roles = "client")]
        public IActionResult ReportGame([FromBody]RankedGameReport report)
        {
            ProcessGameReport(report);                    
            return Ok(_db.GetUserData());
        }      

        private void ProcessGameReport(RankedGameReport report)
        {
            List<PlayerGameStats> pubplayers = new List<PlayerGameStats>();

            foreach (RankedGameReport.PlayerStatLine p in report.PlayerStats)
            {
                ApplicationUser user = _db.UsersWithPlayer().FirstOrDefault(x => x.UserName == p.Name);
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
                    RatingMean = user.PlayerStats.Rating.Mean,
                    RatingStandardDeviation = user.PlayerStats.Rating.StandardDeviation                 
                };                        

                pubplayers.Add(pp);
            }

            Game game = new Game()
            {
                GameId = new Guid(),
                playerStats = pubplayers,
                redScore = report.RedScore,
                blueScore = report.BlueScore,
                date = report.Date
            };
            _db.AddGame(game);

            //apply new ratings
            foreach(KeyValuePair<Player, Rating> kvp in game.GetNewRatings())
            {
                kvp.Key.Rating.Mean = kvp.Value.Mean;
                kvp.Key.Rating.StandardDeviation = kvp.Value.StandardDeviation;
            }
            _db.SaveChanges();
            _leaderboards.FlushLeaderboards();
        }

        public JsonResult GameData()
        {
            List<GameSummaryViewModel> gameSummaries = new List<GameSummaryViewModel>();
            foreach (Game game in _db.Games())
            {
                gameSummaries.Add(new GameSummaryViewModel(game));
            }
            var viewData = gameSummaries.OrderByDescending(x => x.time);
            return Json(gameSummaries);
        }
  
    }
}