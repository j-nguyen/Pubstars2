using HQMEditorDedicated;
using PubstarsGameServer.Data;
using PubstarsGameServer.GameStates;
using PubstarsGameServer.Model;
using PubstarsGameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubstarsGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Settings.InitDefaults();
            GameContext context = null;
            StateMachine sm = new StateMachine();
            
            sm.Init(new Init()).Wait();
          
            while (true)
            {
                IEnumerable<string> lastGamePlayers = context?.LoggedInPlayers.Where(x=>x.Team != HQMTeam.NoTeam).Select(x=>x.Name) ?? new List<string>();

                context = new GameContext();

                Warden warden = new Warden(context);
                CommandListener commandListener = new CommandListener();
                LoginHandler loginHandler = new LoginHandler(context, commandListener);
                SubHandler subHandler = new SubHandler(context, commandListener);
                
                loginHandler.Init().Wait();
    
                sm.AddState(new WaitingForPlayers(context, warden));
                sm.AddState(new GameSetup(context, lastGamePlayers));
                sm.AddState(new Gameplay());
                sm.AddState(new EndGame(context, warden));

                while (!sm.Update().Result)
                {
                    Thread.Sleep(100);
                    loginHandler.HandleLogins();
                    subHandler.Update();                
                };

                GC.Collect();
            }            
        }
    }
}
