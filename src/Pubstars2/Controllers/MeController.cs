using Microsoft.AspNetCore.Mvc;
using Pubstars2.Data;
using System.Linq;

namespace Pubstars2.Controllers
{
    public class MeController : Controller
    {
        IPubstarsDb _db;

        public MeController(IPubstarsDb db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public JsonResult GetEloGraphJson()
        {
            var elos = _db.PlayerGameStats()
                .Where(x => x.Player.Name == "player2") //TODO: replace with current user
                .OrderBy(x => x.Game.date)
                .Select(x => x.RatingMean)
                .ToArray();

            return Json(elos);
        }
    }
}