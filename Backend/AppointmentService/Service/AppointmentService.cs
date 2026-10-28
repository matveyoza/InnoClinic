using Service.Shared.DataTransferObjects;
using AutoMapper;
using Entities.Contracts;
using Entities.Models;
using Service.Contracts;
using FluentValidation;

namespace Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _repository;
        private readonly IMapper _mapper;
        

        public AppointmentService(IAppointmentRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> CreateAppointmentAsync(AppointmentForCreationDto appointmentForCreationDto, CancellationToken cancellationToken = default)
        {
            var appointment = _mapper.Map<Appointment>(appointmentForCreationDto);

            var isCreated = await _repository.CreateAppointmentAsync(appointment, cancellationToken);

            return isCreated;
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            var appointments = await _repository.GetAllAppointmentsAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto?> GetAppointmentByIdAsync(Guid appointmentId, CancellationToken cancellationToken = default)
        {
            var appointment = await _repository.GetAppointmentByIdAsync(appointmentId, cancellationToken);

            return _mapper.Map<AppointmentDto>(appointment);
        }
    }
}