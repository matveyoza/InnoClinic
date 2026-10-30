using AutoMapper;
using Entities.Contracts;
using Entities.Models;
using Service.Contracts;
using Service.Shared.Constants;
using Service.Shared.DataTransferObjects;

namespace Service;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _repository;
    private readonly IMapper _mapper;

    public AppointmentService(IAppointmentRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<AppointmentDto>> GetAppointmentsAsync(
        Guid userId,
        string role,
        CancellationToken cancellationToken)
    {
        if (role != AppRoles.Patient && role != AppRoles.Doctor && role != AppRoles.Admin)
        {
            return Enumerable.Empty<AppointmentDto>();
        }

        var appointments = await _repository.GetAppointmentsByRoleAsync(userId, role, cancellationToken);
        return _mapper.Map<IEnumerable<AppointmentDto>>(appointments);
    }

    public async Task<AppointmentDto> CreateAppointmentAsync(
        Guid currentUserId,
        string role,
        string currentUserName,
        AppointmentForCreationDto request,
        CancellationToken cancellationToken)
    {
        var appointment = _mapper.Map<Appointment>(request);

        switch (role)
        {
            case AppRoles.Patient:
                appointment.PatientId = currentUserId;
                appointment.PatientName = !string.IsNullOrWhiteSpace(currentUserName)
                    ? currentUserName
                    : request.PatientName;
                break;

            case AppRoles.Admin:
                if (request.PatientId == Guid.Empty)
                {
                    throw new ArgumentException("Admin must provide a valid PatientId.", nameof(request.PatientId));
                }
                appointment.PatientId = request.PatientId;
                appointment.PatientName = request.PatientName;
                break;

            case AppRoles.Doctor:
                throw new UnauthorizedAccessException("Doctors are not allowed to create appointments.");

            default:
                throw new UnauthorizedAccessException("Invalid role.");
        }

        appointment.Status = AppointmentStatus.Upcoming;
        appointment.CreatedAtUtc = DateTime.UtcNow;

        await _repository.CreateAppointmentAsync(appointment, cancellationToken);

        return _mapper.Map<AppointmentDto>(appointment);
    }
}