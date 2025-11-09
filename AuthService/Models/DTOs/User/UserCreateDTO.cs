using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.DTOs.User
{
    /// <summary>
    /// DTO para la creación de un nuevo usuario administrador o estándar.
    /// </summary>
    public class UserCreateDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio.")]
        [MaxLength(100, ErrorMessage = "El apellido no puede exceder los 100 caracteres.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "El correo electrónico no es válido.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        public string Password { get; set; } = string.Empty;

        [Phone(ErrorMessage = "El número de teléfono no es válido.")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "El identificador de la empresa es obligatorio.")]
        public Guid CompanyId { get; set; }
    }
}
