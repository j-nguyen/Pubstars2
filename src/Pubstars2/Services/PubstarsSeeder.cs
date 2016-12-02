using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Pubstars2.Data;
using Pubstars2.Models;
using PubstarsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Services
{
    public class PubstarsSeeder
    {
        UserManager<ApplicationUser> _db;
        RoleManager<IdentityRole> _rm;
        IConfiguration _config;
       

        public PubstarsSeeder(UserManager<ApplicationUser> db, IConfiguration config, RoleManager<IdentityRole> rm) 
        {
            _db = db;
            _config = config;
            _rm = rm;
        }

        public async Task SeedPlayers()
        {
            for (int i = 0; i < 2; i++)
            {
                string name = "player" + i;         
                    
                ApplicationUser user = new ApplicationUser()
                {
                    UserName = name,
                    PubstarsPassword = "test",
                    PlayerStats = new Player() { Name = name }
                };
                if (!_db.Users.Contains(user))
                    await _db.CreateAsync(user);                
            }
            await CreateClientUser();
        }

        public async Task CreateClientUser()
        {
            IdentityRole role = new IdentityRole("client");
            if (!_rm.Roles.Contains(role))
            {
                await _rm.CreateAsync(role);
            }
            string name = _config.GetValue<string>("clientUsername");
            string pw =  _config.GetValue<string>("clientPassword");
            ApplicationUser client = new ApplicationUser()
            {
                UserName = name 
            };
            if(await _db.FindByNameAsync(name) == null)
            {
                await _db.CreateAsync(client, pw);
                await _db.AddToRoleAsync(client, role.Name);
            }
           
        }

       
    }
}
