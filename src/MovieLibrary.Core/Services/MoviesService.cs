using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using MovieLibrary.Core.Entities;
using MovieLibrary.Core.Enums;
using MovieLibrary.Core.Repositories.Interfaces;
using MovieLibrary.Core.Services.Interfaces;
using MovieLibrary.Core.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Services
{
    public class MoviesService : IMovieService
    {
        readonly IMovieRepository _movieRepository;
        readonly ICategoryRepository _categoryRepository;
        readonly Mapper _mapper = new Mapper();

        public MoviesService(IMovieRepository movieRepository, ICategoryRepository categoryRepository)
        {
            _movieRepository = movieRepository;
            _categoryRepository = categoryRepository;
        }
        public async Task<ServiceResponse<CreateMovieResponse>> AddMovie(CreateMovieRequest createMovieRequest)
        {
            var categories = _categoryRepository.GetAll().Where(c => createMovieRequest.MovieCategoriesIds.Contains(c.Id));

            if (categories.Count() != createMovieRequest.MovieCategoriesIds.Count())
            {
                return new ServiceResponse<CreateMovieResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.UncorrectCategory });
            }

            var movieCategories = categories
                .Select(c => new MovieCategory { CategoryId = c.Id })
                .ToList();

            var movieEntity = new Movie
            {
                Title = createMovieRequest.Title,
                Description = createMovieRequest.Description,
                Year = createMovieRequest.Year,
                ImdbRating = createMovieRequest.ImdbRating,
                MovieCategories = movieCategories
            };

            var resultMovieId = await _movieRepository.CreateAsync(movieEntity);

            return new ServiceResponse<CreateMovieResponse>(HttpStatusCode.OK, new CreateMovieResponse { Id = resultMovieId });
        }

        public async Task<ServiceResponse<DeleteMovieResponse>> DeleteMovie(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return new ServiceResponse<DeleteMovieResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.MovieDoesNotExist });
            }

            var deletedCategoryId = await _movieRepository.DeleteAsync(movie);

            return new ServiceResponse<DeleteMovieResponse>(HttpStatusCode.OK, new DeleteMovieResponse { Id = deletedCategoryId });
        }

        public async Task<ServiceResponse<GetFilteredMoviesResponse>> GetFilteredMovies(GetFilteredMoviesRequest movieFilters)
        {
            var query = _movieRepository.GetAll().OrderByDescending(m => (double)m.ImdbRating);

            if (!string.IsNullOrEmpty(movieFilters.Text))
            {
                query = (IOrderedQueryable<Movie>)query.Where(m => m.Title.Contains(movieFilters.Text));
            }

            if (movieFilters.Categories != null && movieFilters.Categories.Any())
            {
                query = (IOrderedQueryable<Movie>)query.Where(m => m.MovieCategories.Any(c => movieFilters.Categories.Contains(c.Category.Id)));
            }

            if (movieFilters.MinImdbRating > 0)
            {
                query = (IOrderedQueryable<Movie>)query.Where(m => m.ImdbRating >= movieFilters.MinImdbRating);
            }

            if (movieFilters.MaxImdbRating > 0)
            {
                query = (IOrderedQueryable<Movie>)query.Where(m => m.ImdbRating <= movieFilters.MaxImdbRating);
            }

            var movies = query
                .Skip((movieFilters.Page - 1) * (int)Constants.PageSize)
                .Take((int)Constants.PageSize)
                .ToList();


            var mappedMovies = Mapper.MapMovies(movies);



            return new ServiceResponse<GetFilteredMoviesResponse>(HttpStatusCode.OK, new GetFilteredMoviesResponse { Movies = mappedMovies });
        }

        public async Task<ServiceResponse<GetMovieResponse>> GetMovie(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);

            if (movie == null)
            {
                return new ServiceResponse<GetMovieResponse>(HttpStatusCode.NotFound, new List<string>() { ErrorMessages.MovieDoesNotExist });
            }

            return new ServiceResponse<GetMovieResponse>(HttpStatusCode.OK, new GetMovieResponse { Movie = Mapper.MapMovie(movie) });
        }

        public async Task<ServiceResponse<GetMoviesResponse>> GetMovies()
        {
            var movies = _movieRepository.GetAll().ToList();


            var mappedMovies = Mapper.MapMovies(movies);

            return new ServiceResponse<GetMoviesResponse>(HttpStatusCode.OK, new GetMoviesResponse { Movies = mappedMovies });
        }

        public async Task<ServiceResponse<UpdateMovieResponse>> UpdateMovie(int id, UpdateMovieRequest updateMovieRequest)
        {
            var categories = _categoryRepository.GetAll().Where(c => updateMovieRequest.MovieCategoriesIds.Contains(c.Id));

            if (categories.Count() != updateMovieRequest.MovieCategoriesIds.Count())
            {
                return new ServiceResponse<UpdateMovieResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.UncorrectCategory });
            }

            var movieCategories = categories
                .Select(c => new MovieCategory { CategoryId = c.Id, Category = c })
                .ToList();

            var movieToUpdate = await _movieRepository.GetByIdAsync(id);

            movieToUpdate.Title = updateMovieRequest.Title;
            movieToUpdate.Description = updateMovieRequest.Description;
            movieToUpdate.Year = updateMovieRequest.Year;
            movieToUpdate.ImdbRating = updateMovieRequest.ImdbRating;
            movieToUpdate.MovieCategories = movieCategories;

            var movieUpdateResponse = await _movieRepository.UpdateAsync(movieToUpdate);

            if (movieUpdateResponse == null)
            {
                return new ServiceResponse<UpdateMovieResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.MovieDoesNotExist });
            }

            var mappedMovie = Mapper.MapMovie(movieUpdateResponse);

            return new ServiceResponse<UpdateMovieResponse>(HttpStatusCode.OK, new UpdateMovieResponse { Movie = mappedMovie });
        }
    }
}
