using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoviesApp.Models
{
    public class Movie
    {
        public Movie()
        {
            this.MoviesList = new HashSet<ActerMovie>();
        }
        public int Id { get; set; }
        public string Title { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Price { get; set; }

        public virtual ICollection<ActerMovie> MoviesList { get; set; }
    }
}