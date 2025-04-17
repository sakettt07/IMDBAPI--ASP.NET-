
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using IMDB.Domain.CustomValidation;

namespace IMDB.Domain.Models.RequestModel
{
    public class MovieRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [YearOfRelease]
        public int YearOfRelease { get; set; }
        [Required]
        public string Plot { get; set; }
        [Required]
        public string CoverImage { get; set; }
        [Required]
        public int ProducerId { get; set; }
        [Required]
        public List<int> Genres { get; set; }
        [Required]
        public List<int> Actors { get; set; }
    }
}
