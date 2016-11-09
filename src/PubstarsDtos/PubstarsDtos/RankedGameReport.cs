using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HQMRanked
{
    public class RankedGameReport
    {
        public class PlayerStatLine
        {
            public string Name;
            public HqmTeam Team;
            public int Goals;
            public int Assists;
            public bool Leaver = false;
        }

        public List<PlayerStatLine> PlayerStats;
        public int RedScore;
        public int BlueScore;
        public HqmTeam Winner;
        public PlayerStatLine MVP;
        public double MatchQuality;

        //To be created when game ends. Assumes before Reset.
        public RankedGameReport(List<string> RedTeam, List<string> BlueTeam, int redScore, int blueScore, List<PlayerStatLine> playerStatLine)
        {
            RedScore = redScore;
            BlueScore = blueScore;
            Winner = RedScore > BlueScore ? HqmTeam.Red : HqmTeam.Blue;

            PlayerStats = playerStatLine;

            MVP = PlayerStats[0];
            foreach (PlayerStatLine p in PlayerStats)
            {
                if (p.Goals + p.Assists > MVP.Goals + MVP.Assists)
                {
                    MVP = p;
                }
            }
        }


    }

    public enum HqmTeam
    {
        Red,
        Blue
    }


}
