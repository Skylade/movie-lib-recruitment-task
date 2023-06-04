using System.Collections.Generic;

namespace MovieLibrary.Core.Dtos
{
    public class MovieDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Year { get; set; }
        public decimal ImdbRating { get; set; }
        public ICollection<CategoryDto> Categories { get; set; }
    }
}
