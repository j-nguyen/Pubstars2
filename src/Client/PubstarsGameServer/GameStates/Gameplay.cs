using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.GameStates
{
    class Gameplay : IState
    {
        public Task<bool> Execute()
        {
            Console.WriteLine("Gameplay - Execute");
            return Task.FromResult<bool>(true);
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
    }
}
