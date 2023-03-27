using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository:  IRepository<VillaNumber>
    {

        //Task<List<Villa>> GetAll(); 
        //Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter=null);
        //Task<Villa> GetById(Expression<Func<Villa,bool>> filter = null, bool tracked=true);
        //Task Create(Villa entity);
        Task<VillaNumber> Edit(VillaNumber entity);
        //Task Delete(Villa entity);
        //Task Save();



    }
}
