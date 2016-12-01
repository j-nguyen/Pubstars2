using System.ComponentModel.DataAnnotations;

namespace Pubstars2.Models.AccountViewModels
{                
    public class ExternalLoginConfirmationViewModel
    {
        [Required, MaxLength(20)]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}
