using Forum.Application.Accounts;
using Forum.Application.Topics;

namespace Forum.Application.Comments
{
    public interface ICommentUOW : IUnitofWork
    {
        public ICommentRepository Comments { get; }
        public ITopicRepository Topics { get; }
        public ICurrentUserService CurrentUserService { get; }
    }
}
