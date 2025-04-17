using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Domain.Models.ResponseModel
{
    public class ActorResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public string Gender { get; set; }
        public DateTime DOB { get; set; }
    }
}
