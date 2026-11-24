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
            return await (
                from user in _context.Users.Include(u => u.DoctorProfile)
                join userRole in _context.UserRoles on user.Id equals userRole.UserId
                join role in _context.Roles on userRole.RoleId equals role.Id
                where role.Name == AppRoles.Doctor
                select user
            ).AsNoTracking().ToListAsync(cancellationToken);
        }
    }
}
