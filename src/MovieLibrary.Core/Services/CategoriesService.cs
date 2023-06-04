using MovieLibrary.Core.Dtos;
using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using MovieLibrary.Core.Entities;
using MovieLibrary.Core.Enums;
using MovieLibrary.Core.Repositories.Interfaces;
using MovieLibrary.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Services
{
    public class CategoriesService : ICategoryService
    {
        readonly ICategoryRepository _categoriesRepository;
        public CategoriesService(ICategoryRepository categoriesInterface)
        {
            _categoriesRepository = categoriesInterface;
        }

        public async Task<ServiceResponse<CreateCategoryResponse>> AddCategory(string categoryName)
        {
            var categoryExists = await _categoriesRepository.DoesCategoryExists(categoryName);

            if (categoryExists)
            {
                return new ServiceResponse<CreateCategoryResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.CategoryAlreadyExists });
            }

            var categoryEntity = new Category
            {
                Name = categoryName
            };

            var resultCategoryId = await _categoriesRepository.CreateAsync(categoryEntity);

            return new ServiceResponse<CreateCategoryResponse>(HttpStatusCode.OK, new CreateCategoryResponse { Id = resultCategoryId });
        }

        public async Task<ServiceResponse<DeleteCategoryResponse>> DeleteCategory(int id)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);

            if (category == null)
            {
                return new ServiceResponse<DeleteCategoryResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.CategoryDoesNotExist });
            }

            var deletedCategoryId = await _categoriesRepository.DeleteAsync(category);

            return new ServiceResponse<DeleteCategoryResponse>(HttpStatusCode.OK, new DeleteCategoryResponse { Id = deletedCategoryId });
        }

        public async Task<ServiceResponse<GetCategoriesResponse>> GetCategories()
        {
            var categories = _categoriesRepository.GetAll().ToList();

            var mappedCategories = new List<CategoryDto>();
            foreach (var category in categories)
            {
                var categoryDto = new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                };

                mappedCategories.Add(categoryDto);
            }

            return new ServiceResponse<GetCategoriesResponse>(HttpStatusCode.OK, new GetCategoriesResponse { Categories = mappedCategories });
        }

        public async Task<ServiceResponse<GetCategoryResponse>> GetCategory(int id)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);

            if (category == null)
            {
                return new ServiceResponse<GetCategoryResponse>(HttpStatusCode.NotFound, new List<string>() { ErrorMessages.CategoryDoesNotExist });
            }

            var mappedCategory = new CategoryDto { Id = category.Id, Name = category.Name };

            return new ServiceResponse<GetCategoryResponse>(HttpStatusCode.OK, new GetCategoryResponse { Category = mappedCategory });
        }

        public async Task<ServiceResponse<UpdateCategoryResponse>> UpdateCategory(int id, UpdateCategoryRequest updateCategory)
        {
            var category = await _categoriesRepository.GetByIdAsync(id);

            if (category == null)
            {
                return new ServiceResponse<UpdateCategoryResponse>(HttpStatusCode.BadRequest, new List<string>() { ErrorMessages.CategoryDoesNotExist });
            }

            category.Name = updateCategory.Name;
            var categoryUpdateResponse = await _categoriesRepository.UpdateAsync(category);


            return new ServiceResponse<UpdateCategoryResponse>(HttpStatusCode.OK, new UpdateCategoryResponse { Category = new CategoryDto() { Id = categoryUpdateResponse.Id, Name = categoryUpdateResponse.Name } });
        }

    }
}
