using AutoMapper;
using Entities.Models;
using Service.Shared.DataTransferObjects;

namespace Service.Mapping
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
