using PubstarsModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Pubstars2.Models.PubstarsViewModels
{
    public class GameSummaryViewModel
    {
        [Display(Name="Time")]
        public DateTime time { get; set; }

        [Display(Name = "Red Score")]
        public int redScore { get; set; }

        [Display(Name = "Blue Score")]
        public int blueScore { get; set; }

        public List<StatlineViewModel> redStatLines { get; set; }
        public List<StatlineViewModel> blueStatLines { get; set; }

        public GameSummaryViewModel(Game game)
        {
            List<StatlineViewModel> redstats = new List<StatlineViewModel>();
            List<StatlineViewModel> bluestats = new List<StatlineViewModel>();
            IDictionary<Player, Rating> newRatings = game.GetNewRatings();
            foreach (PlayerGameStats stats in game.playerStats)
            {
                StatlineViewModel vm = new StatlineViewModel()
                {
                    name = stats.Player.Name,
                    goals = stats.Goals.ToString(),
                    assists = stats.Assists.ToString(),
                    ratingChange = Math.Round(newRatings[stats.Player].Mean - stats.RatingMean, 0).ToString(),
                    newRating = Math.Round(newRatings[stats.Player].Mean, 0).ToString()
                };

                if (stats.Team == HqmTeam.red)
                {
                    redstats.Add(vm);
                }
                else
                {
                    bluestats.Add(vm);
                }
            }
            redstats = redstats.OrderByDescending(x => x.newRating).ToList();
            bluestats = bluestats.OrderByDescending(x => x.newRating).ToList();
            time = game.date;
            redScore = game.redScore;
            blueScore = game.blueScore;
            redStatLines = redstats;
            blueStatLines = bluestats;
        }

    }
        
}
