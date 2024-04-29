using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Accounts.Requests
{
    public class LoginRequestModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberLogin { get; set; }

    }
}
