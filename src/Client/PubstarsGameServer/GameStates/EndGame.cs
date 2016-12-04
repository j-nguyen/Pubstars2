using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.GameStates
{
    class EndGame : IState
    {
        public Task<bool> Execute()
        {
            Console.WriteLine("EndGame - Execute");
            return Task.FromResult<bool>(true);
        }

        public Task OnEnter()
        {
            Console.WriteLine("EndGame - OnEnter");
            return Task.FromResult<object>(null);
        }

        public Task OnExit()
        {
            Console.WriteLine("EndGame - OnExit");
            return Task.FromResult<object>(null);
        }
    }
}
