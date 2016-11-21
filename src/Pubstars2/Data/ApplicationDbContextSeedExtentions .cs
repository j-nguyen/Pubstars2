using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Pubstars2.Data;
using Pubstars2.Models.Pubstars;
using PubstarsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models
{
    public static class ApplicationDbContextSeedExtentions 
    {
      
        public static async void SeedUsers(this ApplicationDbContext context)
        {    
            for(int i = 0; i < 25; i++)
            {
                string name = "player" + i;
                if (context.Users.FirstOrDefault(x=>x.UserName == name) == null)
                {
                    ApplicationUser user = new ApplicationUser()
                    {
                        UserName = name,
                        PubstarsPassword = "test",
                        PlayerStats = new Pubstars.PlayerStats()
                    };

                    context.Users.Add(user);
                }
                               
            }
            
        }

        public static async void SeedGames(this ApplicationDbContext context)
        {
            
        }
    }
}
