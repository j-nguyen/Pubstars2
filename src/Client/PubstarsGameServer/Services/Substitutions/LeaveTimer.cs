using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsGameServer.Services.Substitutions
{
    class LeaveTimer
    {
        const int REJOIN_TIME = 30;

        public Leaver Leaver;

        private DateTime m_Start;
        private TimeSpan m_RejoinTime = new TimeSpan(0, 0, REJOIN_TIME);

        public LeaveTimer()
        {
            m_Start = DateTime.UtcNow;
        }

        public bool IsExpired()
        {
            return DateTime.UtcNow > m_Start + m_RejoinTime;
        }
    }
}
