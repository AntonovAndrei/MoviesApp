using System.Collections;
using System.Collections.Generic;

namespace MoviesApp.Services.Dto
{
    public interface IActorMovieService
    {
        ActerMovieDto AddActerMoviesLink(ActerMovieDto acterMovieDto);
        ActerMovieDto DeleteActerMoviesLink(ActerMovieDto acterMovieDto);
        IEnumerable<ActerMovieDto> DeleteAllActorsFilmedInMovieByMovieId(int movieId);
        IEnumerable<ActerMovieDto> DeleteAllMoviesWhereFilmedActorByActorId(int actorId);
        IEnumerable<ActerMovieDto> AddActerMovieLinks(IEnumerable<ActerMovieDto> acterMovieDto);

    }
}