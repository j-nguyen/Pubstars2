using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moserware.Skills;
using Pubstars2.Models.Pubstars;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PubstarsPassword { get; set; }

        public PlayerStats PlayerStats { get; set;}
    }
}
