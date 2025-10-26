using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.helpers;

namespace AuthService.Repositories
{
    public interface IUserService
    {
        Task<Result<User>> GetUserByIdAsync(Guid id);
        Task<Result<IEnumerable<User>>> GetAllUsersAsync();
        Task<Result<User>> CreateUserAsync(User user);
        Task<Result<User>> UpdateUserAsync(User user);
        Task<Result<bool>> DeleteUserAsync(Guid id);
    }
}
