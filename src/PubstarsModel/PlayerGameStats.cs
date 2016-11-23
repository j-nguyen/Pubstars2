using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PubstarsModel
{
    public class PlayerGameStats
    {
        [Key]
        public Guid PlayerGameStatsId { get; set; }

        public Player Player { get; set; }      

        public HqmTeam Team { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public double RatingMean { get; set; }
        public double RatingStandardDeviation { get; set; }

        public Guid GameId { get; set; }
        public virtual Game Game { get; set; }
    }
}
