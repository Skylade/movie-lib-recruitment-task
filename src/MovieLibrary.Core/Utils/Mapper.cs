using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Entities;
using System.Collections.Generic;

namespace MovieLibrary.Core.Utils
{
    public class Mapper
    {
        public static MovieDto MapMovie(Movie movie)
        {
            var mappedCategories = new List<CategoryDto>();

            foreach (var category in movie.MovieCategories)
            {
                var categoryDto = new CategoryDto
                {
                    Id = category.Category.Id,
                    Name = category.Category.Name,
                };

                mappedCategories.Add(categoryDto);
            }

            var movieDto = new MovieDto
            {
                Id = movie.Id,
                Title = movie.Title,
                Description = movie.Description,
                Year = movie.Year,
                ImdbRating = movie.ImdbRating,
                Categories = mappedCategories
            };

            return movieDto;
        }

        public static ICollection<MovieDto> MapMovies(List<Movie> movies)
        {
            var mappedMovies = new List<MovieDto>();

            foreach (var movie in movies)
            {
                var mappedCategories = new List<CategoryDto>();

                foreach (var category in movie.MovieCategories)
                {
                    var categoryDto = new CategoryDto
                    {
                        Id = category.Category.Id,
                        Name = category.Category.Name,
                    };

                    mappedCategories.Add(categoryDto);
                }

                var movieDto = new MovieDto
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Description = movie.Description,
                    Year = movie.Year,
                    ImdbRating = movie.ImdbRating,
                    Categories = mappedCategories
                };

                mappedMovies.Add(movieDto);
            }

            return mappedMovies;
        }
    }
}
