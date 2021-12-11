using AutoMapper;
using MoviesApp.Models;

namespace MoviesApp.Services.Dto.AutoMapperProfiles
{
    public class MovieDtoProfile:Profile
    {
        public MovieDtoProfile()
        {
            CreateMap<Movie, MovieDto>().ReverseMap();
        }
    }
}