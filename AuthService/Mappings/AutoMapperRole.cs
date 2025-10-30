
using AutoMapper;
using AuthService.Models;
using AuthService.Models.DTOs.Role;

namespace AuthService.Mappings
{
    public class AutoMapperRole: Profile
    {
        public AutoMapperRole()
        {
            // De entidad => DTO ()
            CreateMap<Role, RoleReadDTO>();

            // De DTO => Entidad (Crear)
            CreateMap<RoleCreateDTO, Role>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));

            // De DTO => Entidad (Actualizar)
            CreateMap<RoleUpdateDTO, Role>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Para evitar sobreescribir el CreatedAt
        }
    }
}