

using System.Collections.Generic;
using IMDB.Domain.Models.DBModel;

namespace IMDB.Domain.Models.ResponseModel
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int YearOfRelease { get; set; }
        public string Plot { get; set; }
        public string CoverImage { get; set; }
        public int ProducerId { get; set; }
        public List<int> ActorsIds { get; set; }
        public List<int> GenresIds { get; set; }
    }
}
