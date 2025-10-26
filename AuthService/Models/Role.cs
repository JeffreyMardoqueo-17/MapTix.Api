using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuthService.Models
{
    public class Role
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(50, ErrorMessage = "El nombre del rol debe tener entre 3 y 50 caracteres.")]
        [MinLength(3, ErrorMessage = "El nombre del rol debe tener entre 3 y 50 caracteres.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200, ErrorMessage = "La descripción del rol no puede exceder los 200 caracteres.")]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}