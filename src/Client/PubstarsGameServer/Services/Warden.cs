using HQMEditorDedicated;
using PubstarsGameServer.Data;
using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services
{
    class Warden
    {
        private const int MAX_PLAYERS = 20;

        private GameContext m_Context;
        private CancellationTokenSource m_TokenSource;
        bool m_Watching = false;

        public Warden(GameContext context)
        {
            m_Context = context;                      
        }

        public void Start()
        {
            m_TokenSource = new CancellationTokenSource();
            m_Watching = true;
            Task.Run(Watch, m_TokenSource.Token);          
        }

        public void Stop()
        {
            m_Watching = false;
            m_TokenSource.Cancel();
            m_TokenSource.Dispose();     
        }

        private async Task Watch()
        {
            while(m_Watching)
            {
                byte[] playerList = GetPlayerListMemoryBlock();
                for (int i = 0; i < MAX_PLAYERS; i++)
                {
                    if (playerList[i * MemoryAddresses.PLAYER_STRUCT_SIZE] == 1)//in server
                    {
                        HQMTeam t = (HQMTeam)playerList[i * MemoryAddresses.PLAYER_STRUCT_SIZE + MemoryAddresses.TEAM_OFFSET];

                        IEnumerable<byte> namebytes = playerList.Skip(i * MemoryAddresses.PLAYER_STRUCT_SIZE + 0x14).Take(0x18);
                        string name = Encoding.ASCII.GetString(namebytes.ToArray()).Split('\0')[0];

                        if (t != HQMTeam.NoTeam && (int)t != 255) //if on the ice
                        {
                            if (!OnRightTeam(t, name, i))
                            {
                                int team = (int)t;
                                while (team != 255)
                                {
                                    ForceLeaveIce(i);
                                    team = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS + i * MemoryAddresses.PLAYER_STRUCT_SIZE + MemoryAddresses.TEAM_OFFSET, 1)[0];                                 
                                }

                            }
                        }
                    }
                    await Task.Yield();
                }
            }      
        }  

        private bool OnRightTeam(HQMTeam t, string name, int slot)
        {
            RankedPlayer p = m_Context.GetPlayer(name);

            if (p == null) return false;

            else return m_Context.IsLoggedIn(name, slot) 
                && ((t == HQMTeam.Blue && p.Team == HQMTeam.Blue) 
                || (t == HQMTeam.Red && p.Team == HQMTeam.Red));
        }

        private void ForceLeaveIce(int slot)
        {
            MemoryEditor.WriteInt(32, MemoryAddresses.PLAYER_LIST_ADDRESS + slot * MemoryAddresses.PLAYER_STRUCT_SIZE + MemoryAddresses.LEG_STATE_OFFSET);
        }

        byte[] GetPlayerListMemoryBlock()
        {
            return MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS, MAX_PLAYERS * MemoryAddresses.PLAYER_STRUCT_SIZE);
        }
    }
}
