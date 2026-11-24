using Entities.Models;

namespace Entities.Contracts
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetDoctorsAsync(CancellationToken cancellationToken);
    }
}
