using AutoMapper;
using AuthService.Models;
using AuthService.DTOs;
using AuthService.DTOs.User;
using AuthService.Models.DTOs.User;

namespace AuthService.Mappings
{
    public class AutoMapperUser : Profile
    {
        public AutoMapperUser()
        {
            CreateMap<UserCreateDto, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)) // luego se encripta
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(_ => true));
            
            // Entity → DTO de respuesta
            CreateMap<User, UserResponseDto>();
        }
    }
}
