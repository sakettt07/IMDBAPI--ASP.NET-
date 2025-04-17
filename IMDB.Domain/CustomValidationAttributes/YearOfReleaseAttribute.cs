using System;
using System.ComponentModel.DataAnnotations;

namespace IMDB.Domain.CustomValidation
{
    public class YearOfReleaseAttribute : ValidationAttribute
    {
        private readonly int _minYear = 1930;

        public YearOfReleaseAttribute()
        {
            ErrorMessage = $"Year of release must not be smaller than {_minYear}.";
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is int year && year < _minYear)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
