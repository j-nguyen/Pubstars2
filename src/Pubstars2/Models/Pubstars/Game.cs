using Moserware.Skills;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pubstars2.Models.Pubstars
{
    public class Game
    {
        [Key]
        public Guid gameId { get; set; }

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

        public IDictionary<PlayerStats, Rating> GetNewRatings()
        {
            var redTeam = new Team<PlayerStats>();
            var blueTeam = new Team<PlayerStats>();
            foreach (PlayerGameStats p in playerStats)
            {
                if(p.Team == HqmTeam.red)
                {
                    redTeam.AddPlayer(p.Player, new Rating(p.RatingMean, p.RatingUncertainty));
                }
                else if(p.Team == HqmTeam.blue)
                {
                    blueTeam.AddPlayer(p.Player, new Rating(p.RatingMean, p.RatingUncertainty));
                }                
            }
            int redrank = winner == HqmTeam.red ? 1 : 2;
            int bluerank = winner == HqmTeam.blue ? 1 : 2;
            return TrueSkillCalculator.CalculateNewRatings(GameInfo.DefaultGameInfo, Teams.Concat(redTeam, blueTeam), redrank, bluerank);            
        }
    }

    public enum HqmTeam
    {
        red,
        blue
    }
}
