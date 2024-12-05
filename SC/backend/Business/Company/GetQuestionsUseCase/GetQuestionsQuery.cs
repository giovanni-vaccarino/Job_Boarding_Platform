using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.GetQuestionsUseCase;

public record GetQuestionsQuery(int Id) : IRequest<List<QuestionDto>>;