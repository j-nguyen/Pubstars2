using Newtonsoft.Json;
using PubstarsDtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace PubstarsClient
{
    public class RemoteApi
    {
        public static Dictionary<string, UserData> AllUserData = new Dictionary<string,UserData>();

        private const string k_Url = "http://www.hqmpubstars.com/";
        private const string username = "";
        private const string password = "";
        private static string s_Jwt = "";
        /// <summary>
        /// Initialized user data from server
        /// </summary>
        /// <returns>User data</returns>
        public static bool GetUserData()
        {
            GetToken();
            var client = new RestClient(k_Url + "UserData/GetUserData");
            var request = new RestRequest(Method.GET);
            request.AddHeader("Authorization", string.Format("Bearer {0}", s_Jwt));
            var response = client.Execute<Dictionary<string, UserData>>(request);
            if(response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AllUserData = response.Data;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Sends the game result to the server
        /// </summary>
        /// <param name="gameReport"></param>
        /// <returns>Updated user data</returns>
        public static bool SendGameResult(RankedGameReport gameReport)
        {
            RestClient client = new RestClient(k_Url+ "Games/ReportGame");
            var request = new RestRequest(Method.POST);
            var json = JsonConvert.SerializeObject(gameReport);
            request.AddHeader("Authorization", string.Format("Bearer {0}", s_Jwt));
            request.AddParameter("application/json", json, ParameterType.RequestBody);
            var response = client.Execute<Dictionary<string, UserData>>(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                AllUserData = response.Data;
                return true;
            }
            Console.WriteLine(response.StatusCode + " - " + response.ErrorMessage);
            return false;
        }

        public static bool GetToken()
        {
            var client = new RestClient(k_Url+"connect/token");
            var request = new RestRequest(Method.POST);
                        request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "username="+username+"&password="+password+"&grant_type=password", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                TokenResponse r = JsonConvert.DeserializeObject<TokenResponse>(response.Content);
                s_Jwt = r.access_token;
                return true;
            }
            Console.WriteLine(response.StatusCode + " - " + response.ErrorMessage);
            return false;
        }

        public class TokenResponse
        {
            public string token_type { get; set; }
            public string access_token { get; set; }
            public int expires_in { get; set; }
        }
    }

   

   

    
}
