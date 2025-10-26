using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.helpers;
using AuthService.Repositories;
using AuthService.DataBase;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuthService.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CompanyService> _logger;
        public CompanyService(ApplicationDbContext context, ILogger<CompanyService> logger)
        {
            _context = context;
            _logger = logger;
        }
        //obtener todas las compañias 
        public async Task<Result<IEnumerable<Company>>> GetAllCompaniesAsync()
        {
            var companies = await _context.Company
                .AsNoTracking()
                .ToListAsync();
            if (companies.Count == 0)
            {
                _logger.LogWarning("No se encontraron compañias en la base de datos."); // Log warning
                return Result<IEnumerable<Company>>.Fail("No companies found.");// Retornar un resultado de fallo si no se encuentran compañias
            }
            return Result<IEnumerable<Company>>.Ok(companies);
        }

        //obtener compañoas por id
        public async Task<Result<Company>> GetCompanyByIdAsync(Guid id)
        {
            var company = await _context.Company
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
            if (company == null)
            {
                _logger.LogWarning($"Compañia con ID {id} no encontrada."); // Log warning
                return Result<Company>.Fail("Company not found.");
            }
            return Result<Company>.Ok(company);
        }

        //crear compañia
        public async Task<Result<Company>> CreateCompanyAsync(Company company)
        {
            var validationErrors = CompanyValidator.Validate(company);

            if (validationErrors.Any())
            {
                var message = string.Join("; ", validationErrors);
                _logger.LogWarning("Errores de validación al crear compañía: {Message}", message);
                return Result<Company>.Fail(message);
            }
            
            // Normalizar TELEFONO
            company.PhoneNumber = company.PhoneNumber.Replace(" ", "")
                                                    .Replace("-", "")
                                                    .Replace("(", "")
                                                    .Replace(")", "");

            // Validar negocio: empresa duplicada
            if (await _context.Company.AnyAsync(c => c.Name == company.Name))
                return Result<Company>.Fail("A company with this name already exists.");

            company.Id = Guid.NewGuid();
            company.CreatedAt = DateTime.UtcNow;
            company.UpdatedAt = DateTime.UtcNow;

            await _context.Company.AddAsync(company);
            await _context.SaveChangesAsync();

            return Result<Company>.Ok(company);
        }

    }
}