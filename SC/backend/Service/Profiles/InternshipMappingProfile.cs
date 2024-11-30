using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Company;
using backend.Shared;

namespace backend.Service.Profiles;

public class InternshipMappingProfile : Profile
{
    public InternshipMappingProfile()
    {
        CreateMap<Application, ApplicantDetailsDto>()
            .ForMember(dest => dest.CvPath, opt => opt.MapFrom(src => src.Student.CvPath))
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.Student.Skills))
            .ForMember(dest => dest.SubmissionDate, opt => opt.MapFrom(src => src.SubmissionDate))
            .ForMember(dest => dest.ApplicationStatus, opt => opt.MapFrom(src => src.ApplicationStatus));
        
        CreateMap<Internship, JobDetailsDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.JobCategory, opt => opt.MapFrom(src => src.JobCategory))
            .ForMember(dest => dest.JobType, opt => opt.MapFrom(src => src.JobType))
            .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.SkillsRequired, opt => opt.MapFrom(src => src.Requirements));
    }
    
    //Add more mapping here
}