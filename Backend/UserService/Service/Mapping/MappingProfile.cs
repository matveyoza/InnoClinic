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
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Specialty, opt =>
                    opt.MapFrom(src => src.DoctorProfile.Specialty))
                .ForMember(dest => dest.RoomNumber, opt =>
                    opt.MapFrom(src => src.DoctorProfile.RoomNumber));
        }
    }
}
