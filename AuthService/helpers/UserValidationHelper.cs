using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AuthService.DataBase;

namespace AuthService.helpers
{
    /// <summary>
    /// Contiene métodos de validación relacionados exclusivamente con la entidad User.
    /// </summary>
    public static class UserValidationHelper
    {
        /// <summary>
        /// Verifica si ya existe un usuario con el correo proporcionado.
        /// Lanza una excepción si el correo está duplicado.
        /// </summary>
        /// <param name="context">Instancia del DbContext.</param>
        /// <param name="email">Correo electrónico del usuario a validar.</param>
        /// <param name="logger">Logger opcional para registrar advertencias.</param>
        public static async Task EnsureUserEmailIsUniqueAsync(
            ApplicationDbContext context,
            string email,
            ILogger? logger = null)
        {
            bool exists = await context.User.AsNoTracking().AnyAsync(u => u.Email == email);

            if (exists)
            {
                logger?.LogWarning("Intento de registrar usuario con correo duplicado: {Email}", email);
                throw new InvalidOperationException($"Ya existe un usuario registrado con el correo: {email}");
            }
        }

        /// <summary>
        /// Verifica si el usuario con el Id proporcionado existe en la base de datos.
        /// </summary>
        /// <param name="context">Instancia del DbContext.</param>
        /// <param name="userId">Id del usuario a validar.</param>
        /// <param name="logger">Logger opcional para registrar advertencias.</param>
        public static async Task EnsureUserExistsAsync(
            ApplicationDbContext context,
            Guid userId,
            ILogger? logger = null)
        {
            bool exists = await context.User.AsNoTracking().AnyAsync(u => u.Id == userId);

            if (!exists)
            {
                logger?.LogWarning("Intento de operar sobre usuario inexistente: {UserId}", userId);
                throw new InvalidOperationException($"El usuario con Id {userId} no existe en la base de datos.");
            }
        }
    }
}
