using System;
using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Services.Dto
{
    public class MovieDtoForActer//: IComparable<MovieDtoForActer> ругается
    {
        public int? Id { get; set; }
        
        [Required]
        [StringLength(32,ErrorMessage = "Title length can't be more than 32.")]
        public string Title { get; set; }
        
        public bool CompareTo(MovieDtoForActer movieDtoForActer)
        {
            return this.Id == movieDtoForActer.Id;
        }
    }
}