using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.helpers
{
    /// <summary>
    /// Helper responsable de generar tokens JWT para los usuarios autenticados.
    /// </summary>
    /// <remarks>
    /// El token incluye los siguientes claims:
    /// - sub → Id del usuario (Guid)
    /// - email → Correo electrónico del usuario
    /// - companyId → Id de la compañía a la que pertenece
    /// - role → Rol asignado al usuario
    /// - jti → Identificador único del token
    /// 
    /// La firma se realiza con HMAC-SHA256 y expira en 8 horas.
    /// </remarks>
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Constructor que recibe la configuración de la aplicación para acceder a las claves JWT.
        /// </summary>
        /// <param name="configuration">Configuración de la aplicación (appsettings.json).</param>
        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Genera un token JWT firmado con los datos del usuario proporcionado.
        /// </summary>
        /// <param name="user">Instancia del usuario autenticado.</param>
        /// <returns>Token JWT firmado en formato string.</returns>
        public string GenerateToken(User user)
        {
            // Sección de configuración "Jwt" en appsettings.json
            var jwtSection = _configuration.GetSection("Jwt");

            // Clave secreta para la firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSection["Key"]!));

            // Credenciales de firma (algoritmo HMAC-SHA256)
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims (información que viajará dentro del token)
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()), // Identificador del usuario
                new Claim(JwtRegisteredClaimNames.Email, user.Email),       // Correo del usuario
                new Claim("companyId", user.CompanyId.ToString()),          // Empresa asociada
                new Claim("role", user.Role?.Name ?? "User"),               // Rol del usuario
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Identificador único del token
            };

            // Creación del token con parámetros configurables
            var token = new JwtSecurityToken(
                issuer: jwtSection["Issuer"],
                audience: jwtSection["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(8), // Expiración del token (8 horas)
                signingCredentials: creds
            );

            // Se devuelve el token como string (codificado en Base64)
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

/*
=========================
📄 EJEMPLO DE TOKEN JWT
=========================

🔹 appsettings.json (configuración esperada)
-------------------------------------------
"Jwt": {
  "Key": "TU_CLAVE_SECRETA_SEGURA_DE_AL_MENOS_32_CARACTERES",
  "Issuer": "AuthService",
  "Audience": "AuthClients"
}

🔹 Ejemplo de usuario
-------------------------------------------
{
  "Id": "c7b6f1f8-19f3-4c4e-928b-60e4f0a3b5af",
  "Email": "admin@empresa.com",
  "CompanyId": "b9d4c5a9-3f8e-4e6f-9d4b-6cfa1f118b56",
  "Role": { "Name": "AdminCompany" }
}

🔹 Token generado (forma abreviada)
-------------------------------------------
eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.
eyJzdWIiOiJjN2I2ZjFmOC0xOWYzLTRjNGUtOTI4Yi02MGU0ZjBhM2I1YWYiLCJlbWFpbCI6ImFkbWluQGVtcHJlc2EuY29tIiwiY29tcGFueUlkIjoiYjlkNGM1YTktM2Y4ZS00ZTZmLTlkNGItNmNmYTFmMTE4YjU2Iiwicm9sZSI6IkFkbWluQ29tcGFueSIsImp0aSI6ImQ3NGMyYzQ2LTk0NzMtNDk1OC04ZDRjLThkMmZkMzRjNGQ2NCIsImlzcyI6IkF1dGhTZXJ2aWNlIiwiYXVkIjoiQXV0aENsaWVudHMiLCJleHAiOjE3MzA2ODE1MDAsImlhdCI6MTczMDY1MjcwMH0.
R9Uu3dHhIvOli-3dU7YV0ZL5ZJpKZJxV0OazE8jR2zE

🔹 Decodificado (payload JSON)
-------------------------------------------
{
  "sub": "c7b6f1f8-19f3-4c4e-928b-60e4f0a3b5af",
  "email": "admin@empresa.com",
  "companyId": "b9d4c5a9-3f8e-4e6f-9d4b-6cfa1f118b56",
  "role": "AdminCompany",
  "jti": "d74c2c46-9473-4958-8d4c-8d2fd34c4d64",
  "iss": "AuthService",
  "aud": "AuthClients",
  "exp": 1730681500,
  "iat": 1730652700
}
*/
