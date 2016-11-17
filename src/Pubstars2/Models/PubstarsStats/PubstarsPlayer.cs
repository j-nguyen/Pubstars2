using System.ComponentModel.DataAnnotations.Schema;

namespace Pubstars2.Models.PubstarsStats
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
