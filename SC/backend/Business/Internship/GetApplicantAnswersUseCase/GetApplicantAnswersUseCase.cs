using AutoMapper;
using backend.Data;
using backend.Service.Contracts.Company;
using backend.Service.Contracts.Feedback;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic.CompilerServices;

namespace backend.Business.Internship.GetApplicantAnswersUseCase;

public class GetApplicantAnswersUseCase : IRequestHandler<GetApplicantAnswersQuery, ApplicantDetailsResponse>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public GetApplicantAnswersUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApplicantDetailsResponse> Handle(GetApplicantAnswersQuery request, CancellationToken cancellationToken)
    {
        var applicationId = request.ApplicationId;
        var studentId = request.StudentId;

        var student = await _dbContext.Students
            .AsNoTracking()
            .Where(s => s.Id == studentId)
            .FirstOrDefaultAsync(cancellationToken);

        if (student == null)
        {
            throw new KeyNotFoundException($"No student found for Student ID {studentId}.");
        }
        
        var feedbacks = await _dbContext.InternshipFeedbacks
            .Where(f => f.Actor == ProfileType.Student && 
                        _dbContext.Applications.Any(a => a.Id == f.ApplicationId && a.StudentId == studentId))
            .ToListAsync(cancellationToken);
        

        var answers = await _dbContext.Answers
                .Where(answer => answer.ApplicationId == applicationId)
                .Include(answer => answer.InternshipQuestion)
                .ThenInclude(iq => iq.Question)
                .ToListAsync(cancellationToken);

        var answersToSend = answers.Select(answer =>
        {
            if (answer.InternshipQuestion.Question.Type == QuestionType.MultipleChoice)
            {
                var index = int.Parse(answer.StudentAnswer.First());
                answer.StudentAnswer[0] = answer.InternshipQuestion.Question.Options[index];
            }
            return answer;
        }).ToList();

        
        var responseDto = answers.Select(answer => new ApplicantResponseDto
            {
                Question = answer.InternshipQuestion.Question != null ? _mapper.Map<QuestionDto>(answer.InternshipQuestion.Question) : null,
                Answer = answer.StudentAnswer
            }).ToList();
        
    
        var applicantDetailsResponse = new ApplicantDetailsResponse
        {
            Answers = responseDto.ToArray(),
            StudentId = student.Id,
            Name = student!.Name!,
            Skills = student!.Skills,
            Feedbacks = feedbacks.Select(f => _mapper.Map<FeedbackResponseDto>(f)).ToArray()
        };

        return applicantDetailsResponse;
    }
}