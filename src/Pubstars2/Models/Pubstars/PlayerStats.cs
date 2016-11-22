using Moserware.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.Pubstars
{
    public class PlayerStats
    {
        public PlayerStats()
        {
            RatingMean = GameInfo.DefaultGameInfo.DefaultRating.Mean;
            RatingUncertainty = GameInfo.DefaultGameInfo.DefaultRating.StandardDeviation;
        }

        [Key]
        public Guid PlayerStatsId { get; set;}

        public string Name { get; set; }

        //below can be calculated from game history. 
        //Materialized view? - cache and update when needed
        //calculate every request?
        public int GamesPlayed { get; set; }
        public int Wins { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }

        public double RatingMean { get; set; }
        public double RatingUncertainty { get; set; }
        
    }
}
