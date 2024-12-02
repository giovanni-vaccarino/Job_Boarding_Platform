using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Company;

namespace backend.Service.Profiles;

public class QuestionMappingProfile : Profile
{
    public QuestionMappingProfile()
    {
        CreateMap<AddQuestionDto, Question>()
            .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.QuestionType))
            .ForMember(dest => dest.CompanyId, opt => opt.Ignore())
            .ForMember(dest => dest.InternshipQuestions, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore());
        
        CreateMap<Question, QuestionDto>()
            .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.Type));
    }
}