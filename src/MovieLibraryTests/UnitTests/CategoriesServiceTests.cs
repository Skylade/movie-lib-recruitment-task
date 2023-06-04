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
    public class CategoriesServiceTests
    {
        private Mock<ICategoryRepository> _mockCategoryRepository;
        private CategoriesService _categoriesService;

        public CategoriesServiceTests()
        {
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _categoriesService = new CategoriesService(_mockCategoryRepository.Object);
        }

        [Fact]
        public async Task AddCategory_WhenCategoryDoesNotExist_ShouldReturnOkResponse()
        {
            // Arrange
            string categoryName = "TestCategory";
            _mockCategoryRepository.Setup(repo => repo.DoesCategoryExists(categoryName)).ReturnsAsync(false);
            _mockCategoryRepository.Setup(repo => repo.CreateAsync(It.IsAny<Category>())).ReturnsAsync(1);

            // Act
            var response = await _categoriesService.AddCategory(categoryName);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(1, response.Payload.Id);
            _mockCategoryRepository.Verify(repo => repo.DoesCategoryExists(categoryName), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.CreateAsync(It.IsAny<Category>()), Times.Once);
        }

        [Fact]
        public async Task AddCategory_WhenCategoryExists_ShouldReturnBadRequestResponse()
        {
            // Arrange
            string categoryName = "TestCategory";
            _mockCategoryRepository.Setup(repo => repo.DoesCategoryExists(categoryName)).ReturnsAsync(true);

            // Act
            var response = await _categoriesService.AddCategory(categoryName);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ErrorMessages.CategoryAlreadyExists, response.Errors.First());
            _mockCategoryRepository.Verify(repo => repo.DoesCategoryExists(categoryName), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.CreateAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryExists_ShouldReturnOkResponse()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockCategoryRepository.Setup(repo => repo.DeleteAsync(category)).ReturnsAsync(categoryId);

            // Act
            var response = await _categoriesService.DeleteCategory(categoryId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(categoryId, response.Payload.Id);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(category), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_WhenCategoryDoesNotExist_ShouldReturnBadRequestResponse()
        {
            // Arrange
            int categoryId = 1;
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var response = await _categoriesService.DeleteCategory(categoryId);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ErrorMessages.CategoryDoesNotExist, response.Errors.First());
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.DeleteAsync(It.IsAny<Category>()), Times.Never);
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOkResponseWithCategories()
        {
            // Arrange
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Category 1" },
                new Category { Id = 2, Name = "Category 2" },
                new Category { Id = 3, Name = "Category 3" }
            };
            _mockCategoryRepository.Setup(repo => repo.GetAll()).Returns(categories.AsQueryable);

            // Act
            var response = await _categoriesService.GetCategories();

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(categories.Count, response.Payload.Categories.Count);

            for (int i = 0; i < categories.Count; i++)
            {
                var category = categories[i];
                var mappedCategory = response.Payload.Categories.ElementAt(i);

                Assert.Equal(category.Id, mappedCategory.Id);
                Assert.Equal(category.Name, mappedCategory.Name);
            }

            _mockCategoryRepository.Verify(repo => repo.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetCategory_WhenCategoryExists_ShouldReturnOkResponseWithCategory()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId, Name = "Test Category" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);

            // Act
            var response = await _categoriesService.GetCategory(categoryId);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(response.Payload.Category);
            Assert.Equal(category.Id, response.Payload.Category.Id);
            Assert.Equal(category.Name, response.Payload.Category.Name);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);

        }

        [Fact]
        public async Task GetCategory_WhenCategoryDoesNotExist_ShouldReturnNotFoundResponse()
        {
            // Arrange
            int categoryId = 1;
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var response = await _categoriesService.GetCategory(categoryId);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
            Assert.Null(response.Payload);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryExists_ShouldReturnOkResponseWithUpdatedCategory()
        {
            // Arrange
            int categoryId = 1;
            var category = new Category { Id = categoryId, Name = "TestCategory" };
            var updatedCategory = new Category { Id = categoryId, Name = "UpdatedCategory" };
            var updatedCategoryDto = new UpdateCategoryRequest { Name = "UpdatedCategory" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync(category);
            _mockCategoryRepository.Setup(repo => repo.UpdateAsync(category)).ReturnsAsync(updatedCategory);

            // Act
            var response = await _categoriesService.UpdateCategory(categoryId, updatedCategoryDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(updatedCategory.Id, response.Payload.Category.Id);
            Assert.Equal(updatedCategory.Name, response.Payload.Category.Name);
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(category), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_WhenCategoryDoesNotExist_ShouldReturnBadRequestResponse()
        {
            // Arrange
            int categoryId = 1;
            var updatedCategory = new Category { Id = categoryId, Name = "UpdatedCategory" };
            var updatedCategoryDto = new UpdateCategoryRequest { Name = "UpdatedCategory" };
            _mockCategoryRepository.Setup(repo => repo.GetByIdAsync(categoryId)).ReturnsAsync((Category)null);

            // Act
            var response = await _categoriesService.UpdateCategory(categoryId, updatedCategoryDto);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            Assert.Equal(ErrorMessages.CategoryDoesNotExist, response.Errors.First());
            _mockCategoryRepository.Verify(repo => repo.GetByIdAsync(categoryId), Times.Once);
            _mockCategoryRepository.Verify(repo => repo.UpdateAsync(It.IsAny<Category>()), Times.Never);
        }
    }
}
