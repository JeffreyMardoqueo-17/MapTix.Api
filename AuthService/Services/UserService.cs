using System;
using System.Threading.Tasks;
using AuthService.DataBase;
using AuthService.Models;
using Microsoft.EntityFrameworkCore;
using AuthService.helpers;
using Microsoft.Extensions.Logging;
using AuthService.Helpers;

namespace AuthService.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly JwtHelper _jwtHelper;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger, JwtHelper jwtHelper)
        {
            _context = context;
            _logger = logger;
            _jwtHelper = jwtHelper;
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
        public async Task<(User user, string token)> RegisterCompanyAndAdminAsync(Company company, User adminUser)
        {
            // 🔍 Verificar que la empresa exista
            var existingCompany = await _context.Company
                .FirstOrDefaultAsync(c => c.Id == company.Id);

            if (existingCompany == null)
            {   
                _logger.LogWarning("Intento de registrar admin para empresa inexistente: {CompanyId}", company.Id);
                throw new InvalidOperationException("La empresa especificada no existe.");
            }

            // 🔍 Validar duplicado de correo
            if (await _context.User.AnyAsync(u => u.Email == adminUser.Email))
            {
                _logger.LogWarning("Intento de registrar admin con correo duplicado: {Email}", adminUser.Email);
                throw new InvalidOperationException("Ya existe un usuario con ese correo.");
            }

            // 🔍 Obtener rol AdminCompany
            var adminRole = await _context.Role.FirstOrDefaultAsync(r => r.Name == "AdminCompany");
            if (adminRole == null)
            {
                _logger.LogError("El rol 'AdminCompany' no está configurado en la base de datos.");
                throw new InvalidOperationException("El rol 'AdminCompany' no está configurado.");
            }

            try
            {
                // Crear usuario administrador 
                adminUser.Id = Guid.NewGuid();
                adminUser.CompanyId = existingCompany.Id;
                adminUser.RoleId = adminRole.Id;
                adminUser.IsActive = true;
                adminUser.CreatedAt = DateTime.UtcNow;

                // 🔐 Encriptar contraseña con Argon2id (usando helper)
                adminUser.PasswordHash = EncryptHelper.HashPassword(adminUser.PasswordHash);

                _context.User.Add(adminUser);
                await _context.SaveChangesAsync();

                // 🔑 Generar JWT
                var token = _jwtHelper.GenerateToken(adminUser);

                _logger.LogInformation("Usuario administrador creado correctamente para empresa {CompanyId}", existingCompany.Id);

                return (adminUser, token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear usuario administrador para empresa {CompanyId}", company.Id);
                throw;
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


