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
        private bool m_Running = false;

        private Thread m_WardenThread;

        public Warden(GameContext context)
        {
            m_Context = context;
        }

        public void Start()
        {
            if(!m_Running)
            {
                m_WardenThread = new Thread(Run);
                m_Running = true;
                m_WardenThread.Start();
            }            
        }

        public void Stop()
        {
            if(m_Running)
            {
                m_Running = false;
                m_WardenThread.Join();
            }          
        }

        private void Run()
        {
            while(m_Running)
            {
                byte[] playerList = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS, MAX_PLAYERS * MemoryAddresses.PLAYER_STRUCT_SIZE);
                for (int i = 0; i < MAX_PLAYERS; i++)
                {
                    if (playerList[i * MemoryAddresses.PLAYER_STRUCT_SIZE] == 1)//in server
                    {
                        HQMTeam t = (HQMTeam)playerList[i * MemoryAddresses.PLAYER_STRUCT_SIZE + MemoryAddresses.TEAM_OFFSET];

                        IEnumerable<byte> namebytes = playerList.Skip(i * MemoryAddresses.PLAYER_STRUCT_SIZE + 0x14).Take(0x18);
                        string name = Encoding.ASCII.GetString(namebytes.ToArray()).Split('\0')[0];

                        if (t != HQMTeam.NoTeam && (int)t != 255) //if on the ice
                        {
                            bool onRightTeam = (((t == HQMTeam.Blue && m_Context.BlueTeam.Contains(name)) || (t == HQMTeam.Red && m_Context.RedTeam.Contains(name))) && m_Context.IsLoggedIn(name, i));
                            if (!onRightTeam)
                            {
                                int team = (int)t;
                                while (team != 255)
                                {
                                    MemoryEditor.WriteInt(32, MemoryAddresses.PLAYER_LIST_ADDRESS + i * MemoryAddresses.PLAYER_STRUCT_SIZE + MemoryAddresses.LEG_STATE_OFFSET);
                                    team = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS + i * MemoryAddresses.PLAYER_STRUCT_SIZE + MemoryAddresses.TEAM_OFFSET, 1)[0];
                                }

                            }
                        }
                    }                    
                    Thread.Sleep(100); //performance
                }
            }            
        }  
    }
}
