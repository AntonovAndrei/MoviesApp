using System.Collections;
using System.Collections.Generic;

namespace MoviesApp.ViewModels
{
    public class EditActerViewModel: InputActerViewModel
    {
        public bool IsDeleteAllMovies { get; set; }
        
        //Лучше использовать IEnumerable или ICollection?
        public IEnumerable<ActerMovieViewModel> SelectMovies { get; set; }
    }
}