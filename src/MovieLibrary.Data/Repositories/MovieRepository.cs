using Microsoft.EntityFrameworkCore;
using MovieLibrary.Core.Entities;
using MovieLibrary.Core.Repositories.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Repositories
{
    public class MovieRepository : BaseRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieLibraryContext context) : base(context)
        {
        }
        public override IQueryable<Movie> GetAll()
        {
            return Context.Movies.Include(m => m.MovieCategories).ThenInclude(m => m.Category).OrderBy(entity => entity.Id);
        }

        public async override Task<Movie> GetByIdAsync(int id)
        {
            return await Context.Set<Movie>().Include(m => m.MovieCategories).ThenInclude(m => m.Category).SingleOrDefaultAsync(e => e.Id == id);
        }
    }
}
