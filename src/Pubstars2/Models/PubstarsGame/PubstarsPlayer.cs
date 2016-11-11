using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.PubstarsGame
{
    public class PubstarsPlayer
    {
        [Key]
        public string Name { get; set; }
        public HqmTeam team { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
    }
}
