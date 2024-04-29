using Forum.Application.Comments;
using Forum.Application.Comments.Requests;
using Forum.Application.Comments.Responses;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Forum.API.Controllers.V1
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentService _service;

        public CommentController(ICommentService commentService, ILogger<CommentController> logger)
        {
            _service = commentService;
            _logger = logger;

            _logger.LogInformation("controller is executed");
        }

        [HttpGet("{id}")]
        public async Task<CommentResponseModel> Get(CancellationToken cancellationToken, int id)
        {
            return await _service.GetAsync(cancellationToken, id);
        }

        [HttpGet]
        public async Task<List<CommentResponseModel>> GetAll(CancellationToken cancellationToken)
        {
            return await _service.GetAllAsync(cancellationToken);
        }
        [Authorize(Policy = "AdminAndUser")]
        [HttpPost]
        public async Task Post(CancellationToken cancellationToken, CommentRequestPostModel comment)
        {
            await _service.CreateAsync(cancellationToken, comment);
        }

        [Authorize(Policy = "AdminAndUser")]
        [HttpPut]
        public async Task Put(CancellationToken cancellationToken, CommentRequestPutModel comment)
        {
            await _service.UpdateAsync(cancellationToken, comment);
        }

        [Authorize(Policy = "AdminAndUser")]
        [HttpDelete("{id}")]
        public async Task Delete(CancellationToken cancellationToken, int id)
        {
            await _service.DeleteAsync(cancellationToken, id);
        }
    }
}
