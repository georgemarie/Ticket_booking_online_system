using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL.ViewModels
{
    public class FlightSearchVM
    {
        public string From { get; set; }
        public string To { get; set; }
        public DateTime? Date { get; set; }

        public List<FlightService> Flights { get; set; } = new();
    }
}
