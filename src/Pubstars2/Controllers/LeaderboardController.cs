using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pubstars2.Models.PubstarsViewModels;

namespace Pubstars2.Controllers
{
    public class LeaderboardController : Controller
    {
        public IActionResult Index()
        {
            //fakedata
            List<LeaderboardEntryViewModel> entries = new List<LeaderboardEntryViewModel>();
            for (int i = 0; i < 100; i++)
            {
                entries.Add(new LeaderboardEntryViewModel()
                {
                    name = "player " + i,
                    rating = 1000 + (20 * i),
                    gamesPlayed = 20,
                    wins = 10,
                    losses = 10,
                    winPercentage = 50,
                    pointsPerGame = 0.5f,
                    points = 10,
                    goals = 5,
                    assists = 5
                });
            }
            return View(entries.OrderByDescending(o => o.rating));            
        }
    }
}