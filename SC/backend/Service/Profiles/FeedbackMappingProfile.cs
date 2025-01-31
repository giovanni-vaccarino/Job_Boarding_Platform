using AutoMapper;
using backend.Data.Entities;
using backend.Migrations;
using backend.Service.Contracts.Feedback;

namespace backend.Service.Profiles;


public class FeedbackMappingProfile : Profile
{
    public FeedbackMappingProfile()
    {
        CreateMap<InternshipFeedback, FeedbackResponseDto>();
    }
}