using System;
using System.Collections.Generic;

namespace PubstarsDtos
{
    public class RankedGameReport
    {
        public ICollection<PlayerStatLine> PlayerStats { get; set; }
        public int RedScore { get; set; }
        public int BlueScore { get; set; }
        public string WinningTeam { get; set; }
        public DateTime Date { get; set; }
        public string ServerName { get; set; }

        public class PlayerStatLine
        {
            public string Name { get; set; }
            public string Team { get; set; }
            public int Goals { get; set; }
            public int Assists { get; set; }
            public bool Leaver { get; set; }
        }
    }

    
}
