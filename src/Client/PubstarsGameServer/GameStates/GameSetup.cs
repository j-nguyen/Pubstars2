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
    class GameSetup : IState
    {
        private GameContext m_Context;
        private IEnumerable<string> m_LastGamePlayers;
      

        public GameSetup(GameContext context, IEnumerable<string> lastGamePlayers)
        {
            m_Context = context;
            m_LastGamePlayers = lastGamePlayers;
        }

        public Task OnEnter()
        {
            Console.WriteLine("GameSetup - OnEnter");

            CreateTeams();
            return Task.FromResult<object>(null);
        }

        public Task<bool> Execute()
        {
            Console.WriteLine("GameSetup - Execute");
            return Task.FromResult<bool>(true);
        }        

        public Task OnExit()
        {
            Console.WriteLine("GameSetup - OnExit");
            return Task.FromResult<object>(null);
        }

        //TODO: clean this up
        private void CreateTeams()
        {          
            List<string> redTeam = new List<string>();
            List<string> blueTeam = new List<string>();

            //give prio to people who didn't play last game
            List<RankedPlayer> players = new List<RankedPlayer>();
            List<RankedPlayer> others = new List<RankedPlayer>();
            foreach (RankedPlayer p in m_Context.LoggedInPlayers)
            {
                if (m_LastGamePlayers.Contains(p.Name))
                    others.Add(p);
                else if (players.Count < 10)
                    players.Add(p);
            }

            Random r = new Random();
            while (players.Count < Math.Min(10, m_Context.LoggedInPlayers.Count))
            {
                RankedPlayer newPlayer = others[r.Next(others.Count)];
                others.Remove(newPlayer);
                players.Add(newPlayer);
            }

            List<RankedPlayer> SortedRankedPlayers = players.OrderByDescending(x => x.Rating).ToList();
            /*split up goalies
            List<RankedPlayer> goalies = SortedRankedPlayers.Where(x => x.PlayerStruct.Role == HQMRole.G).ToList();
            if(goalies.Count >= 2)
            {
                RedTeam.Add(goalies[0].Name);
                goalies[0].AssignedTeam = HQMTeam.Red;            
                BlueTeam.Add(goalies[1].Name);
                goalies[1].AssignedTeam = HQMTeam.Blue;
                SortedRankedPlayers.Remove(goalies[0]);
                SortedRankedPlayers.Remove(goalies[1]);
            }*/

            double half_max = Math.Ceiling((double)SortedRankedPlayers.Count() / 2);

            for (int i = 0; i < SortedRankedPlayers.Count; i++)
            {
                RankedPlayer p = SortedRankedPlayers[i];
                if (TotalRating(redTeam) < TotalRating(blueTeam) && redTeam.Count < half_max)
                {
                    redTeam.Add(p.Name);
                }
                else if (blueTeam.Count < half_max)
                {
                    blueTeam.Add(p.Name);
                }
                else
                {
                    redTeam.Add(p.Name);
                }
            }

            m_Context.ClearTeams();
            redTeam.ForEach(x => m_Context.AddPlayerToTeam(x, HQMTeam.Red));
            blueTeam.ForEach(x => m_Context.AddPlayerToTeam(x, HQMTeam.Blue));

            //auto join
            while (players.Where(p => p.PlayerStruct.Team == HQMTeam.NoTeam).Count() > 0)
            {
                foreach (RankedPlayer p in players.Where(p => p.PlayerStruct.Team == HQMTeam.NoTeam))
                {
                    p.PlayerStruct.LockoutTime = 0;
                    if (redTeam.Contains(p.Name))
                    {
                        p.PlayerStruct.LegState = 4;
                    }
                    else if (blueTeam.Contains(p.Name))
                        p.PlayerStruct.LegState = 8;
                }
            }
        }

        double TotalRating(List<string> list)
        {        
            return  m_Context.LoggedInPlayers.Where(x => list.Contains(x.Name)).Select(x => x.Rating).Sum();
        }
    }
}
