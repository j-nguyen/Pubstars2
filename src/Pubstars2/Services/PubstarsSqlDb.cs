using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PubstarsModel;
using Microsoft.EntityFrameworkCore;
using Pubstars2.Models;
using PubstarsDtos;
using Microsoft.AspNetCore.Identity;

namespace Pubstars2.Data
{
    public class PubstarsSqlDb : IPubstarsDb
    {
        ApplicationDbContext _db;
        private UserManager<ApplicationUser> _userManager;

        public PubstarsSqlDb(ApplicationDbContext db, UserManager<ApplicationUser> usermanager)
        {
            _userManager = usermanager;
            _db = db;
        }

        public void AddGame(Game game)
        {
            _db.Add(game);
        }

        public IEnumerable<Game> Games()
        {
            return _db.Games
                .Include(x => x.playerStats)
                .ThenInclude(stats => stats.Player)
                .ThenInclude(player => player.Rating);
        }

        public IEnumerable<UserData> GetUserData()
        {
            var userData = new List<UserData>();

            foreach (ApplicationUser user in _userManager.Users
                .Include(x => x.PlayerStats)
                .ThenInclude(x => x.Rating))
            {
                if(user.PlayerStats != null)
                {
                    userData.Add(new UserData()
                    {
                        Name = user.UserName,
                        Password = user.PubstarsPassword,
                        Rating = user.PlayerStats.Rating.Mean
                    });
                }                
            }
            return userData;
        }

        public async Task<UserData> GetUserData(string name)
        {
            var user = await _userManager.Users
                .Include(x => x.PlayerStats)
                .ThenInclude(x => x.Rating)
                .FirstOrDefaultAsync(x => x.UserName == name);

            if (user != null && user.PlayerStats != null)
            {
                return new UserData()
                {
                    Name = user.UserName,
                    Password = user.PubstarsPassword,
                    Rating = user.PlayerStats.Rating.Mean
                };
            }
            return null;
        }

        public IEnumerable<PlayerGameStats> PlayerGameStats()
        {
            return _db.PlayerStats
                .Include(x => x.Player)
                .Include(x => x.Game);
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
            return _db.Users
                .Include(x => x.PlayerStats)
                .ThenInclude(x => x.Rating);
        }

        public void Remove(object entity)
        {
            _db.Remove(entity);
        }
    }
}
