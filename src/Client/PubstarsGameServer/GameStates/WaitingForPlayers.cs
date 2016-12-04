using HQMEditorDedicated;
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
        private const int MIN_PLAYERS = 2;       

        private GameContext m_Context; 
        
        public WaitingForPlayers(GameContext context)
        {
            m_Context = context;
        } 

        public Task OnEnter()
        {           
            Console.WriteLine("WaitingForPlayers - OnEnter");
            Chat.SendMessage("---------------------------------------------------");
            Chat.SendMessage("   All players have been logged out.");
            Chat.SendMessage(" '/join mypassword' to join the next game.");
            Chat.SendMessage("---------------------------------------------------");
            return Task.FromResult<object>(null);
        }

        private DateTime m_MinPlayersReachedTime = DateTime.MaxValue;
        bool m_MinPlayersReached = false;

        public async Task<bool> Execute()
        {
            if(m_Context.LoggedInPlayers.Count >= MIN_PLAYERS && !m_MinPlayersReached)
            {
                Console.WriteLine("Required player count reached");
                //reset game
                GameInfo.IntermissionTime = 0;
                GameInfo.IsGameOver = true;

                //wait for game to reset
                await Task.Delay(10);
                Warden.Instance.Start();
                Chat.SendMessage("---------------------------------------------------");
                Chat.SendMessage("     Required player count reached.");
                Chat.SendMessage("     Game will start in 10 seconds.");
                Chat.SendMessage("---------------------------------------------------");
                m_MinPlayersReached = true;
                m_MinPlayersReachedTime = DateTime.Now;
            }

            if(m_Context.LoggedInPlayers.Count < MIN_PLAYERS && m_MinPlayersReached)
            {
                Console.WriteLine("Not enough players. Aborting game.");
                Chat.SendMessage("Not enough players. Aborting game.");
                Warden.Instance.Stop();
                m_MinPlayersReached = false;
                m_MinPlayersReachedTime = DateTime.MaxValue;
            }

            if(m_MinPlayersReached && DateTime.Now >= m_MinPlayersReachedTime + new TimeSpan(0,0,10))
            {
                Chat.SendMessage("---------------------------------------------------");
                Chat.SendMessage("     Game Starting...");
                Chat.SendMessage("---------------------------------------------------");
                return true;
            }
            return false;
        }

        public Task OnExit()
        {
            Console.WriteLine("WaitingForPlayers - OnExit");
            return Task.FromResult<object>(null);
        }

        
    }
}
