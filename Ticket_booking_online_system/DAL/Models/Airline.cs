using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL.Models
{
    public enum SupportedAirline
    {
        EgyptAir,
        Emirates,
        Lufthansa,
        BritishAirways,
        DeltaAirLines,
        SingaporeAirlines,
        Qantas
    }
    public class Airline
    {
        [Key]
        public int Airline_ID { get; set; }

        [Required(ErrorMessage = "Airline Name is required.")]
        [EnumDataType(typeof(SupportedAirline))]
        public string Airline_Name { get; set; }

        public ICollection<Flight> Flights { get; set; }
        public ICollection<FlightService> FlightServices { get; set; }
    }
}
