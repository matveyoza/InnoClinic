using Entities.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class ServicesRepository : IServicesRepository
    {
        private readonly ServicesDbContext _context;

        public ServicesRepository(ServicesDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BookedTimeSegment>> GetByDoctorIdAsync(Guid doctorId, DateOnly? date = null, CancellationToken cancellationToken = default)
        {
            var query = _context.BookedTimeSegments
                .AsNoTracking()
                .Where(s => s.DoctorId == doctorId);

            if (date.HasValue)
            {
                query = query.Where(s => s.Date == date.Value);
            }

            return await query
                .OrderBy(s => s.Date)
                .ThenBy(s => s.StartTime)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasOverlapAsync(Guid doctorId, DateOnly date, TimeOnly startTime, CancellationToken cancellationToken = default) =>
            await _context.BookedTimeSegments.AnyAsync(s => s.DoctorId == doctorId && s.Date == date, cancellationToken);

        public async Task AddAsync(BookedTimeSegment segment, CancellationToken cancellationToken = default) =>
            await _context.BookedTimeSegments.AddAsync(segment, cancellationToken);

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
            await _context.SaveChangesAsync(cancellationToken);

        public async Task<IEnumerable<DoctorProfile>> GetAllDoctorProfilesAsync(CancellationToken cancellationToken = default) =>
            await _context.DoctorProfiles.AsNoTracking().ToListAsync(cancellationToken);
    }
}
