using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.PubstarsViewModels
{
    public class LeaderboardEntryViewModel
    {
        [Display(Name="Name")]
        public string name { get; set; }

        [Display(Name = "Rating")]
        public int rating { get; set; }

        [Display(Name = "GP")]
        public int gamesPlayed { get; set; }

        [Display(Name = "W")]
        public int wins { get; set; }

        [Display(Name = "L")]
        public int losses { get; set; }

        [Display(Name = "Win%")]
        public int winPercentage { get; set; }

        [Display(Name="PPG")]
        public float pointsPerGame { get; set; }

        [Display(Name="PTS")]
        public int points { get; set; }

        [Display(Name="G")]
        public int goals { get; set; }

        [Display(Name="A")]
        public int assists { get; set; }
    }
}
