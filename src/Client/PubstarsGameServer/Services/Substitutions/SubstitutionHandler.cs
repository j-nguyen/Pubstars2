using HQMEditorDedicated;
using PubstarsGameServer.Data;
using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services.Substitutions
{
    class SubstitutionHandler
    {      
        private GameContext m_Context;
        private CommandListener m_CommandListener;
        private List<LeaveTimer> m_LeaveTimers = new List<LeaveTimer>();
        private List<SubSearch> m_SubSearches = new List<SubSearch>();

        private List<RankedPlayer> m_PotentialSubs = new List<RankedPlayer>();        

        public SubstitutionHandler(GameContext context, CommandListener commandListener)
        {
            m_Context = context;
            m_CommandListener = commandListener;
        }

        public void Update()
        {
            RemoveLoggedOutPlayers();

            if(m_LeaveTimers.Count > 0)
            {
                List<LeaveTimer> toRemove = new List<LeaveTimer>();
                foreach(LeaveTimer t in m_LeaveTimers)
                {                    
                    if(t.IsExpired())
                    {                                                     
                        m_CommandListener.AddCommand("sub", AddToSubList);
                        Chat.SendMessage(">> Leaver Detected: " + t.Leaver.Name);
                        Chat.SendMessage(">> Type /sub to join.");
                        m_SubSearches.Add(new SubSearch(t.Leaver));
                        toRemove.Add(t);
                    }
                    else if (m_Context.GetPlayer(t.Leaver.Name) != null)
                    {
                        //player rejoined
                        m_Context.AddPlayerToTeam(t.Leaver.Name, t.Leaver.TeamLeft);
                        toRemove.Add(t);
                    }                   
                }
                m_LeaveTimers.RemoveAll(x => toRemove.Contains(x));
            }

            if(m_SubSearches.Count > 0)
            {
                List<SubSearch> done = new List<SubSearch>();
                bool sentNoSubMessage = false;
                foreach(SubSearch s in m_SubSearches)
                {                    
                    if (s.IsExpired())
                    {
                        if(m_PotentialSubs.Count > 0)
                        {
                            RankedPlayer p = s.ClosestElo(m_PotentialSubs);
                            m_Context.RemovePlayerFromTeam(p.Name);
                            m_Context.AddPlayerToTeam(p.Name, s.Team);
                            Chat.SendMessage(">> " + p.Name + " added to " + s.Team.ToString() + " team.");
                            done.Add(s);
                        }
                        else
                        {
                            if(!sentNoSubMessage)
                            {
                                Chat.SendMessage(">> No Sub Found.");
                                Chat.SendMessage(">> Type /sub to join.");
                                sentNoSubMessage = true;
                            }
                            
                            s.ResetSearchTime();
                        }
                    }                   
                }
                m_SubSearches.RemoveAll(x => done.Contains(x));

                if(m_SubSearches.Count == 0)
                {
                    m_PotentialSubs = new List<RankedPlayer>();
                    m_CommandListener.RemoveCommand("sub");
                }
            }
        } 

        void AddToSubList(Command cmd)
        {
            string name = cmd.Sender.Name;
            if (IsEligibleSub(cmd.Sender))
            {
                m_PotentialSubs.Add(m_Context.GetPlayer(name));
                Chat.SendMessage(">> " + name + " added to sub list");
            }
            else
                Chat.SendMessage(">> "+ name + " - you must be logged in and not already on a team to sub.");
        }

        bool IsEligibleSub(Player p)
        {
            string name = p.Name;
            return m_Context.IsLoggedIn(name, p.Slot) //is logged in
                && m_Context.GetPlayer(name).Team == HQMTeam.NoTeam; //is not on a team
        }

        public void RemoveLoggedOutPlayers()
        {
            byte[] playerList = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS, 20 * MemoryAddresses.PLAYER_STRUCT_SIZE);

            var loggedOutPlayers = m_Context.LoggedInPlayers
                .Where(p => playerList[p.PlayerStruct.Slot * MemoryAddresses.PLAYER_STRUCT_SIZE] == 0)
                .ToList();

            foreach (RankedPlayer p in loggedOutPlayers)
            {
                m_Context.RemovePlayer(p.Name);
                m_PotentialSubs.RemoveAll(x => x.Name == p.Name);
                if(m_Context.IsPlaying(p))
                {
                    m_Context.Leavers.Add(p.Name);
                    m_LeaveTimers.Add
                    ( 
                        new LeaveTimer()
                        {
                            Leaver = new Leaver() { Name = p.Name, Rating = p.Rating, TeamLeft = p.Team }                      
                        }
                    );
                }     
            }
        }







    }
}
