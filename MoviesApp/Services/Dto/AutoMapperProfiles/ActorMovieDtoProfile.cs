using AutoMapper;
using MoviesApp.Models;

namespace MoviesApp.Services.Dto.AutoMapperProfiles
{
    public class ActorMovieDtoProfile:Profile
    {
        public ActorMovieDtoProfile()
        {
            CreateMap<ActerMovie, ActerMovieDto>().ReverseMap();
        }
    }
}