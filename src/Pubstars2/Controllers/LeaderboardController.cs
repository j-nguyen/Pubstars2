using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pubstars2.Models.PubstarsViewModels;
using Pubstars2.Data;
using Pubstars2.Models.Pubstars;
using System;

namespace Pubstars2.Controllers
{
    public class LeaderboardController : Controller
    {
        ApplicationDbContext _db;

        public LeaderboardController(ApplicationDbContext context)
        {
            _db = context;
        }

        public IActionResult Index()
        {
            //fakedata
            List<LeaderboardEntryViewModel> entries = new List<LeaderboardEntryViewModel>();            
            foreach(PlayerStats stats in _db.PlayerStats)
            {                
                entries.Add(new LeaderboardEntryViewModel()
                {
                    name = stats.Name,
                    rating = Math.Round(stats.RatingMean,2),
                    gamesPlayed = stats.GamesPlayed,
                    wins = stats.Wins,
                    losses = stats.GamesPlayed - stats.Wins,
                    winPercentage = Math.Round(stats.Wins / (double)stats.GamesPlayed, 3),
                    pointsPerGame = (stats.Goals + stats.Assists) / (float)stats.GamesPlayed,
                    points = stats.Goals + stats.Assists,
                    goals = stats.Goals,
                    assists = stats.Assists
                });
            }
            return View(entries.OrderByDescending(o => o.rating));            
        }
    }
}