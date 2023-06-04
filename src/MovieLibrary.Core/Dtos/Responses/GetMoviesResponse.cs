using System.Collections.Generic;

namespace MovieLibrary.Core.Dtos.Responses
{
    public class GetMoviesResponse
    {
        public ICollection<MovieDto> Movies { get; internal set; }
    }
}
