using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public class Airline
    {
        [Key]
        public int Airline_ID { get; set; }

        [Required(ErrorMessage = "Airline Name is required.")]
        [StringLength(150, ErrorMessage = "Airline Name cannot exceed 150 characters.")]
        public string Airline_Name { get; set; }

        public ICollection<Flight> Flights { get; set; }
        public ICollection<FlightService> FlightServices { get; set; }
    }
}
