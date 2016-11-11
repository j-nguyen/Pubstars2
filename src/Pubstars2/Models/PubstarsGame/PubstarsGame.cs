using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.PubstarsGame
{
    public class PubstarsGame
    {
        [Key]
        public string gameId { get; set; }

        public ICollection<PubstarsPlayer> players {get; set;}
        public int redScore { get; set; }
        public int blueScore { get; set; }
        public DateTime date { get; set; }
    }

    public enum HqmTeam
    {
        red,
        blue
    }
}
