using Newtonsoft.Json;
using PubstarsDtos;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer
{
    public class RemoteApi
    {
        private const string k_Url = "http://localhost:5000/";
        private const string username = "pubstars_client";
        private const string password = "64dQz8pRGxCPMjqc";
        private static string s_Jwt = "";
        /// <summary>
        /// Initialized user data from server
        /// </summary>
        /// <returns>User data</returns>
        public static async Task<Dictionary<string, UserData>> GetUserData()
        {
            await GetToken();
            var client = new RestClient(k_Url + "UserData/GetUserData");
            var request = new RestRequest(Method.GET);

            request.AddHeader("Authorization", string.Format("Bearer {0}", s_Jwt));

            var response = await client.ExecuteTaskAsync<Dictionary<string, UserData>>(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("GetUserData successful.");
                return response.Data;
            }
            else
            {
                Console.WriteLine("http error: " + response.StatusCode + " - " + response.Content);
                return null;
            }
        }

        public static async Task<UserData> GetUserData(string name)
        {
            await GetToken();
            //go fetch user
            return null;
        }

        /// <summary>
        /// Sends the game result to the server
        /// </summary>
        /// <param name="gameReport"></param>
        /// <returns>Updated user data</returns>
        public static async Task<bool> SendGameResult(RankedGameReport gameReport)
        {
            RestClient client = new RestClient(k_Url + "Games/ReportGame");
            var request = new RestRequest(Method.POST);
            var json = JsonConvert.SerializeObject(gameReport);

            request.AddHeader("Authorization", string.Format("Bearer {0}", s_Jwt));
            request.AddParameter("application/json", json, ParameterType.RequestBody);

            var response = await client.ExecuteTaskAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                Console.WriteLine("Sent game successfully: " + gameReport.Date);
                return true;
            }
            Console.WriteLine(response.StatusCode + " - " + response.ErrorMessage);
            return false;
        }

        public static async Task<bool> GetToken()
        {
            var client = new RestClient(k_Url + "connect/token");
            var request = new RestRequest(Method.POST);
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddParameter("application/x-www-form-urlencoded", "username=" + username + "&password=" + password + "&grant_type=password", ParameterType.RequestBody);
            IRestResponse response = await client.ExecuteTaskAsync(request);
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
