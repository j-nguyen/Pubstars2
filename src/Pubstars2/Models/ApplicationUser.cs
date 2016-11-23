using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PubstarsModel;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PubstarsPassword { get; set; }

        public Player PlayerStats { get; set;}
    }
}
