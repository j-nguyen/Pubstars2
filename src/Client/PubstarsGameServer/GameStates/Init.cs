using HQMEditorDedicated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.GameStates
{
    class Init : IState
    {
        public Task OnEnter()
        {
            Console.WriteLine("Init - OnEnter");                   
            return Task.FromResult<object>(null);
        }

        public Task<bool> Execute()
        {
            return Task.FromResult( MemoryEditor.Init("pubstars2") );
        }       

        public Task OnExit()
        {
            Console.WriteLine("Init - OnExit");
            Chat.RecordCommandSource();
            Chat.FlushLastCommand();
            return Task.FromResult<object>(null);
        }
    }
}
