using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class Payment
    {
        [Key]
        public int PaymentID { get; set; }

        [Required(ErrorMessage = "Amount is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Amount must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Payment Method is required.")]
        [StringLength(50, ErrorMessage = "Payment Method cannot exceed 50 characters.")]
        public string Method { get; set; }

        [Required(ErrorMessage = "Payment Date is required.")]
        public DateTime Payment_Date { get; set; }

        [ForeignKey("User")]
        [Required(ErrorMessage = "User is required.")]
        public int UserID { get; set; }
        public User User { get; set; }

        [ForeignKey("Booking")]
        [Required(ErrorMessage = "Booking is required.")]
        public int BookingID { get; set; }
        public Booking Booking { get; set; }
    }
}
