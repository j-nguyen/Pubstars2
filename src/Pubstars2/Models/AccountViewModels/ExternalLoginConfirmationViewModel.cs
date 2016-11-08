using System.ComponentModel.DataAnnotations;

namespace Pubstars2.Models.AccountViewModels
{                
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
