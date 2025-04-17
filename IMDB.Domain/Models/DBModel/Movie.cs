using System.Collections.Generic;

namespace IMDB.Domain.Models.DBModel
{
    public class Movie
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public string CoverImage { get; set; }
        public int ProducerId { get; set; }
    }
}
