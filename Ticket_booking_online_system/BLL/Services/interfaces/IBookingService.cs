using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Services.interfaces
{
    public interface IBookingService
    {
        // bool CreateBooking(string flightNumber, int userId);
      bool  CreateBooking(int serviceId, int userId);
        IEnumerable<Booking> GetUserBookings(int userId);

        bool CancelBooking(int bookingId);
    }
}
