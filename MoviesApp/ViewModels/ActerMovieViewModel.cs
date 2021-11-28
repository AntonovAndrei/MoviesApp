using System;

namespace MoviesApp.ViewModels
{
    public class ActerMovieViewModel : IComparable<ActerMovieViewModel>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public int CompareTo(ActerMovieViewModel acterMovieViewModel)
        {
            return this.Id.CompareTo(acterMovieViewModel.Id);
        }
    }
}