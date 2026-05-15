using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.interfaces
{
    public interface IBookingService
    {
        // bool CreateBooking(string flightNumber, int userId);
      bool  CreateBooking(int serviceId, string userId);
        IEnumerable<Booking> GetUserBookings(string userId);

        bool CancelBooking(int bookingId);
        int CreateBookingwithPayement(int serviceId, string userId);
    }
}
