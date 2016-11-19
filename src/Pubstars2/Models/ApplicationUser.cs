using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Pubstars2.Models.Pubstars;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PubstarsPassword { get; set; }

        public PlayerStats PlayerStats { get; set;}
    }
}
