using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
//aqui van lso using necesarios de Entity Framework u otros
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models
{
    public class User
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El nombre de usuario debe tener entre 5 y 100 caracteres.")]
        [MinLength(5, ErrorMessage = "El nombre de usuario debe tener entre 5 y 100 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [MaxLength(100, ErrorMessage = "El apellido de usuario debe tener entre 5 y 100 caracteres.")]
        [MinLength(5, ErrorMessage = "El apellido de usuario debe tener entre 5 y 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string PasswordHash { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public Guid RoleId { get; set; } // FK → Role
        public Role? Role { get; set; }

        [Required]
        public Guid CompanyId { get; set; } // FK → Company
        public Company? Company { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }

}