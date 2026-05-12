using System;
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.CustomDataAnnotation
{
    public class ValidCancellationDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var cancellationDate = (DateTime)value;

               
                if (cancellationDate.Date > DateTime.Today)
                {
                    return new ValidationResult(
                        "Cancellation date cannot be in the future."
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}