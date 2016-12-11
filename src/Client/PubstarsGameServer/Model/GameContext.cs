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
        public IReadOnlyList<string> RedTeam { get { return m_RedTeam; } }
        public IReadOnlyList<string> BlueTeam { get { return m_BlueTeam; } }

        public List<string> Leavers = new List<string>();

        private Dictionary<string, RankedPlayer> m_LoggedInPlayers = new Dictionary<string, RankedPlayer>();
        private List<string> m_RedTeam = new List<string>();
        private List<string> m_BlueTeam = new List<string>();             

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
            m_RedTeam = new List<string>();
            m_BlueTeam = new List<string>();
        }

        public void AddPlayerToTeam(string name, HQMTeam team)
        {
            if(m_LoggedInPlayers.ContainsKey(name))
            {
                if (team == HQMTeam.Red) m_RedTeam.Add(name);               
                else if (team == HQMTeam.Blue) m_BlueTeam.Add(name);
            }
        }

        public void RemovePlayerFromTeam(string name, HQMTeam team)
        {
            if (team == HQMTeam.Red && m_RedTeam.Contains(name)) m_RedTeam.Remove(name);
            if (team == HQMTeam.Blue && m_BlueTeam.Contains(name)) m_BlueTeam.Remove(name);
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
            return RedTeam.Concat(BlueTeam).Contains(p.Name);
        }
    }
}
