using Forum.Application.Comments.Requests;
using Forum.Application.Comments.Responses;
using Forum.Application.Exceptions;
using Forum.Domain.Entities;
using Mapster;

namespace Forum.Application.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ICommentUOW _uow;

        public CommentService(ICommentUOW uow)
        {
            _uow = uow;
        }

        public async Task<CommentResponseModel> GetAsync(CancellationToken cancellationToken, int id)
        {
            var result = await _uow.Comments.GetAsync(cancellationToken, id);

            if (result == null || result.IsDeleted)
            {
                throw new CommentNotFound("Comment with this ID: " + id.ToString() + " was not found!");
            }
            else
            {
                return result.Adapt<CommentResponseModel>();
            }
        }

        public async Task<List<CommentResponseModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _uow.Comments.GetAllAsync(cancellationToken);
            var nonDeletedComments = result.Where(c => !c.IsDeleted).ToList();

            return nonDeletedComments.Adapt<List<CommentResponseModel>>();
        }

        public async Task CreateAsync(CancellationToken cancellationToken, CommentRequestPostModel comment)
        {
            var topic = await _uow.Topics.GetAsync(cancellationToken, comment.TopicId);
            var ToInsert = comment.Adapt<Comment>();
            ToInsert.Created = DateTime.UtcNow;
            ToInsert.UserId = _uow.CurrentUserService.GetCurrentUserId();
            if (topic.Status == TopicStatus.Active)
            {
                await _uow.Comments.CreateAsync(cancellationToken, ToInsert);
                topic.NumberOfComments++;
                await _uow.SaveChangesAsync(cancellationToken);
            }
            else
            {
                throw new Gone($"Topic {comment.TopicId} inactive. Moved to archives"); 
            }
        }

        public async Task DeleteAsync(CancellationToken cancellationToken, int id)
        {
            var entity = await _uow.Comments.GetAsync(cancellationToken, id);

            if (entity == null || !entity.UserId.Equals(_uow.CurrentUserService.GetCurrentUserId()))
            {
                throw new CommentNotFound("Comment with this ID: " + id.ToString() + " was not found!");
            }
            var topic = await _uow.Topics.GetAsync(cancellationToken, entity.TopicId);
            await _uow.Comments.DeleteAsync(cancellationToken, entity);
            topic.NumberOfComments--;
            await _uow.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(CancellationToken cancellationToken, CommentRequestPutModel comment)
        {
            if (!await _uow.Comments.Exists(cancellationToken, comment.Id))
                throw new CommentNotFound("Comment with this ID: " + comment.Id.ToString() + " was not found!");

            var ToUpdate = comment.Adapt<Comment>();

            await _uow.Comments.UpdateAsync(cancellationToken, ToUpdate);
        }
    }
}
