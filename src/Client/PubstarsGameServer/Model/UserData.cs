using HQMEditorDedicated;
using PubstarsDtos;
using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Model
{
    public class UserData
    {
        private Dictionary<string, UserDto> m_UserData;

        public void Init(IEnumerable<UserDto> userData)
        {
            m_UserData = userData.ToDictionary(u => u.Name);  
        }

        public async Task<LoginResult> Login(Player player, string password)
        {
            UserDto user;
            if(m_UserData.TryGetValue(player.Name, out user))
            {
                if (ValidPassword(user, password))
                {
                    return new LoginResult()
                    {
                        Result = user.Name + " - logged in",
                        RankedPlayer = new RankedPlayer(user.Name, player.IPAddress, player, user.Rating)
                    };
                }
                else
                {
                    return new LoginResult()
                    {
                        Result = user.Name + " - invalid password"
                    };
                }                
            }
            else
            {
                UserDto u = await RemoteApi.GetUserData(player.Name);
                if(u == null)
                {                    
                    return new LoginResult()
                    {
                        Result =  player.Name+ " does not exist"
                    };
                }
                else
                {
                    m_UserData[u.Name] = u;
                    return await Login(player, password);
                }               
            }
        }

        private bool ValidPassword(UserDto user, string pw)
        {
            return user.Password == pw;
        }

        public class LoginResult
        {
            public string Result;
            public RankedPlayer RankedPlayer;
        }
    }
}
