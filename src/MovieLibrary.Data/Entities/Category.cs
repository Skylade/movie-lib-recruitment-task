using System.Collections.Generic;

namespace MovieLibrary.Core.Entities
{
    public class Category: EntityBase
    {
        public Category()
        {
            this.MovieCategories = new List<MovieCategory>();
        }

        public string Name { get; set; }

        public virtual ICollection<MovieCategory> MovieCategories { get; set; }
    }
}
