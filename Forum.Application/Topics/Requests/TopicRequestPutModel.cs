namespace Forum.Application.Topics.Requests
{
    public class TopicRequestPutModel
    {
        public int Id { get; set; } 
        public string Title { get; set; }
        public string Content { get; set; }
        public int NumberOfComments { get; set; }
        public TopicState State { get; set; }
        public TopicStatus Status { get; set; }
        public string UserId { get; set; }
    }
}
