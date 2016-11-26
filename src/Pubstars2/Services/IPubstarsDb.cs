using Pubstars2.Models;
using PubstarsDtos;
using PubstarsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Data
{
    public interface IPubstarsDb
    {       
        IEnumerable<ApplicationUser> Users();
        IEnumerable<ApplicationUser> UsersWithPlayer();

        IEnumerable<Player> Players();
        IEnumerable<Game> Games();

        IEnumerable<PlayerGameStats> PlayerGameStats();

        Dictionary<string,UserData> GetUserData();

        void AddGame(Game game);
        void SaveChanges();
    }
}
