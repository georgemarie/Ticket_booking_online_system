using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class TransportationService
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }
        [Required(ErrorMessage = "Vehicle Type is required.")]
        [StringLength(50, ErrorMessage = "Vehicle Type cannot exceed 50 characters.")]
        public string Vehicle_Type { get; set; } // e.g., Bus, Train

        [StringLength(150, ErrorMessage = "Company Name cannot exceed 150 characters.")]
        public string Company_Name { get; set; }

        public DateTime? Departure_Time { get; set; }

        public DateTime? Arrival_Time { get; set; }

        [StringLength(250, ErrorMessage = "Pickup Location detail cannot exceed 250 characters.")]
        public string Pickup_Detail { get; set; }

        [StringLength(250, ErrorMessage = "Dropoff Location detail cannot exceed 250 characters.")]
        public string Dropoff_Detail { get; set; }
    }
}
