using Forum.Application.Topics.Responses;

namespace Forum.Application.Accounts.Responses
{
    public class AdminResponseModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public IEnumerable<string> Roles { get; set; }
        public DateTime? BanExpiration { get; set; }
        public List<TopicResponseModel> Topics { get; set; }
    }
}
