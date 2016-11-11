using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsDtos
{
    public class RankedGameReport
    {      
        public List<PlayerStatLine> PlayerStats { get; set; }
        public int RedScore { get; set; }
        public int BlueScore { get; set; }
        public string WinningTeam { get; set; }
        public DateTime Date { get; set; }
    }

  


}
