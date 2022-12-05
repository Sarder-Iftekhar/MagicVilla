using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class 
    {
        //its Generic repository
        //here edit is not present ,
        //bcz its very possible for every different update there may be diffrent logic 
        //but if its same then you may use here
        Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null);
        Task<T> GetById(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task Create(T entity);
        //Task Edit(Villa entity);
        Task Delete(T entity);
        Task Save();
        //Now IRepository is not limited to one service (villa model)

    }
}
