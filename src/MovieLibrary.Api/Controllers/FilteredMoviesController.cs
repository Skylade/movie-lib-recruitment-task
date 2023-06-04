using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using MovieLibrary.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieLibrary.Api.Controllers
{
    [Route("api/v1/Movie/Filter")]
    [ApiController]
    public class FilteredMoviesController : BaseController
    {
        private readonly IMovieService _movieService;

        public FilteredMoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMovieResponse>>> GetFiltered([FromQuery] GetFilteredMoviesRequest filteredMoviesRequest)
        {
            var FilteredMoviesResponse = await _movieService.GetFilteredMovies(filteredMoviesRequest);

            return SendResponse(FilteredMoviesResponse);
        }
    }
}
