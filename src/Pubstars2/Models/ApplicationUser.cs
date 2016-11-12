using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pubstars2.Models.PubstarsStats;
using System.Collections.Generic;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string psPassword { get; set; }        
        public double ratingMean { get; set; }
        public double ratingUncertainty { get; set; }
    }
}
