using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_VillaAPI.Repository
{
    public class Repository<T>: IRepository<T> where T : class
    {

        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public  Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>(); 

        }
        public async Task Create(T entity)
        {
            //************Normal add
            //await _db.Villas.AddAsync(entity);
            //await Save();

            //*********Generic Add

            await dbSet.AddAsync(entity);
            await Save();

        }

        public async Task<T> GetById(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();

        }

        //public async Task Edit(Villa entity)
        //{
        //    _db.Villas.Update(entity);
        //    await Save();
        //}

        public async Task Delete(T entity)
        {
            dbSet.Remove(entity);
            await Save();

        }

        public async Task Save()
        {
            await _db.SaveChangesAsync();

        }


    }
}
