using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Pubstars2.Models.PubstarsViewModels;
using Microsoft.Extensions.Caching.Memory;
using Pubstars2.Data;
using PubstarsModel;

namespace Pubstars2.Services
{
    public class LeaderboardService : ILeaderboardService
    {
        public const string k_LeaderboardCacheKey = "leaderboardCacheKey";

        IPubstarsDb _db;
        IStatsService _statsService;
        IMemoryCache _cache;

        private bool _dirty;

        public LeaderboardService(IPubstarsDb context, IStatsService stats, IMemoryCache cache)
        {
            _db = context;
            _statsService = stats;
            _cache = cache;
        }

        public IEnumerable<LeaderboardEntryViewModel> GetLeaderboard()
        {
            List<LeaderboardEntryViewModel> entries;
            if (_dirty || !_cache.TryGetValue(k_LeaderboardCacheKey, out entries))
            {
                entries = new List<LeaderboardEntryViewModel>();
                foreach (Player player in _db.Players())
                {
                    int gp = _statsService.GetGamesPlayed(player);
                    int w = _statsService.GetWins(player);
                    int g = _statsService.GetGoals(player);
                    int a = _statsService.GetAssists(player);
                    entries.Add(new LeaderboardEntryViewModel()
                    {
                        name = player.Name,
                        rating = Math.Round(player.Rating.Mean, 2),
                        gamesPlayed = gp,
                        wins = w,
                        losses = gp - w,
                        winPercentage = Math.Round(w / (double)gp, 3),
                        pointsPerGame = Math.Round((g + a) / (double)gp, 2),
                        points = g + a,
                        goals = g,
                        assists = a
                    });
                }
                _cache.Set(k_LeaderboardCacheKey, entries);
                _dirty = false;
            }
            return entries.OrderByDescending(o => o.rating);
        }

        public void SetDirty()
        {
            _dirty = true;
        }
    }
}
