using System;
using System.ComponentModel.DataAnnotations;

namespace PubstarsModel
{
    public class Player
    {
        public Player()
        {
            Rating = new Rating();
            Rating.Mean = Moserware.Skills.GameInfo.DefaultGameInfo.DefaultRating.Mean;
            Rating.StandardDeviation = Moserware.Skills.GameInfo.DefaultGameInfo.DefaultRating.StandardDeviation;
        }

        [Key]
        public Guid PlayerId { get; set;}

        public string Name { get; set; }       

        public Rating Rating { get; set; }
        
    }

    public class Rating
    {
        [Key]
        public Guid RatingId { get; set; }

        public double Mean { get; set; }
        public double StandardDeviation { get; set; }
    }
}
