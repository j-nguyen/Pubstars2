using HQMEditorDedicated;
using PubstarsGameServer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.GameStates
{
    class Gameplay : IState
    {
        bool m_Mercy = false;

        public async Task<bool> Execute()
        {
            if(!m_Mercy)
            {
                if(CheckMercy())
                {
                    m_Mercy = true;
                    Chat.SendMessage("---------------------------------------------------");
                    Chat.SendMessage("  Game is ending due to mercy rule.");
                    Chat.SendMessage("---------------------------------------------------");
                    GameInfo.Period = 3;
                    GameInfo.GameTime = new TimeSpan(0, 0, 0, 1);
                }
            }
            return GameInfo.IsGameOver;
        }

        public Task OnEnter()
        {
            Console.WriteLine("Gameplay - OnEnter");
            return Task.FromResult<object>(null);
        }

        public Task OnExit()
        {
            Console.WriteLine("Gameplay - OnExit");
            return Task.FromResult<object>(null);
        }

        private bool CheckMercy()
        {
            byte[] score = MemoryEditor.ReadBytes(0x018931F8, 8);
            int redScore = score[0];
            int blueScore = score[4];
            return (Math.Abs(redScore - blueScore) >= 1);
        }
    }
}
