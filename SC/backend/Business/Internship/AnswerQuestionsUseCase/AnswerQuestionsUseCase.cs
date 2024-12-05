using AutoMapper;
using backend.Data;
using backend.Data.Entities;
using backend.Service.Contracts.Internship;
using backend.Shared.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace backend.Business.Internship.AnswerQuestionsUseCase;

public class AnswerQuestionsUseCase : IRequestHandler<AnswerQuestionsCommand, ApplicationDto>
{
    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    
    public AnswerQuestionsUseCase(AppDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper = mapper;
    }
    
    public async Task<ApplicationDto> Handle(AnswerQuestionsCommand request, CancellationToken cancellationToken)
    {
        var applicationId = request.ApplicationId;
        var answerQuestionsDto = request.Dto;

        var application = await _dbContext.Applications
                .Include(app => app.Internship)
                .ThenInclude(internship => internship.InternshipQuestions)
                .ThenInclude(iq => iq.Question)
                .FirstOrDefaultAsync(app => app.Id == applicationId, cancellationToken)
                    ?? throw new KeyNotFoundException($"Application with ID {applicationId} not found.");
        
        if (application.ApplicationStatus != ApplicationStatus.OnlineAssessment)
        {
            throw new InvalidOperationException("The application status is not valid for answering assessment questions.");
        }

        var internshipQuestions = application.Internship.InternshipQuestions;

        ValidateAnswerCount(answerQuestionsDto.Questions, internshipQuestions);
        
        foreach (var answer in answerQuestionsDto.Questions)
        {
            var internshipQuestion = internshipQuestions.FirstOrDefault(iq => iq.Question.Id == answer.QuestionId);

            if (internshipQuestion == null)
            {
                throw new ArgumentException($"Question with ID {answer.QuestionId} is not part of the internship.");
            }

            ValidateAnswerByQuestionType(internshipQuestion.Question, answer.Answer);
        }

        foreach (var singleAnswer in answerQuestionsDto.Questions)
        {
            var internshipQuestion = internshipQuestions.FirstOrDefault(iq => iq.Question.Id == singleAnswer.QuestionId)
                ?? throw new KeyNotFoundException($"Question with ID {singleAnswer.QuestionId} not found.");
            
            var answerEntity = new Answer
            {
                ApplicationId = applicationId,
                InternshipQuestionId = internshipQuestion.Id,
                StudentAnswer = singleAnswer.Answer,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _dbContext.Answers.Add(answerEntity);
        }
        
        application.ApplicationStatus = ApplicationStatus.LastEvaluation;
        await _dbContext.SaveChangesAsync(cancellationToken);
        
        return _mapper.Map<ApplicationDto>(application);
    }
    
    private void ValidateAnswerCount(List<SingleAnswerQuestion> providedAnswers, List<InternshipQuestion> internshipQuestions)
    {
        if (providedAnswers.Count != internshipQuestions.Count)
        {
            throw new ArgumentException("The number of provided answers does not match the number of internship questions.");
        }
    }
    
    private void ValidateAnswerByQuestionType(Question question, List<string> answers)
    {
        switch (question.Type)
        {
            case QuestionType.OpenQuestion:
                ValidateOpenQuestion(answers);
                break;
            case QuestionType.MultipleChoice:
                ValidateMultipleChoiceQuestion(answers);
                break;
            case QuestionType.TrueOrFalse:
                ValidateTrueOrFalseQuestion(answers);
                break;
        }
    }
    
    private void ValidateOpenQuestion(List<string> answers)
    {
        if (answers.Count != 1)
        {
            throw new ArgumentException("Open questions must have exactly one answer.");
        }
    }
    
    private void ValidateMultipleChoiceQuestion(List<string> answers)
    {
        if (answers.Count == 0 || answers.Any(a => !int.TryParse(a, out _)))
        {
            throw new ArgumentException("Multiple-choice questions must have one or more valid answer indices.");
        }
    }
    
    private void ValidateTrueOrFalseQuestion(List<string> answers)
    {
        if (answers.Count != 1)
        {
            throw new ArgumentException("True or false questions must have exactly one answer.");
        }

        var answer = answers[0].ToLower();
        if (answer.ToLower() != "true" && answer.ToLower() != "false")
        {
            throw new ArgumentException("The answer to a true or false question must be either 'true' or 'false'.");
        }
    }
}