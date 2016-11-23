using PubstarsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Services
{
    public interface IStatsService
    {
        int GetGamesPlayed(Player p);
        int GetWins(Player p);
        int GetGoals(Player p);
        int GetAssists(Player p);


    }
}
