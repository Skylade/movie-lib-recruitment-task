namespace MovieLibrary.Core.Dtos.Requests
{
    public class CreateMovieRequest
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public int Year { get; set; }

        public decimal ImdbRating { get; set; }

        public int[] MovieCategoriesIds { get; set; }
    }
}
