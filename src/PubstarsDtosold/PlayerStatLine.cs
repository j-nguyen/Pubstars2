using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubstarsDtos
{
    public class PlayerStatLine
    {
        public string Name { get; set; }
        public string Team { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
        public bool Leaver { get; set; }
    }
}
