using HQMEditorDedicated;
using PubstarsGameServer.Model;
using PubstarsGameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services
{
    class LoginHandler
    {
        private LoginManager m_LoginManager;
        private CommandListener m_CommandListener;
        private List<Task<LoginManager.LoginResult>> m_LoginTasks;
        private GameContext m_Context;

        public LoginHandler(GameContext context)
        {
            m_Context = context;
            m_LoginManager = new LoginManager();
            m_LoginTasks = new List<Task<LoginManager.LoginResult>>();
            m_CommandListener = new CommandListener(new Dictionary<string, Action<Command>>()
            {
                { "join", Login }
            });
        }

        public async Task Init()
        {
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

        public void HandleLogins()
        {
            RemoveLoggedOutPlayers();
            m_CommandListener.Listen();
            ResolveLoginTasks();                        
        }     

        private void Login(Command cmd)
        {
            if (cmd.Args.Count() > 0)
            {
                if (m_Context.LoggedInPlayers.Select(x => x.Name).Contains(cmd.Sender.Name))
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
            foreach (Task<LoginManager.LoginResult> t in m_LoginTasks.Where(x => x.IsCompleted))
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

        public void RemoveLoggedOutPlayers()
        {
            byte[] playerList = MemoryEditor.ReadBytes(MemoryAddresses.PLAYER_LIST_ADDRESS, 20 * MemoryAddresses.PLAYER_STRUCT_SIZE);
            m_Context.LoggedInPlayers.RemoveAll(p => playerList[p.PlayerStruct.Slot * MemoryAddresses.PLAYER_STRUCT_SIZE] == 0);
        }
    }
}
