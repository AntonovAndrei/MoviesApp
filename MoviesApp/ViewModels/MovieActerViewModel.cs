using System;

namespace MoviesApp.ViewModels
{
    public class MovieActerViewModel : IComparable<MovieActerViewModel>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public int CompareTo(MovieActerViewModel acterMovieViewModel)
        {
            return this.Id.CompareTo(acterMovieViewModel.Id);
        }
    }
}