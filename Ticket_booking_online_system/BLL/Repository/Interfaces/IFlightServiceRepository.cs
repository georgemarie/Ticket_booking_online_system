using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository.Interfaces
{
    public interface IFlightServiceRepository
         : IGenericRepository<FlightService>
    {
        IEnumerable<FlightService> GetAllWithIncludes();
        public IEnumerable<FlightService> Search(string from, string to, DateTime date);
        FlightService? GetByIdWithIncludes(int id);
    }
}
