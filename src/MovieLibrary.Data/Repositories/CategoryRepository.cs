using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Entities;
using MovieLibrary.Core.Repositories.Interfaces;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(MovieLibraryContext context) : base(context)
        {
        }
        public async Task<bool> DoesCategoryExists(string CategoryName)
        {
            return await Context.Categories.AnyAsync(x => x.Name == CategoryName);
        }
    }
}
