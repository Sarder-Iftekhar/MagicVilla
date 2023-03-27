using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db):base(db)
        {
            _db = db;

        }
        public async Task<VillaNumber> Edit(VillaNumber entity)
        {
            entity.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entity);
            await _db.SaveChangesAsync();
            return entity;
            //await Save();
        }


        //without generic part


        //public async Task Create(Villa entity)
        //{
        //    await _db.Villas.AddAsync(entity); 
        //    await Save();
        //}

        //public async Task<Villa> GetById(Expression<Func<Villa,bool>> filter = null, bool tracked = true)
        //{
        //    IQueryable<Villa> query = _db.Villas;
        //    if(!tracked)
        //    {
        //        query = query.AsNoTracking();
        //    }
        //    if (filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.FirstOrDefaultAsync();
        //}

        //public async Task<List<Villa>> GetAll(Expression<Func<Villa,bool>> filter = null)
        //{
        //    IQueryable<Villa> query = _db.Villas;
        //    if(filter != null)
        //    {
        //        query = query.Where(filter);
        //    }
        //    return await query.ToListAsync();

        //}

  

        //public async Task Delete(Villa entity)
        //{
        //    _db.Villas.Remove(entity);
        //    await Save();

        //}

     


    }
}
