using System.Collections.Generic;
using MoviesApp.Services.Dto;

namespace MoviesApp.Services
{
    public interface IMovieService
    {
        MovieDto GetMovie(int id);
        IEnumerable<MovieDto> GetAllMovies();
        MovieDto UpdateMovie(MovieDto movieDto);
        MovieDto AddMovie(MovieDto movieDto);
        MovieDto DeleteMovie(int id);
        
        //относится ли этот сервис к актерам или фильмам
        IEnumerable<MovieDto> GetAllMoviesByActorId(int id);
        IEnumerable<MovieDtoForActer> GetActersNotFilmedFilmsByActerId(int acterId);
    }
}