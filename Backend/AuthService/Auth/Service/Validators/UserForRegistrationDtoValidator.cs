using FluentValidation;
using Service.Shared;

namespace Service.Validators
{
    public sealed class UserForRegistrationDtoValidator : AbstractValidator<UserForRegistrationDto>
    {
        private static readonly string[] AllowedRoles = { "Patient", "Doctor", "Admin" };

        public UserForRegistrationDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(7).WithMessage("Password must be at least 7 characters long.");

            RuleFor(x => x.FirstName)
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.");

            RuleFor(x => x.LastName)
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.");

            RuleFor(x => x.UserName)
                .NotEmpty().WithMessage("User Name is required.")
                .MaximumLength(50).WithMessage("Username cannot exceed 50 characters.");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("Role is required.")
                .Must(role => AllowedRoles.Contains(role))
                .WithMessage("Role must be 'Patient', 'Doctor', or 'Admin'.");
        }
    }
}
