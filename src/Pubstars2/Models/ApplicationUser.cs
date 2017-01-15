using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PubstarsModel;
using System;
using System.ComponentModel;

namespace Pubstars2.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string PubstarsPassword { get; set; }

        public Player PlayerStats { get; set;}

        /*
        [DefaultValue(typeof(DateTime), "")] //DateTime.MinValue
        public DateTime BannedUntil { get; set; }

        public int NumBans { get; set; }
        */
    }
}
