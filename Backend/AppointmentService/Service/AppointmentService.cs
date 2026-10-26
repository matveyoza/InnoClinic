using Service.Shared.DataTransferObjects;
using AutoMapper;
using Entities.Contracts;
using Entities.Models;
using Service.Contracts;

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

        public async Task<AppointmentDto> CreateAsync(CreateAppointmentDto dto, CancellationToken cancellationToken = default)
        {
            var appointment = _mapper.Map<Appointment>(dto);

            var created = await _repository.CreateAsync(appointment, cancellationToken);

            return _mapper.Map<AppointmentDto>(created);
        }

        public async Task<IReadOnlyList<AppointmentDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var appointments = await _repository.GetAllAsync(cancellationToken);

            return _mapper.Map<IReadOnlyList<AppointmentDto>>(appointments);
        }

        public async Task<AppointmentDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var appointment = await _repository.GetByIdAsync(id, cancellationToken);

            return appointment is null ? null : _mapper.Map<AppointmentDto>(appointment);
        }
    }
}