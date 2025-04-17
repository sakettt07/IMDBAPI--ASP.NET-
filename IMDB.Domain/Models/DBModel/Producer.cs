using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Domain.Models.DBModel
{
    public class Producer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public DateTime DOB { get; set; }
        public string Gender { get; set; }
    }
}
