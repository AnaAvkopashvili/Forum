using Forum.Application.Accounts;
using Forum.Application.Exceptions;
using Forum.Application.Topics;
using Forum.Application.Topics.Requests;
using Forum.Application.Topics.Responses;
using Forum.Domain.Entities;
using Forum.Web.Models;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Forum.Web.Controllers
{
    public class TopicController : Controller
    {
        private readonly ITopicService _topicService;
        private readonly IUserService _userService;
        private readonly ICurrentUserService _currentUserService;
       

        public TopicController(ITopicService topicService, IUserService userService, ICurrentUserService currentUserService)
        {
            _topicService = topicService;
            _userService = userService;
            _currentUserService = currentUserService;
        }

        public async Task<IActionResult> GetAll(CancellationToken cancellationToken, string searchQuery = null)
        {
            var topics = await _topicService.GetAllAsync(cancellationToken);

            if (!string.IsNullOrEmpty(searchQuery))
            {
                var user = await _userService.GetByEmailAsync(cancellationToken, searchQuery);
                topics = await _topicService.GetTopicByUser(cancellationToken, user.Id);
            }
            
            return View(topics);
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> GetAllForAdmin(CancellationToken cancellationToken, string searchQuery = null)
        {
            var topics = await _topicService.GetAllForAdminAsync(cancellationToken);
            if (!string.IsNullOrEmpty(searchQuery))
            {
                var user = await _userService.GetByEmailAsync(cancellationToken, searchQuery);
                var topic = await _topicService.GetTopicByUser(cancellationToken, user.Id);
                topics = topic.Adapt<List<AdminTopicResponseModel>>();
            }
            return View(topics);
        }

        public async Task<IActionResult> SearchEmail(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("GetAll", new { searchQuery } );
            }
            return RedirectToAction("GetAll");
        }

        public async Task<IActionResult> SearchEmailForAdmin(string searchQuery)
        {
            if (!string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("GetAllForAdmin", new { searchQuery });
            }
            return RedirectToAction("GetAllForAdmin");
        }

        public async Task<IActionResult> Details(CancellationToken cancellationToken, int id)
        {
            var topic = await _topicService.GetAsync(cancellationToken, id);
            if (topic == null)
            {
                throw new TopicNotFound("Topic with this ID:" +  id + "was not found");
            }

            return View(topic);
        }
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Policy = "AdminAndUser")]
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Create(CancellationToken cancellationToken, TopicRequestPostModel request)
        {
            var topicToInsert = request.Adapt<Topic>();

            var errors = ModelState.Select(x => x.Value?.Errors)
             .Where(y => y.Count > 0).ToList();

            if (!ModelState.IsValid)
            {
                foreach (var error in errors)
                {
                    ModelState.AddModelError("", error[0].ErrorMessage);
                }
                return View(request);

            }
            try
            {
                await _topicService.CreateAsync(cancellationToken, request);
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

            return RedirectToAction("GetAll");
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> ApproveTopic(CancellationToken cancellationToken, int id)
        {
            await _topicService.ApproveTopicAsync(cancellationToken, id);
            return RedirectToAction(nameof(GetAllForAdmin));
        }

        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> RejectTopic(CancellationToken cancellationToken, int id)
        {
            await _topicService.RejectTopicAsync(cancellationToken, id);
            return RedirectToAction(nameof(GetAllForAdmin));
        }
    }
}
