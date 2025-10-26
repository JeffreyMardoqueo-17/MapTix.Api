using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AuthService.Models
{
    public class Company
    {
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "El nombre de la compañía debe tener entre 5 y 100 caracteres.")]
        [MinLength(5, ErrorMessage = "El nombre de la compañía debe tener entre 5 y 100 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(11, ErrorMessage = "El RUC debe tener 11 caracteres.")]
        [MinLength(11, ErrorMessage = "El RUC debe tener 11 caracteres.")]
        public string RUC { get; set; } = string.Empty;

        [Required]
        [MaxLength(200, ErrorMessage = "La dirección debe tener un máximo de 200 caracteres.")]
        [MinLength(5, ErrorMessage = "La dirección debe tener al menos 5 caracteres.")]
        public string Address { get; set; } = string.Empty;
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string? Email { get; set; }

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
