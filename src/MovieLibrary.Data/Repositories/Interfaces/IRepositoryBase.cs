using MovieLibrary.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Repositories.Interfaces
{
    public interface IRepositoryBase<TEntity> where TEntity : EntityBase
    {
        IQueryable<TEntity> GetAll();
        Task<TEntity> GetByIdAsync(int id);
        Task<int> CreateAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<int> DeleteAsync(TEntity entity);
    }
}
