using System.Collections.Generic;
using MoviesApp.Services.Dto;

namespace MoviesApp.Services
{
    public interface IActorService
    {
        ActerDto GetActor(int id);
        IEnumerable<ActerDto> GetAllActors();
        ActerDto UpdateActor(ActerDto actorDto);
        ActerDto AddActor(ActerDto actorDto);
        ActerDto DeleteActor(int id);
        
        IEnumerable<ActerDto> GetAllActorByMovieId(int movieId);
        IEnumerable<ActerDtoForMovies> GetNotFilmedActersByMovieId(int movieId);
    }
}