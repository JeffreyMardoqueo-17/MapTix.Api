using AutoMapper;
using AuthService.Models;
using AuthService.Models.DTOs.Company;

namespace AuthService.Mappings
{
    public class AutoMapperCompany : Profile
    {
        public AutoMapperCompany()
        {
            // 🔹 De entidad -> DTO (para leer datos)
            CreateMap<Company, CompanyReadDto>();

            // 🔹 De DTO -> Entidad (para crear nuevas compañías)
            CreateMap<CompanyCreateDto, Company>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));

            // 🔹 De DTO -> Entidad (para actualizar)
            CreateMap<CompanyUpdateDto, Company>()
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore()); // Evitamos sobreescribir el CreatedAt
        }
    }
}
