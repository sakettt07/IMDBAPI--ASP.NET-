using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMDB.Domain.CustomValidation;

namespace IMDB.Domain.Models.RequestModel
{
    public class ActorRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Bio { get; set; }
        [Required]
        [DOBValidation]
        public DateTime DOB { get; set; }
        [Required]
        [RegularExpression("^(Male|Female|Transgender|male|female|transgender)$", ErrorMessage = "Gender must be Male, Female, or Transgender.")]
        public string Gender { get; set; }
    }
}
