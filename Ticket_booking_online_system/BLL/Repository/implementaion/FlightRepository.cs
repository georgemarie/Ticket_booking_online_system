using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class FlightRepository : GenericRepository<Flight>, IFlightRepository
    {
        public FlightRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<Flight> Search(string from, string to, DateTime date)
        {
            return _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.OriginLocation)
                .Include(f => f.DestLocation)
                .Where(f =>
                    EF.Functions.Like(f.OriginLocation.City, $"%{from}%") &&
                    EF.Functions.Like(f.DestLocation.City, $"%{to}%") &&
                    
                    f.Depart_Date >= date.Date &&
    f.Depart_Date < date.Date.AddDays(1))
                .ToList();
        }
        public IEnumerable<Flight> GetAllWithIncludes()
        {
            return _context.Flights
                .Include(f => f.Airline)
                .Include(f => f.OriginLocation)
                .Include(f => f.DestLocation)
                .ToList();
        }
    }
    }
