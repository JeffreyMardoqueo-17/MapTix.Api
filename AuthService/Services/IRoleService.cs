using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.helpers;

namespace AuthService.Services
{
    public interface IRoleService
    {
        Task<Result<Role>> GetRoleByIdAsync(Guid id);
        Task<Result<IEnumerable<Role>>> GetAllRolesAsync();
        Task<Result<Role>> CreateRoleAsync(Role role);
        Task<Result<Role>> UpdateRoleAsync(Role role);
        Task<Result<bool>> DeleteRoleAsync(Guid id);
    // metodo para paginacion
    Task<Result<IEnumerable<Role>>> GetRolesByPageAsync(int pageNumber, int pageSize);
    }
}