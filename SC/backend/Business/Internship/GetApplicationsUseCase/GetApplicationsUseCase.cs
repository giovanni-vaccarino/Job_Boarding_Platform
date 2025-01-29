using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Internship;
using backend.Service.Contracts.Student;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.GetApplicationsUseCase;

public class GetApplicationsUseCase : IRequestHandler<GetApplicationsQuery, List<ApplicationDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetApplicationsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<ApplicationDto>> Handle(GetApplicationsQuery request, CancellationToken cancellationToken)
    {
        var internshipId = request.InternshipId;

        var applications = await _dbContext.Applications
            .Where(app => app.InternshipId == internshipId)
            .Include(app => app.Internship)
            .Include(app => app.Student)
            .ToListAsync(cancellationToken);

        var applicationsToReturn = applications.Select(app => new ApplicationDto
        {
            Id = app.Id,
            SubmissionDate = app.CreatedAt,
            ApplicationStatus = app.ApplicationStatus,
            Internship = _mapper.Map<InternshipDto>(app.Internship),
            Student = _mapper.Map<StudentDto>(app.Student)
        }).ToList();
        
        
        foreach (var application in applicationsToReturn)
        {
            Console.WriteLine($"Id: {application.Id}, SubmissionDate: {application.SubmissionDate}, ApplicationStatus: {application.ApplicationStatus}");
            if (application.Student != null)
            {
                Console.WriteLine($"Student Id: {application.Student.Id}, Student Name: {application.Student.Name}");
            }
        }
        
        return applicationsToReturn;
    }
}