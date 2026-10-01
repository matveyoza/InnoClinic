using Entities.Models;
using Service.Shared;
using AutoMapper;

namespace Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserForRegistrationDto, User>();
            CreateMap<UserForRegistrationDto, UserProfileCreationDto>();
        }
    }
}
