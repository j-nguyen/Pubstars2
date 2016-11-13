using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

    }
        
}
