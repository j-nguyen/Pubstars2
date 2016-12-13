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
                return Validate(user, password, player);
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
                    return Validate(u, password, player);
                }               
            }
        }

        private LoginResult Validate(UserDto user, string pw, Player player)
        {
            if (user.Password == pw)
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

        public class LoginResult
        {
            public string Result;
            public RankedPlayer RankedPlayer;
        }
    }
}
