using Moserware.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PubstarsModel
{
    public class Game
    {
        [Key]
        public Guid GameId { get; set; }

        public ICollection<PlayerGameStats> playerStats {get; set;}
        public int redScore { get; set; }
        public int blueScore { get; set; }
        public DateTime date { get; set; }

        public HqmTeam winner
        {
            get
            {
                return redScore > blueScore ? HqmTeam.red : HqmTeam.blue;
            }            
        }

        public IDictionary<Player, Rating> GetNewRatings()
        {
            var redTeam = new Team<Player>();
            var blueTeam = new Team<Player>();
            foreach (PlayerGameStats p in playerStats)
            {
                if(p.Team == HqmTeam.red)
                {
                    redTeam.AddPlayer(p.Player, new Moserware.Skills.Rating(p.RatingMean, p.RatingStandardDeviation));
                }
                else if(p.Team == HqmTeam.blue)
                {
                    blueTeam.AddPlayer(p.Player, new Moserware.Skills.Rating(p.RatingMean, p.RatingStandardDeviation));
                }                
            }
            int redrank = winner == HqmTeam.red ? 1 : 2;
            int bluerank = winner == HqmTeam.blue ? 1 : 2;
            var newRatings = TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(redTeam, blueTeam), redrank, bluerank);
            return newRatings.ToDictionary(x => x.Key, x => new Rating() { Mean = x.Value.Mean, StandardDeviation = x.Value.StandardDeviation });                  
        }
    }

    public enum HqmTeam
    {
        red,
        blue
    }
}
