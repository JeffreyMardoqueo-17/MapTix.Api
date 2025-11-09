using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models.DTOs.User
{
    public class UserResponseDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public Guid RoleId { get; set; }
        public Guid CompanyId { get; set; }
        public DateTime CreatedAt { get; set; }

        // ⚡ No se guarda en DB, solo se devuelve al crear o loguear
        public string Token { get; set; } = string.Empty;
    }
}