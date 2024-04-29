namespace Forum.Application.Comments.Requests
{
    public class CommentRequestPutModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public int TopicId { get; set; }
        
    }
}
