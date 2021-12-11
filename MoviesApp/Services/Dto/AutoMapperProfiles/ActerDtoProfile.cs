using AutoMapper;
using MoviesApp.Models;
using MoviesApp.ViewModels;

namespace MoviesApp.Services.Dto.AutoMapperProfiles
{
    public class ActerDtoProfile: Profile
    {
        public ActerDtoProfile()
        {
            CreateMap<Acter, ActerDto>().ReverseMap();
            CreateMap<Acter, ActerDtoForMovies>();
            //CreateMap<Acter, ActerMovieViewModel>();
        }
    }
}