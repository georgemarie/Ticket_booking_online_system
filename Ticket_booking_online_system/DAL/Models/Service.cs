using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public abstract class Service
    {
        [Key]
        public int ServiceID { get; set; }

        [Required(ErrorMessage = "Base Price is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Base Price must be greater than zero.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal BasePrice { get; set; }

        [ForeignKey("Location")]
        [Required(ErrorMessage = "Location is required.")]
        public int LocationID { get; set; }
        public Location Location { get; set; }

        public ICollection<Review> Reviews { get; set; }
        public ICollection<Booking> Bookings { get; set; }
    }

    public class TransportationService : Service
    {
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

    public class FlightService : Service
    {
        [ForeignKey("Airline")]
        [Required(ErrorMessage = "Airline is required for Flight Services.")]
        public int Airline_ID { get; set; }
        public Airline Airline { get; set; }

        [Required(ErrorMessage = "Flight Number is required for Flight Services.")]
        [StringLength(20)]
        public string Flight_Number { get; set; }
    }

    public class HotelService : Service
    {
        [StringLength(150, ErrorMessage = "Hotel Name cannot exceed 150 characters.")]
        public string Hotel_Name { get; set; }

        [StringLength(100, ErrorMessage = "Room Type cannot exceed 100 characters.")]
        public string Room_Type { get; set; }

        public DateTime? Check_In { get; set; }

        public DateTime? Check_Out { get; set; }
    }
}
