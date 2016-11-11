using PubstarsDtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace HQMRanked
{
    public class RemoteApi
    {
        public static Dictionary<string, UserData> AllUserData = new Dictionary<string,UserData>();

        private const string k_Url = "http://localhost:5000/";
        /// <summary>
        /// Initialized user data from server
        /// </summary>
        /// <returns>User data</returns>
        public static bool GetUserData()
        {
            var client = new RestClient(k_Url + "UserData/GetUserData");
            var request = new RestRequest(Method.GET);             
            var response = client.Execute<Dictionary<string, UserData>>(request);
            AllUserData = response.Data;
            return true;
        }

        /// <summary>
        /// Sends the game result to the server
        /// </summary>
        /// <param name="result"></param>
        /// <returns>Updated user data</returns>
        public static bool SendGameResult(RankedGameReport result)
        {
            RestClient client = new RestClient(k_Url+ "GameReports/PostGameResult");
            var request = new RestRequest(Method.POST);
            request.AddObject(result);
            var response = client.Execute(request);
            return response.StatusCode == System.Net.HttpStatusCode.OK;
        }
    }

   

   

    
}
