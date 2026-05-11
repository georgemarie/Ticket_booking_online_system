using BLL.Repository.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class AirlineRepository: GenericRepository<Airline>, IAirlineRepository
    {
        public AirlineRepository(ApplicationDbContext context)
          : base(context)
        {
        }
    }
}
