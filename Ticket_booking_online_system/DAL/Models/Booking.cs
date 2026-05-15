using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{   public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
    public class Booking
    {
        [Key]
        public int BookingID { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public BookingStatus Status { get; set; }

        
        [ForeignKey("User")]
        public string UserID { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey("Service")]
        public int ServiceID { get; set; }
        public Service? Service { get; set; }

        //[ForeignKey("Flight")]
        //public string Flight_Number { get; set; }
        //public Flight Flight { get; set; }

        public Payment? Payment { get; set; }
        public RefundCancel? RefundCancel { get; set; }
    }
}
