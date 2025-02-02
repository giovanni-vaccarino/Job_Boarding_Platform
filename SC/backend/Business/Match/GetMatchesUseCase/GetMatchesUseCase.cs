using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Match;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Match.GetMatchesUseCase;

public class GetMatchesUseCase : IRequestHandler<GetMatchesQuery, List<MatchDto>>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetMatchesUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<List<MatchDto>> Handle(GetMatchesQuery request, CancellationToken cancellationToken)
    {
        var profileId = request.ProfileId;
        var profileType = request.ProfileType;

        var matches = profileType == ProfileType.Student
            ? await GetStudentMatches(profileId, cancellationToken)
            : await GetCompanyMatches(profileId, cancellationToken);

        return _mapper.Map<List<MatchDto>>(matches);
    }
    
    private async Task<List<Data.Entities.Match>> GetStudentMatches(int studentId, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fetching matches for student with ID: {studentId}");

        var appliedInternshipIds = await _dbContext.Applications
            .Where(a => a.StudentId == studentId)
            .Select(a => a.InternshipId)
            .ToListAsync(cancellationToken);

        return await _dbContext.Matches
            .Include(m => m.Student)
            .Include(m => m.Internship)
            .Where(m => m.StudentId == studentId && !appliedInternshipIds.Contains(m.InternshipId))
            .ToListAsync(cancellationToken);
    }
    
    private async Task<List<Data.Entities.Match>> GetCompanyMatches(int companyId, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Fetching matches for companyId: {companyId}");
        var internshipIds = await _dbContext.Internships
            .Where(i => i.CompanyId == companyId)
            .Select(i => i.Id)
            .ToListAsync(cancellationToken);

        return await _dbContext.Matches
            .Include(m => m.Student)
            .Include(m => m.Internship)
            .Where(m => internshipIds.Contains(m.InternshipId))
            .ToListAsync(cancellationToken);
    }
 }