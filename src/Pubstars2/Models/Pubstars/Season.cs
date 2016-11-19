using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Pubstars2.Models.Pubstars
{
    public class Season
    {
        [Key]
        public string seasonName { get; set; }
        public IEnumerable<Game> games { get; set; }
    }
}
