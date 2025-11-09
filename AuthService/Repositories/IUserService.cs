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
        //obtener usuario por Id
        Task<Result<User>> GetUserByIdAsync(Guid id);
        // Crea un usuario dentro de una company ya existente.
        // Se espera que user.CompanyId esté asignado y que roleId sea el rol a asignar.
        // Task<User> CreateUserAsync(User user, Guid roleId); //aqui sera para crear usuarios internos 

        // Onboarding público: crea Company + User admin en una única transacción,
        // asignando automáticamente el rol AdminCompany al usuario y devolviendo JWT.
        Task<Result<(User user, string token)>> RegisterCompanyAndAdminAsync(Company company, User adminUser);

        // Autentica (login)
        // Task<User?> AuthenticateAsync(string email, string password);

        // Genera JWT para un usuario ya cargado (incluye companyId y role)
        // Task<string> GenerateTokenAsync(User user);

        // Buscar usuario por email
        // Task<User?> GetByEmailAsync(string email);
    }


}
