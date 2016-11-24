using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PubstarsModel;
using Microsoft.EntityFrameworkCore;
using Pubstars2.Models;

namespace Pubstars2.Data
{
    public class PubstarsSqlDb : IPubstarsDb
    {
        ApplicationDbContext _db;

        public PubstarsSqlDb(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddGame(Game game)
        {
            _db.Add(game);
        }

        public IEnumerable<Game> Games()
        {
            return _db.Games.Include(x => x.playerStats).ThenInclude(stats => stats.Player).ThenInclude(player => player.Rating);
        }

        public IEnumerable<Player> Players()
        {
            return _db.Players.Include(x => x.Rating);
        }

        public void SaveChanges()
        {
            _db.SaveChanges();
        }

        public IEnumerable<ApplicationUser> Users()
        {
            return _db.Users;
        }

        public IEnumerable<ApplicationUser> UsersWithPlayer()
        {
            return _db.Users.Include(x => x.PlayerStats).ThenInclude(x => x.Rating);
        }
    }
}
