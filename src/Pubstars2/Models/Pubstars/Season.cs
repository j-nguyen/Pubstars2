using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Pubstars2.Models.Pubstars
{
    public class Season
    {
        [Key]
        public string seasonName { get; set; }
        public IEnumerable<Game> games { get; set; }
    }
}
