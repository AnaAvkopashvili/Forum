using FluentValidation;
using Forum.Application.Accounts.Requests;

namespace Forum.API.Infrastructure.Validators
{
    public class RegistrationRequestModelValidator : AbstractValidator<RegistrationRequestModel>
    {
        public RegistrationRequestModelValidator()
        {
            RuleFor(x => x.UserName)
                   .NotEmpty().WithMessage("Username can not be empty")
                   .Length(0, 50);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password cannot be empty")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long")
                .Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter")
                .Matches("[0-9]").WithMessage("Password must contain at least one number");

            RuleFor(x => x.Email)
                            .NotEmpty().WithMessage("Email cannot be empty")
                            .EmailAddress().WithMessage("Invalid email address");

            RuleFor(x => x.FirstName).NotEmpty().WithMessage("FirstName should not be empty");

            RuleFor(x => x.LastName).NotEmpty().WithMessage("LastName should not be empty");
        }
    }

}
