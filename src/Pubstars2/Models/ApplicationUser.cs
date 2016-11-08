using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string steamId { get; set; }
    }
}
