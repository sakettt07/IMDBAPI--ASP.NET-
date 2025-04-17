using System;
using System.ComponentModel.DataAnnotations;

namespace IMDB.Domain.CustomValidation
{
    public class DOBValidationAttribute : ValidationAttribute
    {
        private readonly DateTime _minDate = new DateTime(1930, 1, 1);

        public DOBValidationAttribute()
        {
            ErrorMessage = $"Date of Birth must not be earlier than {_minDate:yyyy} and must not be in the future.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dob)
            {
                if (dob < _minDate)
                {
                    return new ValidationResult($"Date of Birth must not be earlier than {_minDate:yyyy}.");
                }
                if (dob > DateTime.Today)
                {
                    return new ValidationResult("Date of Birth must not be in the future.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
