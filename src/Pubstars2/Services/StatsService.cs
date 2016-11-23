using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PubstarsModel;
using Pubstars2.Data;
using Microsoft.EntityFrameworkCore;

namespace Pubstars2.Services
{
    public class StatsService : IStatsService
    {
        private readonly ApplicationDbContext _db;

        public StatsService(ApplicationDbContext db)
        {
            _db = db;
        }

        public int GetAssists(Player p)
        {
            return _db.PlayerStats.Where(x => x.Player == p).Sum(x => x.Assists);
        }

        public int GetGamesPlayed(Player p)
        {
            return _db.PlayerStats.Where(x => x.Player == p).Count();
        }

        public int GetGoals(Player p)
        {
            return _db.PlayerStats.Where(x => x.Player == p).Sum(x => x.Goals);
        }

        public int GetWins(Player p)
        {
            return _db.PlayerStats.Include(x=> x.Game).Where(x => x.Player == p && x.Team == x.Game.winner).Count();
        }

        
    }
}
