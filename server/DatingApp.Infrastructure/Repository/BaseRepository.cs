using DatingApp.Data;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Repository
{
    public class BaseRepository<TEntity> where TEntity : class
    {
        protected readonly DatabaseContext dbContext;
        protected readonly DbSet<TEntity> dbSet;

        public BaseRepository(DatabaseContext dbContext)
        {
            this.dbContext = dbContext;
            dbSet = dbContext.Set<TEntity>();
        }

        public async Task<TEntity?> GetByIdAsync(int id)
        {
            return await dbSet.FindAsync(id);
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await dbSet.ToListAsync();
        }

        public void Add(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public void Update(TEntity entity)
        {
            dbSet.Update(entity);
        }

        public void Delete(TEntity entity)
        {
            dbSet.Remove(entity);
        }
    }
}