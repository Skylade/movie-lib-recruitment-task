using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Services.Interfaces
{
    public interface IMovieService
    {
        Task<ServiceResponse<GetMoviesResponse>> GetMovies();
        Task<ServiceResponse<GetFilteredMoviesResponse>> GetFilteredMovies(GetFilteredMoviesRequest movieFilters);
        Task<ServiceResponse<GetMovieResponse>> GetMovie(int id);
        Task<ServiceResponse<CreateMovieResponse>> AddMovie(CreateMovieRequest createMovieRequest);
        Task<ServiceResponse<UpdateMovieResponse>> UpdateMovie(int id, UpdateMovieRequest updateMovieRequest);
        Task<ServiceResponse<DeleteMovieResponse>> DeleteMovie(int id);
    }
}
