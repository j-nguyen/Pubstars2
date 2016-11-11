using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PubstarsDtos;
using Pubstars2.Data;
using Pubstars2.Models.PubstarsGame;

namespace Pubstars2.Controllers
{
    public class GameReportsController : Controller
    {
        private ApplicationDbContext _db;

        public GameReportsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult PostGameResult([FromBody]RankedGameReport report)
        {
            List<PubstarsPlayer> pubplayers = new List<PubstarsPlayer>();
            foreach(RankedGameReport.PlayerStatLine p in report.PlayerStats)
            {
                PubstarsPlayer pp = new PubstarsPlayer()
                {
                    Name = p.Name,
                    team = p.Team == "Red" ? HqmTeam.red : HqmTeam.blue,
                    Goals = p.Goals,
                    Assists = p.Assists
                };

                pubplayers.Add(pp);
            }

            PubstarsGame game = new PubstarsGame()
            {
                gameId = report.Date + "-" + report.ServerName,
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