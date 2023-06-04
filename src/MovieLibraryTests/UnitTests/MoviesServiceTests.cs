using Moq;
using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Entities;
using MovieLibrary.Core.Enums;
using MovieLibrary.Core.Repositories.Interfaces;
using MovieLibrary.Core.Services;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace MovieLibraryTests.UnitTests
{
    public class MoviesServiceTests
    {
        private Mock<IMovieRepository> _mockMovieRepository;
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private MoviesService _movieService;

        public MoviesServiceTests()
        {
            _mockMovieRepository = new Mock<IMovieRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _movieService = new MoviesService(_mockMovieRepository.Object, _mockCategoryRepository.Object);
        }

        [Fact]
        public async Task AddMovie_ShouldReturnOkResponseWithCreatedMovieId()
        {
            // Arrange
            var createMovieRequest = new CreateMovieRequest
            {
                Title = "Test Movie",
                Description = "Test Description",
                Year = 2023,
                ImdbRating = 7.1M,
                MovieCategoriesIds = new[] { 1, 2 }
            };

            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" }
            };

            int createdMovieId = 1;
            _mockMovieRepository.Setup(repo => repo.CreateAsync(It.IsAny<Movie>())).ReturnsAsync(createdMovieId);
            _mockCategoryRepository.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable);

            // Act
            var response = await _movieService.AddMovie(createMovieRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(createdMovieId, response.Payload.Id);
            _mockMovieRepository.Verify(repo => repo.CreateAsync(It.IsAny<Movie>()), Times.Once);
        }

        [Fact]
        public async Task GetMovies_ShouldReturnOkResponseWithMovies()
        {
            // Arrange
            var movies = new List<Movie>
            {
                new Movie
                {
                    Id = 1,
                    Title = "Movie 1",
                    Description = "Description 1",
                    Year = 2021,
                    ImdbRating = 7.5M,
                    MovieCategories = new List<MovieCategory>
                    {
                        new MovieCategory { Id = 1, Category = new Category { Id = 1, Name = "Category 1" } },
                        new MovieCategory { Id = 2, Category = new Category { Id = 2, Name = "Category 2" } }
                    }
                },
                new Movie
                {
                    Id = 2,
                    Title = "Movie 2",
                    Description = "Description 2",
                    Year = 2022,
                    ImdbRating = 8.0M,
                    MovieCategories = new List<MovieCategory>
                    {
                        new MovieCategory { Id = 3, Category = new Category { Id = 1, Name = "Category 1" } },
                        new MovieCategory { Id = 4, Category = new Category { Id = 3, Name = "Category 3" } }
                    }
                }
            };
            _mockMovieRepository.Setup(repo => repo.GetAll()).Returns(movies.AsQueryable);

            // Act
            var response = await _movieService.GetMovies();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(movies.Count, response.Payload.Movies.Count);

            for (int i = 0; i < movies.Count; i++)
            {
                var movie = movies[i];
                var mappedMovie = response.Payload.Movies.ElementAt(i);

                Assert.Equal(movie.Id, mappedMovie.Id);
                Assert.Equal(movie.Title, mappedMovie.Title);
                Assert.Equal(movie.Description, mappedMovie.Description);
                Assert.Equal(movie.Year, mappedMovie.Year);
                Assert.Equal(movie.ImdbRating, mappedMovie.ImdbRating);

                Assert.Equal(movie.MovieCategories.Count, mappedMovie.Categories.Count);

                for (int j = 0; j < movie.MovieCategories.Count; j++)
                {
                    var movieCategory = movie.MovieCategories.ElementAt(j);
                    var mappedCategory = mappedMovie.Categories.ElementAt(j);

                    Assert.Equal(movieCategory.Category.Id, mappedCategory.Id);
                    Assert.Equal(movieCategory.Category.Name, mappedCategory.Name);
                }
            }

            _mockMovieRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetFilteredMovies_ShouldReturnOkResponseWithFilteredAndMappedMovies()
        {
            // Arrange
            var movies = new List<Movie>
        {
            new Movie
            {
                Id = 1,
                Title = "Movie 1",
                Description = "Description 1",
                Year = 2021,
                ImdbRating = 7.5M,
                MovieCategories = new List<MovieCategory>
                {
                    new MovieCategory { Id = 1, Category = new Category { Id = 1, Name = "Category 1" } },
                    new MovieCategory { Id = 2, Category = new Category { Id = 2, Name = "Category 2" } }
                }
            },
            new Movie
            {
                Id = 2,
                Title = "Movie 2",
                Description = "Description 2",
                Year = 2022,
                ImdbRating = 8.0M,
                MovieCategories = new List<MovieCategory>
                {
                    new MovieCategory { Id = 3, Category = new Category { Id = 1, Name = "Category 1" } },
                    new MovieCategory { Id = 4, Category = new Category { Id = 3, Name = "Category 3" } }
                }
            },
            new Movie
            {
                Id = 3,
                Title = "Movie 3",
                Description = "Description 3",
                Year = 2023,
                ImdbRating = 8.5M,
                MovieCategories = new List<MovieCategory>
                {
                    new MovieCategory { Id = 5, Category = new Category { Id = 2, Name = "Category 2" } },
                    new MovieCategory { Id = 6, Category = new Category { Id = 3, Name = "Category 3" } }
                }
            }
        };

            var movieFilters = new GetFilteredMoviesRequest
            {
                Text = "Movie",
                Categories = new List<int> { 1 },
                MinImdbRating = 7.0M,
                MaxImdbRating = 8.0M
            };

            _mockMovieRepository.Setup(repo => repo.GetAll()).Returns(movies.AsQueryable);

            // Act
            var response = await _movieService.GetFilteredMovies(movieFilters);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Contains("Movie 1", response.Payload.Movies.Select(m => m.Title));
            Assert.Contains("Movie 2", response.Payload.Movies.Select(m => m.Title));
        }

        [Fact]
        public async Task DeleteMovie_ExistingMovie_ShouldReturnDeletedMovieId()
        {
            // Arrange
            var existingMovieId = 1;
            var existingMovie = new Movie { Id = existingMovieId };

            _mockMovieRepository
                .Setup(r => r.GetByIdAsync(existingMovieId))
                .ReturnsAsync(existingMovie);

            _mockMovieRepository
                .Setup(r => r.DeleteAsync(existingMovie))
                .ReturnsAsync(existingMovieId);

            // Act
            var result = await _movieService.DeleteMovie(existingMovieId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(existingMovieId, result.Payload.Id);
        }

        [Fact]
        public async Task DeleteMovie_NonExistingMovie_ShouldReturnBadRequest()
        {
            // Arrange
            var nonExistingMovieId = 1;
            Movie nullMovie = null;

            _mockMovieRepository
                .Setup(r => r.GetByIdAsync(nonExistingMovieId))
                .ReturnsAsync(nullMovie);

            // Act
            var result = await _movieService.DeleteMovie(nonExistingMovieId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.MovieDoesNotExist, result.Errors.First());
        }

        [Fact]
        public async Task UpdateMovie_ValidRequest_ShouldReturnUpdatedMovie()
        {
            // Arrange
            var movieId = 1;
            var updateMovieRequest = new UpdateMovieRequest
            {
                Title = "Updated Movie",
                Description = "Updated description",
                Year = 2022,
                ImdbRating = 8.5M,
                MovieCategoriesIds = new int[] { 1, 2 }
            };

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Comedy" }
            };

            var updatedMovieEntity = new Movie
            {
                Id = movieId,
                Title = updateMovieRequest.Title,
                Description = updateMovieRequest.Description,
                Year = updateMovieRequest.Year,
                ImdbRating = updateMovieRequest.ImdbRating,
                MovieCategories = categories.Select(c => new MovieCategory { Category = c }).ToList()
            };

            _mockCategoryRepository
                .Setup(r => r.GetAll())
                .Returns(categories.AsQueryable());

            _mockMovieRepository
                .Setup(r => r.UpdateAsync(It.IsAny<Movie>()))
                .ReturnsAsync(updatedMovieEntity);
            _mockMovieRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(updatedMovieEntity);

            // Act
            var result = await _movieService.UpdateMovie(movieId, updateMovieRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(movieId, result.Payload.Movie.Id);
            Assert.Equal(updateMovieRequest.Title, result.Payload.Movie.Title);
            Assert.Equal(updateMovieRequest.Description, result.Payload.Movie.Description);
            Assert.Equal(updateMovieRequest.Year, result.Payload.Movie.Year);
            Assert.Equal(updateMovieRequest.ImdbRating, result.Payload.Movie.ImdbRating);
            Assert.Equal(categories.Count, result.Payload.Movie.Categories.Count);
            Assert.All(result.Payload.Movie.Categories, mc => Assert.Contains(mc.Id, updateMovieRequest.MovieCategoriesIds));
        }

        [Fact]
        public async Task UpdateMovie_InvalidCategories_ShouldReturnBadRequest()
        {
            // Arrange
            var movieId = 1;
            var updateMovieRequest = new UpdateMovieRequest
            {
                Title = "Updated Movie",
                Description = "Updated description",
                Year = 2022,
                ImdbRating = 8.5M,
                MovieCategoriesIds = new int[] { 1, 2 }
            };

            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Action" },
                new Category { Id = 2, Name = "Comedy" }
            };

            var updatedMovieEntity = new Movie
            {
                Id = movieId,
                Title = updateMovieRequest.Title,
                Description = updateMovieRequest.Description,
                Year = updateMovieRequest.Year,
                ImdbRating = updateMovieRequest.ImdbRating,
                MovieCategories = categories.Select(c => new MovieCategory { Category = c }).ToList()
            };

            _mockCategoryRepository
                .Setup(r => r.GetAll())
                .Returns(categories.AsQueryable());

            _mockMovieRepository
                .Setup(r => r.GetByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(updatedMovieEntity);

            // Act
            var result = await _movieService.UpdateMovie(movieId, updateMovieRequest);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Single(result.Errors);
            Assert.Equal(ErrorMessages.MovieDoesNotExist, result.Errors.First());
        }
    }
}
