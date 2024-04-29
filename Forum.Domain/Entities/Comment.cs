namespace Forum.Domain.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public User User { get; set; }
        public string UserId { get; set; }
        public Topic Topic { get; set; }
        public int TopicId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created { get; set; }
    }
}
