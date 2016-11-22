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
                List<StatlineViewModel> redstats = new List<StatlineViewModel>();
                List<StatlineViewModel> bluestats = new List<StatlineViewModel>();
                IDictionary<PlayerStats, Rating> newRatings = game.GetNewRatings();
                foreach(PlayerGameStats stats in game.playerStats)
                {
                    StatlineViewModel vm = new StatlineViewModel()
                    {
                        name = stats.Player.Name, 
                        goals = stats.Goals.ToString(),
                        assists = stats.Assists.ToString(),
                        ratingChange = Math.Round(newRatings[stats.Player].Mean - stats.RatingMean, 2).ToString(),
                        newRating = Math.Round(newRatings[stats.Player].Mean, 2).ToString()
                    };

                    if (stats.Team == HqmTeam.red)
                    {
                        redstats.Add(vm);
                    }
                    else
                    {
                        bluestats.Add(vm);
                    }
                }
                gameSummaries.Add(new GameSummaryViewModel()
                {
                    time = game.date,
                    redScore = game.redScore,
                    blueScore = game.blueScore,
                    redStatLines = redstats,
                    blueStatLines = bluestats
                });
            }
            
            return View(gameSummaries);
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
                int redgoals = 0;
                int bluegoals = 0;
                int redassists = 0;
                int blueassists = 0;
                Random r = new Random();
                List<RankedGameReport.PlayerStatLine> statlines = new List<RankedGameReport.PlayerStatLine>();
                var playerNumbers = Enumerable.Range(r.Next(_db.Users.Count() - 10), 10).OrderBy(a => r.Next()).ToArray();
                for (int j = 0; j < 10; j++)
                {
                    bool redteam = j % 2 == 0;
                    int g = r.Next(4);
                    int a = 0;
                    if (redteam)
                    {
                        redgoals += g;
                        a = r.Next(redgoals - redassists);                        
                        redassists += a;
                    }
                    else
                    {
                        bluegoals += g;
                        a = r.Next(bluegoals - blueassists);                        
                        blueassists += a;
                    }                   
                    
                    statlines.Add(new RankedGameReport.PlayerStatLine()
                    {
                        Name = "player" + playerNumbers[j],
                        Goals = g,
                        Assists = a,
                        Team = redteam ? "Red" : "Blue",
                        Leaver = false
                    });
                }
                if(redgoals == bluegoals) //tie goes to red team
                {
                    redgoals++;
                    statlines[0].Goals++;
                }

                ProcessGameReport (new RankedGameReport()
                {
                    RedScore = redgoals,
                    BlueScore = bluegoals,
                    WinningTeam = redgoals > bluegoals ? "Red" : "Blue",
                    Date = DateTime.UtcNow,
                    ServerName = "Simulated",
                    PlayerStats = statlines
                });
                

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