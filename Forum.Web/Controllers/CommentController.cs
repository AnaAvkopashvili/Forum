using Microsoft.AspNetCore.Mvc;
using Forum.Application.Comments.Requests;
using Forum.Application.Comments;
using Forum.Application.Topics;
using Forum.Application.Accounts;
using Microsoft.AspNetCore.Authorization;
using Azure.Core;
using Forum.Web.Models;
using System.Diagnostics;
using System.Threading;

namespace Forum.Web.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ITopicService _topicService;

        public CommentController(ICommentService commentService, ITopicService topicService)
        {
            _commentService = commentService;
            _topicService = topicService;
        }
        public IActionResult Create(int? topicId)
        {
            if (topicId == null)
            {
                return NotFound();
            }
            var model = new CommentRequestPostModel { TopicId = topicId.Value};
            return View(model);
        }

        [Authorize(Policy ="AdminAndUser")]
        [HttpPost]
        public async Task<IActionResult> Create(CancellationToken cancellationToken, CommentRequestPostModel request)
        {
            var errors = ModelState.Select(x => x.Value?.Errors)
             .Where(y => y.Count > 0).ToList();

            if (!ModelState.IsValid)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error[0].ErrorMessage);
                }               
            }
            try
            {
                await _commentService.CreateAsync(cancellationToken, request);
            }
            catch (Exception ex)
            {
                var errorViewModel = new ErrorViewModel
                {
                    ErrorMessage = ex.Message,
                    RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
                };

                return View("Error", errorViewModel);
            }
            var topic = await _topicService.GetAsync(cancellationToken, request.TopicId);
            return RedirectToAction("Details", "Topic", new { id = topic.Id });
        }

        [Authorize(Policy = "AdminAndUser")]
        [HttpPost("{id}")]
        public async Task<IActionResult> Delete(CancellationToken cancellationToken, int id)
        {
            await _commentService.DeleteAsync(cancellationToken, id);
            return RedirectToAction("GetAll", "Topic"); 

        }
    }
}

