using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HQMEditorDedicated;
using PubstarsDtos;

namespace HQMRanked
{
    class LoginManager
    {      
        public static List<RankedPlayer> LoggedInPlayers = new List<RankedPlayer>();

        public static void HandleNewLogins(Command cmd)
        {
            if (cmd.Cmd == "join" && cmd.Args.Length > 0)
            {
                RemoveLoggedOutPlayers();
                if (Login(cmd.Sender, cmd.Args[0]))
                {
                    return;
                }               
            }
        }

        public static bool IsLoggedIn(string name, int slot)
        {
            RankedPlayer p = LoggedInPlayers.FirstOrDefault(x => x.Name == name);
            return (p != null && slot == p.PlayerStruct.Slot);
        }

        static bool Login(Player player, string password)
        {
            UserData u;
            if(UserSaveData.AllUserData.TryGetValue(player.Name, out u))
            {
                RankedPlayer rankedPlayer = new RankedPlayer(player.Name, player.IPAddress, player, u);
                if(LoggedInPlayers.FirstOrDefault(x=> x.Name == player.Name) != null)
                {
                    Chat.SendMessage(">> "+u.Name + " is already logged in.");
                }
                /*
                else if(LoggedInPlayers.FirstOrDefault(x=> x.IP.SequenceEqual(player.IPAddress)) != null && !player.IsAdmin)
                {
                    string name = LoggedInPlayers.FirstOrDefault(x => x.IP.SequenceEqual(player.IPAddress)).Name;
                    Chat.SendMessage(">> Failed to log in "+u.Name);
                    Chat.SendMessage(">> " + name + " is already logged in from that IP.");
                }*/
                else if(u.Password == password)
                {
                    Chat.SendMessage(">> "+u.Name+" is now logged in.");
                    LoggedInPlayers.Add(rankedPlayer);                    
                } 
                else
                {
                    Chat.SendMessage(">> "+u.Name + " - wrong password.");
                }                
                return true;
            }
            return false;
        }
    

        public static void RemoveLoggedOutPlayers()
        {
            byte[] playerList = MemoryEditor.ReadBytes(Util.PLAYER_LIST_ADDRESS, Util.MAX_PLAYERS * Util.PLAYER_STRUCT_SIZE);
            LoggedInPlayers.RemoveAll(p => playerList[p.PlayerStruct.Slot * Util.PLAYER_STRUCT_SIZE] == 0);           
        }
    }    

    public class RankedPlayer
    {
        public string Name;
        public byte[] IP;
        public Player PlayerStruct;
        public UserData UserData;
        public HQMTeam AssignedTeam = HQMTeam.NoTeam;
        public bool PlayedLastGame = false;

        public RankedPlayer(string n, byte[] ip, Player p, UserData u)
        {
            Name = n;
            IP = ip;
            PlayerStruct = p;
            UserData = u;
        }
    }


}
