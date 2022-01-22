using AutoMapper;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Services.Dto.AutoMapperProfiles
{
    public class MovieDtoProfile:Profile
    {
        public MovieDtoProfile()
        {
            CreateMap<Movie, MovieDto>().ReverseMap();
            CreateMap<Movie, MovieDtoForActer>();
            //можно ли в сервисном слое обращаться к ViewModels
            //CreateMap<Movie, MovieActerViewModel>();
        }
    }
}