using Forum.Application.Accounts;
using Forum.Application.Comments;
using Forum.Application.Exceptions;
using Forum.Application.Services;
using Forum.Application.Topics.Requests;
using Forum.Application.Topics.Responses;
using Forum.Domain.Entities;
using Mapster;



namespace Forum.Application.Topics
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _repository;
        private readonly ICommentRepository _commentRepository;
        private readonly ICurrentUserService _currentUserService;
        private readonly IAdminService _adminService;
        

        public TopicService(ITopicRepository reposity, ICurrentUserService currentUserService, ICommentRepository commentRepositoy, IAdminService adminService)
        {
            _repository = reposity;
            _currentUserService = currentUserService;
            _commentRepository = commentRepositoy;
            _adminService = adminService;
        }

        public async Task<TopicResponseModel> GetAsync(CancellationToken cancellationToken, int id)
        {
            var result = await _repository.GetAsync(cancellationToken, id);
            if (result == null)
            {
                throw new TopicNotFound("Topic with this ID: " + id.ToString() + " was not found!");
            }
            return result.Adapt<TopicResponseModel>();
        }

        public async Task<List<TopicResponseModel>> GetTopicByUser(CancellationToken cancellationToken, string userId)
        {
            var result = await _repository.GetTopicByUser(cancellationToken, userId);
            return result.Adapt<List<TopicResponseModel>>();
        }
        public async Task<List<TopicResponseModel>> GetAllAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);
            var approvedTopics = result.Where(t => t.State == Domain.Entities.TopicState.Show).ToList();
            return approvedTopics.Adapt<List<TopicResponseModel>>();
        }
   
        public async Task<List<AdminTopicResponseModel>> GetAllForAdminAsync(CancellationToken cancellationToken)
        {
            var result = await _repository.GetAllAsync(cancellationToken);
            return result.Adapt<List<AdminTopicResponseModel>>();
        }
        public async Task CreateAsync(CancellationToken cancellationToken, TopicRequestPostModel topic)
        {
            var activityCount = 0;
            var currentUserId = _currentUserService.GetCurrentUserId();
            var allComments = await _commentRepository.GetAllAsync(cancellationToken);

            foreach(var comment in allComments)
            {
                if (comment.UserId == currentUserId)
                {
                    activityCount++;
                }
            }

            var isAdmin = await _adminService.IsUserInRoleAsync(currentUserId, "Admin");

            if (activityCount >= 1 || isAdmin)
            {
                var ToInsert = topic.Adapt<Topic>();
                ToInsert.Created = DateTime.UtcNow;
                ToInsert.State = Domain.Entities.TopicState.Pending;
                ToInsert.Status = Domain.Entities.TopicStatus.Active;
                ToInsert.UserId = currentUserId;

                await _repository.CreateAsync(cancellationToken, ToInsert);
            }
            else
            {
                throw new UnauthorizedAccess("You have to be active to post something");
            }
        }

        public async Task UpdateAsync(CancellationToken cancellationToken, TopicRequestPutModel topic)
        {
            if (!await _repository.Exists(cancellationToken, topic.Id))
            {
                throw new TopicNotFound("Topic with this ID: " + topic.Id.ToString() + " was not found!");
            }
            var ToInsert = topic.Adapt<Topic>();
            await _repository.UpdateAsync(cancellationToken, ToInsert);
        }
        public async Task ApproveTopicAsync(CancellationToken cancellationToken, int id)
        {
            var topic = await _repository.GetAsync(cancellationToken, id);
            if (topic == null)
            {
                throw new TopicNotFound("Topic with this ID: " + id.ToString() + " was not found!");
            }
            if (topic.State != Domain.Entities.TopicState.Pending)
            {
                throw new InvalidOperation("Only pending topics can be approved.");
            }

            topic.State = Domain.Entities.TopicState.Show;
            await _repository.UpdateAsync(cancellationToken, topic);
        }
        public async Task RejectTopicAsync(CancellationToken cancellationToken, int id)
        {
            var topic = await _repository.GetAsync(cancellationToken, id);
            if (topic == null)
            {
                throw new TopicNotFound("Topic with this ID: " + id.ToString() + " was not found!");
            }
            if (topic.State != Domain.Entities.TopicState.Pending)
            {
                throw new InvalidOperation("Only pending topics can be rejected.");
            }
            topic.State = Domain.Entities.TopicState.Hide;
            await _repository.UpdateAsync(cancellationToken, topic);
        }
        public async Task ArchiveInactiveTopicsAsync(CancellationToken cancellationToken)
        {
            var allTopics = await _repository.GetAllAsync(cancellationToken);
            var acticeTopics = allTopics.Where(x => x.Status == Domain.Entities.TopicStatus.Active);

            foreach (var topic in acticeTopics)
            {
                var lastCommentDate = topic.Comments.OrderByDescending(c => c.Created).FirstOrDefault()?.Created;

                if ((lastCommentDate.HasValue && (DateTime.UtcNow - lastCommentDate.Value).TotalDays > 30)
                    || (DateTime.UtcNow - topic.Created).TotalDays > 30)
                {
                    topic.Status = Domain.Entities.TopicStatus.Inactive;
                    await _repository.UpdateAsync(cancellationToken, topic);
                }
            }
        }
    }
}
