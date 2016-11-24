using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Pubstars2.Models.PubstarsViewModels;
using Pubstars2.Data;
using System;
using PubstarsModel;
using Pubstars2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Pubstars2.Controllers
{
    public class LeaderboardController : Controller
    {
        ILeaderboardService _leaderboards;

        public LeaderboardController(ILeaderboardService ls)
        {
            _leaderboards = ls;
        }

        public IActionResult Index()
        {           
            return View(_leaderboards.GetLeaderboard());            
        }
    }
}