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

        public IEnumerable<FlightService>
           GetAllWithIncludes()
        {
            return _context.FlightServices

                .Include(f => f.Flight)

                    .ThenInclude(f =>
                        f.Airline)

                .Include(f => f.Flight)

                    .ThenInclude(f =>
                        f.OriginLocation)

                .Include(f => f.Flight)

                    .ThenInclude(f =>
                        f.DestLocation)

                .Include(f => f.Service)

                .ToList();
        }
        public IEnumerable<FlightService> Search(string from, string to, DateTime date)
        {
            return _context.FlightServices
                .Include(f => f.Service)             
                .Include(f => f.Flight)
                    .ThenInclude(x => x.OriginLocation)
                .Include(f => f.Flight)
                    .ThenInclude(x => x.DestLocation)
                .Include(f => f.Flight)
                .ThenInclude(x => x.Airline)
                .Where(f =>
                    f.Flight.OriginLocation.City.Contains(from) &&
                    f.Flight.DestLocation.City.Contains(to) &&
                    f.Flight.Depart_Date >= date.Date &&
                    f.Flight.Depart_Date < date.Date.AddDays(1))
                .ToList(); 
        }
    }
}
