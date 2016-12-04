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
        public List<RankedPlayer> Players = new List<RankedPlayer>();

        public List<string> RedTeam = new List<string>();
        public List<string> BlueTeam = new List<string>();

        public bool IsLoggedIn(string name, int slot)
        {
            RankedPlayer p = LoggedInPlayers.FirstOrDefault(x => x.Name == name);
            return (p != null && slot == p.PlayerStruct.Slot);
        }

        public void RemoveLoggedOutPlayers()
        {
            byte[] playerList = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS, 20 * MemoryAddresses.PLAYER_STRUCT_SIZE);
            LoggedInPlayers.RemoveAll(p => playerList[p.PlayerStruct.Slot * MemoryAddresses.PLAYER_STRUCT_SIZE] == 0);
        }
    }
}
