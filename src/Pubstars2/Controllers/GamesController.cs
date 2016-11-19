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
            //test data
            List<GameSummaryViewModel> gameSummaries = new List<GameSummaryViewModel>();
            for (int i = 0; i < 100; i++)
            {
                //todo statlines
                GameSummaryViewModel g = new GameSummaryViewModel()
                {
                    redScore = 3,
                    blueScore = 2,
                    time = DateTime.UtcNow
                };
                g.redStatLines = new List<StatlineViewModel>();
                g.blueStatLines = new List<StatlineViewModel>();
                for(int j = 0; j < 5; j++)
                {
                    StatlineViewModel slm = new StatlineViewModel()
                    {
                        name = "player" + j,
                        goals = 5 - j + "",
                        assists = j + "",
                        ratingChange = "+25",
                        newRating = "2500"
                    };
                    g.redStatLines.Add(slm);
                    g.blueStatLines.Add(slm);
                }
                gameSummaries.Add(g);
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