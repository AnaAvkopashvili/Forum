using Forum.Application.Topics;
using Forum.Application.Topics.Requests;
using Forum.Application.Topics.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers.V1
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TopicController : ControllerBase
    {
        private readonly ILogger<TopicController> _logger;
        private readonly ITopicService _service;

        public TopicController(ITopicService topicService, ILogger<TopicController> logger)
        {
            _service = topicService;
            _logger = logger;

            _logger.LogInformation("controller is executed");
        }

        [HttpGet("{id}")]
        public async Task<TopicResponseModel> Get(CancellationToken cancellationToken, int id)
        {
            return await _service.GetAsync(cancellationToken, id);
        }
        [HttpGet]
        public async Task<List<TopicResponseModel>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }

        [HttpGet] 
        [Authorize(Policy = "AdminOnly")]
        public async Task<List<AdminTopicResponseModel>> GetAllForAdmin(CancellationToken cancellationToken)
        {
            return await _service.GetAllForAdminAsync(cancellationToken);
        }
        [Authorize(Policy = "AdminAndUser")]
        [HttpPost]
        public async Task Post(CancellationToken cancellationToken, TopicRequestPostModel topic)
        {
            await _service.CreateAsync(cancellationToken, topic);
        }

        [Authorize(Policy = "AdminAndUser")]
        [HttpPut]
        public async Task Put(CancellationToken cancellationToken, TopicRequestPutModel topic)
        {
            await _service.UpdateAsync(cancellationToken, topic);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{id}")]
        public async Task<IActionResult> ApproveTopic(CancellationToken cancellationToken, int id)
        {
            await _service.ApproveTopicAsync(cancellationToken, id);
            return RedirectToAction(nameof(GetAll));
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("{id}")]
        public async Task<IActionResult> RejectTopic(CancellationToken cancellationToken, int id)
        {
            await _service.RejectTopicAsync(cancellationToken, id);
            return RedirectToAction(nameof(GetAll));
        }

    }
}
