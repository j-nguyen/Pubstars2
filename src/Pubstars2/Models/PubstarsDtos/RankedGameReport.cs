using System;
using System.Collections.Generic;
using System.Linq;

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

        public static RankedGameReport RandomGame(List<string> names)
        {
            int redgoals = 0;
            int bluegoals = 0;
            int redassists = 0;
            int blueassists = 0;
            Random r = new Random();
            List<RankedGameReport.PlayerStatLine> statlines = new List<RankedGameReport.PlayerStatLine>();
            List<string> players = new List<string>();
            while(players.Count() < 10)
            {
                string name = names[r.Next(names.Count())];
                if (!players.Contains(name))
                    players.Add(name);
            }
            for (int j = 0; j < 10; j++)
            {
                bool redteam = j % 2 == 0;
                int g = r.Next(4);
                int a = 0;
                if (redteam)
                {
                    redgoals += g;
                    a = r.Next(redgoals - redassists);
                    redassists += a;
                }
                else
                {
                    bluegoals += g;
                    a = r.Next(bluegoals - blueassists);
                    blueassists += a;
                }

                statlines.Add(new RankedGameReport.PlayerStatLine()
                {
                    Name = players[j],
                    Goals = g,
                    Assists = a,
                    Team = redteam ? "Red" : "Blue",
                    Leaver = false
                });
            }
            if (redgoals == bluegoals) //tie goes to red team
            {
                redgoals++;
                statlines[0].Goals++;
            }

            return new RankedGameReport()
            {
                RedScore = redgoals,
                BlueScore = bluegoals,
                WinningTeam = redgoals > bluegoals ? "Red" : "Blue",
                Date = DateTime.UtcNow,
                ServerName = "Simulated",
                PlayerStats = statlines
            };
        }
    }

    
}
