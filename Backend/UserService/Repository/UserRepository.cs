using Entities.Contracts;
using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Persistence;
using Service.Shared.Constants;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserDbContext _context;
        public UserRepository(UserDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetDoctorsAsync(CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(u => u.DoctorProfile)
                .Join(_context.UserRoles, user => user.Id, userRole => userRole.UserId, (user, userRole) => new { user, userRole })
                .Join(_context.Roles, combined => combined.userRole.RoleId, role => role.Id, (combined, role) => new { combined.user, role })
                .Where(x => x.role.Name == AppRoles.Doctor)
                .Select(x => x.user)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }
    }
}
