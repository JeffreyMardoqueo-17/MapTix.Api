using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AuthService.Services;
using AuthService.DTOs;
using AuthService.Models;
using AutoMapper;
using AuthService.DTOs.User;
using AuthService.Repositories;
using AuthService.Models.DTOs.User;

namespace AuthService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, IMapper mapper, ILogger<UserController> logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }
        /// <summary>
        /// 🔹 Registra un usuario administrador asociado a una empresa existente.
        /// </summary>
        /// <param name="dto">Datos necesarios para crear el usuario administrador.</param>
        /// <returns>Información del usuario creado y su token JWT.</returns>
        /// <remarks>
        /// Este endpoint:
        /// - Valida la entrada del usuario (DTO).
        /// - Llama al servicio que crea el usuario administrador.
        /// - Devuelve el usuario creado junto con un token JWT.
        /// </remarks>
        [HttpPost("register-admin")]
        [ProducesResponseType(typeof(UserResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UserResponseDto>> RegisterCompanyAndAdminAsync([FromBody] UserCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Intento de registro inválido. Errores: {@Errors}", ModelState.Values);
                return BadRequest(new
                {
                    message = "Los datos enviados no son válidos.",
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            try
            {
                // 🔹 DTO → entidad User
                var user = _mapper.Map<User>(dto);
                var company = new Company { Id = dto.CompanyId };

                // 🔹 Servicio: crea usuario y genera token
                var result = await _userService.RegisterCompanyAndAdminAsync(company, user);

                if (!result.Success || result.Data.user == null)
                {
                    _logger.LogWarning("No se pudo crear el administrador para la compañía {CompanyId}", dto.CompanyId);
                    return BadRequest(new { message = result.Message ?? "No fue posible crear el usuario administrador." });
                }

                // 🔹 Entidad → DTO de salida
                var response = _mapper.Map<UserResponseDto>(result.Data.user);
                response.Token = result.Data.token;

                _logger.LogInformation("Administrador creado correctamente: {Email}", result.Data.user.Email);

                // ⚡ CreatedAtAction apunta a un método GET existente o a una ruta simbólica
                return Created($"api/users/{result.Data.user.Id}", response);
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Error de negocio al registrar administrador: {Message}", ex.Message);
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al registrar administrador.");
                return StatusCode(500, new { message = "Ocurrió un error interno al registrar el administrador." });
            }
        }
    }
}
