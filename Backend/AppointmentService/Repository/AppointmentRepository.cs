using Entities.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> CreateAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return appointment;
        }

        public async Task<IReadOnlyList<Appointment>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Documents)
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Documents)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }
    }
}
