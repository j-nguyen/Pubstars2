using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.PubstarsStats
{
    public class PubstarsSeason
    {
        [Key]
        public string seasonName { get; set; }
        public IEnumerable<PubstarsGame> games { get; set; }
    }
}
