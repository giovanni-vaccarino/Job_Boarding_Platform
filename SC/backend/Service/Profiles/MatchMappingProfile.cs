using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Match;

namespace backend.Service.Profiles;

public class MatchMappingProfile : Profile
{
    public MatchMappingProfile()
    {
        CreateMap<Match, MatchDto>();
    }
}
