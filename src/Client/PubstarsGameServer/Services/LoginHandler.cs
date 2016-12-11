using HQMEditorDedicated;
using PubstarsGameServer.Data;
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
        private UserData m_UserData;
        private CommandListener m_CommandListener;        
        private GameContext m_Context;

        private List<Task<UserData.LoginResult>> m_LoginTasks;

        public LoginHandler(GameContext context, CommandListener cmdListener)
        {
            m_Context = context;
            m_CommandListener = cmdListener;
            m_UserData = new UserData();
            

            m_LoginTasks = new List<Task<UserData.LoginResult>>();          
        }

        public async Task Init()
        {
            var userData = await RemoteApi.GetUserData();
            if (userData != null)
            {
                m_UserData.Init(userData);
                m_CommandListener.AddCommand("join", Login);
                m_CommandListener.AddCommand("info", Info);
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
                Task<UserData.LoginResult> loginTask = m_UserData.Login(cmd.Sender, pw);
                m_LoginTasks.Add(loginTask);
            }
        }

        private void ResolveLoginTasks()
        {
            foreach (Task<UserData.LoginResult> t in m_LoginTasks.Where(x => x.IsCompleted))
            {
                UserData.LoginResult result = t.Result;
                Chat.SendMessage(">> " + result.Result);
                if (result.RankedPlayer != null)
                {
                    m_Context.LoggedInPlayers.Add(result.RankedPlayer);
                }
            }
            m_LoginTasks.RemoveAll(x => x.IsCompleted);
        }

        private void Info(Command cmd)
        {
            Chat.SendMessage(">> "+ m_Context.LoggedInPlayers.Count + " players logged in.");

        }

      
    }
}
