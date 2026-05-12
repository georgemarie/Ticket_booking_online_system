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


        public bool CreateBooking(int serviceId)
        {
            Booking booking = new Booking()
            {
                ServiceID = serviceId,

                UserID = 2,

                Date = DateTime.Now,

                Status = "Confirmed"
            };

            _bookingRepo.Add(booking);

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
