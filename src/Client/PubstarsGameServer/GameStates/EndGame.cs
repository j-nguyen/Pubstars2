using HQMEditorDedicated;
using PubstarsDtos;
using PubstarsGameServer.Model;
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

        public EndGame(GameContext context)
        {
            m_Context = context;
        }
        public Task<bool> Execute()
        {
            Console.WriteLine("EndGame - Execute");
            return Task.FromResult<bool>(true);
        }

        public async Task OnEnter()
        {
            Console.WriteLine("EndGame - OnEnter");
            Chat.SendMessage("Game over. Recording stats...");
            int redScore = GameInfo.RedScore;
            int blueScore = GameInfo.BlueScore;
            List<RankedGameReport.PlayerStatLine> stats = CreateStatLines(m_Context.RedTeam, m_Context.BlueTeam);

            var timeUtc = DateTime.UtcNow;
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            DateTime easternTime = TimeZoneInfo.ConvertTimeFromUtc(timeUtc, easternZone);

            RankedGameReport report = new RankedGameReport()
            {
                RedScore = redScore,
                BlueScore = blueScore,
                WinningTeam = redScore > blueScore ? "Red" : "Blue",
                PlayerStats = stats,
                Date = easternTime
            };

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

        private List<RankedGameReport.PlayerStatLine> CreateStatLines(List<string> RedTeam, List<String> BlueTeam)
        {
            List<RankedGameReport.PlayerStatLine> stats = new List<RankedGameReport.PlayerStatLine>();
            foreach (string s in RedTeam.Concat(BlueTeam))
            {
                RankedGameReport.PlayerStatLine player = new RankedGameReport.PlayerStatLine();
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
