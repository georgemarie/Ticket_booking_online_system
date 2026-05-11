using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class FlightServiceRepository
         : GenericRepository<FlightService>,
           IFlightServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public FlightServiceRepository(ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }

        public IEnumerable<FlightService> GetAllFlights()
        {
            return _context.Services
                .OfType<FlightService>()
                .Include(f => f.Location)
                .Include(f => f.Airline)
                .ToList();
        }
    }
}
