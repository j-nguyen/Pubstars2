using HQMEditorDedicated;
using PubstarsDtos;
using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services
{
    public class LoginManager
    {
        private Dictionary<string, UserData> m_UserInfo;

        public async Task<bool> Init()
        {
            m_UserInfo = await RemoteApi.GetUserData();
            return m_UserInfo != null;
        }

        public async Task<LoginResult> Login(Player player, string password)
        {
            UserData user;
            if(m_UserInfo.TryGetValue(player.Name, out user))
            {
                if (ValidPassword(user, password))
                {
                    return new LoginResult()
                    {
                        Result = user.Name + " - logged in successfully",
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
                UserData u = await RemoteApi.GetUserData(player.Name);
                if(u == null)
                {                    
                    return new LoginResult()
                    {
                        Result =  player.Name+ " does not exist."
                    };
                }
                else
                {
                    m_UserInfo[u.Name] = u;
                    return await Login(player, password);
                }               
            }
        }

        private bool ValidPassword(UserData user, string pw)
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
