using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string psPassword { get; set; }        
        public double ratingMean { get; set; }
        public double ratingUncertainty { get; set; }
    }
}
