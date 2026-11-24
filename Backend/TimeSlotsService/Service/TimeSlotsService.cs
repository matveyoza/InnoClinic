using Entities.Contracts;
using Entities.Models;
using Service.Shared.DataTransferObjects;

namespace Service
{
    public class TimeSlotsService
    {
        private readonly ITimeSlotsRepository _repository;

        public TimeSlotsService(ITimeSlotsRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<BookedTimeSegmentDto>> GetDoctorBookedSegmentsAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default)
        {
            var segments = await _repository.GetByDoctorIdAsync(doctorId, date, cancellationToken);

            return segments.Select(s => new BookedTimeSegmentDto(s.Id, s.DoctorId, s.Date, s.StartTime, s.EndTime));
        }

        public async Task<BookedTimeSegmentDto> BookTimeSegmentAsync(BookTimeSegmentDto dto, CancellationToken cancellationToken = default)
        {
            if (dto.StartTime >= dto.EndTime)
            {
                throw new ArgumentException("StartTime must be earlier than EndTime.");
            }

            var isOverlapping = await _repository.HasOverlapAsync(dto.DoctorId, dto.Date, dto.StartTime, dto.EndTime, cancellationToken);

            if (isOverlapping)
            {
                throw new InvalidOperationException("This time segment overlaps with an existing booking.");
            }

            var segment = new BookedTimeSegment
            {
                Id = Guid.NewGuid(),
                DoctorId = dto.DoctorId,
                Date = dto.Date,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            await _repository.AddAsync(segment, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);

            return new BookedTimeSegmentDto(segment.Id, segment.DoctorId, segment.Date, segment.StartTime, segment.EndTime);
        }
    }
}
