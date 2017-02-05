﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services.Logging
{
    class ConsoleLogger : ILoggerProvider
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
