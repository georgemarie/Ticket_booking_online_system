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
        //public override void Add(FlightService entity)
        //{
        //    if (entity.Flight == null)
        //        throw new InvalidOperationException(
        //            $"Flight navigation property is null. Flight_Number={entity.Flight_Number}");

        //    // DEBUG - remove after fix confirmed
        //    Console.WriteLine($"Airline_ID on Flight: {entity.Flight.Airline_ID}");
        //    Console.WriteLine($"Origin_LocationID: {entity.Flight.Origin_LocationID}");
        //    Console.WriteLine($"Dest_LocationID: {entity.Flight.Dest_LocationID}");

        //    if (entity.Service != null)
        //    {
        //        entity.Service.ServiceType = "Flight";
        //        _context.Services.Add(entity.Service);
        //        _context.SaveChanges();
        //        entity.ServiceId = entity.Service.ServiceID;
        //    }

        //    entity.Flight.Airline = null;
        //    entity.Flight.OriginLocation = null;
        //    entity.Flight.DestLocation = null;

        //    _context.Flights.Add(entity.Flight);
        //    _context.SaveChanges();
        //    entity.Flight_Number = entity.Flight.Flight_Number;

        //    _context.FlightServices.Add(entity);
        //}
        public override void Add(FlightService entity)
        {
            // 1. Save Service first
            if (entity.Service != null)
            {
                // This ensures the Service points to a valid ID selected in the dropdown
                entity.Service.LocationID = entity.Flight.Origin_LocationID;

                entity.Service.ServiceType = "Flight";
                _context.Services.Add(entity.Service);
                _context.SaveChanges();
                entity.ServiceId = entity.Service.ServiceID;
            }

            // 2. Save Flight
            if (entity.Flight != null)
            {
                entity.Flight.Airline = null;
                entity.Flight.OriginLocation = null;
                entity.Flight.DestLocation = null;

                // Ensure the FK string matches
                entity.Flight_Number = entity.Flight.Flight_Number;

                _context.Flights.Add(entity.Flight);
                _context.SaveChanges();
            }

            // 3. Save Link
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
