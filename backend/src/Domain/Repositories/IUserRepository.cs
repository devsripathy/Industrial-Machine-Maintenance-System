using System.Collections.Generic;
using System.Threading.Tasks;
using SentinelOps.Domain.Entities;

namespace SentinelOps.Domain.Repositories;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task AddAsync(User user);
    void Update(User user);
    Task<IEnumerable<string>> GetUserPermissionsAsync(int userId);
}
