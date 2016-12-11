using HQMEditorDedicated;
using PubstarsGameServer.Data;
using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services
{
    class SubHandler
    {
        private GameContext m_Context;
        private CommandListener m_CommandListener;

        private Queue<LeaveTimer> m_LeaveTimers = new Queue<LeaveTimer>();

        private bool m_FindingReplacement = false;
        private DateTime m_ReplacementSearchStart = DateTime.MaxValue;

        private RankedPlayer m_Leaver;
        private List<RankedPlayer> m_PotentialSubs;

        public SubHandler(GameContext context, CommandListener commandListener)
        {
            m_Context = context;
            m_CommandListener = commandListener;
        }

        public void Update()
        {
            RemoveLoggedOutPlayers();
            UpdateLeaveTimers();

            if (m_FindingReplacement && IsReplacementSearchOver())
            {
                ChooseSub();
            }
        }

        public void RemoveLoggedOutPlayers()
        {
            byte[] playerList = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS, 20 * MemoryAddresses.PLAYER_STRUCT_SIZE);
            var loggedOutPlayers = m_Context.LoggedInPlayers.Where(p => playerList[p.PlayerStruct.Slot * MemoryAddresses.PLAYER_STRUCT_SIZE] == 0).ToList();
            foreach(RankedPlayer p in loggedOutPlayers)
            {
                m_Context.RemovePlayer(p.Name);
                                
                if(m_Context.IsPlaying(p))
                {
                    //leaver found
                    m_Leaver = p;
                    m_LeaveTimers.Enqueue(new LeaveTimer()
                    {
                        Name = p.Name,
                        LeaveTime = DateTime.UtcNow
                    });
                }                
            }
        }

        private void UpdateLeaveTimers()
        {
            if (m_LeaveTimers.Count == 0) return;            

            LeaveTimer timer = m_LeaveTimers.Peek();

            if(m_Context.GetPlayer(timer.Name) != null)
            {
                //player rejoined
                m_LeaveTimers.Dequeue();
                return;
            }

            if(timer.IsExpired())
            {
                timer = m_LeaveTimers.Dequeue();                
                m_Context.Leavers.Add(timer.Name);
                Chat.SendMessage("Leaver Detected: " + timer.Name);

                TriggerReplacementFinder();
            }
        }

        private void TriggerReplacementFinder()
        {
            m_CommandListener.AddCommand("sub", AddSub);
            Chat.SendMessage("Type /sub to join. Sub will be picked in 10 seconds.");
            m_FindingReplacement = true;
            m_ReplacementSearchStart = DateTime.UtcNow;
            m_PotentialSubs = new List<RankedPlayer>();
        }

        private bool IsReplacementSearchOver()
        {
            return DateTime.UtcNow > m_ReplacementSearchStart + new TimeSpan(0, 0, 10);
        }

        void AddSub(Command cmd)
        {
            string name = cmd.Sender.Name;
            if (IsEligibleSub(cmd.Sender))
            {
                m_PotentialSubs.Add(m_Context.GetPlayer(name));
                Chat.SendMessage(">> " + name + " added to sub list");
            }
            else
                Chat.SendMessage(name + " - you must be logged in and not already on a team to sub.");
            
        }

        bool IsEligibleSub(Player p)
        {
            string name = p.Name;
            return m_Context.IsLoggedIn(name, p.Slot) //is logged in
                && !m_Context.RedTeam.Concat(m_Context.BlueTeam).Contains(name); //is not on a team
        }

        void ChooseSub()
        {
            m_CommandListener.RemoveCommand("sub");
            m_FindingReplacement = false;

            if (m_PotentialSubs.Count == 0)
            {
                Chat.SendMessage("No subs found.");
                TriggerReplacementFinder();
                return;
            }

            var sub = ClosestElo();
            if(m_Context.RedTeam.Contains(m_Leaver.Name))
            {
                m_Context.RemovePlayerFromTeam(m_Leaver.Name, HQMTeam.Red);
                m_Context.AddPlayerToTeam(sub.Name, HQMTeam.Red);
                Chat.SendMessage(sub.Name + " added to the Red Team");
            }
            else if(m_Context.BlueTeam.Contains(m_Leaver.Name))
            {
                m_Context.RemovePlayerFromTeam(m_Leaver.Name, HQMTeam.Blue);
                m_Context.AddPlayerToTeam(sub.Name, HQMTeam.Blue);
                Chat.SendMessage(sub.Name + " added to the Blue Team");
            }                      
            else
            {
                throw new Exception(); //something went really wrong.
            }
        }

        RankedPlayer ClosestElo()
        {
            RankedPlayer closestElo = m_PotentialSubs[0];
            foreach (RankedPlayer p in m_PotentialSubs)
            {
                if (Math.Abs(p.Rating - m_Leaver.Rating) < Math.Abs(closestElo.Rating - m_Leaver.Rating))
                    closestElo = p;
            }
            return closestElo;
        }

        class LeaveTimer
        {
            public string Name;
            public DateTime LeaveTime;

            public bool IsExpired()
            {
                return DateTime.UtcNow > LeaveTime + new TimeSpan(0, 0, 30);
            }
        }
    }
}
