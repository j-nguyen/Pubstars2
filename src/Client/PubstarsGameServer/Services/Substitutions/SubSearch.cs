using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services.Substitutions
{
    class SubSearch
    {
        const int SUB_SEARCH_TIME = 10;

        public HQMEditorDedicated.HQMTeam Team { get { return m_Leaver.TeamLeft; } }

        private DateTime m_Start;
        private TimeSpan m_SearchTime { get { return new TimeSpan(0, 0, SUB_SEARCH_TIME); } }
        private Leaver m_Leaver;

        public SubSearch(Leaver leaver)
        {
            m_Start = DateTime.UtcNow;
            m_Leaver = leaver;
        }

        public RankedPlayer ClosestElo(List<RankedPlayer> players)
        {
            if (players == null || players.Count <= 0)
            {
                throw new ArgumentException("players must have at least one entry.");
            }

            RankedPlayer closestElo = players[0];
            foreach (RankedPlayer p in players)
            {
                if (Math.Abs(p.Rating - m_Leaver.Rating) < Math.Abs(closestElo.Rating - m_Leaver.Rating))
                    closestElo = p;
            }
            return closestElo;
        }

        public void ResetSearchTime()
        {
            m_Start = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > m_Start + m_SearchTime;
        }
    }
}
