using Service.Shared.DataTransferObjects;
using AutoMapper;
using Entities.Models;

namespace Service.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<string, DateOnly>().ConvertUsing(s => DateOnly.Parse(s));
        CreateMap<DateOnly, string>().ConvertUsing(d => d.ToString("yyyy-MM-dd"));

        CreateMap<string, TimeOnly>().ConvertUsing(s => TimeOnly.Parse(s));
        CreateMap<TimeOnly, string>().ConvertUsing(t => t.ToString("HH:mm:ss"));

        CreateMap<AppointmentDocumentItem, AppointmentDocumentItemDto>();

        CreateMap<Appointment, AppointmentDto>()
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));

        CreateMap<AppointmentDocumentItemForCreationDto, AppointmentDocumentItem>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
            .ForMember(dest => dest.Appointment, opt => opt.Ignore());

        CreateMap<CreateAppointmentDto, Appointment>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AppointmentStatus.Upcoming))
            .ForMember(dest => dest.CreatedAtUtc, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents))
            .AfterMap((_, dest) =>
            {
                if (dest.Documents != null)
                {
                    foreach (var doc in dest.Documents)
                    {
                        doc.AppointmentId = dest.Id;
                    }
                }
            });
    }
}