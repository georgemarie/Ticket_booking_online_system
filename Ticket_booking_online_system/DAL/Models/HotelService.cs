using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class HotelService
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        [StringLength(150, ErrorMessage = "Hotel Name cannot exceed 150 characters.")]
        public string Hotel_Name { get; set; }

        [StringLength(100, ErrorMessage = "Room Type cannot exceed 100 characters.")]
        public string Room_Type { get; set; }

        public DateTime? Check_In { get; set; }

        public DateTime? Check_Out { get; set; }
    }
}
