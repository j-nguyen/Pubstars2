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

        private LoginManager m_LoginManager = new LoginManager();
        private CommandListener m_CommandListener;

        private List<Task<LoginManager.LoginResult>> m_LoginTasks = new List<Task<LoginManager.LoginResult>>();

        private GameContext m_Context; 
        
        public WaitingForPlayers(GameContext context)
        {
            m_Context = context;
        } 


        public async Task OnEnter()
        {
            m_CommandListener = new CommandListener(new Dictionary<string, Action<Command>>()
            {
                { "join", Login }
            });

            Console.WriteLine("WaitingForPlayers - OnEnter");
            if (await m_LoginManager.Init())
            {
                Console.WriteLine("LoginManager initialized successfully.");
            }
            else
            {
                Console.WriteLine("LoginManager failed to initialize.");
                Console.ReadLine();
            }
        }

        private DateTime m_MinPlayersReachedTime = DateTime.MaxValue;
        bool m_MinPlayersReached = false;

        public async Task<bool> Execute()
        {
            m_Context.RemoveLoggedOutPlayers();
            m_CommandListener.Listen();
            ResolveLoginTasks();

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
                Chat.SendMessage("Logins are now closed. Game Starting...");
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

        private void Login(Command cmd)
        {
            if (cmd.Args.Count() > 0)
            {
                if(m_Context.LoggedInPlayers.Select(x=>x.Name).Contains(cmd.Sender.Name))
                {
                    Chat.SendMessage(">> " + cmd.Sender.Name + " is already logged in");
                    return;
                }
                string pw = cmd.Args[0];
                Task<LoginManager.LoginResult> loginTask = m_LoginManager.Login(cmd.Sender, pw);
                m_LoginTasks.Add(loginTask);
            }
        }

        private void ResolveLoginTasks()
        {
            foreach (Task<LoginManager.LoginResult> t in m_LoginTasks.Where(x=>x.IsCompleted))
            {               
                LoginManager.LoginResult result = t.Result;
                Chat.SendMessage(">> " + result.Result);
                if (result.RankedPlayer != null)
                {
                    m_Context.LoggedInPlayers.Add(result.RankedPlayer);
                }                
            }
            m_LoginTasks.RemoveAll(x => x.IsCompleted);
        }
    }
}
