using Service.Shared.DataTransferObjects;
using AutoMapper;
using Entities.Models;

namespace Service.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<AppointmentDocument, DocumentDto>();

        CreateMap<Appointment, Dto>()
            .ForMember(dest => dest.Documents, opt => opt.MapFrom(src => src.Documents));

        CreateMap<DocumentForCreationDto, AppointmentDocument>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => Guid.NewGuid()))
            .ForMember(dest => dest.AppointmentId, opt => opt.Ignore())
            .ForMember(dest => dest.Appointment, opt => opt.Ignore());

        CreateMap<AppointmentForCreationDto, Appointment>()
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