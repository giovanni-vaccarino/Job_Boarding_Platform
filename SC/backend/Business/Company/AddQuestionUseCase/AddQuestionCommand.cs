using backend.Service.Contracts.Company;
using MediatR;

namespace backend.Business.Company.AddQuestionUseCase;

public record AddQuestionCommand(int Id, AddQuestionDto Dto) : IRequest<QuestionDto>;