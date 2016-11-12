﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.PubstarsGame
{
    public class PubstarsPlayer
    {
        public string PubstarsPlayerId { get; set; }

        [ForeignKey("PubstarsPlayerId")]
        public virtual ApplicationUser User { get; set; }

        public HqmTeam Team { get; set; }
        public int Goals { get; set; }
        public int Assists { get; set; }
    }
}
