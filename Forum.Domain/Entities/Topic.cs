namespace Forum.Domain.Entities
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
    public class Topic
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public int NumberOfComments { get; set; }
        public TopicState State { get; set; }
        public TopicStatus Status { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
