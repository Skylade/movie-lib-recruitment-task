using System.Collections.Generic;

namespace MovieLibrary.Core.Dtos.Responses
{
    public class GetFilteredMoviesResponse
    {
        public ICollection<MovieDto> Movies { get; internal set; }
    }
}
