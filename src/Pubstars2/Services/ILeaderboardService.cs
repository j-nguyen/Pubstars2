using Microsoft.Extensions.Caching.Memory;
using Pubstars2.Data;
using Pubstars2.Models.PubstarsViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Services
{
    public interface ILeaderboardService
    {
        void FlushLeaderboards();
        IEnumerable<PlayerStatsViewModel> GetLeaderboard();
    }
}
