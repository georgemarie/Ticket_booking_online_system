using System;
using System.ComponentModel.DataAnnotations;
using DAL.Models;

namespace DAL.CustomDataAnnotation
{
    public class CheckOutAfterCheckInAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var checkOut = (DateTime)value;
                var currentService = (HotelService)validationContext.ObjectInstance;

                if (currentService.Check_In == null)
                    return ValidationResult.Success;

                var checkIn = currentService.Check_In.Value;

                if (checkOut < checkIn.AddDays(1))
                {
                    return new ValidationResult(
                        "Check-Out date must be at least one day after Check-In date."
                    );
                }
            }

            return ValidationResult.Success;
        }
    }
}