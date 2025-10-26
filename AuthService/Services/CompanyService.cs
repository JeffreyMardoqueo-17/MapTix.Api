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
using AuthService.Helpers;

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
            if (!string.IsNullOrWhiteSpace(company.PhoneNumber))
            {
                company.PhoneNumber = company.PhoneNumber.Replace(" ", "")
                                                        .Replace("-", "")
                                                        .Replace("(", "")
                                                        .Replace(")", "");
            }
            else
                company.PhoneNumber = null;

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
        //modificar compañia
        public async Task<Result<Company>> UpdateCompanyAsync(Guid id, Company company)
        {
            var existingCompany = await _context.Company.FindAsync(company.Id);
            if (existingCompany == null)
            {
                _logger.LogWarning($"Compañia con ID {company.Id} no encontrada para actualización.");
                return Result<Company>.Fail("Company not found.");
            }

            var validationErrors = CompanyValidator.Validate(company);
            if (validationErrors.Any())
            {
                var message = string.Join("; ", validationErrors);
                _logger.LogWarning("Errores de validación al actualizar compañía: {Message}", message);
                return Result<Company>.Fail(message);
            }
            if (!string.IsNullOrWhiteSpace(company.PhoneNumber))
            {
                company.PhoneNumber = company.PhoneNumber.Replace(" ", "")
                                                        .Replace("-", "")
                                                        .Replace("(", "")
                                                        .Replace(")", "");
            }
            else
                company.PhoneNumber = null;

            existingCompany.Name = company.Name;
            existingCompany.Address = company.Address;
            existingCompany.PhoneNumber = company.PhoneNumber;
            existingCompany.UpdatedAt = DateTime.UtcNow;

            _context.Company.Update(existingCompany);
            await _context.SaveChangesAsync();

            return Result<Company>.Ok(existingCompany);
        }

        //eliminar compañia
        public async Task<Result<bool>> DeleteCompanyAsync(Guid id)
        {
            var companyResult = await GetCompanyByIdAsync(id);
            if (!companyResult.Success)
                return Result<bool>.Fail(companyResult.Message!);

            // Reatach entity (porque GetCompanyByIdAsync usa AsNoTracking)
            _context.Company.Attach(companyResult.Data!);
            _context.Company.Remove(companyResult.Data!);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }

    }
}