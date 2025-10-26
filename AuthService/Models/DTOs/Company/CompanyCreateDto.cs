using System;
using System.ComponentModel.DataAnnotations;

namespace AuthService.Models.DTOs.Company
{
    public class CompanyCreateDto
    {
        [Required]
        [MaxLength(100)]
        [MinLength(5)]
        public string Name { get; set; } = string.Empty;

        [MaxLength(11)]
        [MinLength(11)]
        public string? RUC { get; set; }

        [MaxLength(200)]
        [MinLength(5)]
        public string Address { get; set; } = string.Empty;

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? PhoneNumber { get; set; }
    }
}
