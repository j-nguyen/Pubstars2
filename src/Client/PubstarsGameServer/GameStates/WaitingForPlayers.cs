using HQMEditorDedicated;
using PubstarsGameServer.Data;
using PubstarsGameServer.Model;
using PubstarsGameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.GameStates
{
    class WaitingForPlayers : IState
    {
              

        private GameContext m_Context;
        private Warden m_Warden;

        public WaitingForPlayers(GameContext context, Warden warden)
        {
            m_Context = context;
            m_Warden = warden;
        } 

        public Task OnEnter()
        {           
            Console.WriteLine("WaitingForPlayers: OnEnter");
            Chat.SendMessage("---------------------------------------------------");
            Chat.SendMessage("   All players have been logged out.");
            Chat.SendMessage(" '/join password' to join the next game.");
            Chat.SendMessage("---------------------------------------------------");
            return Task.FromResult<object>(null);
        }

        private DateTime m_MinPlayersReachedTime = DateTime.MaxValue;
        bool m_MinPlayersReached = false;

        public async Task<bool> Execute()
        {
            if(m_Context.LoggedInPlayers.Count() >= Settings.MIN_PLAYERS && !m_MinPlayersReached)
            {
                await Task.Delay(10); //allow last message to be sent
                Console.WriteLine("WaitingForPlayers: Required player count reached.");
                //reset game
                GameInfo.IntermissionTime = 0;
                GameInfo.IsGameOver = true;

                //wait for game to reset
                await Task.Delay(10);
                m_Warden.Start();
                Chat.SendMessage("---------------------------------------------------");
                Chat.SendMessage("     Required player count reached.");
                Chat.SendMessage("     Game will start in 10 seconds.");
                Chat.SendMessage("---------------------------------------------------");
                m_MinPlayersReached = true;
                m_MinPlayersReachedTime = DateTime.UtcNow;
            }

            if(m_Context.LoggedInPlayers.Count < Settings.MIN_PLAYERS && m_MinPlayersReached)
            {
                Console.WriteLine("WaitingForPlayers: Not enough players. Aborting game.");
                Chat.SendMessage("Not enough players. Aborting game.");
                m_Warden.Stop();
                m_MinPlayersReached = false;
                m_MinPlayersReachedTime = DateTime.MaxValue;
            }

            if(m_MinPlayersReached && DateTime.UtcNow >= m_MinPlayersReachedTime + new TimeSpan(0,0,10))
            {
                Chat.SendMessage("---------------------------------------------------");
                Chat.SendMessage("         Game Starting");
                Chat.SendMessage("---------------------------------------------------");
                return true;
            }
            return false;
        }

        public Task OnExit()
        {
            Console.WriteLine("WaitingForPlayers: OnExit");
            return Task.FromResult<object>(null);
        }

        
    }
}
