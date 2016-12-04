using PubstarsGameServer.GameStates;
using PubstarsGameServer.Model;
using PubstarsGameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer
{
    class Program
    {
        static void Main(string[] args)
        {
            StateMachine sm = new StateMachine();

            //add global states

            sm.Init(new Init()).Wait();

            GameContext context = null;

            while (true)
            {
                IEnumerable<string> lastGamePlayers = context?.Players.Select(x => x.Name);

                context = new GameContext();
                Warden warden = new Warden(context);

                sm.AddState(new WaitingForPlayers(context));
                sm.AddState(new GameSetup(context, lastGamePlayers));
                sm.AddState(new Gameplay());
                sm.AddState(new EndGame());
                while(sm.Update().Result);
            }            
        }
    }
}
