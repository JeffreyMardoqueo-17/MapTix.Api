using AuthService.Models;
using System.Collections.Generic;

namespace AuthService.helpers
{
    public static class CompanyValidator
    {
        public static List<string> Validate(Company company)
        {
            var errors = new List<string>();

            if (company == null)
            {
                errors.Add("Company object is null.");
                return errors;
            }

            if (string.IsNullOrWhiteSpace(company.Name))
                errors.Add("Company name is required.");

            if (company.Name.Length < 5 || company.Name.Length > 100)
                errors.Add("Company name must be between 5 and 100 characters.");

            if (string.IsNullOrWhiteSpace(company.Address))
                errors.Add("Company address is required.");

            if (company.Address.Length < 5)
                errors.Add("Address must be at least 5 characters long.");

            return errors;
        }
    }
}
