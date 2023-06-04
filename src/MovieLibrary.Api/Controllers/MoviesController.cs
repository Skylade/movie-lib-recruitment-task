using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using MovieLibrary.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieLibrary.Api.Controllers
{
    [Route("api/v1/MovieManagement")]
    [ApiController]
    public class MoviesController : BaseController
    {
        private readonly IMovieService _movieService;

        public MoviesController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetMovieResponse>>> Get()
        {
            var MoviesResponse = await _movieService.GetMovies();

            return SendResponse(MoviesResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetMovieResponse>> Get(int id)
        {
            var MovieResponse = await _movieService.GetMovie(id);

            return SendResponse(MovieResponse);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreateMovieRequest createMovieRequest)
        {
            var CreateMovieResponse = await _movieService.AddMovie(createMovieRequest);

            return SendResponse(CreateMovieResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] UpdateMovieRequest updateMovieRequest)
        {
            var UpdateMovieResponse = await _movieService.UpdateMovie(id, updateMovieRequest);

            return SendResponse(UpdateMovieResponse);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var DeleteMovieResponse = await _movieService.DeleteMovie(id);

            return SendResponse(DeleteMovieResponse);
        }
    }
}
