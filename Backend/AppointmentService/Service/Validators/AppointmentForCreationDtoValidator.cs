using Service.Shared.DataTransferObjects;
using FluentValidation;

namespace Service.Validators
{
    public class AppointmentForCreationDtoValidator : AbstractValidator<AppointmentForCreationDto>
    {
        public AppointmentForCreationDtoValidator()
        {
            RuleFor(x => x.PatientId)
                .NotEmpty().WithMessage("PatientId is required and cannot be empty.");

            RuleFor(x => x.PatientName)
                .NotEmpty().WithMessage("Patient name is required.")
                .MaximumLength(20).WithMessage("Patient name cannot exceed 100 characters.");

            RuleFor(x => x.DoctorName)
                .NotEmpty().WithMessage("Doctor name is required.");

            RuleFor(x => x.DoctorId)
                .NotEmpty().WithMessage("DoctorId is required.");

            RuleFor(x => x.Date)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow))
                .WithMessage("Appointment date cannot be in the past.");

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage("Appointment time is required.");

            RuleFor(x => x.Room)
                .GreaterThan(0).WithMessage("Room number must be greater than 0.");
        }
    }
}
