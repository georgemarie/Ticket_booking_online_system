using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class HotelRepository : GenericRepository<HotelService>, IHotelRepository
    {
        public HotelRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IEnumerable<HotelService>
            GetAllWithIncludes()
        {
            return _context.HotelServices

                .Include(h => h.Service)
                .ThenInclude(s => s.Location)
                .ToList();
        }

        public IEnumerable<HotelService> Search(string city)
        {
            return _context.HotelServices
                .Include(h => h.Service)
                .ThenInclude(s => s.Location)
                .Where(h => h.Service.Location.City.ToLower().Contains(city.ToLower()))
                .ToList();             
        }
        //public IEnumerable<HotelService> GetAllHotels()
        //{
        //    return _dbSet
        //        .Include(h => h.Location)
        //        .ToList();
        //}


        //public HotelService GetHotelById(int id)
        //{
        //    return _dbSet
        //        .Include(h => h.Location)
        //        .FirstOrDefault(h => h.ServiceID == id);
        //}


        //public IEnumerable<HotelService> SearchByCity(string city)
        //{
        //    return _dbSet
        //        .Include(h => h.Location)
        //        .Where(h => h.Location.City.Contains(city))
        //        .ToList();
        //}
    }
}
