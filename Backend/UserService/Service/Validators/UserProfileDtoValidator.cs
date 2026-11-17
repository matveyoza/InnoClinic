using Service.Shared.DataTransferObjects;
using FluentValidation;

namespace Service.Validators
{
    public class UserProfileDtoValidator : AbstractValidator<UserProfileDto>
    {
        public UserProfileDtoValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("User ID is required.")
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage("User ID must be a valid GUID format.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("User Email is required.")
                .EmailAddress()
                .WithMessage("A valid email address is required.");
        }
    }
}
