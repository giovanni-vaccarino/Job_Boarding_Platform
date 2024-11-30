using AutoMapper;
using backend.Data.Entities;
using backend.Service.Contracts.Student;

namespace backend.Service.Profiles;

public class StudentMappingProfile : Profile
{
    public StudentMappingProfile()
    {
        CreateMap<Student, StudentDto>();
    }
}
