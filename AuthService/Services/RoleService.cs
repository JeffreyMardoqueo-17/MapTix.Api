using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.DataBase;
using AuthService.Repositories;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using AuthService.helpers;

namespace AuthService.Services
{
    public class RoleService : IRoleService
    {
        /// <summary>
        // ///  Task<Result<Role>> GetRoleByIdAsync(Guid id);
        // Task<Result<IEnumerable<Role>>> GetAllRolesAsync();
        // Task<Result<Role>> CreateRoleAsync(Role role);
        // Task<Result<Role>> UpdateRoleAsync(Role role);
        // Task<Result<bool>> DeleteRoleAsync(Guid id);
        /// </summary>
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RoleService> _logger;
        public RoleService(ApplicationDbContext context, ILogger<RoleService> logger)
        {
            _context = context;
            _logger = logger;
        }
        //obtener todos los roles 
        public async Task<Result<IEnumerable<Role>>> GetAllRolesAsync()
        {
            var roles = await _context.Role
                .AsNoTracking()
                .ToListAsync();
            if (roles.Count == 0)
            {
                _logger.LogWarning("No se encontraron roles en la base de datos."); // Log warning
                return Result<IEnumerable<Role>>.Fail("No roles found.");// Retornar un resultado de fallo si no se encuentran roles
            }
            return Result<IEnumerable<Role>>.Ok(roles);
        }

        // obtener roles paginados
        public async Task<Result<IEnumerable<Role>>> GetRolesByPageAsync(int pageNumber, int pageSize)
        {
            if (pageNumber <= 0 || pageSize <= 0)
            {
                _logger.LogWarning("Parámetros de paginación inválidos. pageNumber: {PageNumber}, pageSize: {PageSize}", pageNumber, pageSize);
                return Result<IEnumerable<Role>>.Fail("Invalid pagination parameters.");
            }

            var roles = await _context.Role
                .AsNoTracking()
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            if (roles == null || roles.Count == 0)
            {
                _logger.LogWarning("No se encontraron roles para la página solicitada. pageNumber: {PageNumber}, pageSize: {PageSize}", pageNumber, pageSize);
                return Result<IEnumerable<Role>>.Fail("No roles found for the requested page.");
            }

            return Result<IEnumerable<Role>>.Ok(roles);
        }

        //obtener roles por id
        public async Task<Result<Role>> GetRoleByIdAsync(Guid id)
        {
            var role = await _context.Role
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == id);
            if (role == null)
            {
                _logger.LogWarning($"Role con ID {id} no encontrado."); // Log warning
                return Result<Role>.Fail("Role not found.");
            }
            return Result<Role>.Ok(role);
        }
        //crear Role
        public async Task<Result<Role>> CreateRoleAsync(Role role)
        {
            var existentRole = await _context.Role
                 .AsNoTracking()
                 .FirstOrDefaultAsync(r => r.Name.ToLower() == role.Name.ToLower());
            if (existentRole != null)
            {
                _logger.LogWarning($"Role con nombre {role.Name} ya existe."); // Log warning
                return Result<Role>.Fail("Role with the same name already exists.");
            }

            role.Id = Guid.NewGuid();
            role.CreatedAt = DateTime.UtcNow;

            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            return Result<Role>.Ok(role);
        }
        //actualizar Role
        public async Task<Result<Role>> UpdateRoleAsync(Role role)
        {
            var existingRole = await _context.Role.FindAsync(role.Id);
            if (existingRole == null)
            {
                _logger.LogWarning($"Role con ID {role.Id} no encontrado para actualización."); // Log warning
                return Result<Role>.Fail("Role not found.");
            }

            existingRole.Name = role.Name;
            existingRole.Description = role.Description;

            _context.Role.Update(existingRole);
            await _context.SaveChangesAsync();

            return Result<Role>.Ok(existingRole);
        }
        //eliminar Role
        public async Task<Result<bool>> DeleteRoleAsync(Guid id)
        {
            var role = await _context.Role.FindAsync(id);
            if (role == null)
            {
                _logger.LogWarning($"Role con ID {id} no encontrado para eliminación."); // Log warning
                return Result<bool>.Fail("Role not found.");
            }

            _context.Role.Remove(role);
            await _context.SaveChangesAsync();

            return Result<bool>.Ok(true);
        }
    }
}