using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Custom_Data_Annotation
{
    public class UniquePassportAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext))!;
                var passportNum = value as string;

                var currentUser = validationContext.ObjectInstance as ApplicationUser;

                if (currentUser == null)
                    return ValidationResult.Success;

                if (context.Users.Any(u => u.Passport_num == passportNum && u.Id != currentUser.Id))
                {
                    return new ValidationResult("Passport number already exists. Must be unique.");
                }
            }

            return ValidationResult.Success;
        }
    }
}
