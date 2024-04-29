using FluentValidation;
using Forum.Application.Topics.Requests;

namespace Forum.Application.Topics.Validators
{
    public class TopicRequestPostModelValidator : AbstractValidator<TopicRequestPostModel>
    {
        public TopicRequestPostModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title cannot be empty.")
                .MaximumLength(100).WithMessage("Title must be less than 100 characters long.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content cannot be empty.")
                .MaximumLength(1000).WithMessage("Content must be less than 1000 characters long.");
        }
    }
}
