using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.helpers;

namespace AuthService.Services
{
    
    public interface ICompanyService
    {
        Task<Result<Company>> GetCompanyByIdAsync(Guid id);
        Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync();
        Task<Result<Company>> CreateCompanyAsync(Company company);
        Task<Result<Company>> UpdateCompanyAsync(Company company);
        Task<Result<bool>> DeleteCompanyAsync(Guid id);
    }
}