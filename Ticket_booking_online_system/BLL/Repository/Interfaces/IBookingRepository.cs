using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository.Interfaces
{
    public interface IBookingRepository : IGenericRepository<Booking>
    {
        IEnumerable<Booking> GetUserBookings(string userId);
        new IEnumerable<Booking> GetAll();

        Booking GetBookingWithDetails(int bookingId);

        void Cancel(int bookingId);

    }
}
