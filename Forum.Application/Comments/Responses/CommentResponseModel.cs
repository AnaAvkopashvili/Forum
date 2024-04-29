
namespace Forum.Application.Comments.Responses
{
    public class CommentResponseModel
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public string UserUserName {  get; set; }
        public string UserId {  get; set; }
        public bool IsDeleted { get; set; }
        public DateTime Created {  get; set; }
    }
}
