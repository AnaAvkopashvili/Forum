using System.ComponentModel.DataAnnotations;

namespace Forum.Application.Accounts.Requests
{
    public class UserPutModel
    {
        public string Id {  get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        //public string ProfileImageUrl { get; set; }
        [Required]
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }

        public string Email { get; set; }
        public string UserName { get; set; }
    }
}
