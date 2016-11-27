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

        public LeaderboardService(IPubstarsDb context, IStatsService stats, IMemoryCache cache)
        {
            _db = context;
            _statsService = stats;
            _cache = cache;
        }

        public IEnumerable<PlayerStatsViewModel> GetLeaderboard()
        {
            List<PlayerStatsViewModel> entries;
            if (!_cache.TryGetValue(k_LeaderboardCacheKey, out entries))
            {
                entries = new List<PlayerStatsViewModel>();
                foreach (Player player in _db.Players())
                {
                    int gp = _statsService.GetGamesPlayed(player);
                    int w = _statsService.GetWins(player);
                    int g = _statsService.GetGoals(player);
                    int a = _statsService.GetAssists(player);
                    entries.Add(new PlayerStatsViewModel(player.Name, player.Rating.Mean, g, a, gp, w));                    
                }
                _cache.Set(k_LeaderboardCacheKey, entries);                
            }
            return entries.OrderByDescending(o => o.Rating);
        }

        public void FlushLeaderboards()
        {
            _cache.Remove(k_LeaderboardCacheKey);
        }

        
    }
}
