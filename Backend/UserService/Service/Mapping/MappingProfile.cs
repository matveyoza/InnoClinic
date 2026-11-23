using AutoMapper;
using Users.Entities.Models;
using Users.Service.Shared.DataTransferObjects;

namespace Users.Service.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserProfileDto, User>();
            CreateMap<User, UserDto>();
        }
    }
}
