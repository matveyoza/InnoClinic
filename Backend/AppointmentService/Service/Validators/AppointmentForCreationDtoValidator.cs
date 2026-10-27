using Service.Shared.DataTransferObjects;
using FluentValidation;

namespace Service.Validators
{
    public class AppointmentForCreationDtoValidator : AbstractValidator<AppointmentForCreationDto>
    {
        public AppointmentForCreationDtoValidator()
        {
            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("Patient name is required.")
                .MaximumLength(20).WithMessage("Patient name cannot exceed 100 characters.");

            RuleFor(x => x.DoctorName)
                .NotEmpty().WithMessage("Doctor name is required.");

            RuleFor(x => x.Date)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Appointment date cannot be in the past.");

            RuleFor(x => x.Room)
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");
        }
    }
}
