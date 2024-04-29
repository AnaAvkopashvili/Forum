using Forum.Application.Accounts;
using Forum.Application.Comments;
using Forum.Application.Comments.Requests;
using Forum.Application.Comments.Responses;
using Forum.Application.Exceptions;
using Forum.Domain.Entities;
using Moq;
using System.Threading;

namespace Forum.Application.Tests
{
    public class CommentServiceTests 
    {
        private readonly Mock<ICommentUOW> _uow;
        private readonly Mock<ICommentRepository> _comment;
        private readonly Mock<ICurrentUserService> _currentUserService;

        private readonly CommentService _service;

        public CommentServiceTests()
        {
            _uow = new Mock<ICommentUOW>();
            _currentUserService = new Mock<ICurrentUserService>();
            _service = new CommentService(_uow.Object);
            _comment = new Mock<ICommentRepository>();
        }

        [Fact]
        public async Task GetAsync_WhenCommentExists_ReturnsComment()
        {
            var comment = new Comment { Id = 1, IsDeleted = false };
            _uow.Setup(x => x.Comments.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(comment);

            var result = await _service.GetAsync(CancellationToken.None, 1);

            Assert.NotNull(result);
            Assert.IsType<CommentResponseModel>(result);
        }

        [Fact]
        public async Task GetAsync_WhenCommentNotFound_ThrowsCommentNotFoundException()
        {
            _uow.Setup(x => x.Comments.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync((Comment)null);

            var task = async () => await _service.GetAsync(CancellationToken.None, 1);

            var exception = await Assert.ThrowsAsync<CommentNotFound>(task);
            Assert.Equal($"Comment with this ID: {1} was not found!", exception.Message);

        }

        [Fact]
        public async Task GetAsync_WhenCommentIsDeleted_ThrowsCommentNotFoundException()
        {
            var comment = new Comment { Id = 1, IsDeleted = true };
            
            _uow.Setup(x => x.Comments.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(comment);

            var task = async () => await _service.GetAsync(CancellationToken.None, 1);

            var exception = await Assert.ThrowsAsync<CommentNotFound>(task);
            Assert.Equal($"Comment with this ID: {1} was not found!", exception.Message);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsNonDeletedComments()
        {
            var comments = new List<Comment>
        {
            new Comment { Id = 1, IsDeleted = false },
            new Comment { Id = 2, IsDeleted = true },
            new Comment { Id = 3, IsDeleted = false }
        };
            _uow.Setup(x => x.Comments.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(comments);

            var result = await _service.GetAllAsync(CancellationToken.None);

            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task CreateAsync_WhenTopicIsActive_CreatesComment()
        {
            var topic = new Topic { Id = 1, Status = TopicStatus.Active };
            var comment = new CommentRequestPostModel { TopicId = 1 };

            _uow.Setup(x => x.Topics.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(topic);
            _currentUserService.Setup(x => x.GetCurrentUserId())
                .Returns("1");
            _uow.SetupGet(x => x.CurrentUserService).Returns(_currentUserService.Object);
            _uow.SetupGet(x => x.Comments).Returns(_comment.Object);

            await _service.CreateAsync(CancellationToken.None, comment);

            _uow.Verify(x => x.Comments.CreateAsync(It.IsAny<CancellationToken>(), It.IsAny<Comment>()), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenTopicIsInactive_ThrowsGoneException()
        {
            var topic = new Topic { Id = 1, Status = TopicStatus.Inactive };
            var comment = new CommentRequestPostModel { TopicId = 1 };
            _uow.Setup(x => x.Topics.GetAsync(It.IsAny<CancellationToken>(), 1))
                 .ReturnsAsync(topic);
            _currentUserService.Setup(x => x.GetCurrentUserId())
                .Returns("1");
            _uow.SetupGet(x => x.CurrentUserService).Returns(_currentUserService.Object);

            var task = async () => await _service.CreateAsync(CancellationToken.None, comment);

            var exception = await Assert.ThrowsAsync<Gone>(task);
            Assert.Equal($"Topic {1} inactive. Moved to archives", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenCommentExistsAndAuthorisLoggedIn_DeletesComment()
        {
            var comment = new Comment { Id = 1, UserId = "1", TopicId = 1 };
            var topic = new Topic { Id = 1, NumberOfComments = 1 };
            _uow.Setup(x => x.Comments.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(comment);
            _uow.Setup(x => x.Topics.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(topic);
            _uow.SetupGet(x => x.CurrentUserService).Returns(_currentUserService.Object);
            _currentUserService.Setup(x => x.GetCurrentUserId())
                .Returns("1");

            await _service.DeleteAsync(CancellationToken.None, 1);

            _uow.Verify(x => x.Comments.DeleteAsync(It.IsAny<CancellationToken>(), It.IsAny<Comment>()), Times.Once);
            _uow.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenCommentNotFound_ThrowsCommentNotFoundException()
        {
            _uow.Setup(x => x.Comments.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync((Comment)null);
           
            var task = async () => await _service.DeleteAsync(CancellationToken.None, 1);

            var exception = await Assert.ThrowsAsync<CommentNotFound>(task);
            Assert.Equal($"Comment with this ID: {1} was not found!", exception.Message);
        }

        [Fact]
        public async Task DeleteAsync_WhenCommentAuthorisNotLoggedIn_ThrowsCommentNotFoundException()
        {
            var comment = new Comment { Id = 1, UserId = "2" };
            _uow.Setup(x => x.Comments.GetAsync(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(comment);
            _currentUserService.Setup(x => x.GetCurrentUserId())
                .Returns("1");
            _uow.SetupGet(x => x.CurrentUserService).Returns(_currentUserService.Object);

            var task = async () => await _service.DeleteAsync(CancellationToken.None, 1);

            var exception = await Assert.ThrowsAsync<CommentNotFound>(task);
            Assert.Equal($"Comment with this ID: {1} was not found!", exception.Message);
        }

        [Fact]
        public async Task UpdateAsync_WhenCommentExists_UpdatesComment()
        { 
           var comment = new CommentRequestPutModel { Id = 1 }; 
           _uow.Setup(x => x.Comments.Exists(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(true); 
           await _service.UpdateAsync(CancellationToken.None, comment);
           _uow.Verify(x => x.Comments.UpdateAsync(It.IsAny<CancellationToken>(), It.IsAny<Comment>()), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenCommentDoesNotExist_ThrowsCommentNotFoundException()
        {
            var comment = new CommentRequestPutModel { Id = 1 };
            _uow.Setup(x => x.Comments.Exists(It.IsAny<CancellationToken>(), 1))
                .ReturnsAsync(false);
            var ex = await Assert.ThrowsAsync<CommentNotFound>(async () =>
            {
                await _service.UpdateAsync(CancellationToken.None, comment);
            });

            Assert.Equal($"Comment with this ID: {1} was not found!", ex.Message);
        }
    }   
}
