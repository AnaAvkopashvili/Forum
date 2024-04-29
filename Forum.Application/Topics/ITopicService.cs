using Forum.Application.Topics.Requests;
using Forum.Application.Topics.Responses;

namespace Forum.Application.Topics
{
    public interface ITopicService
    {
        Task<TopicResponseModel> GetAsync(CancellationToken cancellationToken, int id);
        Task<List<TopicResponseModel>> GetAllAsync(CancellationToken cancellationToken);
        Task<List<AdminTopicResponseModel>> GetAllForAdminAsync(CancellationToken cancellationToken);
        Task<List<TopicResponseModel>> GetTopicByUser(CancellationToken cancellationToken, string userId);
        Task CreateAsync(CancellationToken cancellationToken, TopicRequestPostModel topic);
        Task UpdateAsync(CancellationToken cancellationToken, TopicRequestPutModel topic);
        Task ApproveTopicAsync(CancellationToken cancellationToken, int topicId);
        Task RejectTopicAsync(CancellationToken cancellationToken, int id);
        Task ArchiveInactiveTopicsAsync(CancellationToken cancellationToken);
    }
}
