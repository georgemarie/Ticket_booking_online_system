using BLL.Repository.Interfaces;
using DAL.Models;
using Microsoft.EntityFrameworkCore;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public IEnumerable<Review> GetAllWithIncludes()
        {
            return _context.Reviews
                .Include(r => r.Service)
                .Include(r => r.User)
                .ToList();
        }

        public Review? GetByIdWithIncludes(int id)
        {
            return _context.Reviews
                .Include(r => r.Service)
                .Include(r => r.User)
                .FirstOrDefault(r => r.ReviewID == id);
        }

    }
}
