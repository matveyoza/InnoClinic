using Entities.Models;
using Service.Shared;
using AutoMapper;

namespace Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName ?? src.Email))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email));

            CreateMap<UserForRegistrationDto, UserProfileCreationDto>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
