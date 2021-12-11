using AutoMapper;
using MoviesApp.ViewModels;

namespace MoviesApp.Models.AutoMapperProfiles
{
    public class ActerProfile : Profile
    {
        public ActerProfile()
        {
            CreateMap<Acter, InputActerViewModel>().ReverseMap();
            CreateMap<Acter, DeleteActerViewModel>();
            CreateMap<Acter, EditActerViewModel>().ReverseMap();
            CreateMap<Acter, ActerViewModel>().ReverseMap();
        }
    }
}