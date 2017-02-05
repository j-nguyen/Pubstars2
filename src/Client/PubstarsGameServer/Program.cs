using HQMEditorDedicated;
using PubstarsGameServer.Data;
using PubstarsGameServer.GameStates;
using PubstarsGameServer.Model;
using PubstarsGameServer.Services;
using PubstarsGameServer.Services.Logging;
using PubstarsGameServer.Services.Substitutions;
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

            LoggerFactory loggerFactory = new LoggerFactory();
            loggerFactory.AddProvider(new ConsoleLogger());
            loggerFactory.AddProvider(new FileLogger());

            sm.Init(new Init()).Wait();
          
            while (true)
            {
                IEnumerable<string> lastGamePlayers = context?.LoggedInPlayers
                    .Where(x=>x.Team != HQMTeam.NoTeam)
                    .Select(x=>x.Name) ?? new List<string>();

                context = new GameContext();

                Warden warden = new Warden(context);
                CommandListener commandListener = new CommandListener();
                LoginHandler loginHandler = new LoginHandler(context, commandListener);
                SubstitutionHandler subHandler = new SubstitutionHandler(context, commandListener);
                
                loginHandler.Init().Wait();
    
                sm.AddState(new WaitingForPlayers(context, warden));
                sm.AddState(new GameSetup(context, lastGamePlayers));
                sm.AddState(new Gameplay());
                sm.AddState(new EndGame(context, warden));

                while (!sm.Update().Result)
                {
                    Thread.Sleep(50);
                    loginHandler.HandleLogins();
                    subHandler.Update();
                    warden.Watch();         
                };
            }            
        }
    }
}
