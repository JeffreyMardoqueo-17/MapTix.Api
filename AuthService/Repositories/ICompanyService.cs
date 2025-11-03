using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.helpers;

namespace AuthService.Repositories
{

    public interface ICompanyService
    {
        Task<Result<Company>> GetCompanyByIdAsync(Guid id);
        Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync();
        Task<Result<IEnumerable<Company>>> GetAllInactiveCompaniesAsync();
        Task<Result<Company>> CreateCompanyAsync(Company company);
        Task<Result<Company>> UpdateCompanyAsync(Guid id, Company company);
        Task<Result<bool>> DeleteCompanyAsync(Guid id); //soft delete
        Task<Result<Company>> ActivateCompanyAsync(Guid id); // Reactivar
    }
}