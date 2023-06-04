using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using System.Threading.Tasks;

namespace MovieLibrary.Core.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<ServiceResponse<GetCategoriesResponse>> GetCategories();
        Task<ServiceResponse<GetCategoryResponse>> GetCategory(int id);
        Task<ServiceResponse<CreateCategoryResponse>> AddCategory(string category);
        Task<ServiceResponse<UpdateCategoryResponse>> UpdateCategory(int id, UpdateCategoryRequest category);
        Task<ServiceResponse<DeleteCategoryResponse>> DeleteCategory(int id);
    }
}
