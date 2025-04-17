using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Domain.Models.DBModel
{
    public class Review
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public int MovieId { get; set; }
    }
}
