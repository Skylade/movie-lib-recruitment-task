using Microsoft.AspNetCore.Mvc;
using MovieLibrary.Core.Dtos.Requests;
using MovieLibrary.Core.Dtos.Responses;
using MovieLibrary.Core.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieLibrary.Api.Controllers
{
    [Route("api/v1/CategoryManagement")]
    [ApiController]
    public class CategoriesController : BaseController
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<GetCategoryResponse>>> Get()
        {
            var CategoriesResponse = await _categoryService.GetCategories();

            return SendResponse(CategoriesResponse);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryResponse>> Get(int id)
        {
            var CategoryResponse = await _categoryService.GetCategory(id);

            return SendResponse(CategoryResponse);
        }

        [HttpPost]
        public async Task<ActionResult<CreateCategoryResponse>> Post([FromBody] CreateCategoryRequest categoryCreateRequest)
        {
            var CreateCategoryResponse = await _categoryService.AddCategory(categoryCreateRequest.CategoryName);

            return SendResponse(CreateCategoryResponse);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<UpdateCategoryResponse>> Put(int id, [FromBody] UpdateCategoryRequest categoryUpdateRequest)
        {
            var UpdateCategoryResponse = await _categoryService.UpdateCategory(id, categoryUpdateRequest);

            return SendResponse(UpdateCategoryResponse);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<DeleteCategoryResponse>> Delete(int id)
        {
            var DeleteCategoryResponse = await _categoryService.DeleteCategory(id);

            return SendResponse(DeleteCategoryResponse);
        }
    }
}
