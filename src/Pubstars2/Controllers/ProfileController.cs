using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pubstars2.Data;
using Pubstars2.Models;
using Pubstars2.Models.MeViewModels;
using Pubstars2.Models.PubstarsViewModels;
using Pubstars2.Services;
using PubstarsModel;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Controllers
{
    public class ProfileController : Controller
    {
        IPubstarsDb _db;
        IStatsService _stats;
        UserManager<ApplicationUser> _userManager;

        public ProfileController(IPubstarsDb db, IStatsService stats, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _stats = stats;
            _userManager = userManager;
        }

        [Route("Profile/{username}")]
        public IActionResult Player(string username)
        {            
            Player player = _db.UsersWithPlayer().FirstOrDefault(x => x.UserName == username)?.PlayerStats;
            if (player == null)
            {
                return NotFound();
            }
            else
            {
                ProfileViewModel me = new ProfileViewModel();
                int gp = _stats.GetGamesPlayed(player);
                int w = _stats.GetWins(player);
                int g = _stats.GetGoals(player);
                int a = _stats.GetAssists(player);
                me.PlayerStats = new PlayerStatsViewModel(player.Name, player.Rating.Mean, g, a, gp, w);
                return View(me);
            }            
        }

        public async Task<IActionResult> Index()
        {
            ApplicationUser user = await _userManager.GetUserAsync(HttpContext.User);
            if (user == null)
            {
                return NotFound("user not found");
            }           
            else
            {
                Player player = _db.UsersWithPlayer().FirstOrDefault(x => x.UserName == user.UserName)?.PlayerStats;
                if(player == null)
                {
                    return NotFound("player not found");
                }
                ProfileViewModel me = new ProfileViewModel();
                int gp = _stats.GetGamesPlayed(player);
                int w = _stats.GetWins(player);
                int g = _stats.GetGoals(player);
                int a = _stats.GetAssists(player);
                me.PlayerStats = new PlayerStatsViewModel(player.Name, player.Rating.Mean, g, a, gp, w);
                return View(me);
            }
        }

        public JsonResult GetEloGraphJson(string Username)
        {
            var elos = _db.PlayerGameStats()
                .Where(x => x.Player.Name == Username)?
                .OrderBy(x => x.Game.date)
                .Select(x => x.RatingMean)
                .ToArray();           

            return Json(elos);
        }
    }
}