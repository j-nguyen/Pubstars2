using PubstarsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace HQMRanked
{
    public class UserSaveData
    {
        public static Dictionary<string, UserData> AllUserData = new Dictionary<string,UserData>();

        /// <summary>
        /// Initialized user data from server
        /// </summary>
        /// <returns>User data</returns>
        public static async Task<Dictionary<string, UserData>> Init()
        {
            await Task.Delay(1);
            return new Dictionary<string, UserData>();
        }

        /// <summary>
        /// Sends the game result to the server
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Updated user data</returns>
        public static async Task<Dictionary<string, UserData>> PostGameResult(RankedGameReport result)
        {
            await Task.Delay(1);
            return new Dictionary<string, UserData>();
        }
    }

   

   

    
}
