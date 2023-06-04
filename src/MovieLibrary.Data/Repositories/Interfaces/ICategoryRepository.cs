using MovieLibrary.Core.Entities;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepositoryBase<Category>
    {
        Task<bool> DoesCategoryExists(string CategoryName);
    }
}
