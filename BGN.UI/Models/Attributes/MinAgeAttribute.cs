using System.ComponentModel.DataAnnotations;

namespace BGN.UI.Models.Attributes
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var today = DateTime.Today;

                // Calculate the age
                var age = today.Year - dateOfBirth.Year;

                // Check if the birthdate hasn't occurred yet this year
                if (dateOfBirth > today.AddYears(-age))
                {
                    age--; // If the birthday hasn't occurred yet this year, subtract one year from age
                }

                // If the calculated age is greater than or equal to the minimum required age
                if (age >= _minAge)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult($"You must be at least {_minAge} years old.");
                }
            }

            return new ValidationResult("Invalid date of birth.");
        }
    }
}