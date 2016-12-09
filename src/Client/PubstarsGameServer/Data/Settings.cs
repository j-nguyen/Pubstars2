using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace PubstarsGameServer.Data
{
    static class Settings
    {
        const string k_Mercy = "mercy_diff";
        const string k_MinPlayers = "min_players";

        public static int MERCY_DIFF { get; private set; }
        public static int MIN_PLAYERS { get; private set; }

        public static void InitDefaults()
        {
            AppSettingsReader reader = new AppSettingsReader();
            MERCY_DIFF = (int)reader.GetValue(k_Mercy, typeof(int));
            MIN_PLAYERS = (int)reader.GetValue(k_MinPlayers, typeof(int));
        }
    }  
}
