using System;
using Isopoh.Cryptography.Argon2;

namespace AuthService.Helpers
{
    public static class EncryptHelper
    {
        /// <summary>
        /// Genera un hash seguro de la contraseña usando Argon2id.
        /// </summary>
        public static string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty.");

            // Argon2.Hash() ya genera salt + parámetros internamente.
            // Devuelve un hash completo listo para almacenar en la base de datos.
            return Argon2.Hash(password);
        }

        /// <summary>
        /// Verifica si una contraseña coincide con el hash almacenado.
        /// </summary>
        public static bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(storedHash))
                return false;

            try
            {
                return Argon2.Verify(storedHash, password);
            }
            catch
            {
                return false;
            }
        }
    }
}
