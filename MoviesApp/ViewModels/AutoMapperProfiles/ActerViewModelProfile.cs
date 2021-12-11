using AutoMapper;
using MoviesApp.Services.Dto;
using MoviesApp.ViewModels;

namespace MoviesApp.Models.AutoMapperProfiles
{
    public class ActerProfile : Profile
    {
        public ActerProfile()
        {
            CreateMap<ActerDto, InputActerViewModel>().ReverseMap();
            CreateMap<ActerDto, DeleteActerViewModel>();
            CreateMap<ActerDto, EditActerViewModel>().ReverseMap();
            CreateMap<ActerDto, ActerViewModel>().ReverseMap();
        }
    }
}