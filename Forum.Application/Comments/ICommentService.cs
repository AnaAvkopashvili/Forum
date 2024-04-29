using Forum.Application.Comments.Requests;
using Forum.Application.Comments.Responses;
using Forum.Domain.Entities;

namespace Forum.Application.Comments
{
    public interface ICommentService
    {
        Task<List<CommentResponseModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<CommentResponseModel> GetAsync(CancellationToken cancellationToken, int id);
        Task CreateAsync(CancellationToken cancellationToken, CommentRequestPostModel comment);
        Task UpdateAsync(CancellationToken cancellationToken, CommentRequestPutModel comment);
        Task DeleteAsync(CancellationToken cancellationToken, int id);
    }
}
