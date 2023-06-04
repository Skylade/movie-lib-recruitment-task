using System.Collections.Generic;

namespace MovieLibrary.Core.Dtos.Requests
{
    public class GetFilteredMoviesRequest
    {
        public string Text { get; set; }
        public IEnumerable<int> Categories { get; set; }
        public decimal MinImdbRating { get; set; }

        public decimal MaxImdbRating { get; set; }
        public int Page { get; set; }
    }
}
