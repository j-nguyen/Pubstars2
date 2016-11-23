using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pubstars2.Models.PubstarsViewModels;
using Pubstars2.Data;
using System;
using PubstarsModel;
using Pubstars2.Services;
using Microsoft.EntityFrameworkCore;

namespace Pubstars2.Controllers
{
    public class LeaderboardController : Controller
    {
        ApplicationDbContext _db;
        IStatsService _statsService;

        public LeaderboardController(ApplicationDbContext context, IStatsService stats)
        {
            _db = context;
            _statsService = stats;
        }

        public IActionResult Index()
        {
            //fakedata
            List<LeaderboardEntryViewModel> entries = new List<LeaderboardEntryViewModel>();            
            foreach(Player player in _db.Players.Include(x => x.Rating))
            {
                int gp = _statsService.GetGamesPlayed(player);
                int w = _statsService.GetWins(player);
                int g = _statsService.GetGoals(player);
                int a = _statsService.GetAssists(player);
                entries.Add(new LeaderboardEntryViewModel()
                {
                    name = player.Name,
                    rating = Math.Round(player.Rating.Mean,2),
                    gamesPlayed = gp,
                    wins = w,
                    losses = gp - w,
                    winPercentage = Math.Round(w / (double)gp, 3),
                    pointsPerGame = Math.Round((g + a) / (double)gp,2),
                    points = g + a,
                    goals = g,
                    assists = a
                });
            }
            return View(entries.OrderByDescending(o => o.rating));            
        }
    }
}