using AutoMapper;
using MoviesApp.Models;

namespace MoviesApp.Services.Dto.AutoMapperProfiles
{
    public class ActerDtoProfile: Profile
    {
        public ActerDtoProfile()
        {
            CreateMap<Acter, ActerDto>().ReverseMap();
        }
    }
}