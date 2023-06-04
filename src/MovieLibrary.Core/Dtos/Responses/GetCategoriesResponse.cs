using System.Collections.Generic;

namespace MovieLibrary.Core.Dtos.Responses
{
    public class GetCategoriesResponse
    {
        public ICollection<CategoryDto> Categories { get; internal set; }
    }
}
