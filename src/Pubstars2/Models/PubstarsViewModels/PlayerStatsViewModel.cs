using System;
using System.ComponentModel.DataAnnotations;

namespace Pubstars2.Models.PubstarsViewModels
{
    public class PlayerStatsViewModel
    {
        [Display(Name="Name")]
        public string Name { get; set; }

        [Display(Name = "Rating")]
        public string Rating { get; set; }

        [Display(Name = "GP")]
        public int GamesPlayed { get; set; }

        [Display(Name = "W")]
        public int Wins { get; set; }

        [Display(Name = "L")]
        public int Losses { get; set; }

        [Display(Name = "Win%")]
        public double WinPercentage { get; set; }

        [Display(Name="PPG")]
        public double PointsPerGame { get; set; }

        [Display(Name="PTS")]
        public int Points { get; set; }

        [Display(Name="G")]
        public int Goals { get; set; }

        [Display(Name="A")]
        public int Assists { get; set; }

        public PlayerStatsViewModel(string name, string rating, int goals, int assists, int gamesplayed, int wins)
        {
            Name = name;
            Rating = rating;
            GamesPlayed = gamesplayed;
            Wins = wins;
            Losses = gamesplayed - wins;
            WinPercentage = Math.Round(wins / (double)gamesplayed, 3);
            PointsPerGame = Math.Round((goals + assists) / (double)gamesplayed, 2);
            Points = goals + assists;
            Goals = goals;
            Assists = assists;
        }
    }
}
