using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Pubstars2.Data;
using PubstarsModel;
using Pubstars2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Pubstars2.Controllers
{
    public class AdminController : Controller
    {
        IPubstarsDb _db;
        RoleManager<IdentityRole> _rm;
        UserManager<ApplicationUser> _um;
        ApplicationDbContext _ctx;

        public AdminController(IPubstarsDb db, RoleManager<IdentityRole> rm, UserManager<ApplicationUser> um, ApplicationDbContext ctx)
        {
            _db = db;
            _rm = rm;
            _um = um;
            _ctx = ctx;
        }

        public IActionResult Index()
        {
            return View();
        }

        private void ResetSeason()
        {
            DeleteAllGames();
            ResetRankings();
        }

        private void DeleteAllGames()
        {
            var games = _ctx.Games.AsEnumerable();
            _ctx.Games.RemoveRange(games);
            _ctx.SaveChanges();
        }

        private void ResetRankings()
        {            
            foreach(Rating t in _ctx.Rating)
            {              
                t.Mean = Moserware.Skills.GameInfo.DefaultGameInfo.DefaultRating.Mean;
                t.StandardDeviation = Moserware.Skills.GameInfo.DefaultGameInfo.DefaultRating.StandardDeviation;           
            }
            _db.SaveChanges();
        }

        private void ResetUncertainty()
        {
            foreach (ApplicationUser p in _db.UsersWithPlayer())
            {
                p.PlayerStats.Rating.StandardDeviation = Moserware.Skills.GameInfo.DefaultGameInfo.DefaultRating.StandardDeviation;
            }
            _db.SaveChanges();
        }

        private void DeleteUser(string name)
        {
            ApplicationUser user = _db.UsersWithPlayer().FirstOrDefault(x => x.UserName == name);
            if (user != null)
            {
                user.PlayerStats.Name = "[deleted]";
                _db.Remove(user);
                _db.SaveChanges();
            }
            else
                throw new ArgumentException(name + " - does not exist.");
        }

        private async Task MakeAdmin(string name)
        {
            ApplicationUser user = _db.Users().FirstOrDefault(x => x.UserName == name);
            if (user != null)
            {
                IdentityRole role = new IdentityRole("admin");
                if (!_rm.Roles.Contains(role))
                {
                    await _rm.CreateAsync(role);                    
                }
                await _um.AddToRoleAsync(user, role.Name);
            }
            else
                throw new ArgumentException(name + " - does not exist.");
        }
    }
}