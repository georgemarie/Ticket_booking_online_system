using DAL.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BLL.Repository.Interfaces
{
    public interface IReviewRepository: IGenericRepository<Review>
    {
        IEnumerable<Review> GetAllWithIncludes();
        Review? GetByIdWithIncludes(int id);

    }
}
