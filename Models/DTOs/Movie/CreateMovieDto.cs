using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTOs.Movie
{
    public class CreateMovieDto
    {
        public string NameMovie { get; set; }

        public string Description { get; set; }

        public string RouteImage { get; set; }

        public int Duration { get; set; }

        public enum CreateTypeClassification { Siete, Trece, Dieciocho }

        public CreateTypeClassification Clasification { get; set; }

        public int categoryId { get; set; }
    }
}