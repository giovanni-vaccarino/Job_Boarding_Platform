using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Company;

namespace backend.Service.Profiles;


public class CompanyMappingProfile : Profile
{
    public CompanyMappingProfile()
    {
        CreateMap<Company, CompanyDto>();
    }
}
