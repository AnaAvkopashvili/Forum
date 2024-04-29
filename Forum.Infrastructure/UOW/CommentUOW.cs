using Forum.Application.Accounts;
using Forum.Application.Comments;
using Forum.Application.Topics;
using Forum.Persistence.Identity;

namespace Forum.Infrastructure.UOW
{
    public class CommentUOW : UnitofWork, ICommentUOW
    {
        public CommentUOW(AppDbContext context, ICommentRepository commentRepository, ITopicRepository topicRepository, ICurrentUserService currentUserService)
            : base(context)
        {
            Topics = topicRepository;
            Comments = commentRepository;
            CurrentUserService = currentUserService;
        }

        public ICommentRepository Comments { get; }
        public ITopicRepository Topics { get; }
        public ICurrentUserService CurrentUserService { get; }
    }
}
