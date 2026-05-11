using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository.Interfaces
{
    public interface IHotelRepository : IGenericRepository<HotelService>
    {
        IEnumerable<HotelService> GetAllHotels();

        HotelService GetHotelById(int id);

        IEnumerable<HotelService> SearchByCity(string city);
    }
}
