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

        public async Task<bool> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken = default)
        {
            _context.Appointments.Add(appointment);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public async Task<IReadOnlyList<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Documents)
                .AsNoTracking()
                .OrderByDescending(a => a.CreatedAtUtc)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetAppointmentByIdAsync(Guid appointmentId, CancellationToken cancellationToken = default)
        {
            return await _context.Appointments
                .Include(a => a.Documents)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == appointmentId, cancellationToken);
        }
    }
}
