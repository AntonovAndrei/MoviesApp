using System;
using System.ComponentModel.DataAnnotations;
using MoviesApp.Filters;

namespace MoviesApp.ViewModels
{
    public class InputActerViewModel
    {
        [Required]
        [StringLength(32,ErrorMessage = "Name length can't be more than 32.")]
        [ActorName]
        public string Name { get; set; }
        [Required]
        [StringLength(32,ErrorMessage = "Last name length can't be more than 32.")]
        [ActorName]
        public string LastName { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        public DateTime BirthdayDate { get; set; }
    }
}