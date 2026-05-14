using DAL.CustomDataAnnotation;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public enum FlightClass
    {
        Economy,
        Business,
        FirstClass
    }
    public class Flight
    {
        [Key]
        [Required(ErrorMessage = "Flight Number is required.")]
        [StringLength(20, ErrorMessage = "Flight Number cannot exceed 20 characters.")]
        public string Flight_Number { get; set; }

        [ForeignKey("OriginLocation")]
        [Required(ErrorMessage = "Origin Location is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an origin city.")]
        public int Origin_LocationID { get; set; }

        [BindNever]
        public Location? OriginLocation { get; set; }

        [ForeignKey("DestLocation")]
        [Required(ErrorMessage = "Destination Location is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select a destination city.")]
        public int Dest_LocationID { get; set; }

        [BindNever]
        public Location? DestLocation { get; set; }

        //[Required(ErrorMessage = "Arrival Time is required.")]
        [ArrivalAfterDeparture]
        public DateTime? Arrival_Time { get; set; }

        [Required(ErrorMessage = "Departure Date is required.")]
        public DateTime Depart_Date { get; set; }

        [Required(ErrorMessage = "Available Seats is required.")]
        [Range(0, 1000, ErrorMessage = "Available Seats cannot be negative.")]
        public int Available_Seats { get; set; }

        [Required(ErrorMessage = "Class is required.")]
        public FlightClass Class { get; set; }
            
        [ForeignKey("Airline")]
        [Required(ErrorMessage = "Airline is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "Please select an airline.")]
        public int Airline_ID { get; set; }

        [BindNever]
        public Airline? Airline { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
