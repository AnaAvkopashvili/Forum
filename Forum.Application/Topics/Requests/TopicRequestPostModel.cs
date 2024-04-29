
namespace Forum.Application.Topics.Requests
{
    public enum TopicStatus
    {
        Inactive = 1,
        Active = 2
    }
    public enum TopicState
    {
        Pending = 1,
        Show = 2,
        Hide = 3
    }
    public class TopicRequestPostModel
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }
}
