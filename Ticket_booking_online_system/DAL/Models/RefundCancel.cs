using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class RefundCancel
    {
        [Key]
        public int RefundID { get; set; }

        [Required(ErrorMessage = "Refund Amount is required.")]
        [Range(0, double.MaxValue, ErrorMessage = "Refund Amount cannot be negative.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        [Required(ErrorMessage = "Cancellation Date is required.")]
        public DateTime Cancellation_Date { get; set; }

        [ForeignKey("Booking")]
        [Required(ErrorMessage = "Booking is required.")]
        public int BookingID { get; set; }
        public Booking Booking { get; set; }
    }
}
