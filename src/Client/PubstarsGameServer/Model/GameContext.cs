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
        public IReadOnlyList<RankedPlayer> LoggedInPlayers { get { return m_LoggedInPlayers.Values.ToList(); } }


        public List<string> Leavers = new List<string>();

        private Dictionary<string, RankedPlayer> m_LoggedInPlayers = new Dictionary<string, RankedPlayer>();
        

        public void RemovePlayer(string name)
        {
            m_LoggedInPlayers.Remove(name);
        }

        public void AddPlayer(RankedPlayer p)
        {
            m_LoggedInPlayers.Add(p.Name, p);
        }

        public void ClearTeams()
        {
            foreach(RankedPlayer rp in m_LoggedInPlayers.Values)
            {
                rp.Team = HQMTeam.NoTeam;
            }
        }

        public void AddPlayerToTeam(string name, HQMTeam team)
        {
            RankedPlayer p;
            if (m_LoggedInPlayers.TryGetValue(name, out p))
            {
                p.Team = team;
            }
            else
                throw new InvalidOperationException("Could not add " + name + "to " + team);
        }

        public void RemovePlayerFromTeam(string name)
        {
            RankedPlayer p;
            if (m_LoggedInPlayers.TryGetValue(name, out p))
            {
                p.Team = HQMTeam.NoTeam;
            }
        }

        public bool IsLoggedIn(string name, int slot)
        {
            RankedPlayer p = m_LoggedInPlayers.Values.FirstOrDefault(x => x.Name == name);
            return (p != null && slot == p.PlayerStruct.Slot);
        }

        public RankedPlayer GetPlayer(string name)
        {
            RankedPlayer p = null;
            m_LoggedInPlayers.TryGetValue(name, out p);
            return p;
            
        }

        public bool IsPlaying(RankedPlayer p)
        {
            return p.Team != HQMTeam.NoTeam;
        }
    }
}
