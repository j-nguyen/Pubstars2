using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PubstarsModel
{
    public class Season
    {
        [Key]
        public string seasonName { get; set; }
        public IEnumerable<Game> games { get; set; }
    }
}
