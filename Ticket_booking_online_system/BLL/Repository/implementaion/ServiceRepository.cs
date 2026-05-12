using BLL.Repository.Interfaces;
using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class ServiceRepository
        : GenericRepository<Service>,
          IServiceRepository
    {
        private readonly ApplicationDbContext _context;

        public ServiceRepository(
            ApplicationDbContext context)
            : base(context)
        {
            _context = context;
        }


    }
}
