using HQMEditorDedicated;
using PubstarsDtos;
using PubstarsGameServer.Model;
using PubstarsGameServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.GameStates
{
    class EndGame : IState
    {
        public GameContext m_Context;
        private Warden m_Warden;

        public EndGame(GameContext context, Warden warden)
        {
            m_Context = context;
            m_Warden = warden;
        }
        public Task<bool> Execute()
        {
            Console.WriteLine("EndGame - Execute");
            return Task.FromResult<bool>(!GameInfo.IsGameOver);
        }

        public async Task OnEnter()
        {
            Console.WriteLine("EndGame - OnEnter");
            
            int redScore = GameInfo.RedScore;
            int blueScore = GameInfo.BlueScore;
            List<GameDto.PlayerStatLine> stats = CreateStatLines(m_Context.LoggedInPlayers.Where(x=>x.Team == HQMTeam.Red).Select(x=>x.Name), m_Context.LoggedInPlayers.Where(x => x.Team == HQMTeam.Blue).Select(x => x.Name));            

            GameDto report = new GameDto()
            {
                RedScore = redScore,
                BlueScore = blueScore,
                WinningTeam = redScore > blueScore ? "Red" : "Blue",
                PlayerStats = stats,
                Date = DateTime.UtcNow
            };
            
            Chat.SendMessage("Game over. Stats have been Recorded.");
            m_Warden.Stop();

            if (!await RemoteApi.SendGameResult(report))
            {
                await RemoteApi.GetToken();
                if (!await RemoteApi.SendGameResult(report))
                {
                    Console.WriteLine("Could not post game result");
                }
            }
          
        }

        public Task OnExit()
        {
            Console.WriteLine("EndGame - OnExit");            
            return Task.FromResult<object>(null);
        }

        private List<GameDto.PlayerStatLine> CreateStatLines(IEnumerable<string> RedTeam, IEnumerable<string> BlueTeam)
        {
            List<GameDto.PlayerStatLine> stats = new List<GameDto.PlayerStatLine>();
            foreach (string s in RedTeam.Concat(BlueTeam))
            {
                GameDto.PlayerStatLine player = new GameDto.PlayerStatLine();
                player.Name = s;
                player.Team = RedTeam.Contains(s) ? "Red" : "Blue";

                RankedPlayer rp = m_Context.LoggedInPlayers.FirstOrDefault(x => x.Name == s);
                if (rp != null && rp.Name == rp.PlayerStruct.Name && rp.PlayerStruct.InServer)
                {
                    player.Goals = rp.PlayerStruct.Goals;
                    player.Assists = rp.PlayerStruct.Assists;
                }
                else
                {
                    player.Leaver = true;
                }

                stats.Add(player);
            }
            return stats;
        }
    }
}
