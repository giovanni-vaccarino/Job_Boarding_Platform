using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;

namespace backend.Service.Profiles;

public class ApplicationMappingProfile : Profile
{
    public ApplicationMappingProfile()
    {
        CreateMap<Application, ApplicationDto>()
            .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.CreatedAt))
            .ForMember(dest => dest.Internship, opt => opt.MapFrom(src => src.Internship));
    }
}