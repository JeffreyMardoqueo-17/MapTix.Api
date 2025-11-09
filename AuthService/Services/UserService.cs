using System;
using System.Threading.Tasks;
using AuthService.DataBase;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using AuthService.Helpers;
using Microsoft.Extensions.Logging;
using AuthService.helpers;
using AuthService.Repositories;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly JwtHelper _jwtHelper;
        private readonly CompanyService _companyService;
        private readonly RoleService _roleService;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger, JwtHelper jwtHelper, CompanyService companyService, RoleService roleService)
        {
            _context = context;
            _logger = logger;
            _jwtHelper = jwtHelper;
            _companyService = companyService;
            _roleService = roleService;
        }
        /// <summary>
        /// 🟢 Registra un usuario administrador asociado a una empresa existente.
        /// </summary>
        /// <param name="company">Objeto Company que contiene el Id de la empresa.</param>
        /// <param name="adminUser">Datos del usuario administrador que se creará.</param>
        /// <returns>Tupla con el usuario creado y el token JWT generado.</returns>
        /// <remarks>
        /// Este método:
        /// - Verifica que la empresa exista.
        /// - Valida duplicado de correo.
        /// - Obtiene el rol "AdminCompany".
        /// - Encripta la contraseña con Argon2id.
        /// - Registra el usuario en la base de datos.
        /// - Devuelve el usuario y un JWT válido por 8 horas.
        /// </remarks>
        public async Task<Result<(User user, string token)>> RegisterCompanyAndAdminAsync(Company company, User adminUser)
        {
            // 🔍 Verificar que la empresa exista
            var existingCompany = await _companyService.GetCompanyByIdAsync(company.Id);
            if (existingCompany == null || existingCompany.Data == null)
            {
                _logger.LogWarning("Intento de registrar admin para empresa inexistente: {CompanyId}", company.Id);
                throw new InvalidOperationException("La empresa no existe.");
            }

            //  Validar correo unico
            await UserValidationHelper.EnsureUserEmailIsUniqueAsync(_context, adminUser.Email, _logger);

            //  Obtener rol AdminCompany
            var adminRole = await _roleService.GetRoleByNameAsync("AdminCompany");
            if (adminRole == null || adminRole.Data == null)
            {
                _logger.LogError("El rol 'AdminCompany' no está configurado en la base de datos.");
                throw new InvalidOperationException("El rol 'AdminCompany' no está configurado.");
            }

            try
            {
                // Crear usuario administrador 
                adminUser.Id = Guid.NewGuid();
                adminUser.CompanyId = existingCompany.Data.Id;
                adminUser.RoleId = adminRole.Data.Id;
                adminUser.IsActive = true;
                adminUser.CreatedAt = DateTime.UtcNow;

                //  Encriptar contraseña con Argon2id (usando helper)
                adminUser.PasswordHash = EncryptHelper.HashPassword(adminUser.PasswordHash);

                _context.User.Add(adminUser);
                await _context.SaveChangesAsync();

                //  Generar JWT
                var token = _jwtHelper.GenerateToken(adminUser);

                _logger.LogInformation("Usuario administrador creado correctamente para empresa {CompanyId}", existingCompany.Data.Id);

                return Result<(User user, string token)>.Ok((adminUser, token));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario administrador para empresa {CompanyId}", company.Id);
                throw;
            }
        }
        public async Task<Result<User>> GetUserByIdAsync(Guid id)
        {
            try
            {
                var user = await _context.User
                    .AsNoTracking()
                    .Include(u => u.Role)        // Agregi la relacion a las demas tablas 
                    .Include(u => u.Company)     // Agregi la relacion a las demas tablas 
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (user == null)
                {
                    _logger.LogWarning("Usuario no encontrado: {UserId}", id);
                    return Result<User>.Fail("Usuario no encontrado.");
                }

                _logger.LogInformation("Usuario encontrado correctamente: {UserId}", id);
                return Result<User>.Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener usuario por Id: {UserId}", id);
                return Result<User>.Fail("Error al obtener el usuario.");
            }
        }

    }
}


// 🧩 Implementaciones pendientes
// public Task<User> CreateUserAsync(User user, Guid roleId) => throw new NotImplementedException();
// public Task<User?> AuthenticateAsync(string email, string password) => throw new NotImplementedException();
// public Task<string> GenerateTokenAsync(User user) => Task.FromResult(_jwtHelper.GenerateToken(user));
// public async Task<User?> GetByEmailAsync(string email) => 
//     await _context.Users.FirstOrDefaultAsync(u => u.Email == email);


