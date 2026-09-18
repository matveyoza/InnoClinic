using FluentValidation;
using Service.Shared;

namespace Service.Validators
{
    public class UserForAuthenticationDtoValidator : AbstractValidator<UserForAuthenticationDto>
    {
        public UserForAuthenticationDtoValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }
}
