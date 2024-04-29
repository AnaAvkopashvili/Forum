using Forum.Application.Comments.Responses;
using Forum.Application.Topics.Requests;

namespace Forum.Application.Topics.Responses
{
    public class TopicResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int NumberOfComments { get; set; }
        public DateTime Created { get; set; }
        public string UserUserName { get; set; }
        public TopicState State { get; set; }
        public TopicStatus Status { get; set; }
        //public string SearchQuery {  get; set; }
        public List<CommentResponseModel> Comments { get; set; }
    }
}
