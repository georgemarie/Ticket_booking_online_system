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
        
        public override void Add(FlightService entity)
        {
            if (entity.Service != null)
            {
                entity.Service.LocationID = entity.Flight.Origin_LocationID;

                entity.Service.ServiceType = "Flight";
                _context.Services.Add(entity.Service);
                _context.SaveChanges();
                entity.ServiceId = entity.Service.ServiceID;
            }
            if (entity.Flight != null)
            {
                entity.Flight.Airline = null;
                entity.Flight.OriginLocation = null;
                entity.Flight.DestLocation = null;
                entity.Flight_Number = entity.Flight.Flight_Number;

                _context.Flights.Add(entity.Flight);
                _context.SaveChanges();
            }
            _context.FlightServices.Add(entity);
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
        public FlightService? GetByIdWithIncludes(int id)
        {
            return _context.FlightServices
                .Include(fs => fs.Flight)
                .Include(fs => fs.Service)
                .FirstOrDefault(fs => fs.Id == id);
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
