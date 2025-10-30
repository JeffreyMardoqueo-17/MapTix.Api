using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs.Role
{
    public class RoleCreateDTO
    {
        [Required]
        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        [MinLength(3)]
        public string Description { get; set; } = string.Empty;
    }
}