using System;
using System.Collections.Generic;
using System.Text;
using DAL.Models;

namespace BLL.Repository.Interfaces
{
<<<<<<< HEAD
    public interface IServiceRepository : IGenericRepository<Service>
=======
    public interface IServiceRepository:IGenericRepository<T> where T : class
>>>>>>> d0ee83d2b73f6d8de8b441e2828a5bf64d3f32da
    {

    }
}