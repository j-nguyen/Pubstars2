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
      
        public static void SeedUsers(this ApplicationDbContext context)
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
                        PlayerStats = new PlayerStats() { Name = name }
                    };
                    context.Users.Add(user);
                }                               
            }
            context.SaveChanges();            
        }        
    }
}
