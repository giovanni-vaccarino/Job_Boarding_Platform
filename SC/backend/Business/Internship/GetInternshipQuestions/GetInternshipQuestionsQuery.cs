using backend.Service.Contracts.Company;
using backend.Service.Contracts.Internship;
using MediatR;

namespace backend.Business.Internship.GetInternshipQuestions;

public record GetInternshipQuestionsQuery(int InternshipId): IRequest<List<QuestionDto>>;