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
                .Must(BeAValidGuid)
                .WithMessage("User ID must be a valid GUID format.");
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage("User Email is required.");
        }

        private bool BeAValidGuid(string id)
        {
            return Guid.TryParse(id, out _);
        }
    }
}
