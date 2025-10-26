using AuthService.Models;
using System.Text.RegularExpressions;

namespace AuthService.Helpers
{
    public static class CompanyValidator
    {
        // 📦 Expresiones regulares compiladas (solo se cargan una vez)
        private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);
        private static readonly Regex _rucRegex = new(@"^\d{10,15}$", RegexOptions.Compiled); 
        private static readonly Regex _digitsRegex = new(@"\D", RegexOptions.Compiled);

        /// <summary>
        /// Valida una entidad Company devolviendo una lista de errores si los hay.
        /// </summary>
        public static IEnumerable<string> Validate(Company company)
        {
            var errors = new List<string>();

            if (company == null)
            {
                errors.Add("Company object is null.");
                return errors;
            }

            // 🧾 Name
            if (string.IsNullOrWhiteSpace(company.Name))
                errors.Add("Company name is required.");
            else if (company.Name.Length < 5 || company.Name.Length > 100)
                errors.Add("Company name must be between 5 and 100 characters.");

            // 🏠 Address
            if (string.IsNullOrWhiteSpace(company.Address))
                errors.Add("Company address is required.");
            else if (company.Address.Length < 5)
                errors.Add("Address must be at least 5 characters long.");

            // 📧 Email (opcional)
            if (!string.IsNullOrWhiteSpace(company.Email) && !_emailRegex.IsMatch(company.Email))
                errors.Add("Invalid email format.");

            // ☎️ Phone number (opcional, con normalización previa)
            if (!string.IsNullOrWhiteSpace(company.PhoneNumber))
            {
                var normalizedPhone = NormalizePhone(company.PhoneNumber);
                var digits = _digitsRegex.Replace(normalizedPhone, "");
                if (digits.Length < 8 || digits.Length > 15)
                    errors.Add("Phone number must contain between 8 and 15 digits.");
            }

            // 🧾 RUC (si existe, validar formato numérico)
            if (!string.IsNullOrWhiteSpace(company.RUC) && !_rucRegex.IsMatch(company.RUC))
                errors.Add("RUC must contain only numbers (10–15 digits).");

            return errors;
        }

        /// <summary>
        /// Limpia el formato del número de teléfono.
        /// </summary>
        public static string NormalizePhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
                return phone ?? string.Empty;

            return phone
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("(", "")
                .Replace(")", "");
        }
    }
}
