using FluentValidation;
using Forum.Application.Comments.Requests;

namespace Forum.Application.Comments.Validators
{
    public class CommentRequestPostModelValidator : AbstractValidator<CommentRequestPostModel>
    {
        public CommentRequestPostModelValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .MaximumLength(1000).WithMessage("Content must be less than 1000 characters long.");
        }
    }
}
