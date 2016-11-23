using Pubstars2.Data;
using PubstarsModel;
using System.Linq;

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
                        PlayerStats = new Player() { Name = name }
                    };
                    context.Users.Add(user);
                }                               
            }
            context.SaveChanges();            
        }        
    }
}
