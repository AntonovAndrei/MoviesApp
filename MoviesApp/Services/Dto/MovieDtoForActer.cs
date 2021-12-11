using System.ComponentModel.DataAnnotations;

namespace MoviesApp.Services.Dto
{
    public class MovieDtoForActer
    {
        public int? Id { get; set; }
        
        [Required]
        [StringLength(32,ErrorMessage = "Title length can't be more than 32.")]
        public string Title { get; set; }
    }
}