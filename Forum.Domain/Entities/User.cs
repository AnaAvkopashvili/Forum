using Microsoft.AspNetCore.Identity;

namespace Forum.Domain.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Topic> Topics { get; set; }
        public DateTime? BanExpiration { get; set; }
        public string ProfileImageUrl { get; set; }
    }
}
