using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Filters
{
    public class ActorNameAttribute : ValidationAttribute
    {
        public string GetErrorMessage() =>
            $"The field must have at least 4 characters.";

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var name = (string)value;

            if (name.Length <= 3)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}