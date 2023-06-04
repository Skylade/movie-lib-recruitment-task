using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Entities;
using MovieLibrary.Core.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Repositories
{
    public abstract class BaseRepository<TEntity> : IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        protected MovieLibraryContext Context { get; set; }

        public BaseRepository(MovieLibraryContext context)
        {
            Context = context;
        }

        public async virtual Task<TEntity> GetByIdAsync(int id)
        {
            return await Context.Set<TEntity>().SingleOrDefaultAsync(e => e.Id == id);
        }
        public virtual IQueryable<TEntity> GetAll()
        {
            return Context.Set<TEntity>().OrderBy(entity => entity.Id);
        }
        public async virtual Task<int> CreateAsync(TEntity entity)
        {
            var created = await Context.Set<TEntity>().AddAsync(entity);
            await Context.SaveChangesAsync();
            return created.Entity.Id;
        }

        public async virtual Task<TEntity> UpdateAsync(TEntity entity)
        {
            var result = Context.Set<TEntity>().Update(entity);
            await Context.SaveChangesAsync();
            return result.Entity;
        }

        public async virtual Task<int> DeleteAsync(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            await Context.SaveChangesAsync();
            return entity.Id;
        }
    }
}
