using DAL.Models;
using System.ComponentModel.DataAnnotations;
using Ticket_booking_online_system.Data;

namespace DAL.CustomDataAnnotation
{
    public class ArrivalAfterDepartureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext))!;

                var arrivalTime = (DateTime)value;
                var currentFlight = (Flight)validationContext.ObjectInstance;

                if (arrivalTime <= currentFlight.Depart_Date)
                {
                    return new ValidationResult("Arrival time must be after the departure date.");
                }
            }

            return ValidationResult.Success;
        }
    }
}