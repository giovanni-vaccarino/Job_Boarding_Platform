using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared;

namespace backend.Service.Profiles;

public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Application, ApplicantDetailsDto>()
            .ForMember(dest => dest.CvPath, opt => opt.MapFrom(src => src.Student.CvPath))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Student.Skills))
            .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
            .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus));
    }
    
    //Add more mapping here
}