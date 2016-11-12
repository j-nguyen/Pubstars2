using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.PubstarsViewModels
{
    public class StatlineViewModel
    {
        string name { get; set; }
        int goals { get; set; }
        int assists { get; set; }
        double ratingChange { get; set; }
        double newRating { get; set; }
    }
}
