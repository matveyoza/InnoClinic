using Entities.Contracts;
using Entities.Models;
using Service.Shared.DataTransferObjects;
using Service.Contracts;
using AutoMapper;

namespace Service
{
    public class ServicesService : IServicesService
    {
        private readonly IServicesRepository _repository;
        private readonly IMapper _mapper;

        public ServicesService(IServicesRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BookedTimeSegmentDto>> GetDoctorBookedSegmentsAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default)
        {
            var segments = await _repository.GetByDoctorIdAsync(doctorId, date, cancellationToken);

            return _mapper.Map<IEnumerable<BookedTimeSegmentDto>>(segments);
        }

        public async Task<bool> BookTimeSegmentAsync(BookTimeSegmentDto dto, CancellationToken cancellationToken = default)
        {
            var isOverlapping = await _repository.HasOverlapAsync(dto.DoctorId, dto.Date, dto.StartTime, cancellationToken);

            if (isOverlapping)
                return false;

            var segment = _mapper.Map<BookedTimeSegment>(dto);
            segment.Id = Guid.NewGuid();

            _repository.Add(segment);
            await _repository.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<IEnumerable<DoctorProfileDto>> GetAllDoctorProfilesAsync(CancellationToken cancellationToken = default)
        {
            var profiles = await _repository.GetAllDoctorProfilesAsync(cancellationToken);
            return _mapper.Map<IEnumerable<DoctorProfileDto>>(profiles);
        }
    }
}
