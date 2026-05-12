using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DAL.Models
{
    public class FlightService
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("Service")]
        public int ServiceId { get; set; }

        public Service Service { get; set; }
        //[ForeignKey("Airline")]
        //[Required(ErrorMessage = "Airline is required for Flight Services.")]
        //public int Airline_ID { get; set; }
        //public Airline Airline { get; set; }
        [ForeignKey("Flight")]
        [Required(ErrorMessage = "Flight Number is required for Flight Services.")]
        [StringLength(20)]
        public string Flight_Number { get; set; }

        public Flight Flight { get; set; }
    }
}
