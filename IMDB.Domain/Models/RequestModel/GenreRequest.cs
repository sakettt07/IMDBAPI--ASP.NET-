﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMDB.Domain.Models.RequestModel
{
    public class GenreRequest
    {
        [Required]
        public string Name { get; set; }
    }
}
