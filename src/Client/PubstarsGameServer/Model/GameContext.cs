using HQMEditorDedicated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Model
{
    class GameContext
    {
        public List<RankedPlayer> LoggedInPlayers = new List<RankedPlayer>();

        public List<string> RedTeam = new List<string>();
        public List<string> BlueTeam = new List<string>();

        public List<string> Leavers = new List<string>();

        public bool IsLoggedIn(string name, int slot)
        {
            RankedPlayer p = LoggedInPlayers.FirstOrDefault(x => x.Name == name);
            return (p != null && slot == p.PlayerStruct.Slot);
        }

        public RankedPlayer GetPlayer(string name)
        {
            return LoggedInPlayers.FirstOrDefault(x => x.Name == name);
        }

        public bool IsPlaying(RankedPlayer p)
        {
            return RedTeam.Concat(BlueTeam).Contains(p.Name);
        }
    }
}
