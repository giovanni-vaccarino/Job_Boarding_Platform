using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using backend.Shared;

namespace backend.Service.Profiles;

public class InternshipMappingProfile : Profile
{
    public InternshipMappingProfile()
    {
        CreateMap<Internship, InternshipDto>()
            .ForMember(dest => dest.DateCreated, opt => opt.MapFrom(src => src.CreatedAt));
        
        CreateMap<AddJobDetailsDto, Internship>()
            .ForMember(dest => dest.Requirements, opt => opt.MapFrom(src => src.Requirements))
            .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
            .ForMember(dest => dest.Company, opt => opt.Ignore())
            .ForMember(dest => dest.Applications, opt => opt.Ignore())
            .ForMember(dest => dest.InternshipQuestions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
    }
}