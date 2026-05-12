using BLL.Repository.Interfaces;
using BLL.Services.interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.Implmentation
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepo;
        private readonly IFlightRepository _flightRepo;

        public BookingService(
            IBookingRepository bookingRepo,
            IFlightRepository flightRepo)
        {
            _bookingRepo = bookingRepo;
            _flightRepo = flightRepo;
        }


        public bool CreateBooking(string flightNumber)
        {
            var flight = _flightRepo
                .GetAllWithIncludes()
                .FirstOrDefault(f => f.Flight_Number == flightNumber);

            if (flight == null)
                return false;

            if (flight.Available_Seats <= 0)
                return false;

            Booking booking = new Booking()
            {
               //Flight_Number = flight.Flight_Number,
                UserID = 2,
                Date = DateTime.Now,
                Status = "Confirmed"
            };

            _bookingRepo.Add(booking);

            flight.Available_Seats--;

            _flightRepo.Update(flight);

            _bookingRepo.Save();

            return true;
        }

        public IEnumerable<Booking> GetUserBookings(int userId)
        {
            return _bookingRepo.GetUserBookings(userId);
        }

       
        public bool CancelBooking(int bookingId)
        {
            var booking = _bookingRepo.GetById(bookingId);

            if (booking == null)
                return false;

            _bookingRepo.Cancel(bookingId);

            _bookingRepo.Save();

            return true;
        }
    }
    }
