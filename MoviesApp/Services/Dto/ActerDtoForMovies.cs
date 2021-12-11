using System.ComponentModel.DataAnnotations;
using MoviesApp.Filters;

namespace MoviesApp.Services.Dto
{
    public class ActerDtoForMovies
    {
        public int? Id { get; set; }
        
        [Required]
        [StringLength(32,ErrorMessage = "Name length can't be more than 32.")]
        [ActorName]
        public string Name { get; set; }


        public bool CompareTo(ActerDtoForMovies acterDtoForMovies)
        {
            return this.Id == acterDtoForMovies.Id;
        }
    }
}