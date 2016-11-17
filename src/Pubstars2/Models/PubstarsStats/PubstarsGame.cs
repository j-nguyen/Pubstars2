using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pubstars2.Models.PubstarsStats
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
