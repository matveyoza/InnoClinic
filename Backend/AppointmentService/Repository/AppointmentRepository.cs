using Entities.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Service.Shared.Constants;

namespace Repository
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly AppointmentDbContext _context;

        public AppointmentRepository(AppointmentDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Appointment>> GetAppointmentsByRoleAsync(Guid userId, string role, CancellationToken cancellationToken)
        {
            var query = _context.Appointments
                .Include(a => a.Documents)
                .AsNoTracking();

            query = role switch
            {
                AppRoles.Patient => query.Where(a => a.PatientId == userId),
                AppRoles.Doctor => query.Where(a => a.DoctorId == userId),
                AppRoles.Admin => query,
                _ => query.Where(_ => false)
            };

            return await query
                .OrderByDescending(a => a.Date)
                .ThenByDescending(a => a.Time)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Documents)
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
        }

        public async Task CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
