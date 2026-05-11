using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository.Interfaces
{
    public interface IFlightRepository : IGenericRepository<Flight>
    {
        IEnumerable<Flight> Search(string from, string to, DateTime date);
        public IEnumerable<Flight> GetAllWithIncludes();
    }
}
