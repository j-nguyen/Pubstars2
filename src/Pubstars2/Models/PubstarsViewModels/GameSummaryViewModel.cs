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
        public DateTime Time { get; set; }

        [Display(Name = "Red Score")]
        public int RedScore { get; set; }

        [Display(Name = "Blue Score")]
        public int BlueScore { get; set; }

        public double BlueAvgRating { get; set; }
        public double RedAvgRating { get; set; }
        

        public List<StatlineViewModel> RedStatLines { get; set; }
        public List<StatlineViewModel> BlueStatLines { get; set; }

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

            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTime(game.date, TimeZoneInfo.Utc, easternZone);

            redstats = redstats.OrderByDescending(x => x.newRating).ToList();
            bluestats = bluestats.OrderByDescending(x => x.newRating).ToList();
            Time = easternTime;
            RedScore = game.redScore;
            BlueScore = game.blueScore;
            RedStatLines = redstats;
            BlueStatLines = bluestats;

            BlueAvgRating = Math.Round(game.AvgRating(HqmTeam.blue));
            RedAvgRating = Math.Round(game.AvgRating(HqmTeam.red));
        }

    }
        
}
