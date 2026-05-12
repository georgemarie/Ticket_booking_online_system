using DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Custom_Data_Annotation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext))!;
                var email = value as string;

                var currentUser = (User)validationContext.ObjectInstance;

                if (context.Users.Any(s => s.Email == email && s.UserID != currentUser.UserID))
                {
                    return new ValidationResult("Email already exists. Must be unique.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
