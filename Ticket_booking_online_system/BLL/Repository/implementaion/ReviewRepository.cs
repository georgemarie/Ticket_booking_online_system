using BLL.Repository.Interfaces;
using DAL.Models;
using Ticket_booking_online_system.Data;

namespace BLL.Repository.implementaion
{
    public class ReviewRepository : GenericRepository<Review>, IReviewRepository
    {
        public ReviewRepository(ApplicationDbContext context)
            : base(context)
        {
        }
    }
}
