using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pubstars2.Models.Pubstars
{
    public class PlayerGameStats
    {
        public PlayerStats Player { get; set; }

        [Key]
        public Guid PlayerGameStatsId { get; set; }

        public HqmTeam Team { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public double RatingMean { get; set; }
        public double RatingUncertainty { get; set; }
    }
}
